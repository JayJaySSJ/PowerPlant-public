using PowerPlant.Domain;
using PowerPlant.Infrastructure;
using PowerPlant.Wcf.ServiceDefinition;
using System;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace PowerPlant.Wcf.SelfhostServer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var readingsRepository = new ReadingsRepository();
            var plantDataProvider = new PlantDataProvider();
            var dateProvider = new DateProvider();

            var plantAssetsConditionMonitoring = new PlantAssetsConditionMonitoring(
                readingsRepository,
                plantDataProvider,
                dateProvider,
                new MembersService(new MembersRepository()));

            ReadingsService.CreateInstance(
                readingsRepository,
                plantDataProvider,
                dateProvider);

            plantAssetsConditionMonitoring.StartMonitoring();

            //Create a URI to serve as the base address
            Uri httpUrl = new Uri("http://localhost:6666/PowerPlant");

            //Create ServiceHost
            ServiceHost host = new ServiceHost(typeof(PowerPlantServiceDefinition), httpUrl);

            //Add a service endpoint
            var binding = new WSHttpBinding();
            host.AddServiceEndpoint(typeof(IInspectionsManagementClient), binding, "Inspections");
            host.AddServiceEndpoint(typeof(IMembersManagementClient), binding, "Members");
            host.AddServiceEndpoint(typeof(IReadingsManagementClient), binding, "Readings");
            //Enable metadata exchange
            var smb = new ServiceMetadataBehavior();
            smb.HttpGetEnabled = true;
            host.Description.Behaviors.Add(smb);

            //Start the Service
            host.Open();
            Console.WriteLine("Service is host at " + DateTime.Now.ToString());
            Console.WriteLine("Host is running... Press  key to stop");
            Console.ReadLine();
        }
    }
}