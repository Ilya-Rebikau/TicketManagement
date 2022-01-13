using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Models;

namespace TicketManagement.DataAccess.Repositories
{
    /// <summary>
    /// Repository for event.
    /// </summary>
    internal class EventRepository : IRepository<Event>
    {
        public async Task<IEnumerable<Event>> GetAllAsync()
        {
            using SqlConnection connection = new SqlConnection(DbConnection.GetStringConnection());
            await connection.OpenAsync();
            IList<Event> events = new List<Event>();
            string sql = "Select Id, Name, Description, LayoutId, TimeStart, TimeEnd from event";
            SqlCommand cmd = new SqlCommand(sql, connection);
            SqlDataReader reader = await cmd.ExecuteReaderAsync();
            if (reader.HasRows)
            {
                while (await reader.ReadAsync())
                {
                    events.Add(new Event
                    {
                        Id = (int)reader["Id"],
                        Name = reader["Name"].ToString(),
                        Description = reader["Description"].ToString(),
                        LayoutId = (int)reader["LayoutId"],
                        TimeStart = DateTime.Parse(reader["TimeStart"].ToString()),
                        TimeEnd = DateTime.Parse(reader["TimeEnd"].ToString()),
                    });
                }
            }

            reader.Close();
            return events;
        }

        public async Task<Event> GetByIdAsync(int id)
        {
            using SqlConnection connection = new SqlConnection(DbConnection.GetStringConnection());
            await connection.OpenAsync();
            string sql = "Select Id, Name, Description, LayoutId, TimeStart, TimeEnd from event where Id = @id";
            SqlCommand cmd = new SqlCommand(sql, connection);
            SqlParameter idParam = new SqlParameter("@id", id);
            cmd.Parameters.Add(idParam);
            SqlDataReader reader = await cmd.ExecuteReaderAsync();
            Event eventModel = new Event();
            if (reader.HasRows)
            {
                while (await reader.ReadAsync())
                {
                    eventModel = new Event
                    {
                        Id = (int)reader["Id"],
                        Name = reader["Name"].ToString(),
                        Description = reader["Description"].ToString(),
                        LayoutId = (int)reader["LayoutId"],
                        TimeStart = DateTime.Parse(reader["TimeStart"].ToString()),
                        TimeEnd = DateTime.Parse(reader["TimeEnd"].ToString()),
                    };
                }
            }

            return eventModel;
        }

        public async Task<Event> CreateAsync(Event obj)
        {
            using SqlConnection connection = new SqlConnection(DbConnection.GetStringConnection());
            await connection.OpenAsync();
            string sql = "sp_CreateEvent";
            SqlCommand command = new SqlCommand(sql, connection);
            command.CommandType = CommandType.StoredProcedure;
            SqlParameter nameParam = new SqlParameter("@name", obj.Name);
            SqlParameter descriptionParam = new SqlParameter("@description", obj.Description);
            SqlParameter layoutIdParam = new SqlParameter("@layoutId", obj.LayoutId);
            SqlParameter timeStartParam = new SqlParameter("@timeStart", obj.TimeStart.ToString());
            SqlParameter timeEndParam = new SqlParameter("@timeEnd", obj.TimeEnd.ToString());
            command.Parameters.Add(nameParam);
            command.Parameters.Add(descriptionParam);
            command.Parameters.Add(layoutIdParam);
            command.Parameters.Add(timeStartParam);
            command.Parameters.Add(timeEndParam);
            await command.ExecuteNonQueryAsync();
            return obj;
        }

        public async Task<Event> UpdateAsync(Event obj)
        {
            using SqlConnection connection = new SqlConnection(DbConnection.GetStringConnection());
            await connection.OpenAsync();
            string sql = "sp_UpdateEvent";
            SqlCommand command = new SqlCommand(sql, connection);
            command.CommandType = CommandType.StoredProcedure;
            SqlParameter idParam = new SqlParameter("@id", obj.Id);
            SqlParameter nameParam = new SqlParameter("@name", obj.Name);
            SqlParameter descriptionParam = new SqlParameter("@description", obj.Description);
            SqlParameter layoutIdParam = new SqlParameter("@layoutId", obj.LayoutId);
            SqlParameter timeStartParam = new SqlParameter("@timeStart", obj.TimeStart.ToString());
            SqlParameter timeEndParam = new SqlParameter("@timeEnd", obj.TimeEnd.ToString());
            command.Parameters.Add(idParam);
            command.Parameters.Add(nameParam);
            command.Parameters.Add(descriptionParam);
            command.Parameters.Add(layoutIdParam);
            command.Parameters.Add(timeStartParam);
            command.Parameters.Add(timeEndParam);
            await command.ExecuteNonQueryAsync();
            return obj;
        }

        public async Task<Event> DeleteAsync(Event obj)
        {
            using SqlConnection connection = new SqlConnection(DbConnection.GetStringConnection());
            await connection.OpenAsync();
            string sql = "sp_DeleteEvent";
            SqlCommand command = new SqlCommand(sql, connection);
            command.CommandType = CommandType.StoredProcedure;
            SqlParameter idParam = new SqlParameter("@id", obj.Id);
            command.Parameters.Add(idParam);
            await command.ExecuteNonQueryAsync();
            return obj;
        }
    }
}
