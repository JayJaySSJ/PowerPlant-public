using PowerPlant.Domain.Models;
using PowerPlant.Domain.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace PowerPlant.Domain
{
    public class ReadingsService
    {
        public static ReadingsService Instance { get; private set; }

        private readonly IReadingsRepository _readingsRepository;
        private readonly IPlantDataProvider _plantDataProvider;
        private readonly IDateProvider _dateProvider;

        public event EventHandler<NewDataSet> OnNewDataSetReceival = null;
        public event EventHandler<PowerDataSet[]> OnNewPowerDataSetReceival = null;

        private DateTime _timeStart;

        private DateTime _readingTime;
        private NewDataSet _newDataSet;
        private PowerDataSet[] _powerDataSet;

        private ReadingsService(
            IReadingsRepository readingsRepository,
            IPlantDataProvider plantDataProvider,
            IDateProvider dateProvider
            )
        {
            _readingsRepository = readingsRepository;
            _plantDataProvider = plantDataProvider;
            _dateProvider = dateProvider;

            _plantDataProvider.OnPlantDataArrival += NewDataSetReceived;
        }

        public static void CreateInstance(
            IReadingsRepository readingsRepository,
            IPlantDataProvider plantDataProvider,
            IDateProvider dateProvider)
        {
            Instance = Instance ?? new ReadingsService(
                readingsRepository,
                plantDataProvider,
                dateProvider);
        }

        private void NewDataSetReceived(object sender, NewDataSet dataSet)
        {
            _readingTime = _dateProvider.Now;

            const int hourInSeconds = 3600;
            var timeElapsed = _dateProvider.Now - _timeStart;

            _newDataSet = dataSet;
            OnNewDataSetReceival?.Invoke(this, dataSet);

            var powerDataSets = dataSet.TurbinesData
                .Select(x => new PowerDataSet
                {
                    Name = x.Name,
                    CurrentValue = x.CurrentPower.CurrentValue,
                    EnergyProduced = x.CurrentPower.CurrentValue * timeElapsed.Seconds / hourInSeconds
                })
                .ToArray();

            _powerDataSet = powerDataSets;
            OnNewPowerDataSetReceival?.Invoke(this, powerDataSets);
        }

        public async Task UpdateTimeAsync(DateTime dateTime)
        {
            _timeStart = dateTime;
        }

        public async Task<NewDataSet> GetNewDataSetAsync()
        {
            return _newDataSet;
        }

        public async Task<PowerDataSet[]> GetPowerDataSetAsync()
        {
            return _powerDataSet;
        }

        public async Task<DateTime> GetReadingTimeAsync()
        {
            return _readingTime;
        }

        public async Task<List<CriticalReading>> GetCriticalReadingsAsync(DateTime floorValue, DateTime ceilingValue)
        {
            return await _readingsRepository.GetCriticalReadingsAsync(floorValue, ceilingValue);
        }

        public async Task<List<CriticalStatistics>> GetCriticalStatisticsAsync(DateTime floorValue, DateTime ceilingValue)
        {
            return await _readingsRepository.GetCriticalStatisticsAsync(floorValue, ceilingValue);
        }
    }
}