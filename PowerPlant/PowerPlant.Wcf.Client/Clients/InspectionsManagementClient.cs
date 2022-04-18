using PowerPlant.Wcf.ServiceDefinition;
using PowerPlant.Wcf.ServiceDefinition.Models;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

namespace PowerPlant.Wcf.Client.Clients
{
    internal class InspectionsManagementClient : ClientBase<IInspectionsManagementClient>
    {
        public async Task<bool> CreateAsync(InspectionTicket inspectionTicket)
        {
            return await base.Channel.CreateInspectionTicketAsync(inspectionTicket);
        }

        public async Task<List<InspectionTicket>> GetAllAsync()
        {
            return await base.Channel.GetAllStatusTicketsAsync();
        }

        public async Task<bool> ItemExistsAsync(string itemName)
        {
            return await base.Channel.ItemExistsAsync(itemName);
        }

        public async Task<bool> OpenTicketExistsAsync(string itemName)
        {
            return await base.Channel.OpenTicketExistsAsync(itemName);
        }

        public async Task<bool> AssignAsync(InspectionTicket pickedTicket)
        {
            return await base.Channel.AssignTicketAsync(pickedTicket);
        }
    }
}