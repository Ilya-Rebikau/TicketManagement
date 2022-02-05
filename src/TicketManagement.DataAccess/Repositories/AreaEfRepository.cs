using System.Linq;
using System.Threading.Tasks;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Models;

namespace TicketManagement.DataAccess.Repositories
{
    internal class AreaEfRepository : EfRepository<Area>, IRepository<Area>
    {
        public AreaEfRepository(TicketManagementContext dbContext)
            : base(dbContext)
        {
        }

        public async override Task<Area> DeleteAsync(Area obj)
        {
            await DeleteSeatsInAreaAsync(obj);
            return await base.DeleteAsync(obj);
        }

        private async Task DeleteSeatsInAreaAsync(Area obj)
        {
            var seats = DbContext.Seats;
            var seatsInArea = seats.Where(s => s.AreaId == obj.Id).ToList();
            foreach (var seat in seatsInArea)
            {
                DbContext.Seats.Remove(seat);
            }

            await DbContext.SaveChangesAsync();
        }
    }
}
