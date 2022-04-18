using PowerPlant.WebApi.Client.Models;
using PowerPlant.WebApi.Client.Clients;
using System;
using System.Timers;

namespace PowerPlant.WebApi.Client
{
    internal class ReadingsHandler
    {
        private readonly ReadingsWebApiClient _readingsWebApiClient;

        private readonly CliHelper _cliHelper;

        public ReadingsHandler()
        {
            _readingsWebApiClient = new ReadingsWebApiClient();

            _cliHelper = new CliHelper();
        }

        internal void RunReadings()
        {
            var timer = new Timer(1000);
            timer.Elapsed += OnNewDataSetReceival;
            timer.Start();

            ConsoleKeyInfo key;
            do
            {
                key = Console.ReadKey();
            }
            while (key.Key != ConsoleKey.Escape);

            timer.Stop();
            Console.Clear();
            return;
        }

        internal void RunPowerReadings()
        {
            _readingsWebApiClient.UpdateTime(DateTime.Now);

            var timer = new Timer(1000);
            timer.Elapsed += OnPowerDataSetReceival;
            timer.Start();

            ConsoleKeyInfo key;
            do
            {
                key = Console.ReadKey();
            }
            while (key.Key != ConsoleKey.Escape);

            timer.Stop();
            Console.Clear();
            return;
        }

        private void OnNewDataSetReceival(object sender, ElapsedEventArgs e)
        {
            var newDataSet = _readingsWebApiClient.GetNewDataSetAsync().Result;

            if (newDataSet != null && newDataSet.CauldronsData != null && newDataSet.TurbinesData != null && newDataSet.TransformatorsData != null)
            {
                Console.Clear();

                Console.WriteLine(newDataSet.PlantName);
                Console.WriteLine("Receival time: " + _readingsWebApiClient.GetReadingTimeAsync().Result.ToString());
                foreach (var cauldron in newDataSet.CauldronsData)
                {
                    Console.WriteLine(cauldron.Name);
                    PrintValue("WaterPressure", cauldron.WaterPressure);
                    PrintValue("WaterTemperature", cauldron.WaterTemperature);
                    PrintValue("CamberTemperature", cauldron.CamberTemperature);
                }

                foreach (var turbine in newDataSet.TurbinesData)
                {
                    Console.WriteLine(turbine.Name);
                    PrintValue("SteamPressure", turbine.SteamPressure);
                    PrintValue("OverheaterSteamTemperature", turbine.OverheaterSteamTemperature);
                    PrintValue("OutputVoltage", turbine.OutputVoltage);
                    PrintValue("RotationSpeed", turbine.RotationSpeed);
                    PrintValue("CurrentPower", turbine.CurrentPower);
                }

                foreach (var transformator in newDataSet.TransformatorsData)
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

        private void OnPowerDataSetReceival(object sender, ElapsedEventArgs e)
        {
            var dataSets = _readingsWebApiClient.GetPowerDataSetAsync().Result;
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

        public void PrintChosenCriticalReadings()
        {
            Console.Clear();
            var floorValue = _cliHelper.GetValidDateTime("start-date");
            var ceilingValue = _cliHelper.GetValidDateTime("end-date");

            var dataSets = _readingsWebApiClient.GetCriticalReadingsAsync(floorValue, ceilingValue).Result;

            if (dataSets != null)
            {
                foreach (var dataSet in dataSets)
                {
                    Console.WriteLine($"Logged member:\t{dataSet.LoggedMember}");
                    Console.WriteLine($"Plant name:\t{dataSet.PlantName}");
                    Console.WriteLine($"Item name:\t{dataSet.ItemName}");
                    Console.WriteLine($"Parameter name:\t{dataSet.ParameterName}");
                    Console.WriteLine($"Reading time:\t{dataSet.ReadingTime}");
                    Console.WriteLine($"Min. value:\t{dataSet.MinValue}");
                    Console.WriteLine($"Max. value:\t{dataSet.MaxValue}\n");
                }
            }
        }

        public void PrintChosenCriticalStatistics()
        {
            Console.Clear();
            var floorValue = _cliHelper.GetValidDateTime("start-date");
            var ceilingValue = _cliHelper.GetValidDateTime("end-date");

            var dataSets = _readingsWebApiClient.GetCriticalStatisticsAsync(floorValue, ceilingValue).Result;

            if (dataSets != null)
            {
                foreach (var dataSet in dataSets)
                {
                    Console.WriteLine($"Item name: \t\t\t{dataSet.ItemName}");
                    Console.WriteLine($"Anomaly statistics count: \t{dataSet.CriticalReadingsCount}\n");
                }
            }
        }
    }
}