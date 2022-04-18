using PowerPlant.Domain.Models;
using System.Collections.Generic;
using System.Linq;
using InspectionTicketWcf = PowerPlant.Wcf.ServiceDefinition.Models.InspectionTicket;
using StatusWcf = PowerPlant.Wcf.ServiceDefinition.Models.Status;
using MemberWcf = PowerPlant.Wcf.ServiceDefinition.Models.Member;
using MemberFunctionWcf = PowerPlant.Wcf.ServiceDefinition.Models.MemberFunction;
using CriticalStatisticsWcf = PowerPlant.Wcf.ServiceDefinition.Models.CriticalStatistics;
using CriticalReadingWcf = PowerPlant.Wcf.ServiceDefinition.Models.CriticalReading;
using NewDataSetWcf = PowerPlant.Wcf.ServiceDefinition.Models.NewDataSet;
using CauldronDataWcf = PowerPlant.Wcf.ServiceDefinition.Models.CauldronData;
using TurbineDataWcf = PowerPlant.Wcf.ServiceDefinition.Models.TurbineData;
using TransformatorDataWcf = PowerPlant.Wcf.ServiceDefinition.Models.TransformatorData;
using AssetParameterDataWcf = PowerPlant.Wcf.ServiceDefinition.Models.AssetParameterData;
using PowerDataSetWcf = PowerPlant.Wcf.ServiceDefinition.Models.PowerDataSet;

namespace PowerPlant.Wcf.SelfhostServer
{
    internal class Mapper
    {
        public InspectionTicket MapTicketToDomain(InspectionTicketWcf inspectionTicketWcf)
        {
            return new InspectionTicket
            {
                AssignmentDate = inspectionTicketWcf.AssignmentDate,
                Comment = inspectionTicketWcf.Comment,
                CreationDate = inspectionTicketWcf.CreationDate,
                Id = inspectionTicketWcf.Id,
                ItemName = inspectionTicketWcf.ItemName,
                TerminationDate = inspectionTicketWcf.TerminationDate,
                Status = (Status)inspectionTicketWcf.Status,
                Assignment = inspectionTicketWcf.Assignment
            };
        }

        public List<InspectionTicketWcf> MapTicketsToWcf(List<InspectionTicket> inspectionTickets)
        {
            return inspectionTickets
                .Select(x => new InspectionTicketWcf
                {
                    AssignmentDate = x.AssignmentDate,
                    Comment = x.Comment,
                    CreationDate = x.CreationDate,
                    Id = x.Id,
                    ItemName = x.ItemName,
                    TerminationDate = x.TerminationDate,
                    Status = (StatusWcf)x.Status,
                    Assignment = x.Assignment
                })
                .ToList();
        }

        public Member MapMemberToDomain(MemberWcf memberWcf)
        {
            return new Member
            {
                Function = (MemberFunction)memberWcf.Function,
                Id = memberWcf.Id,
                Login = memberWcf.Login,
                Password = memberWcf.Password
            };
        }

        public Dictionary<int, MemberWcf> MapMembersToWcf(Dictionary<int, Member> members)
        {
            return members
                .ToDictionary(
                x => x.Key,
                x => MapMemberToWcf(x.Value));
        }

        public MemberWcf MapMemberToWcf(Member member)
        {
            return new MemberWcf
            {
                Function = (MemberFunctionWcf)member.Function,
                Id = member.Id,
                Login = member.Login,
                Password = member.Password
            };
        }

        public List<CriticalStatisticsWcf> MapCriticalStatisticsToWcf(List<CriticalStatistics> criticalStatistics)
        {
            return criticalStatistics
                .Select(x => new CriticalStatisticsWcf
                {
                    CriticalReadingsCount = x.CriticalReadingsCount,
                    ItemName = x.ItemName
                })
                .ToList();
        }

        public List<CriticalReadingWcf> MapCriticalReadingsToWcf(List<CriticalReading> dataToSaves)
        {
            return dataToSaves
                .Select(x => new CriticalReadingWcf
                {
                    ItemName = x.ItemName,
                    LoggedMember = x.LoggedMember,
                    MaxValue = x.MaxValue,
                    MinValue = x.MinValue,
                    ParameterName = x.ParameterName,
                    PlantName = x.PlantName,
                    ReadingTime = x.ReadingTime
                })
                .ToList();
        }

