using Newtonsoft.Json;
using PowerPlant.Domain.Interfaces;
using PowerPlant.Domain.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace PowerPlant.Infrastructure
{
    public class SerializationRepository : ISerializationRepository
    {
        public async Task<bool> ExecuteAsync(List<CriticalReading> dataToSave, string filePath)
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