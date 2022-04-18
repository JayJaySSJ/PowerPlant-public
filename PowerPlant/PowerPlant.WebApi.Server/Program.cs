using Microsoft.Owin.Hosting;
using PowerPlant.Domain;
using PowerPlant.Infrastructure;
using System;

namespace PowerPlant.WebApi.Server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const string baseAddress = "http://localhost:666/";

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

            using (WebApp.Start<StartUp>(baseAddress))
            {
                Console.WriteLine("Server running...");
                Console.ReadLine();
            }

            plantAssetsConditionMonitoring.StopMonitoring();
        }
    }
}