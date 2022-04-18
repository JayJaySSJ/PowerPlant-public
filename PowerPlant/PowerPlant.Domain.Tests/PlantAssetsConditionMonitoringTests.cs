using Moq;
using NUnit.Framework;
using PowerPlant.Domain.Interfaces;
using PowerPlant.Domain.Models;
using System;
using System.Collections.Generic;

namespace PowerPlant.Domain.Tests
{
    [TestFixture]
    public class PlantAssetsConditionMonitoringTests
    {
        private Mock<IReadingsRepository> _readingsRepositoryMock;
        private Mock<IPlantDataProvider> _plantDataProviderMock;
        private Mock<IDateProvider> _dateProviderMock;
        private Mock<IMembersService> _membersServiceMock;

        private PlantAssetsConditionMonitoring _sut;

        private readonly DateTime _now = new DateTime(1993, 07, 15, 07, 00, 00);
        private NewDataSet _testDataSet;
        private List<CriticalReading> _criticalReading;

        [SetUp]
        public void Setup()
        {
            _readingsRepositoryMock = new Mock<IReadingsRepository>();
            _plantDataProviderMock = new Mock<IPlantDataProvider>();
            _dateProviderMock = new Mock<IDateProvider>();
            _membersServiceMock = new Mock<IMembersService>();

            _sut = new PlantAssetsConditionMonitoring(
            _readingsRepositoryMock.Object,
                _plantDataProviderMock.Object,
                _dateProviderMock.Object,
                _membersServiceMock.Object);

            _testDataSet = new NewDataSet()
            {
                PlantName = "PowerPlant 1",
                CauldronsData = new[]
                {
                    new CauldronData
                    {
                        Name = "Cauldron_0",
                        WaterPressure = new AssetParameterData
                        {
                            MinValue = 10.2,
                            MaxValue = 13.8,
                            CurrentValue = 12
                        },
                        WaterTemperature = new AssetParameterData
                        {
                            MinValue = 297.5,
                            MaxValue = 402.2,
                            CurrentValue = 350
                        },
                        CamberTemperature = new AssetParameterData
                        {
                            MinValue = 340,
                            MaxValue = 460,
                            CurrentValue = 400
                        }
                    },

                    new CauldronData
                    {
                        Name = "Cauldron_1",
                        WaterPressure = new AssetParameterData
                        {
                            MinValue = 10.2,
                            MaxValue = 13.8,
                            CurrentValue = 12
                        },
                        WaterTemperature = new AssetParameterData
                        {
                            MinValue = 297.5,
                            MaxValue = 402.2,
                            CurrentValue = 350
                        },
                        CamberTemperature = new AssetParameterData
                        {
                            MinValue = 340,
                            MaxValue = 460,
                            CurrentValue = 400
                        }
                    }
                },
                TurbinesData = new[]
                {
                    new TurbineData
                    {
                        Name = "Turbine_0",
                        SteamPressure = new AssetParameterData
                        {
                            MinValue = 76.5,
                            MaxValue = 103.5,
                            CurrentValue = 90
                        },
                        OverheaterSteamTemperature = new AssetParameterData
                        {
                            MinValue = 467.5,
                            MaxValue = 632.5,
                            CurrentValue = 550
                        },
                        OutputVoltage = new AssetParameterData
                        {
                            MinValue = 11900,
                            MaxValue = 16100,
                            CurrentValue = 14000
                        },
                        RotationSpeed = new AssetParameterData
                        {
                            MinValue = 2550,
                            MaxValue = 3450,
                            CurrentValue = 3000
                        },
                        CurrentPower = new AssetParameterData
                        {
                            MinValue = 85,
                            MaxValue = 115,
                            CurrentValue = 100
                        }
                    },
                    new TurbineData
                    {
                        Name = "Turbine_1",
                        SteamPressure = new AssetParameterData
                        {
                            MinValue = 76.5,
                            MaxValue = 103.5,
                            CurrentValue = 90
                        },
                        OverheaterSteamTemperature = new AssetParameterData
                        {
                            MinValue = 467.5,
                            MaxValue = 632.5,
                            CurrentValue = 550
                        },
                        OutputVoltage = new AssetParameterData
                        {
                            MinValue = 11900,
                            MaxValue = 16100,
                            CurrentValue = 14000
                        },
                        RotationSpeed = new AssetParameterData
                        {
                            MinValue = 2550,
                            MaxValue = 3450,
                            CurrentValue = 3000
                        },
                        CurrentPower = new AssetParameterData
                        {
                            MinValue = 85,
                            MaxValue = 115,
                            CurrentValue = 100
                        }
                    }
                },
                TransformatorsData = new[]
                {
                    new TransformatorData
                    {
                        Name = "Transformator_0",
                        InputVoltage = new AssetParameterData
                        {
                            MinValue = 11900,
                            MaxValue = 16100,
                            CurrentValue = 14000
                        },
                        OutputVoltage = new AssetParameterData
                        {
                            MinValue = 93500,
                            MaxValue = 126500,
                            CurrentValue = 110000
                        }
                    },
                    new TransformatorData
                    {
                        Name = "Transformator_1",
                        InputVoltage = new AssetParameterData
                        {
                            MinValue = 11900,
                            MaxValue = 16100,
                            CurrentValue = 14000
                        },
                        OutputVoltage = new AssetParameterData
                        {
                            MinValue = 93500,
                            MaxValue = 126500,
                            CurrentValue = 110000
                        }
                    }
                }
            };

            _criticalReading = new List<CriticalReading>();
            _readingsRepositoryMock
                .Setup(x => x.SaveReading(Capture.In(_criticalReading)));

            _dateProviderMock
                .Setup(x => x.Now)
                .Returns(_now);

            _membersServiceMock
                .Setup(x => x.GetLoggedMember())
                .Returns("admin");
        }

