using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Models;

namespace TicketManagement.DataAccess.Repositories
{
    /// <summary>
    /// Repository for event seat.
    /// </summary>
    internal class EventSeatRepository : IRepository<EventSeat>
    {
        public async Task<IEnumerable<EventSeat>> GetAllAsync()
        {
            using SqlConnection connection = new SqlConnection(DbConnection.GetStringConnection());
            await connection.OpenAsync();
            IList<EventSeat> eventSeats = new List<EventSeat>();
            string sql = "Select Id, EventAreaId, Row, Number, State from eventseat";
            SqlCommand cmd = new SqlCommand(sql, connection);
            SqlDataReader reader = await cmd.ExecuteReaderAsync();
            if (reader.HasRows)
            {
                while (await reader.ReadAsync())
                {
                    eventSeats.Add(new EventSeat
                    {
                        Id = (int)reader["Id"],
                        EventAreaId = (int)reader["EventAreaId"],
                        Row = (int)reader["Row"],
                        Number = (int)reader["Number"],
                        State = (int)reader["State"],
                    });
                }
            }

            reader.Close();
            return eventSeats;
        }

        public async Task<EventSeat> GetByIdAsync(int id)
        {
            using SqlConnection connection = new SqlConnection(DbConnection.GetStringConnection());
            await connection.OpenAsync();
            string sql = "Select Id, EventAreaId, Row, Number, State from eventseat where Id = @id";
            SqlCommand cmd = new SqlCommand(sql, connection);
            SqlParameter idParam = new SqlParameter("@id", id);
            cmd.Parameters.Add(idParam);
            SqlDataReader reader = await cmd.ExecuteReaderAsync();
            EventSeat eventSeat = new EventSeat();
            if (reader.HasRows)
            {
                while (await reader.ReadAsync())
                {
                    eventSeat = new EventSeat
                    {
                        Id = (int)reader["Id"],
                        EventAreaId = (int)reader["EventAreaId"],
                        Row = (int)reader["Row"],
                        Number = (int)reader["Number"],
                        State = (int)reader["State"],
                    };
                }
            }

            return eventSeat;
        }

        public async Task<EventSeat> CreateAsync(EventSeat obj)
        {
            using SqlConnection connection = new SqlConnection(DbConnection.GetStringConnection());
            await connection.OpenAsync();
            string sql = "Insert into eventseat (EventAreaId, Row, Number, State) values (@eventAreaId, @row, @number, @state)";
            SqlCommand command = new SqlCommand(sql, connection);
            SqlParameter eventAreaIdParam = new SqlParameter("@eventAreaId", obj.EventAreaId);
            SqlParameter rowParam = new SqlParameter("@row", obj.Row);
            SqlParameter numberParam = new SqlParameter("@number", obj.Number);
            SqlParameter stateParam = new SqlParameter("@state", obj.State);
            command.Parameters.Add(eventAreaIdParam);
            command.Parameters.Add(rowParam);
            command.Parameters.Add(numberParam);
            command.Parameters.Add(stateParam);
            await command.ExecuteNonQueryAsync();
            return obj;
        }

        public async Task<EventSeat> UpdateAsync(EventSeat obj)
        {
            using SqlConnection connection = new SqlConnection(DbConnection.GetStringConnection());
            await connection.OpenAsync();
            string sql = "Update eventseat set EventAreaId = @eventAreaId, Row = @row, Number = @number, State = @state where Id = @id";
            SqlCommand command = new SqlCommand(sql, connection);
            SqlParameter idParam = new SqlParameter("@id", obj.Id);
            SqlParameter eventAreaIdParam = new SqlParameter("@eventAreaId", obj.EventAreaId);
            SqlParameter rowParam = new SqlParameter("@row", obj.Row);
            SqlParameter numberParam = new SqlParameter("@number", obj.Number);
            SqlParameter stateParam = new SqlParameter("@state", obj.State);
            command.Parameters.Add(idParam);
            command.Parameters.Add(eventAreaIdParam);
            command.Parameters.Add(rowParam);
            command.Parameters.Add(numberParam);
            command.Parameters.Add(stateParam);
            await command.ExecuteNonQueryAsync();
            return obj;
        }

        public async Task<EventSeat> DeleteAsync(EventSeat obj)
        {
            using SqlConnection connection = new SqlConnection(DbConnection.GetStringConnection());
            await connection.OpenAsync();
            string sql = "Delete from eventseat where Id = @id";
            SqlCommand command = new SqlCommand(sql, connection);
            SqlParameter idParam = new SqlParameter("@id", obj.Id);
            command.Parameters.Add(idParam);
            await command.ExecuteNonQueryAsync();
            return obj;
        }
    }
}
