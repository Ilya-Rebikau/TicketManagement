using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Models;

namespace TicketManagement.DataAccess.RepositoriesEf
{
    /// <summary>
    /// Ef repository for area.
    /// </summary>
    internal class AreaEfRepository : EfRepository<Area>, IRepository<Area>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AreaEfRepository"/> class.
        /// </summary>
        /// <param name="dbContext">DbContext object.</param>
        public AreaEfRepository(DbContext dbContext)
            : base(dbContext)
        {
        }

        public async override Task<Area> DeleteAsync(Area obj)
        {
            await DeleteSeatsInAreaAsync(obj);
            return await base.DeleteAsync(obj);
        }

        /// <summary>
        /// Delete seats in this area.
        /// </summary>
        /// <param name="obj">Deleting area.</param>
        /// <returns>Task.</returns>
        private async Task DeleteSeatsInAreaAsync(Area obj)
        {
            var seats = DbContext.Set<Seat>();
            var seatsInArea = seats.Where(s => s.AreaId == obj.Id).ToList();
            foreach (var seat in seatsInArea)
            {
                DbContext.Set<Seat>().Remove(seat);
            }

            await DbContext.SaveChangesAsync();
        }
    }
}
