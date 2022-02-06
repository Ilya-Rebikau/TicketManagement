using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Models;

namespace TicketManagement.DataAccess.Repositories
{
    internal class LayoutEfRepository : EfRepository<Layout>, IRepository<Layout>
    {
        public LayoutEfRepository(TicketManagementContext dbContext)
            : base(dbContext)
        {
        }

        public async override Task<Layout> DeleteAsync(Layout obj)
        {
            await DeleteAllAsync(obj);
            return await base.DeleteAsync(obj);
        }

        private async Task DeleteAllAsync(Layout obj)
        {
            await DeleteEventsAsync(obj);
            await DeleteAreasAsync(obj);
        }

        private async Task DeleteAreasAsync(Layout obj)
        {
            var areas = DbContext.Areas;
            var areasInLayout = areas.Where(a => a.LayoutId == obj.Id).ToList();
            foreach (var area in areasInLayout)
            {
                await DeleteSeatsAsync(area);
                DbContext.Areas.Remove(area);
            }

            await DbContext.SaveChangesAsync();
        }

        private async Task DeleteSeatsAsync(Area area)
        {
            var seats = DbContext.Seats;
            var seatsInArea = seats.Where(s => s.AreaId == area.Id).ToList();
            foreach (var seat in seatsInArea)
            {
                DbContext.Seats.Remove(seat);
            }

            await DbContext.SaveChangesAsync();
        }

        private async Task DeleteEventsAsync(Layout layout)
        {
            var events = DbContext.Events;
            var eventsInLayout = events.Where(e => e.LayoutId == layout.Id).ToList();
            foreach (var @event in eventsInLayout)
            {
                await DeleteEventAreasAsync(@event);
            }

            await DbContext.SaveChangesAsync();
        }

        private async Task DeleteEventAreasAsync(Event @event)
        {
            var eventAreas = DbContext.EventAreas;
            var eventAreasInEvent = eventAreas.Where(e => e.EventId == @event.Id).ToList();
            foreach (var eventArea in eventAreasInEvent)
            {
                await DeleteEventSeatsAsync(eventArea);
                DbContext.EventAreas.Remove(eventArea);
            }

            await DbContext.SaveChangesAsync();
        }

        private async Task DeleteEventSeatsAsync(EventArea eventArea)
        {
            var eventSeats = DbContext.EventSeats;
            var eventSeatsInEventArea = eventSeats.Where(s => s.EventAreaId == eventArea.Id).ToList();
            foreach (var eventSeat in eventSeatsInEventArea)
            {
                DbContext.EventSeats.Remove(eventSeat);
            }

            await DbContext.SaveChangesAsync();
        }
    }
}
