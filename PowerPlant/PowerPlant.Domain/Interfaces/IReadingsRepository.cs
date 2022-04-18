using PowerPlant.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PowerPlant.Domain.Interfaces
{
    public interface IReadingsRepository
    {
        void SaveReading(CriticalReading dataToSave);
        Task<List<CriticalReading>> GetCriticalReadingsAsync(DateTime floorValue, DateTime ceilingValue);
        Task<List<CriticalStatistics>> GetCriticalStatisticsAsync(DateTime floorValue, DateTime ceilingValue);
        Task<bool> CheckIfItemUnderMaintenanceAsync(string itemName);
    }
}