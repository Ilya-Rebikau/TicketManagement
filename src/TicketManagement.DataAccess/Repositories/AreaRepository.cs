using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Models;

namespace TicketManagement.DataAccess.Repositories
{
    /// <summary>
    /// Repository for area.
    /// </summary>
    internal class AreaRepository : IRepository<Area>
    {
        public IEnumerable<Area> GetAll()
        {
            using SqlConnection connection = new SqlConnection(DbConnection.GetStringConnection());
            connection.Open();
            IList<Area> areas = new List<Area>();
            string sql = "Select Id, LayoutId, Description, CoordX, CoordY from area";
            SqlCommand cmd = new SqlCommand(sql, connection);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    areas.Add(new Area
                    {
                        Id = (int)reader["Id"],
                        LayoutId = (int)reader["LayoutId"],
                        Description = reader["Description"].ToString(),
                        CoordX = (int)reader["CoordX"],
                        CoordY = (int)reader["CoordY"],
                    });
                }
            }

            reader.Close();
            return areas;
        }

        public Area GetById(int id)
        {
            using SqlConnection connection = new SqlConnection(DbConnection.GetStringConnection());
            connection.Open();
            string sql = "Select Id, LayoutId, Description, CoordX, CoordY from area where Id = @id";
            SqlCommand cmd = new SqlCommand(sql, connection);
            SqlParameter idParam = new SqlParameter("@id", id);
            cmd.Parameters.Add(idParam);
            SqlDataReader reader = cmd.ExecuteReader();
            Area area = new Area();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    area = new Area
                    {
                        Id = (int)reader["Id"],
                        LayoutId = (int)reader["LayoutId"],
                        Description = reader["Description"].ToString(),
                        CoordX = (int)reader["CoordX"],
                        CoordY = (int)reader["CoordY"],
                    };
                }
            }

            return area;
        }

        public Area Create(Area obj)
        {
            using SqlConnection connection = new SqlConnection(DbConnection.GetStringConnection());
            string sql = "Insert into area (LayoutId, Description, CoordX, CoordY) values (@layoutId, @description, @coordX, @coordY)";
            connection.Open();
            SqlCommand command = new SqlCommand(sql, connection);
            SqlParameter layoutIdParam = new SqlParameter("@layoutId", obj.LayoutId);
            SqlParameter descriptionParam = new SqlParameter("@description", obj.Description);
            SqlParameter coordXParam = new SqlParameter("@coordX", obj.CoordX);
            SqlParameter coordYParam = new SqlParameter("@coordY", obj.CoordY);
            command.Parameters.Add(layoutIdParam);
            command.Parameters.Add(descriptionParam);
            command.Parameters.Add(coordXParam);
            command.Parameters.Add(coordYParam);
            command.ExecuteNonQuery();
            return obj;
        }

        public Area Update(Area obj)
        {
            using SqlConnection connection = new SqlConnection(DbConnection.GetStringConnection());
            connection.Open();
            string sql = "Update area set LayoutId = @layoutId, Description = @description, CoordX = @coordX, CoordY = @coordY where Id = @id";
            SqlCommand command = new SqlCommand(sql, connection);
            SqlParameter idParam = new SqlParameter("@id", obj.Id);
            SqlParameter layoutIdParam = new SqlParameter("@layoutId", obj.LayoutId);
            SqlParameter descriptionParam = new SqlParameter("@description", obj.Description);
            SqlParameter coordXParam = new SqlParameter("@coordX", obj.CoordX);
            SqlParameter coordYParam = new SqlParameter("@coordY", obj.CoordY);
            command.Parameters.Add(idParam);
            command.Parameters.Add(layoutIdParam);
            command.Parameters.Add(descriptionParam);
            command.Parameters.Add(coordXParam);
            command.Parameters.Add(coordYParam);
            command.ExecuteNonQuery();
            return obj;
        }

        public Area Delete(Area obj)
        {
            using SqlConnection connection = new SqlConnection(DbConnection.GetStringConnection());
            connection.Open();
            string sql = "Delete from area where Id = @id";
            SqlCommand command = new SqlCommand(sql, connection);
            SqlParameter idParam = new SqlParameter("@id", obj.Id);
            command.Parameters.Add(idParam);
            command.ExecuteNonQuery();
            return obj;
        }
    }
}
