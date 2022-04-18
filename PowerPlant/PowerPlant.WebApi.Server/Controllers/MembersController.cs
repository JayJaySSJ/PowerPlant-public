using PowerPlant.Domain;
using PowerPlant.Domain.Models;
using PowerPlant.Infrastructure;
using PowerPlant.WebApi.Server.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace PowerPlant.WebApi.Server.Controllers
{
    [RoutePrefix("api/v1/members")]
    public class MembersController : ApiController
    {
        private readonly MembersService _membersService;

        public MembersController()
        {
            _membersService = new MembersService(new MembersRepository());
        }

        [HttpPost]
        [Route("create")]
        public async Task<bool> CreateAsync([FromBody] Member member)
        {
            if (!_membersService.IsAdminLogged())
            {
                return false;
            }
            return await _membersService.CreateAsync(member);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<bool> DeleteAsync([FromUri] int id)
        {
            if (!_membersService.IsAdminLogged())
            {
                return false;
            }
            return await _membersService.DeleteAsync(id);
        }

        [HttpGet]
        [Route("all")]
        public async Task<Dictionary<int, Member>> GetAllAsync()
        {
            return await _membersService.GetAllAsync();
        }

        [HttpGet]
        [Route("login/{login}")]
        public async Task<Member> GetAsync(string login)
        {
            return await _membersService.GetAsync(login);
        }

        [HttpPost]
        [Route("credentials")]
        public async Task<bool> MemberExistsAsync([FromBody] MemberCredentials userCredentials)
        {
            return await _membersService.MemberExistsAsync(userCredentials.Login, userCredentials.Password);
        }

        [HttpPost]
        [Route("login")]
        public async Task UpdateLoggedMemberAsync([FromBody] string login)
        {
            await _membersService.UpdateLoggedMemberAsync(login);
        }
    }
}