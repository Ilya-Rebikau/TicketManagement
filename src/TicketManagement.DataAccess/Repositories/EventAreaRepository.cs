using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Models;

namespace TicketManagement.DataAccess.Repositories
{
    /// <summary>
    /// Repository for event area.
    /// </summary>
    internal class EventAreaRepository : IRepository<EventArea>
    {
        public IEnumerable<EventArea> GetAll()
        {
            using SqlConnection connection = new SqlConnection(DbConnection.GetStringConnection());
            connection.Open();
            IList<EventArea> eventAreas = new List<EventArea>();
            string sql = "Select Id, EventId, Description, CoordX, CoordY, Price from eventarea";
            SqlCommand cmd = new SqlCommand(sql, connection);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    eventAreas.Add(new EventArea
                    {
                        Id = (int)reader["Id"],
                        EventId = (int)reader["EventId"],
                        Description = reader["Description"].ToString(),
                        CoordX = (int)reader["CoordX"],
                        CoordY = (int)reader["CoordY"],
                        Price = double.Parse(reader["Price"].ToString()),
                    });
                }
            }

            reader.Close();
            return eventAreas;
        }

        public EventArea GetById(int id)
        {
            using SqlConnection connection = new SqlConnection(DbConnection.GetStringConnection());
            connection.Open();
            string sql = "Select Id, EventId, Description, CoordX, CoordY, Price from eventarea where Id = @id";
            SqlCommand cmd = new SqlCommand(sql, connection);
            SqlParameter idParam = new SqlParameter("@id", id);
            cmd.Parameters.Add(idParam);
            SqlDataReader reader = cmd.ExecuteReader();
            EventArea eventArea = new EventArea();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    eventArea = new EventArea
                    {
                        Id = (int)reader["Id"],
                        EventId = (int)reader["EventId"],
                        Description = reader["Description"].ToString(),
                        CoordX = (int)reader["CoordX"],
                        CoordY = (int)reader["CoordY"],
                        Price = double.Parse(reader["Price"].ToString()),
                    };
                }
            }

            return eventArea;
        }

        public EventArea Create(EventArea obj)
        {
            using SqlConnection connection = new SqlConnection(DbConnection.GetStringConnection());
            connection.Open();
            string sql = "Insert into eventarea (EventId, Description, CoordX, CoordY, Price) values (@eventId, @description, @coordX, @coordY, @price)";
            SqlCommand command = new SqlCommand(sql, connection);
            SqlParameter eventIdParam = new SqlParameter("@eventId", obj.EventId);
            SqlParameter descriptionParam = new SqlParameter("@description", obj.Description);
            SqlParameter coordXParam = new SqlParameter("@coordX", obj.CoordX);
            SqlParameter coordYParam = new SqlParameter("@coordY", obj.CoordY);
            SqlParameter priceParam = new SqlParameter("@price", obj.Price);
            command.Parameters.Add(eventIdParam);
            command.Parameters.Add(descriptionParam);
            command.Parameters.Add(coordXParam);
            command.Parameters.Add(coordYParam);
            command.Parameters.Add(priceParam);
            command.ExecuteNonQuery();
            return obj;
        }

        public EventArea Update(EventArea obj)
        {
            using SqlConnection connection = new SqlConnection(DbConnection.GetStringConnection());
            connection.Open();
            string sql = "Update eventarea set EventId = @eventId, Description = @description, CoordX = @coordX, CoordY = @coordY, Price = @price where Id = @id";
            SqlCommand command = new SqlCommand(sql, connection);
            SqlParameter idParam = new SqlParameter("@id", obj.Id);
            SqlParameter eventIdParam = new SqlParameter("@eventId", obj.EventId);
            SqlParameter descriptionParam = new SqlParameter("@description", obj.Description);
            SqlParameter coordXParam = new SqlParameter("@coordX", obj.CoordX);
            SqlParameter coordYParam = new SqlParameter("@coordY", obj.CoordY);
            SqlParameter priceParam = new SqlParameter("@price", obj.Price);
            command.Parameters.Add(idParam);
            command.Parameters.Add(eventIdParam);
            command.Parameters.Add(descriptionParam);
            command.Parameters.Add(coordXParam);
            command.Parameters.Add(coordYParam);
            command.Parameters.Add(priceParam);
            command.ExecuteNonQuery();
            return obj;
        }

        public EventArea Delete(EventArea obj)
        {
            using SqlConnection connection = new SqlConnection(DbConnection.GetStringConnection());
            connection.Open();
            string sql = "Delete from eventarea where Id = @id";
            SqlCommand command = new SqlCommand(sql, connection);
            SqlParameter idParam = new SqlParameter("@id", obj.Id);
            command.Parameters.Add(idParam);
            command.ExecuteNonQuery();
            return obj;
        }
    }
}
