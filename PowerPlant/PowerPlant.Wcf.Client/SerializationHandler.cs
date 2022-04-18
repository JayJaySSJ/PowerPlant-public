using Newtonsoft.Json;
using PowerPlant.Wcf.Client.Clients;
using PowerPlant.Wcf.ServiceDefinition.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace PowerPlant.Wcf.Client
{
    public class SerializationHandler
    {
        private readonly ReadingsManagementClient _readingsManagementClient;

        private readonly CliHelper _cliHelper;

        public SerializationHandler()
        {
            _readingsManagementClient = new ReadingsManagementClient();

            _cliHelper = new CliHelper();
        }

        public async void SerializeCriticalReadingsToJsonAsync()
        {
            Console.Clear();
            var filePath = _cliHelper.GetString("Write file-name to save data to [without extension]");
            filePath += ".json";

            var startDate = _cliHelper.GetValidDateTime("start-date");
            var endDate = _cliHelper.GetValidDateTime("end-date");

            var content = await _readingsManagementClient.GetCriticalReadingsAsync(startDate, endDate);

            var result = await ExecuteJson(content, filePath);

            var defaultColor = Console.ForegroundColor;
            Console.ForegroundColor = _cliHelper.GetConsoleColor(result, defaultColor);

            var message = result
                ? "Serialized correctly"
                : "(!) Serialization failed";
            Console.WriteLine(message);

            Console.ForegroundColor = defaultColor;
        }

        public async Task<bool> ExecuteJson(List<CriticalReading> dataToSave, string filePath)
        {
            try
            {
                await Task.Run(() => {
                    string jsonToSave = JsonConvert.SerializeObject(dataToSave, Formatting.Indented);
                    File.WriteAllText(filePath, jsonToSave);
                });

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}