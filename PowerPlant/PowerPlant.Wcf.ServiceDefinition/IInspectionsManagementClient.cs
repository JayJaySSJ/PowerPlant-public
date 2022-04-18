using PowerPlant.Wcf.ServiceDefinition.Models;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

namespace PowerPlant.Wcf.ServiceDefinition
{
    [ServiceContract]
    public interface IInspectionsManagementClient
    {
        [OperationContract]
        Task<bool> CreateInspectionTicketAsync(InspectionTicket inspectionTicket);

        [OperationContract]
        Task<List<InspectionTicket>> GetAllStatusTicketsAsync();

        [OperationContract]
        Task<bool> ItemExistsAsync(string itemName);

        [OperationContract]
        Task<bool> OpenTicketExistsAsync(string itemName);

        [OperationContract]
        Task<bool> AssignTicketAsync(InspectionTicket pickedTicket);
    }
}