        public NewDataSetWcf MapNewDataSetToWcf(NewDataSet newDataSet)
        {
            return new NewDataSetWcf
            {
                PlantName = newDataSet.PlantName,
                CauldronsData = newDataSet.CauldronsData
                    .Select(c => new CauldronDataWcf
                    {
                        Name = c.Name,
                        WaterPressure = new AssetParameterDataWcf
                        {
                            MinValue = c.WaterPressure.MinValue,
                            MaxValue = c.WaterPressure.MaxValue,
                            TypicalValue = c.WaterPressure.TypicalValue,
                            CurrentValue = c.WaterPressure.CurrentValue,
                            Unit = c.WaterPressure.Unit
                        },
                        WaterTemperature = new AssetParameterDataWcf
                        {
                            MinValue = c.WaterTemperature.MinValue,
                            MaxValue = c.WaterTemperature.MaxValue,
                            TypicalValue = c.WaterTemperature.TypicalValue,
                            CurrentValue = c.WaterTemperature.CurrentValue,
                            Unit = c.WaterTemperature.Unit
                        },
                        CamberTemperature = new AssetParameterDataWcf
                        {
                            MinValue = c.CamberTemperature.MinValue,
                            MaxValue = c.CamberTemperature.MaxValue,
                            TypicalValue = c.CamberTemperature.TypicalValue,
                            CurrentValue = c.CamberTemperature.CurrentValue,
                            Unit = c.CamberTemperature.Unit
                        }
                    })
                    .ToArray(),
                TurbinesData = newDataSet.TurbinesData
                    .Select(tu => new TurbineDataWcf
                    {
                        Name = tu.Name,
                        OverheaterSteamTemperature = new AssetParameterDataWcf
                        {
                            MinValue = tu.OverheaterSteamTemperature.MinValue,
                            MaxValue = tu.OverheaterSteamTemperature.MaxValue,
                            TypicalValue = tu.OverheaterSteamTemperature.TypicalValue,
                            CurrentValue = tu.OverheaterSteamTemperature.CurrentValue,
                            Unit = tu.OverheaterSteamTemperature.Unit
                        },
                        SteamPressure = new AssetParameterDataWcf
                        {
                            MinValue = tu.SteamPressure.MinValue,
                            MaxValue = tu.SteamPressure.MaxValue,
                            TypicalValue = tu.SteamPressure.TypicalValue,
                            CurrentValue = tu.SteamPressure.CurrentValue,
                            Unit = tu.SteamPressure.Unit
                        },
                        RotationSpeed = new AssetParameterDataWcf
                        {
                            MinValue = tu.RotationSpeed.MinValue,
                            MaxValue = tu.RotationSpeed.MaxValue,
                            TypicalValue = tu.RotationSpeed.TypicalValue,
                            CurrentValue = tu.RotationSpeed.CurrentValue,
                            Unit = tu.RotationSpeed.Unit
                        },
                        CurrentPower = new AssetParameterDataWcf
                        {
                            MinValue = tu.CurrentPower.MinValue,
                            MaxValue = tu.CurrentPower.MaxValue,
                            TypicalValue = tu.CurrentPower.TypicalValue,
                            CurrentValue = tu.CurrentPower.CurrentValue,
                            Unit = tu.CurrentPower.Unit
                        },
                        OutputVoltage = new AssetParameterDataWcf
                        {
                            MinValue = tu.OutputVoltage.MinValue,
                            MaxValue = tu.OutputVoltage.MaxValue,
                            TypicalValue = tu.OutputVoltage.TypicalValue,
                            CurrentValue = tu.OutputVoltage.CurrentValue,
                            Unit = tu.OutputVoltage.Unit
                        }
                    })
                    .ToArray(),
                TransformatorsData = newDataSet.TransformatorsData
                    .Select(tr => new TransformatorDataWcf
                    {
                        Name = tr.Name,
                        InputVoltage = new AssetParameterDataWcf
                        {
                            MinValue = tr.InputVoltage.MinValue,
                            MaxValue = tr.InputVoltage.MaxValue,
                            TypicalValue = tr.InputVoltage.TypicalValue,
                            CurrentValue = tr.InputVoltage.CurrentValue,
                            Unit = tr.InputVoltage.Unit
                        },
                        OutputVoltage = new AssetParameterDataWcf
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
        }

        public PowerDataSetWcf[] MapPowerDataSetsToWcf(PowerDataSet[] powerDataSets)
        {
            return powerDataSets
                .Select(x => new PowerDataSetWcf
                {
                    Name = x.Name,
                    EnergyProduced = x.EnergyProduced,
                    CurrentValue = x.CurrentValue
                })
                .ToArray();
        }
    }
}