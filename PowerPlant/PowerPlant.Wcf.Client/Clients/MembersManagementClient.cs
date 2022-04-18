using PowerPlant.Wcf.ServiceDefinition;
using PowerPlant.Wcf.ServiceDefinition.Models;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

namespace PowerPlant.Wcf.Client.Clients
{
    internal class MembersManagementClient : ClientBase<IMembersManagementClient>
    {
        public async Task<bool> CreateAsync(Member member)
        {
            return await base.Channel.CreateAsync(member);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await base.Channel.DeleteAsync(id);
        }

        public async Task<Dictionary<int, Member>> GetAllAsync()
        {
            return await base.Channel.GetAllAsync();
        }

        public async Task<Member> GetAsync(string login)
        {
            return await base.Channel.GetAsync(login);
        }

        public async Task<bool> MemberExists(string login, string password)
        {
            return await base.Channel.MemberExists(login, password);
        }

        public async void UpdateLoggedMemberAsync(string login)
        {
            await base.Channel.UpdateLoggedMemberAsync(login);
        }
    }
}