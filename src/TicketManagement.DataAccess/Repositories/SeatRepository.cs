using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Models;

namespace TicketManagement.DataAccess.Repositories
{
    /// <summary>
    /// Repository for seat.
    /// </summary>
    internal class SeatRepository : IRepository<Seat>
    {
        public IEnumerable<Seat> GetAll()
        {
            using SqlConnection connection = new SqlConnection(DbConnection.GetStringConnection());
            connection.Open();
            IList<Seat> seats = new List<Seat>();
            string sql = "Select Id, AreaId, Row, Number from seat";
            SqlCommand cmd = new SqlCommand(sql, connection);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    seats.Add(new Seat
                    {
                        Id = (int)reader["Id"],
                        AreaId = (int)reader["AreaId"],
                        Row = (int)reader["Row"],
                        Number = (int)reader["Number"],
                    });
                }
            }

            reader.Close();
            return seats;
        }

        public Seat GetById(int id)
        {
            using SqlConnection connection = new SqlConnection(DbConnection.GetStringConnection());
            connection.Open();
            string sql = "Select Id, AreaId, Row, Number from seat where Id = @id";
            SqlCommand cmd = new SqlCommand(sql, connection);
            SqlParameter idParam = new SqlParameter("@id", id);
            cmd.Parameters.Add(idParam);
            SqlDataReader reader = cmd.ExecuteReader();
            Seat seat = new Seat();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    seat = new Seat
                    {
                        Id = (int)reader["Id"],
                        AreaId = (int)reader["AreaId"],
                        Row = (int)reader["Row"],
                        Number = (int)reader["Number"],
                    };
                }
            }

            return seat;
        }

        public Seat Create(Seat obj)
        {
            using SqlConnection connection = new SqlConnection(DbConnection.GetStringConnection());
            connection.Open();
            string sql = "Insert into seat (AreaId, Row, Number) values (@areaId, @row, @number)";
            SqlCommand command = new SqlCommand(sql, connection);
            SqlParameter areaIdParam = new SqlParameter("@areaId", obj.AreaId);
            SqlParameter rowParam = new SqlParameter("@row", obj.Row);
            SqlParameter numberParam = new SqlParameter("@number", obj.Number);
            command.Parameters.Add(areaIdParam);
            command.Parameters.Add(rowParam);
            command.Parameters.Add(numberParam);
            command.ExecuteNonQuery();
            return obj;
        }

        public Seat Update(Seat obj)
        {
            using SqlConnection connection = new SqlConnection(DbConnection.GetStringConnection());
            connection.Open();
            string sql = "Update seat set AreaId = @areaId, Row = @row, Number = @number where Id = @id";
            SqlCommand command = new SqlCommand(sql, connection);
            SqlParameter idParam = new SqlParameter("@id", obj.Id);
            SqlParameter areaIdParam = new SqlParameter("@areaId", obj.AreaId);
            SqlParameter rowParam = new SqlParameter("@row", obj.Row);
            SqlParameter numberParam = new SqlParameter("@number", obj.Number);
            command.Parameters.Add(idParam);
            command.Parameters.Add(areaIdParam);
            command.Parameters.Add(rowParam);
            command.Parameters.Add(numberParam);
            command.ExecuteNonQuery();
            return obj;
        }

        public Seat Delete(Seat obj)
        {
            using SqlConnection connection = new SqlConnection(DbConnection.GetStringConnection());
            connection.Open();
            string sql = "Delete from seat where Id = @id";
            SqlCommand command = new SqlCommand(sql, connection);
            SqlParameter idParam = new SqlParameter("@id", obj.Id);
            command.Parameters.Add(idParam);
            command.ExecuteNonQuery();
            return obj;
        }
    }
}
