using PowerPlant.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PowerPlant.Domain.Interfaces
{
    public interface IInspectionsRepository
    {
        Task<bool> CreateInspectionTicket(InspectionTicket inspectionTicket);
        Task<List<InspectionTicket>> GetAllTickets();
        Task<bool> OpenTicketExistsAsync(string itemName);
        Task<bool> AssignAsync(InspectionTicket pickedTicket);
    }
}