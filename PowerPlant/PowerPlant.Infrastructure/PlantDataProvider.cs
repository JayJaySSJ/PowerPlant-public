using PowerPlant.Domain.Interfaces;
using PowerPlant.Domain.Models;
using PowerPlantDataProvider.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PowerPlant.Infrastructure
{
    public class PlantDataProvider : IPlantDataProvider
    {
        public event EventHandler<NewDataSet> OnPlantDataArrival = null;

        private static List<string> _itemNames;

        public PlantDataProvider()
        {
            PowerPlantDataProvider.PowerPlant.Instance.OnNewDataSetArrival += ReceiveDataSet;
        }

        private void ReceiveDataSet(object sender, object dataSet)
        {
            var originalData = (PowerPlantDataSet)dataSet;

            var plantData = new NewDataSet
            {
                PlantName = originalData.PlantName,
                CauldronsData = originalData.Cauldrons
                    .Select(c => new CauldronData
                    {
                        Name = c.Name,
                        WaterPressure = new AssetParameterData
                        {
                            MinValue = c.WaterPressure.MinValue,
                            MaxValue = c.WaterPressure.MaxValue,
                            TypicalValue = c.WaterPressure.TypicalValue,
                            CurrentValue = c.WaterPressure.CurrentValue,
                            Unit = c.WaterPressure.Unit
                        },
                        WaterTemperature = new AssetParameterData
                        {
                            MinValue = c.WaterTemperature.MinValue,
                            MaxValue = c.WaterTemperature.MaxValue,
                            TypicalValue = c.WaterTemperature.TypicalValue,
                            CurrentValue = c.WaterTemperature.CurrentValue,
                            Unit = c.WaterTemperature.Unit
                        },
                        CamberTemperature = new AssetParameterData
                        {
                            MinValue = c.CamberTemperature.MinValue,
                            MaxValue = c.CamberTemperature.MaxValue,
                            TypicalValue = c.CamberTemperature.TypicalValue,
                            CurrentValue = c.CamberTemperature.CurrentValue,
                            Unit = c.CamberTemperature.Unit
                        }
                    })
                    .ToArray(),
                TurbinesData = originalData.Turbines
                    .Select(tu => new TurbineData
                    {
                        Name = tu.Name,
                        OverheaterSteamTemperature = new AssetParameterData
                        {
                            MinValue = tu.OverheaterSteamTemperature.MinValue,
                            MaxValue = tu.OverheaterSteamTemperature.MaxValue,
                            TypicalValue = tu.OverheaterSteamTemperature.TypicalValue,
                            CurrentValue = tu.OverheaterSteamTemperature.CurrentValue,
                            Unit = tu.OverheaterSteamTemperature.Unit
                        },
                        SteamPressure = new AssetParameterData
                        {
                            MinValue = tu.SteamPressure.MinValue,
                            MaxValue = tu.SteamPressure.MaxValue,
                            TypicalValue = tu.SteamPressure.TypicalValue,
                            CurrentValue = tu.SteamPressure.CurrentValue,
                            Unit = tu.SteamPressure.Unit
                        },
                        RotationSpeed = new AssetParameterData
                        {
                            MinValue = tu.RotationSpeed.MinValue,
                            MaxValue = tu.RotationSpeed.MaxValue,
                            TypicalValue = tu.RotationSpeed.TypicalValue,
                            CurrentValue = tu.RotationSpeed.CurrentValue,
                            Unit = tu.RotationSpeed.Unit
                        },
                        CurrentPower = new AssetParameterData
                        {
                            MinValue = tu.CurrentPower.MinValue,
                            MaxValue = tu.CurrentPower.MaxValue,
                            TypicalValue = tu.CurrentPower.TypicalValue,
                            CurrentValue = tu.CurrentPower.CurrentValue,
                            Unit = tu.CurrentPower.Unit
                        },
                        OutputVoltage = new AssetParameterData
                        {
                            MinValue = tu.OutputVoltage.MinValue,
                            MaxValue = tu.OutputVoltage.MaxValue,
                            TypicalValue = tu.OutputVoltage.TypicalValue,
                            CurrentValue = tu.OutputVoltage.CurrentValue,
                            Unit = tu.OutputVoltage.Unit
                        }
                    })
                    .ToArray(),
                TransformatorsData = originalData.Transformators
                    .Select(tr => new TransformatorData
                    {
                        Name = tr.Name,
                        InputVoltage = new AssetParameterData
                        {
                            MinValue = tr.InputVoltage.MinValue,
                            MaxValue = tr.InputVoltage.MaxValue,
                            TypicalValue = tr.InputVoltage.TypicalValue,
                            CurrentValue = tr.InputVoltage.CurrentValue,
                            Unit = tr.InputVoltage.Unit
                        },
                        OutputVoltage = new AssetParameterData
                        {
                            MinValue = tr.OutputVoltage.MinValue,
                            MaxValue = tr.OutputVoltage.MaxValue,
                            TypicalValue = tr.OutputVoltage.TypicalValue,
                            CurrentValue = tr.OutputVoltage.CurrentValue,
                            Unit = tr.OutputVoltage.Unit
                        }
                    })
                    .ToArray()
            };

            OnPlantDataArrival?.Invoke(this, plantData);

            _itemNames = CreateItemNames(plantData);
        }

        private List<string> CreateItemNames(NewDataSet newDataSet)
        {
            var names = new List<string>();

            foreach (var cauldron in newDataSet.CauldronsData)
            {
                names.Add(cauldron.Name);
            }
            foreach (var turbine in newDataSet.TurbinesData)
            {
                names.Add(turbine.Name);
            }
            foreach (var transformator in newDataSet.TransformatorsData)
            {
                names.Add(transformator.Name);
            }

            return names;
        }

        public List<string> GetItemNames()
        {
            return _itemNames;
        }
    }
}