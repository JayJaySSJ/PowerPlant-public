using PowerPlant.Domain.Interfaces;
using PowerPlant.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PowerPlant.Domain
{
    public class InspectionsService
    {
        private readonly IInspectionsRepository _inspiectionsRepository;
        private readonly IPlantDataProvider _plantDataProvider;

        public InspectionsService(
            IInspectionsRepository inspiectionsRepository,
            IPlantDataProvider plantDataProvider)
        {
            _inspiectionsRepository = inspiectionsRepository;
            _plantDataProvider = plantDataProvider;
        }

        public async Task<bool> ItemExistsAsync(string itemName)
        {
            var names = _plantDataProvider.GetItemNames();

            foreach (var name in names)
            {
                if(itemName == name)
                {
                    return true;
                }
            }

            return false;
        }

        public async Task<bool> CreateAsync(InspectionTicket inspectionTicket)
        {
            return await _inspiectionsRepository.CreateInspectionTicket(inspectionTicket);
        }

        public async Task<List<InspectionTicket>> GetAllAsync()
        {
            return await _inspiectionsRepository.GetAllTickets();
        }

        public async Task<bool> OpenTicketExistsAsync(string itemName)
        {
            return await _inspiectionsRepository.OpenTicketExistsAsync(itemName);
        }

        public async Task<bool> AssignAsync(InspectionTicket pickedTicket)
        {
            return await _inspiectionsRepository.AssignAsync(pickedTicket);
        }
    }
}