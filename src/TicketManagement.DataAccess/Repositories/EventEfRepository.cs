﻿using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Models;

namespace TicketManagement.DataAccess.Repositories
{
    internal class EventEfRepository : EfRepository<Event>, IRepository<Event>
    {
        public EventEfRepository(TicketManagementContext dbContext)
            : base(dbContext)
        {
        }

        public override async Task<Event> CreateAsync(Event obj)
        {
            await Task.Run(() =>
            {
                SqlParameter nameParam = new SqlParameter("@name", obj.Name);
                SqlParameter descriptionParam = new SqlParameter("@description", obj.Description);
                SqlParameter layoutIdParam = new SqlParameter("@layoutId", obj.LayoutId);
                SqlParameter timeStartParam = new SqlParameter("@timeStart", obj.TimeStart.ToString());
                SqlParameter timeEndParam = new SqlParameter("@timeEnd", obj.TimeEnd.ToString());
                SqlParameter imageParam = new SqlParameter("@image", obj.Image);
                DbContext.Events.FromSqlRaw("sp_CreateEvent @name, @description, @layoutId, @timeStart, @timeEnd, @image",
                    nameParam, descriptionParam, layoutIdParam, timeStartParam, timeEndParam, imageParam);
            });

            return obj;
        }

        public override async Task<Event> UpdateAsync(Event obj)
        {
            await Task.Run(() =>
            {
                SqlParameter idParam = new SqlParameter("@id", obj.Id);
                SqlParameter nameParam = new SqlParameter("@name", obj.Name);
                SqlParameter descriptionParam = new SqlParameter("@description", obj.Description);
                SqlParameter layoutIdParam = new SqlParameter("@layoutId", obj.LayoutId);
                SqlParameter timeStartParam = new SqlParameter("@timeStart", obj.TimeStart.ToString());
                SqlParameter timeEndParam = new SqlParameter("@timeEnd", obj.TimeEnd.ToString());
                SqlParameter imageParam = new SqlParameter("@image", obj.Image);
                DbContext.Events.FromSqlRaw("sp_UpdateEvent @idParam, @name, @description, @layoutId, @timeStart, @timeEnd, @image",
                    idParam, nameParam, descriptionParam, layoutIdParam, timeStartParam, timeEndParam, imageParam);
            });

            return obj;
        }

        public override async Task<Event> DeleteAsync(Event obj)
        {
            await Task.Run(() =>
            {
                SqlParameter idParam = new SqlParameter("@id", obj.Id);
                DbContext.Events.FromSqlRaw("sp_DeleteEvent @idParam", idParam);
            });

            return obj;
        }
    }
}
