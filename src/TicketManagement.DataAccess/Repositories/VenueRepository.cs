using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Models;

namespace TicketManagement.DataAccess.Repositories
{
    /// <summary>
    /// Repository for venue.
    /// </summary>
    internal class VenueRepository : IRepository<Venue>
    {
        public async Task<IEnumerable<Venue>> GetAllAsync()
        {
            using SqlConnection connection = new SqlConnection(DbConnection.GetStringConnection());
            await connection.OpenAsync();
            IList<Venue> venues = new List<Venue>();
            string sql = "Select Id, Description, Address, Phone, Name from venue";
            SqlCommand cmd = new SqlCommand(sql, connection);
            SqlDataReader reader = await cmd.ExecuteReaderAsync();
            if (reader.HasRows)
            {
                while (await reader.ReadAsync())
                {
                    venues.Add(new Venue
                    {
                        Id = (int)reader["Id"],
                        Description = reader["Description"].ToString(),
                        Address = reader["Address"].ToString(),
                        Phone = reader["Phone"].ToString(),
                        Name = reader["Name"].ToString(),
                    });
                }
            }

            reader.Close();
            return venues;
        }

        public async Task<Venue> GetByIdAsync(int id)
        {
            using SqlConnection connection = new SqlConnection(DbConnection.GetStringConnection());
            await connection.OpenAsync();
            string sql = "Select Id, Description, Address, Phone, Name from venue where Id = @id";
            SqlCommand cmd = new SqlCommand(sql, connection);
            SqlParameter idParam = new SqlParameter("@id", id);
            cmd.Parameters.Add(idParam);
            SqlDataReader reader = await cmd.ExecuteReaderAsync();
            Venue venue = new Venue();
            if (reader.HasRows)
            {
                while (await reader.ReadAsync())
                {
                    venue = new Venue
                    {
                        Id = (int)reader["Id"],
                        Description = reader["Description"].ToString(),
                        Address = reader["Address"].ToString(),
                        Phone = reader["Phone"].ToString(),
                        Name = reader["Name"].ToString(),
                    };
                }
            }

            return venue;
        }

        public async Task<Venue> CreateAsync(Venue obj)
        {
            using SqlConnection connection = new SqlConnection(DbConnection.GetStringConnection());
            await connection.OpenAsync();
            string sql = "Insert into venue (Description, Address, Phone, Name) values (@description, @address, @phone, @name)";
            SqlCommand command = new SqlCommand(sql, connection);
            SqlParameter descriptionParam = new SqlParameter("@description", obj.Description);
            SqlParameter addressParam = new SqlParameter("@address", obj.Address);
            SqlParameter phoneParam = new SqlParameter("@phone", obj.Phone);
            SqlParameter nameParam = new SqlParameter("@name", obj.Name);
            command.Parameters.Add(descriptionParam);
            command.Parameters.Add(addressParam);
            command.Parameters.Add(phoneParam);
            command.Parameters.Add(nameParam);
            await command.ExecuteNonQueryAsync();
            return obj;
        }

        public async Task<Venue> UpdateAsync(Venue obj)
        {
            using SqlConnection connection = new SqlConnection(DbConnection.GetStringConnection());
            await connection.OpenAsync();
            string sql = "Update venue set Description = @description, Address = @address, Phone = @phone, Name = @name where Id = @id";
            SqlCommand command = new SqlCommand(sql, connection);
            SqlParameter idParam = new SqlParameter("@id", obj.Id);
            SqlParameter descriptionParam = new SqlParameter("@description", obj.Description);
            SqlParameter addressParam = new SqlParameter("@address", obj.Address);
            SqlParameter phoneParam = new SqlParameter("@phone", obj.Phone);
            SqlParameter nameParam = new SqlParameter("@name", obj.Name);
            command.Parameters.Add(idParam);
            command.Parameters.Add(descriptionParam);
            command.Parameters.Add(addressParam);
            command.Parameters.Add(phoneParam);
            command.Parameters.Add(nameParam);
            await command.ExecuteNonQueryAsync();
            return obj;
        }

        public async Task<Venue> DeleteAsync(Venue obj)
        {
            using SqlConnection connection = new SqlConnection(DbConnection.GetStringConnection());
            await connection.OpenAsync();
            string sql = "Delete from venue where Id = @id";
            SqlCommand command = new SqlCommand(sql, connection);
            SqlParameter idParam = new SqlParameter("@id", obj.Id);
            command.Parameters.Add(idParam);
            await command.ExecuteNonQueryAsync();
            return obj;
        }
    }
}
