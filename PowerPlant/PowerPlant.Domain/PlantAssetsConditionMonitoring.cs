using PowerPlant.Domain.Interfaces;
using PowerPlant.Domain.Models;
using System;
using System.Collections.Generic;

namespace PowerPlant.Domain
{
    public class PlantAssetsConditionMonitoring
    {
        private readonly IReadingsRepository _readingsRepository;
        private readonly IPlantDataProvider _plantDataProvider;
        private readonly IDateProvider _dateProvider;
        private readonly IMembersService _memberService;

        private string _previousCriticalReading = null;
        private readonly Dictionary<string, bool> _savedCriticalReadings = new Dictionary<string, bool>();

        public PlantAssetsConditionMonitoring(
            IReadingsRepository readingsRepository,
            IPlantDataProvider plantDataProvider,
            IDateProvider dateProvider,
            IMembersService memberService)
        {
            _readingsRepository = readingsRepository;
            _plantDataProvider = plantDataProvider;
            _dateProvider = dateProvider;
            _memberService = memberService;
        }

        public void StartMonitoring()
        {
            _plantDataProvider.OnPlantDataArrival += MonitorReadings;
        }

        public void StopMonitoring()
        {
            _plantDataProvider.OnPlantDataArrival -= MonitorReadings;
        }

