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
    public class InspectionsRepository : IInspectionsRepository
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["PowerPlantDbConnectionString"].ConnectionString;

        public async Task<bool> CreateInspectionTicket(InspectionTicket inspectionTicket)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    var createTicketCommandText = $@"
                        INSERT INTO [InspectionTickets]
                        ([CreationDate], [ItemName], [Status])
                        VALUES (@CreationDate, @ItemName, @Status)
                        ;";

                    var createTicketCommandSql = new SqlCommand(createTicketCommandText, connection);
                    createTicketCommandSql.Parameters.Add("@CreationDate", SqlDbType.DateTime2).Value = inspectionTicket.CreationDate;
                    createTicketCommandSql.Parameters.Add("@ItemName", SqlDbType.VarChar, 255).Value = inspectionTicket.ItemName;
                    createTicketCommandSql.Parameters.Add("@Status", SqlDbType.Int).Value = inspectionTicket.Status;

                    await createTicketCommandSql.ExecuteNonQueryAsync();

                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<List<InspectionTicket>> GetAllTickets()
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    var getAllCommandText = "SELECT * FROM [InspectionTickets];";

                    var getAllCommandSql = new SqlCommand(getAllCommandText, connection);

                    var reader = await getAllCommandSql.ExecuteReaderAsync();

                    var tickets = new List<InspectionTicket>();

                    while (await reader.ReadAsync())
                    {
                        tickets.Add(new InspectionTicket
                        {
                            Id = int.Parse(reader["Id"].ToString()),
                            CreationDate = DateTime.Parse(reader["CreationDate"].ToString()),
                            AssignmentDate = reader["AssignmentDate"] == DBNull.Value
                                ? (DateTime?)null
                                : DateTime.Parse(reader["AssignmentDate"].ToString()),
                            TerminationDate = reader["TerminationDate"] == DBNull.Value
                                ? (DateTime?)null
                                : DateTime.Parse(reader["TerminationDate"].ToString()),
                            ItemName = reader["ItemName"].ToString(),
                            Comment = reader["Comment"] == DBNull.Value
                                ? null
                                : reader["Comment"].ToString(),
                            Status = (Status)int.Parse(reader["Status"].ToString()),
                            Assignment = reader["Assignment"] == DBNull.Value
                                ? (int?)null
                                : int.Parse(reader["Assignment"].ToString())
                        });
                    }

                    return tickets;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<InspectionTicket>();
            }
        }

        public async Task<bool> OpenTicketExistsAsync(string itemName)
        {
            try
            {
                using(var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var memberExistsCommandText = $@"
                        SELECT COUNT (DISTINCT Id) AS countDb
                        FROM [InspectionTickets]
                        WHERE [ItemName] = @itemName
                        AND [TerminationDate] IS NULL
                        ;";

                    var memberExistsCommandSql = new SqlCommand(memberExistsCommandText, connection);
                    memberExistsCommandSql.Parameters.Add("@itemName", SqlDbType.VarChar, 255).Value = itemName;

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

        public async Task<bool> AssignAsync(InspectionTicket pickedTicket)
        {
            try
            {
                using(var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var assignCommandText = @"
                        UPDATE [InspectionTickets]
                        SET
                        [AssignmentDate] = @AssignmentDate,
                        [Comment] = @Comment,
                        [Assignment] = @Assignment,
                        [Status] = @Status
                        WHERE [Id] = @Id;
                        ";

                    var assignCommandSql = new SqlCommand(assignCommandText, connection);
                    assignCommandSql.Parameters.Add("@AssignmentDate", SqlDbType.DateTime2).Value = pickedTicket.AssignmentDate;
                    assignCommandSql.Parameters.Add("@Comment", SqlDbType.VarChar, 255).Value = pickedTicket.Comment;
                    assignCommandSql.Parameters.Add("@Assignment", SqlDbType.Int).Value = pickedTicket.Assignment;
                    assignCommandSql.Parameters.Add("@Status", SqlDbType.Int).Value = pickedTicket.Status;
                    assignCommandSql.Parameters.Add("@Id", SqlDbType.Int).Value = pickedTicket.Id;

                    await assignCommandSql.ExecuteNonQueryAsync();

                    return true;
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