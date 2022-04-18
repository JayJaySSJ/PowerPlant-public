using Newtonsoft.Json;
using PowerPlant.WebApi.Client.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;

namespace PowerPlant.WebApi.Client.Clients
{
    public class ReadingsWebApiClient
    {
        private readonly HttpClient _httpClient;

        private static string _baseUrl => ConfigurationManager.AppSettings["url"];
        private static readonly string _clientPath = _baseUrl + "/api/v1/readings";

        public ReadingsWebApiClient()
        {
            _httpClient = new HttpClient();
        }

        public async Task<NewDataSet> GetNewDataSetAsync()
        {
            try
            {
                var responseBody = await _httpClient.GetAsync($@"{_clientPath}/dataset");

                var result = await responseBody.Content.ReadAsStringAsync();

                if (!responseBody.IsSuccessStatusCode)
                {
                    return new NewDataSet();
                }

                return JsonConvert.DeserializeObject<NewDataSet>(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", ex.Message);
                return new NewDataSet();
            }
        }

        public async Task<PowerDataSet[]> GetPowerDataSetAsync()
        {
            try
            {
                var responseBody = await _httpClient.GetAsync($@"{_clientPath}/powerdataset");

                var result = await responseBody.Content.ReadAsStringAsync();

                if (!responseBody.IsSuccessStatusCode)
                {
                    return new PowerDataSet[0];
                }

                return JsonConvert.DeserializeObject<PowerDataSet[]>(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", ex.Message);
                return new PowerDataSet[0];
            }
        }

        public async Task<DateTime> GetReadingTimeAsync()
        {
            try
            {
                var responseBody = await _httpClient.GetAsync($@"{_clientPath}/time");

                var result = await responseBody.Content.ReadAsStringAsync();

                if (!responseBody.IsSuccessStatusCode)
                {
                    return new DateTime();
                }

                return JsonConvert.DeserializeObject<DateTime>(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", ex.Message);
                return new DateTime();
            }
        }

        public async void UpdateTime(DateTime dateTime)
        {
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(dateTime), System.Text.Encoding.UTF8, "application/json");

                await _httpClient.PostAsync($@"{_clientPath}", content);
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", ex.Message);
            }
        }

        public async Task<List<CriticalReading>> GetCriticalReadingsAsync(DateTime floorValue, DateTime ceilingValue)
        {
            try
            {
                var url = $@"{_clientPath}/dates/{floorValue:yyyy-MM-ddTHH:mm:ss}/{ceilingValue:yyyy-MM-ddTHH:mm:ss}";

                var responseBody = await _httpClient.GetAsync(url);

                var result = await responseBody.Content.ReadAsStringAsync();

                if (!responseBody.IsSuccessStatusCode)
                {
                    return new List<CriticalReading>();
                }

                return JsonConvert.DeserializeObject<List<CriticalReading>>(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", ex.Message);
                return new List<CriticalReading>();
            }
        }

        public async Task<List<CriticalStatistics>> GetCriticalStatisticsAsync(DateTime floorValue, DateTime ceilingValue)
        {
            try
            {
                var responseBody = await _httpClient.GetAsync($@"{_clientPath}/statisticdates/{floorValue:yyyy-MM-ddTHH:mm:ss}/{ceilingValue:yyyy-MM-ddTHH:mm:ss}");

                var result = await responseBody.Content.ReadAsStringAsync();

                if (!responseBody.IsSuccessStatusCode)
                {
                    return new List<CriticalStatistics>();
                }

                return JsonConvert.DeserializeObject<List<CriticalStatistics>>(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", ex.Message);
                return new List<CriticalStatistics>();
            }
        }
    }
}