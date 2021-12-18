using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Models;

namespace TicketManagement.DataAccess.Repositories
{
    /// <summary>
    /// Repository for venue.
    /// </summary>
    internal class VenueRepository : IRepository<Venue>
    {
        public IEnumerable<Venue> GetAll()
        {
            using SqlConnection connection = new SqlConnection(DbConnection.GetStringConnection());
            connection.Open();
            IList<Venue> venues = new List<Venue>();
            string sql = "Select Id, Description, Address, Phone, Name from venue";
            SqlCommand cmd = new SqlCommand(sql, connection);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
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
            return venues.AsEnumerable();
        }

        public Venue GetById(int id)
        {
            using SqlConnection connection = new SqlConnection(DbConnection.GetStringConnection());
            connection.Open();
            string sql = "Select Id, Description, Address, Phone, Name from venue where Id = @id";
            SqlCommand cmd = new SqlCommand(sql, connection);
            SqlParameter idParam = new SqlParameter("@id", id);
            cmd.Parameters.Add(idParam);
            SqlDataReader reader = cmd.ExecuteReader();
            Venue venue = new Venue();
            if (reader.HasRows)
            {
                while (reader.Read())
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

        public Venue Create(Venue obj)
        {
            using SqlConnection connection = new SqlConnection(DbConnection.GetStringConnection());
            connection.Open();
            string sql = "Insert into venue (Description, Address, Phone, Name) values (@description, @address, @phone, @name)";
            SqlCommand command = new SqlCommand(sql, connection);
            SqlParameter descriptionParam = new SqlParameter("@venueId", obj.Description);
            SqlParameter addressParam = new SqlParameter("@address", obj.Address);
            SqlParameter phoneParam = new SqlParameter("@phone", obj.Phone);
            SqlParameter nameParam = new SqlParameter("@name", obj.Name);
            command.Parameters.Add(descriptionParam);
            command.Parameters.Add(addressParam);
            command.Parameters.Add(phoneParam);
            command.Parameters.Add(nameParam);
            command.ExecuteNonQuery();
            return obj;
        }

        public Venue Update(Venue obj)
        {
            using SqlConnection connection = new SqlConnection(DbConnection.GetStringConnection());
            connection.Open();
            string sql = "Update venue set Description = @description, Address = @address, Phone = @phone, Name = @name where Id = @id";
            SqlCommand command = new SqlCommand(sql, connection);
            SqlParameter idParam = new SqlParameter("@id", obj.Id);
            SqlParameter descriptionParam = new SqlParameter("@venueId", obj.Description);
            SqlParameter addressParam = new SqlParameter("@address", obj.Address);
            SqlParameter phoneParam = new SqlParameter("@phone", obj.Phone);
            SqlParameter nameParam = new SqlParameter("@name", obj.Name);
            command.Parameters.Add(idParam);
            command.Parameters.Add(descriptionParam);
            command.Parameters.Add(addressParam);
            command.Parameters.Add(phoneParam);
            command.Parameters.Add(nameParam);
            command.ExecuteNonQuery();
            return obj;
        }

        public Venue Delete(Venue obj)
        {
            using SqlConnection connection = new SqlConnection(DbConnection.GetStringConnection());
            connection.Open();
            string sql = "Delete from venue where Id = @id";
            SqlCommand command = new SqlCommand(sql, connection);
            SqlParameter idParam = new SqlParameter("@id", obj.Id);
            command.Parameters.Add(idParam);
            command.ExecuteNonQuery();
            return obj;
        }
    }
}
