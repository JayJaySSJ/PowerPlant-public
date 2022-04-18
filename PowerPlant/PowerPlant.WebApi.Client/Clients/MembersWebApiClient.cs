using Newtonsoft.Json;
using PowerPlant.WebApi.Client.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;

namespace PowerPlant.WebApi.Client.Clients
{
    internal class MembersWebApiClient
    {
        private readonly HttpClient _httpClient;

        private static string _baseUrl => ConfigurationManager.AppSettings["url"];
        private static readonly string _clientPath = _baseUrl + "/api/v1/members";

        public MembersWebApiClient()
        {
            _httpClient = new HttpClient();
        }

        public async Task<bool> CreateAsync(Member member)
        {
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(member), System.Text.Encoding.UTF8, "application/json");

                var responseBody = await _httpClient.PostAsync($@"{_clientPath}/create", content);

                var result = await responseBody.Content.ReadAsStringAsync();

                if (!responseBody.IsSuccessStatusCode)
                {
                    return false;
                }

                return bool.Parse(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", ex.Message);
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var responseBody = await _httpClient.DeleteAsync($@"{_clientPath}/{id}");

                var result = await responseBody.Content.ReadAsStringAsync();

                if (!responseBody.IsSuccessStatusCode)
                {
                    return false;
                }

                return bool.Parse(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", ex.Message);
                return false;
            }
        }

        public async Task<Dictionary<int, Member>> GetAllAsync()
        {
            try
            {
                var responseBody = await _httpClient.GetAsync($@"{_clientPath}/all");

                var result = await responseBody.Content.ReadAsStringAsync();

                if (!responseBody.IsSuccessStatusCode)
                {
                    return new Dictionary<int, Member>();
                }

                return JsonConvert.DeserializeObject<Dictionary<int, Member>>(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", ex.Message);
                return new Dictionary<int, Member>();
            }
        }

        public async Task<Member> GetAsync(string login)
        {
            try
            {
                var responseBody = await _httpClient.GetAsync($@"{_clientPath}/login/{login}");

                var result = await responseBody.Content.ReadAsStringAsync();

                if (!responseBody.IsSuccessStatusCode)
                {
                    return new Member();
                }

                return JsonConvert.DeserializeObject<Member>(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", ex.Message);
                return new Member();
            }
        }

        public async Task<bool> MemberExists(MemberCredentials memberCredentials)
        {
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(memberCredentials), System.Text.Encoding.UTF8, "application/json");

                var responseBody = await _httpClient.PostAsync($@"{_clientPath}/credentials", content);

                var result = await responseBody.Content.ReadAsStringAsync();

                if (!responseBody.IsSuccessStatusCode)
                {
                    return false;
                }

                return bool.Parse(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", ex.Message);
                return false;
            }
        }

        public async void UpdateLoggedMemberAsync(string login)
        {
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(login), System.Text.Encoding.UTF8, "application/json");

                await _httpClient.PostAsync($@"{_clientPath}/login", content);
            }
            catch (Exception e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }
        }
    }
}