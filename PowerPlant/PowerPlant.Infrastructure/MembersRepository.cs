using PowerPlant.Domain.Interfaces;
using PowerPlant.Domain.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace PowerPlant.Infrastructure
{
    public class MembersRepository : IMembersRepository
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["PowerPlantDbConnectionString"].ConnectionString;

        public async Task<bool> CreateAsync(Member member)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    string createMemberCommandText = @"
                        INSERT INTO [Members] 
                        ([Login], [Password], [Function])
                        VALUES (@Login, @Password, @Function)
                        ";
                    var createMemberCommandSql = new SqlCommand(createMemberCommandText, connection);
                    createMemberCommandSql.Parameters.Add("@Login", SqlDbType.VarChar, 255).Value = member.Login;
                    createMemberCommandSql.Parameters.Add("@Password", SqlDbType.VarChar, 255).Value = member.Password;
                    createMemberCommandSql.Parameters.Add("@Function", SqlDbType.Int).Value = member.Function;
                    await createMemberCommandSql.ExecuteNonQueryAsync();

                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var deleteMemberCommandText = "DELETE FROM [Members] WHERE [Id] = @Id";

                    var deleteMemberCommandSql = new SqlCommand(deleteMemberCommandText, connection);
                    deleteMemberCommandSql.Parameters.Add("@Id", SqlDbType.Int).Value = id;
                    await deleteMemberCommandSql.ExecuteNonQueryAsync();

                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<Member> GetAsync(string login)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var getMemberCommandText = "SELECT * FROM [Members] WHERE [Login] = @Login";

                    var getMemberCommandSql = new SqlCommand(getMemberCommandText, connection);
                    getMemberCommandSql.Parameters.Add("Login", SqlDbType.VarChar, 255).Value = login;

                    var reader = await getMemberCommandSql.ExecuteReaderAsync();
                    await reader.ReadAsync();

                    return new Member
                    {
                        Id = int.Parse(reader["Id"].ToString()),
                        Login = reader["Login"].ToString(),
                        Password = reader["Password"].ToString(),
                        Function = (MemberFunction)int.Parse(reader["Function"].ToString())
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new Member();
            }
        }

        public async Task<Dictionary<int, Member>> GetAllAsync()
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var members = new Dictionary<int, Member>();
                    var getMembersCommandText = "SELECT * FROM [Members];";

                    var getMembersCommandSql = new SqlCommand(getMembersCommandText, connection);

                    var reader = await getMembersCommandSql.ExecuteReaderAsync();

                    while (await reader.ReadAsync())
                    {
                        var member = new Member()
                        {
                            Id = int.Parse(reader["Id"].ToString()),
                            Login = reader["Login"].ToString(),
                            Password = reader["Password"].ToString(),
                            Function = (MemberFunction)int.Parse(reader["Function"].ToString())
                        };

                        members.Add(member.Id, member);
                    }

                    return members;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new Dictionary<int, Member>();
            }
        }

        public async Task<int> GetTotalCountAsync()
        {
            int membersCountDb = 0;

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    var getMembersCountCommandText = "SELECT COUNT (DISTINCT Id) AS membersCountDb FROM [Members];";

                    var getMembersCountCommandSql = new SqlCommand(getMembersCountCommandText, connection);
                    var reader = await getMembersCountCommandSql.ExecuteReaderAsync();

                    await reader.ReadAsync();
                    membersCountDb = int.Parse(reader["membersCountDb"].ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }

            return membersCountDb;
        }

        public async Task<bool> MemberExistsAsync(string login)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    var memberExistsCommandText = $"SELECT COUNT (DISTINCT Id) AS countDb FROM [Members] WHERE [Login] = @Login;";

                    var memberExistsCommandSql = new SqlCommand(memberExistsCommandText, connection);
                    memberExistsCommandSql.Parameters.Add("@Login", SqlDbType.VarChar, 255).Value = login;

                    var reader = await memberExistsCommandSql.ExecuteReaderAsync();
                    await reader.ReadAsync();

                    if (int.Parse(reader["countDb"].ToString()) < 1)
                    {
                        return false;
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}