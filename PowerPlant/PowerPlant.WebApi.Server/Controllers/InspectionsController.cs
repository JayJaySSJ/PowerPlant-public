using PowerPlant.Domain;
using PowerPlant.Domain.Models;
using PowerPlant.Infrastructure;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace PowerPlant.WebApi.Server.Controllers
{
    [RoutePrefix("api/v1/inspections")]
    public class InspectionsController : ApiController
    {
        private readonly InspectionsService _inspectionsService;
        private readonly MembersService _membersService;

        public InspectionsController()
        {
            _inspectionsService = new InspectionsService(
                new InspectionsRepository(),
                new PlantDataProvider());
            _membersService = new MembersService(
                new MembersRepository());
        }

        [HttpGet]
        [Route("itemName/{itemName}")]
        public async Task<bool> ItemExistsAsync(string itemName)
        {
            return await _inspectionsService.ItemExistsAsync(itemName);
        }

        [HttpPost]
        [Route("")]
        public async Task<bool> CreateInspectionTicketAsync([FromBody] InspectionTicket inspectionTicket)
        {
            if (_membersService.GetAsync(_membersService.GetLoggedMember()).Result.Function == MemberFunction.Engineer)
            {
                return false;
            }
            return await _inspectionsService.CreateAsync(inspectionTicket);
        }

        [HttpGet]
        [Route("tickets")]//{http://localhost:666/api/v1/inspections/tickets}
        public async Task<List<InspectionTicket>> GetAllStatusTicketsAsync()
        {
            return await _inspectionsService.GetAllAsync();
        }

        [HttpGet]
        [Route("ticket/{itemName}")]
        public async Task<bool> OpenTicketExistsAsync(string itemName)
        {
            return await _inspectionsService.OpenTicketExistsAsync(itemName);
        }

        [HttpPost]
        [Route("assign")]
        public async Task<bool> AssignAsync([FromBody] InspectionTicket ticket)
        {
            return await _inspectionsService.AssignAsync(ticket);
        }
    }
}