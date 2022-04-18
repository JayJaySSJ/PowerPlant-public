using PowerPlant.Domain.Models;
using PowerPlant.Domain;
using PowerPlant.Infrastructure;
using System;

namespace PowerPlant
{
    internal class ReadingsHandler
    {
        private readonly ReadingsService _readingsService;
        private readonly DateProvider _dateProvider;

        public ReadingsHandler()
        {
            ReadingsService.CreateInstance(
                new ReadingsRepository(),
                new PlantDataProvider(),
                new DateProvider());

            _readingsService = ReadingsService.Instance;

            _dateProvider = new DateProvider();
        }

        internal void RunReadings()
        {
            _readingsService.OnNewDataSetReceival += PowerPlantDataArrived;

            ConsoleKeyInfo key;
            do
            {
                key = Console.ReadKey();
            }
            while (key.Key != ConsoleKey.Escape);

            _readingsService.OnNewDataSetReceival -= PowerPlantDataArrived;

            Console.Clear();
            return;
        }

        internal void RunPowerReadings()
        {
            _readingsService.UpdateTimeAsync(_dateProvider.Now);

            _readingsService.OnNewPowerDataSetReceival += PrintPower;

            ConsoleKeyInfo key;
            do
            {
                key = Console.ReadKey();
            }
            while (key.Key != ConsoleKey.Escape);

            _readingsService.OnNewPowerDataSetReceival -= PrintPower;

            Console.Clear();
            return;
        }

        private void PowerPlantDataArrived(object sender, NewDataSet dataSet)
        {
            if (dataSet.CauldronsData != null && dataSet.TurbinesData != null && dataSet.TransformatorsData != null)
            {
                Console.Clear();

                Console.WriteLine(dataSet.PlantName + " " + _dateProvider.Now.ToString("O"));
                foreach (var cauldron in dataSet.CauldronsData)
                {
                    Console.WriteLine(cauldron.Name);
                    PrintValue("WaterPressure", cauldron.WaterPressure);
                    PrintValue("WaterTemperature", cauldron.WaterTemperature);
                    PrintValue("CamberTemperature", cauldron.CamberTemperature);
                }

                foreach (var turbine in dataSet.TurbinesData)
                {
                    Console.WriteLine(turbine.Name);
                    PrintValue("SteamPressure", turbine.SteamPressure);
                    PrintValue("OverheaterSteamTemperature", turbine.OverheaterSteamTemperature);
                    PrintValue("OutputVoltage", turbine.OutputVoltage);
                    PrintValue("RotationSpeed", turbine.RotationSpeed);
                    PrintValue("CurrentPower", turbine.CurrentPower);
                }

                foreach (var transformator in dataSet.TransformatorsData)
                {
                    Console.WriteLine(transformator.Name);
                    PrintValue("InputVoltage", transformator.InputVoltage);
                    PrintValue("OutputVoltage", transformator.OutputVoltage);
                }
            }
        }

        private void PrintValue(string name, AssetParameterData value)
        {
            if (value.CurrentValue > value.MaxValue || value.CurrentValue < value.MinValue)
            {
                Console.Write("\t" + name + "\t");
                var defaultColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(value.CurrentValue + " " + value.Unit);
                Console.ForegroundColor = defaultColor;
            }
            else
            {
                Console.WriteLine("\t" + name + "\t" + value.CurrentValue + " " + value.Unit);
            }
        }

        private void PrintPower(object sender, PowerDataSet[] dataSets)
        {
            var totalEnergy = 0d;

            if (dataSets != null)
            {
                Console.Clear();

                foreach (var turbine in dataSets)
                {
                    Console.WriteLine(turbine.Name);
                    Console.Write("\tCurrentPower\t");
                    Console.Write(turbine.CurrentValue);
                    Console.WriteLine(" MW");

                    totalEnergy += turbine.EnergyProduced;
                }

                Console.Write($"\n\tTotal energy produced\t");
                Console.Write(totalEnergy);
                Console.WriteLine(" MWh");
            }
        }
    }
}