        public void MonitorReadings(object sender, NewDataSet dataSet)
        {
            DateTime time = _dateProvider.Now;

            if (dataSet.CauldronsData != null && dataSet.TurbinesData != null && dataSet.TransformatorsData != null)
            {
                foreach (var cauldron in dataSet.CauldronsData)
                {
                    if (cauldron.WaterPressure.CurrentValue > cauldron.WaterPressure.MaxValue || cauldron.WaterPressure.CurrentValue < cauldron.WaterPressure.MinValue)
                    {
                        SaveIfNewAsync(new CriticalReading { LoggedMember = string.Empty, PlantName = dataSet.PlantName, ItemName = cauldron.Name, ParameterName = "WaterPressure", ReadingTime = time, MinValue = cauldron.WaterPressure.MinValue, MaxValue = cauldron.WaterPressure.MaxValue });
                    }

                    if (cauldron.WaterTemperature.CurrentValue > cauldron.WaterTemperature.MaxValue || cauldron.WaterTemperature.CurrentValue < cauldron.WaterTemperature.MinValue)
                    {
                        SaveIfNewAsync(new CriticalReading { LoggedMember = string.Empty, PlantName = dataSet.PlantName, ItemName = cauldron.Name, ParameterName = "WaterTemperature", ReadingTime = time, MinValue = cauldron.WaterTemperature.MinValue, MaxValue = cauldron.WaterTemperature.MaxValue });
                    }

                    if (cauldron.CamberTemperature.CurrentValue > cauldron.CamberTemperature.MaxValue || cauldron.CamberTemperature.CurrentValue < cauldron.CamberTemperature.MinValue)
                    {
                        SaveIfNewAsync(new CriticalReading { LoggedMember = string.Empty, PlantName = dataSet.PlantName, ItemName = cauldron.Name, ParameterName = "CamberTemperature", ReadingTime = time, MinValue = cauldron.CamberTemperature.MinValue, MaxValue = cauldron.CamberTemperature.MaxValue });
                    }
                }

                foreach (var turbine in dataSet.TurbinesData)
                {
                    if (turbine.OverheaterSteamTemperature.CurrentValue > turbine.OverheaterSteamTemperature.MaxValue || turbine.OverheaterSteamTemperature.CurrentValue < turbine.OverheaterSteamTemperature.MinValue)
                    {
                        SaveIfNewAsync(new CriticalReading { LoggedMember = string.Empty, PlantName = dataSet.PlantName, ItemName = turbine.Name, ParameterName = "OverheaterSteamTemperature", ReadingTime = time, MinValue = turbine.OverheaterSteamTemperature.MinValue, MaxValue = turbine.OverheaterSteamTemperature.MaxValue });
                    }

                    if (turbine.SteamPressure.CurrentValue > turbine.SteamPressure.MaxValue || turbine.SteamPressure.CurrentValue < turbine.SteamPressure.MinValue)
                    {
                        SaveIfNewAsync(new CriticalReading { LoggedMember = string.Empty, PlantName = dataSet.PlantName, ItemName = turbine.Name, ParameterName = "SteamPressure", ReadingTime = time, MinValue = turbine.SteamPressure.MinValue, MaxValue = turbine.SteamPressure.MaxValue });
                    }

                    if (turbine.RotationSpeed.CurrentValue > turbine.RotationSpeed.MaxValue || turbine.RotationSpeed.CurrentValue < turbine.RotationSpeed.MinValue)
                    {
                        SaveIfNewAsync(new CriticalReading { LoggedMember = string.Empty, PlantName = dataSet.PlantName, ItemName = turbine.Name, ParameterName = "RotationSpeed", ReadingTime = time, MinValue = turbine.RotationSpeed.MinValue, MaxValue = turbine.RotationSpeed.MaxValue });
                    }

                    if (turbine.CurrentPower.CurrentValue > turbine.CurrentPower.MaxValue || turbine.CurrentPower.CurrentValue < turbine.CurrentPower.MinValue)
                    {
                        SaveIfNewAsync(new CriticalReading { LoggedMember = string.Empty, PlantName = dataSet.PlantName, ItemName = turbine.Name, ParameterName = "CurrentPower", ReadingTime = time, MinValue = turbine.CurrentPower.MinValue, MaxValue = turbine.CurrentPower.MaxValue });
                    }

                    if (turbine.OutputVoltage.CurrentValue > turbine.OutputVoltage.MaxValue || turbine.OutputVoltage.CurrentValue < turbine.OutputVoltage.MinValue)
                    {
                        SaveIfNewAsync(new CriticalReading { LoggedMember = string.Empty, PlantName = dataSet.PlantName, ItemName = turbine.Name, ParameterName = "OutputVoltage", ReadingTime = time, MinValue = turbine.OutputVoltage.MinValue, MaxValue = turbine.OutputVoltage.MaxValue });
                    }
                }

                foreach (var transformator in dataSet.TransformatorsData)
                {
                    if (transformator.InputVoltage.CurrentValue > transformator.InputVoltage.MaxValue || transformator.InputVoltage.CurrentValue < transformator.InputVoltage.MinValue)
                    {
                        SaveIfNewAsync(new CriticalReading { LoggedMember = string.Empty, PlantName = dataSet.PlantName, ItemName = transformator.Name, ParameterName = "InputVoltage", ReadingTime = time, MinValue = transformator.InputVoltage.MinValue, MaxValue = transformator.InputVoltage.MaxValue });
                    }

                    if (transformator.OutputVoltage.CurrentValue > transformator.OutputVoltage.MaxValue || transformator.OutputVoltage.CurrentValue < transformator.OutputVoltage.MinValue)
                    {
                        SaveIfNewAsync(new CriticalReading { LoggedMember = string.Empty, PlantName = dataSet.PlantName, ItemName = transformator.Name, ParameterName = "OutputVoltage", ReadingTime = time, MinValue = transformator.OutputVoltage.MinValue, MaxValue = transformator.OutputVoltage.MaxValue });
                    }
                }
            }
        }

        public async void SaveIfNewAsync(CriticalReading dataToSave)
        {
            if(await _readingsRepository.CheckIfItemUnderMaintenanceAsync(dataToSave.ItemName))
            {
                return;
            }

            var actualCriticalReading = dataToSave.ItemName + dataToSave.ParameterName;

            if (!_savedCriticalReadings.ContainsKey(actualCriticalReading))
            {
                if (_previousCriticalReading != null) { _savedCriticalReadings[_previousCriticalReading] = false; }

                dataToSave.LoggedMember = _memberService.GetLoggedMember();
                _readingsRepository.SaveReading(dataToSave);

                _savedCriticalReadings.Add(actualCriticalReading, true);
                _previousCriticalReading = actualCriticalReading;
            }
            else if (_savedCriticalReadings[actualCriticalReading] == true)
            {
                return;
            }
            else
            {
                _savedCriticalReadings[_previousCriticalReading] = false;
                _savedCriticalReadings[actualCriticalReading] = true;

                dataToSave.LoggedMember = _memberService.GetLoggedMember();
                _readingsRepository.SaveReading(dataToSave);

                _previousCriticalReading = actualCriticalReading;
            }
        }
    }
}