﻿using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Models;

namespace TicketManagement.DataAccess.Repositories
{
    /// <summary>
    /// Repository for layout.
    /// </summary>
    internal class LayoutRepository : IRepository<Layout>
    {
        public IEnumerable<Layout> GetAll()
        {
            using SqlConnection connection = new SqlConnection(DbConnection.GetStringConnection());
            connection.Open();
            IList<Layout> layouts = new List<Layout>();
            string sql = "Select Id, VenueId, Description, Name from layout";
            SqlCommand cmd = new SqlCommand(sql, connection);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    layouts.Add(new Layout
                    {
                        Id = (int)reader["Id"],
                        VenueId = (int)reader["VenueId"],
                        Description = reader["Description"].ToString(),
                        Name = reader["Name"].ToString(),
                    });
                }
            }

            reader.Close();
            return layouts;
        }

        public Layout GetById(int id)
        {
            using SqlConnection connection = new SqlConnection(DbConnection.GetStringConnection());
            connection.Open();
            string sql = "Select Id, VenueId, Description, Name from layout where Id = @id";
            SqlCommand cmd = new SqlCommand(sql, connection);
            SqlParameter idParam = new SqlParameter("@id", id);
            cmd.Parameters.Add(idParam);
            SqlDataReader reader = cmd.ExecuteReader();
            Layout layout = new Layout();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    layout = new Layout
                    {
                        Id = (int)reader["Id"],
                        VenueId = (int)reader["VenueId"],
                        Description = reader["Description"].ToString(),
                        Name = reader["Name"].ToString(),
                    };
                }
            }

            return layout;
        }

        public Layout Create(Layout obj)
        {
            using SqlConnection connection = new SqlConnection(DbConnection.GetStringConnection());
            connection.Open();
            string sql = "Insert into layout (VenueId, Description, Name) values (@venueId, @description, @name)";
            SqlCommand command = new SqlCommand(sql, connection);
            SqlParameter venueIdParam = new SqlParameter("@venueId", obj.VenueId);
            SqlParameter descriptionParam = new SqlParameter("@description", obj.Description);
            SqlParameter nameParam = new SqlParameter("@name", obj.Name);
            command.Parameters.Add(venueIdParam);
            command.Parameters.Add(descriptionParam);
            command.Parameters.Add(nameParam);
            command.ExecuteNonQuery();
            return obj;
        }

        public Layout Update(Layout obj)
        {
            using SqlConnection connection = new SqlConnection(DbConnection.GetStringConnection());
            connection.Open();
            string sql = "Update layout set VenueId = @venueId, Description = @description, Name = @name where Id = @id";
            SqlCommand command = new SqlCommand(sql, connection);
            SqlParameter idParam = new SqlParameter("@id", obj.Id);
            SqlParameter venueIdParam = new SqlParameter("@venueId", obj.VenueId);
            SqlParameter descriptionParam = new SqlParameter("@description", obj.Description);
            SqlParameter nameParam = new SqlParameter("@name", obj.Name);
            command.Parameters.Add(idParam);
            command.Parameters.Add(venueIdParam);
            command.Parameters.Add(descriptionParam);
            command.Parameters.Add(nameParam);
            command.ExecuteNonQuery();
            return obj;
        }

        public Layout Delete(Layout obj)
        {
            using SqlConnection connection = new SqlConnection(DbConnection.GetStringConnection());
            connection.Open();
            string sql = "Delete from layout where Id = @id";
            SqlCommand command = new SqlCommand(sql, connection);
            SqlParameter idParam = new SqlParameter("@id", obj.Id);
            command.Parameters.Add(idParam);
            command.ExecuteNonQuery();
            return obj;
        }
    }
}