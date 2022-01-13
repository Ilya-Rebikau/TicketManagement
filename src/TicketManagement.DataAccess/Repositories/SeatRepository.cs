using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Models;

namespace TicketManagement.DataAccess.Repositories
{
    /// <summary>
    /// Repository for seat.
    /// </summary>
    internal class SeatRepository : IRepository<Seat>
    {
        public async Task<IEnumerable<Seat>> GetAllAsync()
        {
            using SqlConnection connection = new SqlConnection(DbConnection.GetStringConnection());
            await connection.OpenAsync();
            IList<Seat> seats = new List<Seat>();
            string sql = "Select Id, AreaId, Row, Number from seat";
            SqlCommand cmd = new SqlCommand(sql, connection);
            SqlDataReader reader = await cmd.ExecuteReaderAsync();
            if (reader.HasRows)
            {
                while (await reader.ReadAsync())
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

        public async Task<Seat> GetByIdAsync(int id)
        {
            using SqlConnection connection = new SqlConnection(DbConnection.GetStringConnection());
            await connection.OpenAsync();
            string sql = "Select Id, AreaId, Row, Number from seat where Id = @id";
            SqlCommand cmd = new SqlCommand(sql, connection);
            SqlParameter idParam = new SqlParameter("@id", id);
            cmd.Parameters.Add(idParam);
            SqlDataReader reader = await cmd.ExecuteReaderAsync();
            Seat seat = new Seat();
            if (reader.HasRows)
            {
                while (await reader.ReadAsync())
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

        public async Task<Seat> CreateAsync(Seat obj)
        {
            using SqlConnection connection = new SqlConnection(DbConnection.GetStringConnection());
            await connection.OpenAsync();
            string sql = "Insert into seat (AreaId, Row, Number) values (@areaId, @row, @number)";
            SqlCommand command = new SqlCommand(sql, connection);
            SqlParameter areaIdParam = new SqlParameter("@areaId", obj.AreaId);
            SqlParameter rowParam = new SqlParameter("@row", obj.Row);
            SqlParameter numberParam = new SqlParameter("@number", obj.Number);
            command.Parameters.Add(areaIdParam);
            command.Parameters.Add(rowParam);
            command.Parameters.Add(numberParam);
            await command.ExecuteNonQueryAsync();
            return obj;
        }

        public async Task<Seat> UpdateAsync(Seat obj)
        {
            using SqlConnection connection = new SqlConnection(DbConnection.GetStringConnection());
            await connection.OpenAsync();
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
            await command.ExecuteNonQueryAsync();
            return obj;
        }

        public async Task<Seat> DeleteAsync(Seat obj)
        {
            using SqlConnection connection = new SqlConnection(DbConnection.GetStringConnection());
            await connection.OpenAsync();
            string sql = "Delete from seat where Id = @id";
            SqlCommand command = new SqlCommand(sql, connection);
            SqlParameter idParam = new SqlParameter("@id", obj.Id);
            command.Parameters.Add(idParam);
            await command.ExecuteNonQueryAsync();
            return obj;
        }
    }
}
