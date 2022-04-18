using PowerPlant.Wcf.ServiceDefinition.Models;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

namespace PowerPlant.Wcf.ServiceDefinition
{
    [ServiceContract]
    public interface IMembersManagementClient
    {
        [OperationContract]
        Task<bool> CreateAsync(Member member);

        [OperationContract]
        Task<bool> DeleteAsync(int id);

        [OperationContract]
        Task<Dictionary<int, Member>> GetAllAsync();

        [OperationContract]
        Task<Member> GetAsync(string login);

        [OperationContract]
        Task<bool> MemberExists(string login, string password);

        [OperationContract]
        Task UpdateLoggedMemberAsync(string login);
    }
}