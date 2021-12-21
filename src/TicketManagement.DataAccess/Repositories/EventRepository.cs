using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Models;

namespace TicketManagement.DataAccess.Repositories
{
    /// <summary>
    /// Repository for event.
    /// </summary>
    internal class EventRepository : IRepository<Event>
    {
        public IEnumerable<Event> GetAll()
        {
            using SqlConnection connection = new SqlConnection(DbConnection.GetStringConnection());
            connection.Open();
            IList<Event> events = new List<Event>();
            string sql = "Select Id, Name, Description, LayoutId, TimeStart, TimeEnd from event";
            SqlCommand cmd = new SqlCommand(sql, connection);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
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

        public Event GetById(int id)
        {
            using SqlConnection connection = new SqlConnection(DbConnection.GetStringConnection());
            connection.Open();
            string sql = "Select Id, Name, Description, LayoutId, TimeStart, TimeEnd from event where Id = @id";
            SqlCommand cmd = new SqlCommand(sql, connection);
            SqlParameter idParam = new SqlParameter("@id", id);
            cmd.Parameters.Add(idParam);
            SqlDataReader reader = cmd.ExecuteReader();
            Event eventModel = new Event();
            if (reader.HasRows)
            {
                while (reader.Read())
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

        public Event Create(Event obj)
        {
            using SqlConnection connection = new SqlConnection(DbConnection.GetStringConnection());
            connection.Open();
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
            command.ExecuteNonQuery();
            return obj;
        }

        public Event Update(Event obj)
        {
            using SqlConnection connection = new SqlConnection(DbConnection.GetStringConnection());
            connection.Open();
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
            command.ExecuteNonQuery();
            return obj;
        }

        public Event Delete(Event obj)
        {
            using SqlConnection connection = new SqlConnection(DbConnection.GetStringConnection());
            connection.Open();
            string sql = "sp_DeleteEvent";
            SqlCommand command = new SqlCommand(sql, connection);
            command.CommandType = CommandType.StoredProcedure;
            SqlParameter idParam = new SqlParameter("@id", obj.Id);
            command.Parameters.Add(idParam);
            command.ExecuteNonQuery();
            return obj;
        }
    }
}
