using PowerPlant.Domain.Interfaces;
using PowerPlant.Domain.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace PowerPlant.Infrastructure
{
    public class ReadingsRepository : IReadingsRepository
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["PowerPlantDbConnectionString"].ConnectionString;

        public void SaveReading(CriticalReading dataToSave)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    var saveReadingCommandText = @"
                        INSERT INTO [CriticalReadings] 
                        ([PlantName], [ItemName], [ParameterName], [ReadingTime], [LoggedUserLogin], [MinValue], [MaxValue])
                        VALUES
                        (@PlantName, @ItemName, @ParameterName, @ReadingTime, @LoggedUserLogin, @MinValue, @MaxValue);";

                    var saveReadingCommandSql = new SqlCommand(saveReadingCommandText, connection);
                    saveReadingCommandSql.Parameters.Add("@PlantName", SqlDbType.VarChar, 255).Value = dataToSave.PlantName;
                    saveReadingCommandSql.Parameters.Add("@ItemName", SqlDbType.VarChar, 255).Value = dataToSave.ItemName;
                    saveReadingCommandSql.Parameters.Add("@ParameterName", SqlDbType.VarChar, 255).Value = dataToSave.ParameterName;
                    saveReadingCommandSql.Parameters.Add("@ReadingTime", SqlDbType.DateTime2).Value = dataToSave.ReadingTime;
                    saveReadingCommandSql.Parameters.Add("@LoggedUserLogin", SqlDbType.VarChar, 255).Value = dataToSave.LoggedMember;
                    saveReadingCommandSql.Parameters.Add("@MinValue", SqlDbType.Float, 8).Value = dataToSave.MinValue;
                    saveReadingCommandSql.Parameters.Add("@MaxValue", SqlDbType.Float, 8).Value = dataToSave.MaxValue;
                    saveReadingCommandSql.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task<List<CriticalReading>> GetCriticalReadingsAsync(DateTime floorValue, DateTime ceilingValue)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    string getReadingsCommandText = @"
                        SELECT * FROM [CriticalReadings]
                        WHERE
                        ReadingTime > @floorValue
                        AND
                        ReadingTime < @ceilingValue
                        ;";

                    var getReadingsCommandSql = new SqlCommand(getReadingsCommandText, connection);
                    getReadingsCommandSql.Parameters.Add("@floorValue", SqlDbType.DateTime2).Value = floorValue;
                    getReadingsCommandSql.Parameters.Add("@ceilingValue", SqlDbType.DateTime2).Value = ceilingValue;

                    var reader = await getReadingsCommandSql.ExecuteReaderAsync();

                    var dataSetToSerialize = new List<CriticalReading>();

                    while (await reader.ReadAsync())
                    {
                        var dataToSave = new CriticalReading()
                        {
                            PlantName = reader["PlantName"].ToString(),
                            ItemName = reader["ItemName"].ToString(),
                            ParameterName = reader["ParameterName"].ToString(),
                            ReadingTime = DateTime.Parse(reader["ReadingTime"].ToString()),
                            LoggedMember = reader["LoggedUserLogin"].ToString(),
                            MinValue = double.Parse(reader["MinValue"].ToString()),
                            MaxValue = double.Parse(reader["MaxValue"].ToString())
                        };

                        dataSetToSerialize.Add(dataToSave);
                    }

                    return dataSetToSerialize;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<CriticalReading>();
            }
        }

        public async Task<List<CriticalStatistics>> GetCriticalStatisticsAsync(DateTime floorValue, DateTime ceilingValue)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var criticalStatisticsList = new List<CriticalStatistics>();

                    var getCountCommandText = @"
                        SELECT [ItemName],
                        COUNT(420) AS count
                        FROM [CriticalReadings]
                        WHERE 
                        ReadingTime > @floorValue
                        AND
                        ReadingTime < @ceilingValue
                        GROUP BY [ItemName]
                        ;";

                    var getCountCommandSql = new SqlCommand(getCountCommandText, connection);
                    getCountCommandSql.Parameters.Add("@floorValue", SqlDbType.DateTime2).Value = floorValue;
                    getCountCommandSql.Parameters.Add("@ceilingValue", SqlDbType.DateTime2).Value = ceilingValue;

                    var reader = await getCountCommandSql.ExecuteReaderAsync();

                    while (await reader.ReadAsync())
                    {
                        var name = reader["ItemName"].ToString();
                        var count = int.Parse(reader["count"].ToString());

                        criticalStatisticsList.Add(new CriticalStatistics { ItemName = name, CriticalReadingsCount = count });
                    }

                    reader.Close();

                    return criticalStatisticsList;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<CriticalStatistics>();
            }
        }

        public async Task<bool> CheckIfItemUnderMaintenanceAsync(string itemName)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var memberExistsCommandText = $@"
                        SELECT COUNT(420) AS countDb
                        FROM [InspectionTickets]
                        WHERE [ItemName] = @ItemName
                        AND [AssignmentDate] IS NOT NULL;
                        ;";

                    var memberExistsCommandSql = new SqlCommand(memberExistsCommandText, connection);
                    memberExistsCommandSql.Parameters.Add("@ItemName", SqlDbType.VarChar, 255).Value = itemName;

                    var reader = await memberExistsCommandSql.ExecuteReaderAsync();
                    await reader.ReadAsync();

                    if (int.Parse(reader["countDb"].ToString()) > 0)
                    {
                        return true;
                    }

                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}