        [Test]
        public void MonitorReadings_CauldronsDataIsNull_ReadingsDidntGetThroughMonitoring()
        {
            //Arrange
            _testDataSet.CauldronsData = null;

            //Act
            _sut.MonitorReadings(this, _testDataSet);

            //Assert
            _readingsRepositoryMock
                .Verify(x => x.SaveReading(It.IsAny<CriticalReading>()), Times.Never);
        }

        [Test]
        public void MonitorReadings_AllParametersOnTypicalValues_NothingSavedIntoDb()
        {
            //Arrange

            //Act
            _sut.MonitorReadings(this, _testDataSet);

            //Assert
            _readingsRepositoryMock
                .Verify(x => x.SaveReading(It.IsAny<CriticalReading>()), Times.Never);
        }

        [Test]
        public void MonitorReadings_Transformator1InputVoltageOverMaxValue_ReadingsPassedThroughFilteringCorrectly()
        {
            //Arrange
            var transformator1Data = new TransformatorData
            {
                Name = "Transformator_1",
                InputVoltage = new AssetParameterData
                {
                    MinValue = 11900,
                    MaxValue = 16100,
                    CurrentValue = 19169
                },
                OutputVoltage = new AssetParameterData
                {
                    MinValue = 93500,
                    MaxValue = 126500,
                    CurrentValue = 110000
                }
            };
            _testDataSet.TransformatorsData.SetValue(transformator1Data, 1);

            //Act
            _sut.MonitorReadings(this, _testDataSet);

            //Assert
            _readingsRepositoryMock
                .Verify(x => x.SaveReading(It.IsAny<CriticalReading>()), Times.Once);

            Assert.AreEqual("admin", _criticalReading[0].LoggedMember);
            Assert.AreEqual("Transformator_1", _criticalReading[0].ItemName);
            Assert.AreEqual("InputVoltage", _criticalReading[0].ParameterName);
            Assert.AreEqual(new DateTime(1993, 07, 15, 07, 00, 00), _criticalReading[0].ReadingTime);
        }

