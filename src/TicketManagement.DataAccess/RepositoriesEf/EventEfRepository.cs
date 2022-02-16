using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Models;

namespace TicketManagement.DataAccess.RepositoriesEf
{
    /// <summary>
    /// EntityFramework repository for events using stored procedures.
    /// </summary>
    internal class EventEfRepository : EfRepository<Event>, IRepository<Event>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventEfRepository"/> class.
        /// </summary>
        /// <param name="dbContext">DbContext object.</param>
        public EventEfRepository(DbContext dbContext)
            : base(dbContext)
        {
        }

        public override async Task<Event> CreateAsync(Event obj)
        {
            var idParam = new SqlParameter
            {
                ParameterName = "@id",
                SqlDbType = System.Data.SqlDbType.Int,
                Direction = System.Data.ParameterDirection.Output,
            };
            var nameParam = new SqlParameter("@name", obj.Name);
            var descriptionParam = new SqlParameter("@description", obj.Description);
            var layoutIdParam = new SqlParameter("@layoutId", obj.LayoutId);
            var timeStartParam = new SqlParameter("@timeStart", obj.TimeStart);
            var timeEndParam = new SqlParameter("@timeEnd", obj.TimeEnd);
            var imageParam = new SqlParameter("@imageUrl", obj.ImageUrl);
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
            var idParam = new SqlParameter("@id", obj.Id);
            var nameParam = new SqlParameter("@name", obj.Name);
            var descriptionParam = new SqlParameter("@description", obj.Description);
            var layoutIdParam = new SqlParameter("@layoutId", obj.LayoutId);
            var timeStartParam = new SqlParameter("@timeStart", obj.TimeStart);
            var timeEndParam = new SqlParameter("@timeEnd", obj.TimeEnd);
            var imageParam = new SqlParameter("@imageUrl", obj.ImageUrl);
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
            var idParam = new SqlParameter("@id", obj.Id);
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
            var eventAreas = DbContext.Set<EventArea>();
            var eventAreasInEvent = eventAreas.Where(e => e.EventId == obj.Id).ToList();
            foreach (var eventArea in eventAreasInEvent)
            {
                var eventSeats = DbContext.Set<EventSeat>();
                var eventSeatsInEventArea = eventSeats.Where(s => s.EventAreaId == eventArea.Id).ToList();
                foreach (var eventSeat in eventSeatsInEventArea)
                {
                    DbContext.Set<EventSeat>().Remove(eventSeat);
                }

                await DbContext.SaveChangesAsync();
                DbContext.Set<EventArea>().Remove(eventArea);
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
            var areas = DbContext.Set<Area>().ToList();
            var seats = DbContext.Set<Seat>();
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
