using PowerPlant.Wcf.ServiceDefinition.Models;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

namespace PowerPlant.Wcf.ServiceDefinition
{
    [ServiceContract]
    public interface IReadingsManagementClient
    {
        [OperationContract]
        Task<List<CriticalStatistics>> GetCriticalStatisticsAsync(DateTime floorValue, DateTime ceilingValue);

        [OperationContract]
        Task<List<CriticalReading>> GetCriticalReadingsAsync(DateTime floorValue, DateTime ceilingValue);

        [OperationContract]
        Task<NewDataSet> GetNewDataSetAsync();

        [OperationContract]
        Task<PowerDataSet[]> GetPowerDataSetAsync();

        [OperationContract]
        Task<DateTime> GetReadingTimeAsync();

        [OperationContract]
        Task UpdateTime(DateTime dateTime);
    }
}