        [Test]
        public void MonitorReadings_Turbine0AndTransformator0OutputVoltageBelowMinValue_ReadingsPassedCorrectlyTwice()
        {
            //Arrange
            var turbine0Data = new TurbineData
            {
                Name = "Turbine_0",
                SteamPressure = new AssetParameterData
                {
                    MinValue = 76.5,
                    MaxValue = 103.5,
                    CurrentValue = 90
                },
                OverheaterSteamTemperature = new AssetParameterData
                {
                    MinValue = 467.5,
                    MaxValue = 632.5,
                    CurrentValue = 550
                },
                OutputVoltage = new AssetParameterData
                {
                    MinValue = 11900,
                    MaxValue = 16100,
                    CurrentValue = 10000
                },
                RotationSpeed = new AssetParameterData
                {
                    MinValue = 2550,
                    MaxValue = 3450,
                    CurrentValue = 3000
                },
                CurrentPower = new AssetParameterData
                {
                    MinValue = 85,
                    MaxValue = 115,
                    CurrentValue = 100
                }
            };
            var transformator0Data = new TransformatorData
            {
                Name = "Transformator_0",
                InputVoltage = new AssetParameterData
                {
                    MinValue = 11900,
                    MaxValue = 16100,
                    CurrentValue = 14000
                },
                OutputVoltage = new AssetParameterData
                {
                    MinValue = 93500,
                    MaxValue = 126500,
                    CurrentValue = 80000
                }
            };

            _testDataSet.TurbinesData.SetValue(turbine0Data, 0);
            _testDataSet.TransformatorsData.SetValue(transformator0Data, 0);

            //Act
            _sut.MonitorReadings(this, _testDataSet);

            //Assert
            _readingsRepositoryMock
                .Verify(x => x.SaveReading(It.IsAny<CriticalReading>()), Times.Exactly(2));

            Assert.AreEqual(2, _criticalReading.Count);

            Assert.AreEqual("admin", _criticalReading[0].LoggedMember);
            Assert.AreEqual("Turbine_0", _criticalReading[0].ItemName);
            Assert.AreEqual("OutputVoltage", _criticalReading[0].ParameterName);
            Assert.AreEqual(new DateTime(1993, 07, 15, 07, 00, 00), _criticalReading[0].ReadingTime);

            Assert.AreEqual("admin", _criticalReading[1].LoggedMember);
            Assert.AreEqual("Transformator_0", _criticalReading[1].ItemName);
            Assert.AreEqual("OutputVoltage", _criticalReading[1].ParameterName);
            Assert.AreEqual(new DateTime(1993, 07, 15, 07, 00, 00), _criticalReading[1].ReadingTime);
        }

        [Test]
        public void SaveIfNewAsync_PreviousSavedReadingsDuplication_NothingSavedIntoDb()
        {
            //Arrange
            var dataToSave = new CriticalReading
            {
                ItemName = "Cauldron_1",
                ParameterName = "WaterTemperature"
            };

            //Act
            _sut.SaveIfNewAsync(dataToSave);
            _sut.SaveIfNewAsync(dataToSave);

            //Assert
            _readingsRepositoryMock
                .Verify(x => x.SaveReading(It.IsAny<CriticalReading>()), Times.Once);

            Assert.AreEqual("admin", _criticalReading[0].LoggedMember);
        }

        [Test]
        public void SaveIfNewAsync_CriticalReadingsReceived_ReadingsPushedToBeSavedCorrectly()
        {
            //Arrange
            var dataToSave = new CriticalReading
            {
                ItemName = "Transformator_1",
                ParameterName = "InputVoltage"
            };

            //Act
            _sut.SaveIfNewAsync(dataToSave);

            //Assert
            _readingsRepositoryMock
                .Verify(x => x.SaveReading(It.IsAny<CriticalReading>()), Times.Once);

            Assert.AreEqual("admin", _criticalReading[0].LoggedMember);
        }
    }
}