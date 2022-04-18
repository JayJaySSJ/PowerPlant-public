using PowerPlant.Wcf.ServiceDefinition;
using PowerPlant.Wcf.ServiceDefinition.Models;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

namespace PowerPlant.Wcf.Client.Clients
{
    internal class ReadingsManagementClient : ClientBase<IReadingsManagementClient>
    {
        public async Task<List<CriticalStatistics>> GetCriticalStatisticsAsync(DateTime beginDate, DateTime endDate)
        {
            return await base.Channel.GetCriticalStatisticsAsync(beginDate, endDate);
        }

        public async Task<List<CriticalReading>> GetCriticalReadingsAsync(DateTime beginDate, DateTime endDate)
        {
            return await base.Channel.GetCriticalReadingsAsync(beginDate, endDate);
        }

        public async Task<NewDataSet> GetNewDataSetAsync()
        {
            return await base.Channel.GetNewDataSetAsync();
        }

        public async Task<PowerDataSet[]> GetPowerDataSetAsync()
        {
            return await base.Channel.GetPowerDataSetAsync();
        }

        public async Task<DateTime> GetReadingTimeAsync()
        {
            return await base.Channel.GetReadingTimeAsync();
        }

        public async void UpdateTime(DateTime dateTime)
        {
            await base.Channel.UpdateTime(dateTime);
        }
    }
}