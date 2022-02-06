using System.Linq;
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
            SqlParameter idParam = new SqlParameter
            {
                ParameterName = "@id",
                SqlDbType = System.Data.SqlDbType.Int,
                Direction = System.Data.ParameterDirection.Output,
            };
            SqlParameter nameParam = new SqlParameter("@name", obj.Name);
            SqlParameter descriptionParam = new SqlParameter("@description", obj.Description);
            SqlParameter layoutIdParam = new SqlParameter("@layoutId", obj.LayoutId);
            SqlParameter timeStartParam = new SqlParameter("@timeStart", obj.TimeStart);
            SqlParameter timeEndParam = new SqlParameter("@timeEnd", obj.TimeEnd);
            SqlParameter imageParam = new SqlParameter("@imageUrl", obj.ImageUrl);
            await DbContext.Database.ExecuteSqlRawAsync("sp_CreateEvent @name, @description, @layoutId, @timeStart, @timeEnd, @imageUrl, @id OUT",
                nameParam, descriptionParam, layoutIdParam, timeStartParam, timeEndParam, imageParam, idParam);
            obj.Id = int.Parse(idParam.Value.ToString());
            await CopyAreasAndSeatsIntoEvent(obj);
            return obj;
        }

        public override async Task<Event> UpdateAsync(Event obj)
        {
            var oldEvent = await GetByIdAsync(obj.Id);
            int oldLayoutId = oldEvent.LayoutId;
            SqlParameter idParam = new SqlParameter("@id", obj.Id);
            SqlParameter nameParam = new SqlParameter("@name", obj.Name);
            SqlParameter descriptionParam = new SqlParameter("@description", obj.Description);
            SqlParameter layoutIdParam = new SqlParameter("@layoutId", obj.LayoutId);
            SqlParameter timeStartParam = new SqlParameter("@timeStart", obj.TimeStart);
            SqlParameter timeEndParam = new SqlParameter("@timeEnd", obj.TimeEnd);
            SqlParameter imageParam = new SqlParameter("@imageUrl", obj.ImageUrl);
            await DbContext.Database.ExecuteSqlRawAsync("sp_UpdateEvent @id, @name, @description, @layoutId, @timeStart, @timeEnd, @imageUrl",
                idParam, nameParam, descriptionParam, layoutIdParam, timeStartParam, timeEndParam, imageParam);
            if (oldLayoutId != obj.LayoutId)
            {
                await DeleteEventAreasAndEventSeatsAsync(obj);
                await CopyAreasAndSeatsIntoEvent(obj);
            }

            return obj;
        }

        public override async Task<Event> DeleteAsync(Event obj)
        {
            await DeleteEventAreasAndEventSeatsAsync(obj);
            SqlParameter idParam = new SqlParameter("@id", obj.Id);
            await DbContext.Database.ExecuteSqlRawAsync("sp_DeleteEvent @id", idParam);
            return obj;
        }

        /// <summary>
        /// Deleting event areas and event seats for this event.
        /// </summary>
        /// <param name="obj">Deleting event.</param>
        /// <returns>Task.</returns>
        private async Task DeleteEventAreasAndEventSeatsAsync(Event obj)
        {
            var eventAreas = DbContext.EventAreas;
            var eventAreasInEvent = eventAreas.Where(e => e.EventId == obj.Id).ToList();
            foreach (var eventArea in eventAreasInEvent)
            {
                var eventSeats = DbContext.EventSeats;
                var eventSeatsInEventArea = eventSeats.Where(s => s.EventAreaId == eventArea.Id).ToList();
                foreach (var eventSeat in eventSeatsInEventArea)
                {
                    DbContext.EventSeats.Remove(eventSeat);
                }

                await DbContext.SaveChangesAsync();
                DbContext.EventAreas.Remove(eventArea);
            }

            await DbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Copy areas and seats to event areas and event seats during event creation.
        /// </summary>
        /// <param name="obj">Creating event.</param>
        /// <returns>Task.</returns>
        private async Task CopyAreasAndSeatsIntoEvent(Event obj)
        {
            var areas = DbContext.Areas.ToList();
            var seats = DbContext.Seats;
            var areasInLayout = areas.Where(a => a.LayoutId == obj.LayoutId).ToList();
            foreach (var area in areasInLayout)
            {
                var eventArea = new EventArea
                {
                    EventId = obj.Id,
                    Description = area.Description,
                    CoordX = area.CoordX,
                    CoordY = area.CoordY,
                    Price = area.BasePrice,
                };
                await DbContext.AddAsync(eventArea);
                await DbContext.SaveChangesAsync();
                var seatsInArea = seats.Where(s => s.AreaId == area.Id).ToList();
                foreach (var seat in seatsInArea)
                {
                    var eventSeat = new EventSeat
                    {
                        EventAreaId = eventArea.Id,
                        Number = seat.Number,
                        Row = seat.Row,
                        State = 0,
                    };
                    await DbContext.AddAsync(eventSeat);
                    await DbContext.SaveChangesAsync();
                }
            }
        }
    }
}
