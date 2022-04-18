using PowerPlant.Domain.Interfaces;
using PowerPlant.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PowerPlant.Domain
{
    public interface IMembersService
    {
        Task<bool> CreateAsync(Member member);
        Task<bool> DeleteAsync(int id);
        Task<Dictionary<int, Member>> GetAllAsync();
        string GetLoggedMember();
        Task<Member> GetAsync(string login);
        Task<bool> MemberExistsAsync(string login, string password);
        Task UpdateLoggedMemberAsync(string login);
        bool IsAdminLogged();
    }

    public class MembersService : IMembersService

    {
        private readonly IMembersRepository _membersRepository;

        private static string _loggedMember = "N/A";

        public MembersService(IMembersRepository memberRepository)
        {
            _membersRepository = memberRepository;
        }

        public async Task UpdateLoggedMemberAsync(string login)
        {
            _loggedMember = login;
        }

        public string GetLoggedMember()
        {
            return _loggedMember;
        }

        public async Task<bool> MemberExistsAsync(string login, string password)
        {
            Member member = new Member();

            if (await _membersRepository.MemberExistsAsync(login))
            {
                member = await _membersRepository.GetAsync(login);
            }

            return member.Password == password;
        }

        public async Task<Member> GetAsync(string login)
        {
            if (await _membersRepository.MemberExistsAsync(login))
            {
                return await _membersRepository.GetAsync(login);
            }
            
            return new Member();
        }

        public async Task<Dictionary<int, Member>> GetAllAsync()
        {
            return await _membersRepository.GetAllAsync();
        }

        public async Task<bool> CreateAsync(Member member)
        {
            return await _membersRepository.CreateAsync(member);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _membersRepository.DeleteAsync(id);
        }

        public bool IsAdminLogged()
        {
            if (_loggedMember != "N/A")
            {
                return GetAsync(_loggedMember).Result.Function == MemberFunction.Admin;
            }

            return false;
        }
    }
}