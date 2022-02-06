using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Models;

namespace TicketManagement.DataAccess.Repositories
{
    internal class EventAreaEfRepository : EfRepository<EventArea>, IRepository<EventArea>
    {
        public EventAreaEfRepository(TicketManagementContext dbContext)
            : base(dbContext)
        {
        }

        public async override Task<EventArea> DeleteAsync(EventArea obj)
        {
            await DeleteEventSeatsAsync(obj);
            return await base.DeleteAsync(obj);
        }

        private async Task DeleteEventSeatsAsync(EventArea obj)
        {
            var eventSeats = DbContext.EventSeats;
            var eventSeatsInEventArea = eventSeats.Where(s => s.EventAreaId == obj.Id).ToList();
            foreach (var eventSeat in eventSeatsInEventArea)
            {
                DbContext.EventSeats.Remove(eventSeat);
            }

            await DbContext.SaveChangesAsync();
        }
    }
}
