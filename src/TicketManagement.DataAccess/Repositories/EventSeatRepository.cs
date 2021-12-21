using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Models;

namespace TicketManagement.DataAccess.Repositories
{
    /// <summary>
    /// Repository for event seat.
    /// </summary>
    internal class EventSeatRepository : IRepository<EventSeat>
    {
        public IEnumerable<EventSeat> GetAll()
        {
            using SqlConnection connection = new SqlConnection(DbConnection.GetStringConnection());
            connection.Open();
            IList<EventSeat> eventSeats = new List<EventSeat>();
            string sql = "Select Id, EventAreaId, Row, Number, State from eventseat";
            SqlCommand cmd = new SqlCommand(sql, connection);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
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

        public EventSeat GetById(int id)
        {
            using SqlConnection connection = new SqlConnection(DbConnection.GetStringConnection());
            connection.Open();
            string sql = "Select Id, EventAreaId, Row, Number, State from eventseat where Id = @id";
            SqlCommand cmd = new SqlCommand(sql, connection);
            SqlParameter idParam = new SqlParameter("@id", id);
            cmd.Parameters.Add(idParam);
            SqlDataReader reader = cmd.ExecuteReader();
            EventSeat eventSeat = new EventSeat();
            if (reader.HasRows)
            {
                while (reader.Read())
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

        public EventSeat Create(EventSeat obj)
        {
            using SqlConnection connection = new SqlConnection(DbConnection.GetStringConnection());
            connection.Open();
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
            command.ExecuteNonQuery();
            return obj;
        }

        public EventSeat Update(EventSeat obj)
        {
            using SqlConnection connection = new SqlConnection(DbConnection.GetStringConnection());
            connection.Open();
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
            command.ExecuteNonQuery();
            return obj;
        }

        public EventSeat Delete(EventSeat obj)
        {
            using SqlConnection connection = new SqlConnection(DbConnection.GetStringConnection());
            connection.Open();
            string sql = "Delete from eventseat where Id = @id";
            SqlCommand command = new SqlCommand(sql, connection);
            SqlParameter idParam = new SqlParameter("@id", obj.Id);
            command.Parameters.Add(idParam);
            command.ExecuteNonQuery();
            return obj;
        }
    }
}
