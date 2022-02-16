using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Models;

namespace TicketManagement.DataAccess.RepositoriesEf
{
    /// <summary>
    /// Ef repository for layout.
    /// </summary>
    internal class LayoutEfRepository : EfRepository<Layout>, IRepository<Layout>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutEfRepository"/> class.
        /// </summary>
        /// <param name="dbContext">DbContext object.</param>
        public LayoutEfRepository(DbContext dbContext)
            : base(dbContext)
        {
        }

        public async override Task<Layout> DeleteAsync(Layout obj)
        {
            await DeleteAllAsync(obj);
            return await base.DeleteAsync(obj);
        }

        /// <summary>
        /// Delete events, event areas, event seats, areas, seats.
        /// </summary>
        /// <param name="obj">Deleting layout.</param>
        /// <returns>Task.</returns>
        private async Task DeleteAllAsync(Layout obj)
        {
            await DeleteEventsAsync(obj);
            await DeleteAreasAsync(obj);
        }

        /// <summary>
        /// Delete areas in layout.
        /// </summary>
        /// <param name="obj">Deleting layout.</param>
        /// <returns>Task.</returns>
        private async Task DeleteAreasAsync(Layout obj)
        {
            var areas = DbContext.Set<Area>();
            var areasInLayout = areas.Where(a => a.LayoutId == obj.Id).ToList();
            foreach (var area in areasInLayout)
            {
                await DeleteSeatsAsync(area);
                DbContext.Set<Area>().Remove(area);
            }

            await DbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Delete seats in area.
        /// </summary>
        /// <param name="area">Deleting area.</param>
        /// <returns>Task.</returns>
        private async Task DeleteSeatsAsync(Area area)
        {
            var seats = DbContext.Set<Seat>();
            var seatsInArea = seats.Where(s => s.AreaId == area.Id).ToList();
            foreach (var seat in seatsInArea)
            {
                DbContext.Set<Seat>().Remove(seat);
            }

            await DbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Delete events in layout.
        /// </summary>
        /// <param name="layout">Deleting layout.</param>
        /// <returns>Task.</returns>
        private async Task DeleteEventsAsync(Layout layout)
        {
            var events = DbContext.Set<Event>();
            var eventsInLayout = events.Where(e => e.LayoutId == layout.Id).ToList();
            foreach (var @event in eventsInLayout)
            {
                await DeleteEventAreasAsync(@event);
                DbContext.Set<Event>().Remove(@event);
            }

            await DbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Delete event areas in event.
        /// </summary>
        /// <param name="event">Deleting event.</param>
        /// <returns>Task.</returns>
        private async Task DeleteEventAreasAsync(Event @event)
        {
            var eventAreas = DbContext.Set<EventArea>();
            var eventAreasInEvent = eventAreas.Where(e => e.EventId == @event.Id).ToList();
            foreach (var eventArea in eventAreasInEvent)
            {
                await DeleteEventSeatsAsync(eventArea);
                DbContext.Set<EventArea>().Remove(eventArea);
            }

            await DbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Delete event seats in event area.
        /// </summary>
        /// <param name="eventArea">Deleting event area.</param>
        /// <returns>Task.</returns>
        private async Task DeleteEventSeatsAsync(EventArea eventArea)
        {
            var eventSeats = DbContext.Set<EventSeat>();
            var eventSeatsInEventArea = eventSeats.Where(s => s.EventAreaId == eventArea.Id).ToList();
            foreach (var eventSeat in eventSeatsInEventArea)
            {
                DbContext.Set<EventSeat>().Remove(eventSeat);
            }

            await DbContext.SaveChangesAsync();
        }
    }
}
