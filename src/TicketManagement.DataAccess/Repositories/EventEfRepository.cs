using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Models;

namespace TicketManagement.DataAccess.Repositories
{
    /// <summary>
    /// EntityFramework repository for events using stored procedures.
    /// </summary>
    internal class EventEfRepository : EfRepository<Event>, IRepository<Event>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventEfRepository"/> class.
        /// </summary>
        /// <param name="dbContext">TicketManagementContext object.</param>
        public EventEfRepository(TicketManagementContext dbContext)
            : base(dbContext)
        {
        }

        public override async Task<Event> CreateAsync(Event obj)
        {
            SqlParameter nameParam = new SqlParameter("@name", obj.Name);
            SqlParameter descriptionParam = new SqlParameter("@description", obj.Description);
            SqlParameter layoutIdParam = new SqlParameter("@layoutId", obj.LayoutId);
            SqlParameter timeStartParam = new SqlParameter("@timeStart", obj.TimeStart);
            SqlParameter timeEndParam = new SqlParameter("@timeEnd", obj.TimeEnd);
            SqlParameter imageParam = new SqlParameter("@imageUrl", obj.ImageUrl);
            await DbContext.Database.ExecuteSqlRawAsync("sp_CreateEvent @name, @description, @layoutId, @timeStart, @timeEnd, @imageUrl",
                nameParam, descriptionParam, layoutIdParam, timeStartParam, timeEndParam, imageParam);
            return obj;
        }

        public override async Task<Event> UpdateAsync(Event obj)
        {
            SqlParameter idParam = new SqlParameter("@id", obj.Id);
            SqlParameter nameParam = new SqlParameter("@name", obj.Name);
            SqlParameter descriptionParam = new SqlParameter("@description", obj.Description);
            SqlParameter layoutIdParam = new SqlParameter("@layoutId", obj.LayoutId);
            SqlParameter timeStartParam = new SqlParameter("@timeStart", obj.TimeStart);
            SqlParameter timeEndParam = new SqlParameter("@timeEnd", obj.TimeEnd);
            SqlParameter imageParam = new SqlParameter("@imageUrl", obj.ImageUrl);
            await DbContext.Database.ExecuteSqlRawAsync("sp_UpdateEvent @id, @name, @description, @layoutId, @timeStart, @timeEnd, @imageUrl",
                idParam, nameParam, descriptionParam, layoutIdParam, timeStartParam, timeEndParam, imageParam);
            return obj;
        }

        public override async Task<Event> DeleteAsync(Event obj)
        {
            SqlParameter idParam = new SqlParameter("@id", obj.Id);
            await DbContext.Database.ExecuteSqlRawAsync("sp_DeleteEvent @id", idParam);
            return obj;
        }
    }
}
