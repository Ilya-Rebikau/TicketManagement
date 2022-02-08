using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Models;

namespace TicketManagement.DataAccess.Repositories
{
    /// <summary>
    /// Repository for area.
    /// </summary>
    internal class AreaRepository : IRepository<Area>
    {
        public async Task<IQueryable<Area>> GetAllAsync()
        {
            using SqlConnection connection = new SqlConnection(DbConnection.GetStringConnection());
            await connection.OpenAsync();
            IList<Area> areas = new List<Area>();
            string sql = "Select Id, LayoutId, Description, CoordX, CoordY, BasePrice from areas";
            SqlCommand cmd = new SqlCommand(sql, connection);
            SqlDataReader reader = await cmd.ExecuteReaderAsync();
            if (reader.HasRows)
            {
                while (await reader.ReadAsync())
                {
                    areas.Add(new Area
                    {
                        Id = (int)reader["Id"],
                        LayoutId = (int)reader["LayoutId"],
                        Description = reader["Description"].ToString(),
                        CoordX = (int)reader["CoordX"],
                        CoordY = (int)reader["CoordY"],
                        BasePrice = double.Parse(reader["BasePrice"].ToString()),
                    });
                }
            }

            await reader.CloseAsync();
            return areas.AsQueryable();
        }

        public async Task<Area> GetByIdAsync(int id)
        {
            using SqlConnection connection = new SqlConnection(DbConnection.GetStringConnection());
            await connection.OpenAsync();
            string sql = "Select Id, LayoutId, Description, CoordX, CoordY, BasePrice from areas where Id = @id";
            SqlCommand cmd = new SqlCommand(sql, connection);
            SqlParameter idParam = new SqlParameter("@id", id);
            cmd.Parameters.Add(idParam);
            SqlDataReader reader = await cmd.ExecuteReaderAsync();
            Area area = new Area();
            if (reader.HasRows)
            {
                while (await reader.ReadAsync())
                {
                    area = new Area
                    {
                        Id = (int)reader["Id"],
                        LayoutId = (int)reader["LayoutId"],
                        Description = reader["Description"].ToString(),
                        CoordX = (int)reader["CoordX"],
                        CoordY = (int)reader["CoordY"],
                        BasePrice = double.Parse(reader["BasePrice"].ToString()),
                    };
                }
            }

            return area;
        }

        public async Task<Area> CreateAsync(Area obj)
        {
            using SqlConnection connection = new SqlConnection(DbConnection.GetStringConnection());
            string sql = "Insert into areas (LayoutId, Description, CoordX, CoordY, BasePrice) values (@layoutId, @description, @coordX, @coordY, @price)";
            await connection.OpenAsync();
            SqlCommand command = new SqlCommand(sql, connection);
            SqlParameter layoutIdParam = new SqlParameter("@layoutId", obj.LayoutId);
            SqlParameter descriptionParam = new SqlParameter("@description", obj.Description);
            SqlParameter coordXParam = new SqlParameter("@coordX", obj.CoordX);
            SqlParameter coordYParam = new SqlParameter("@coordY", obj.CoordY);
            SqlParameter priceParam = new SqlParameter("@price", obj.BasePrice);
            command.Parameters.Add(layoutIdParam);
            command.Parameters.Add(descriptionParam);
            command.Parameters.Add(coordXParam);
            command.Parameters.Add(coordYParam);
            command.Parameters.Add(priceParam);
            await command.ExecuteNonQueryAsync();
            return obj;
        }

        public async Task<Area> UpdateAsync(Area obj)
        {
            using SqlConnection connection = new SqlConnection(DbConnection.GetStringConnection());
            await connection.OpenAsync();
            string sql = "Update areas set LayoutId = @layoutId, Description = @description, CoordX = @coordX, CoordY = @coordY, BasePrice = @price where Id = @id";
            SqlCommand command = new SqlCommand(sql, connection);
            SqlParameter idParam = new SqlParameter("@id", obj.Id);
            SqlParameter layoutIdParam = new SqlParameter("@layoutId", obj.LayoutId);
            SqlParameter descriptionParam = new SqlParameter("@description", obj.Description);
            SqlParameter coordXParam = new SqlParameter("@coordX", obj.CoordX);
            SqlParameter coordYParam = new SqlParameter("@coordY", obj.CoordY);
            SqlParameter priceParam = new SqlParameter("@price", obj.BasePrice);
            command.Parameters.Add(idParam);
            command.Parameters.Add(layoutIdParam);
            command.Parameters.Add(descriptionParam);
            command.Parameters.Add(coordXParam);
            command.Parameters.Add(coordYParam);
            command.Parameters.Add(priceParam);
            await command.ExecuteNonQueryAsync();
            return obj;
        }

        public async Task<Area> DeleteAsync(Area obj)
        {
            using SqlConnection connection = new SqlConnection(DbConnection.GetStringConnection());
            await connection.OpenAsync();
            string sql = "Delete from areas where Id = @id";
            SqlCommand command = new SqlCommand(sql, connection);
            SqlParameter idParam = new SqlParameter("@id", obj.Id);
            command.Parameters.Add(idParam);
            await command.ExecuteNonQueryAsync();
            return obj;
        }
    }
}
