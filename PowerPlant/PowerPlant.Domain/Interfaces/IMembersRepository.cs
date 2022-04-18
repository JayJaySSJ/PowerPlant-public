using PowerPlant.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PowerPlant.Domain.Interfaces
{
    public interface IMembersRepository
    {
        Task<bool> CreateAsync(Member member);
        Task<bool> DeleteAsync(int id);
        Task<Dictionary<int, Member>> GetAllAsync();
        Task<Member> GetAsync(string login);
        Task<int> GetTotalCountAsync();
        Task<bool> MemberExistsAsync(string login);
    }
}