using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Models;

namespace TicketManagement.DataAccess.RepositoriesEf
{
    /// <summary>
    /// Ef repository for event area.
    /// </summary>
    internal class EventAreaEfRepository : EfRepository<EventArea>, IRepository<EventArea>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventAreaEfRepository"/> class.
        /// </summary>
        /// <param name="dbContext">DbContext object.</param>
        public EventAreaEfRepository(DbContext dbContext)
            : base(dbContext)
        {
        }

        public async override Task<EventArea> DeleteAsync(EventArea obj)
        {
            await DeleteEventSeatsAsync(obj);
            return await base.DeleteAsync(obj);
        }

        /// <summary>
        /// Delete event seats in event area.
        /// </summary>
        /// <param name="obj">Deleting event area.</param>
        /// <returns>Task.</returns>
        private async Task DeleteEventSeatsAsync(EventArea obj)
        {
            var eventSeats = DbContext.Set<EventSeat>();
            var eventSeatsInEventArea = eventSeats.Where(s => s.EventAreaId == obj.Id).ToList();
            foreach (var eventSeat in eventSeatsInEventArea)
            {
                DbContext.Set<EventSeat>().Remove(eventSeat);
            }

            await DbContext.SaveChangesAsync();
        }
    }
}
