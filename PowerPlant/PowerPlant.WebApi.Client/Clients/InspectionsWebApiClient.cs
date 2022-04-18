using Newtonsoft.Json;
using PowerPlant.WebApi.Client.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;

namespace PowerPlant.WebApi.Client.Clients
{
    public class InspectionsWebApiClient
    {
        private readonly HttpClient _httpClient;

        private static string _baseUrl => ConfigurationManager.AppSettings["url"];
        private static readonly string _clientPath = _baseUrl + "/api/v1/inspections";

        public InspectionsWebApiClient()
        {
            _httpClient = new HttpClient();
        }

        public async Task<bool> ItemExistsAsync(string itemName)
        {
            try
            {
                var responseBody = await _httpClient.GetAsync($@"{_clientPath}/itemName/{itemName}");

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

        public async Task<bool> CreateAsync(InspectionTicket inspectionTicket)
        {
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(inspectionTicket), System.Text.Encoding.UTF8, "application/json");

                var responseBody = await _httpClient.PostAsync($@"{_clientPath}", content);

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

        public async Task<List<InspectionTicket>> GetAllAsync()
        {
            try
            {
                var responseBody = await _httpClient.GetAsync($@"{_clientPath}/tickets");

                var result = await responseBody.Content.ReadAsStringAsync();

                if (!responseBody.IsSuccessStatusCode)
                {
                    return new List<InspectionTicket>();
                }

                return JsonConvert.DeserializeObject<List<InspectionTicket>>(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", ex.Message);
                return new List<InspectionTicket>();
            }
        }

        public async Task<bool> OpenTicketExistsAsync(string itemName)
        {
            try
            {
                var responseBody = await _httpClient.GetAsync($@"{_clientPath}/ticket/{itemName}");

                var result = await responseBody.Content.ReadAsStringAsync();

                if (!responseBody.IsSuccessStatusCode)
                {
                    return false;
                }

                return JsonConvert.DeserializeObject<bool>(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", ex.Message);
                return false;
            }
        }

        public async Task<bool> AssignAsync(InspectionTicket inspectionTicket)
        {
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(inspectionTicket), System.Text.Encoding.UTF8, "application/json");

                var responseBody = await _httpClient.PostAsync($@"{_clientPath}/assign", content);

                var result = await responseBody.Content.ReadAsStringAsync();

                if (!responseBody.IsSuccessStatusCode)
                {
                    return false;
                }

                return bool.Parse(result);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
                return false;
            }
        }
    }
}