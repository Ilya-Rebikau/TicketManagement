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
            await DeleteAreasAndSeatsAsync(obj);
            return await base.DeleteAsync(obj);
        }

        private async Task DeleteAreasAndSeatsAsync(Layout obj)
        {
            var areas = DbContext.Areas;
            var areasInLayout = areas.Where(a => a.LayoutId == obj.Id).ToList();
            foreach (var area in areasInLayout)
            {
                var seats = DbContext.Seats;
                var seatsInArea = seats.Where(s => s.AreaId == area.Id).ToList();
                foreach (var seat in seatsInArea)
                {
                    DbContext.Seats.Remove(seat);
                }

                await DbContext.SaveChangesAsync();
                DbContext.Areas.Remove(area);
            }

            await DbContext.SaveChangesAsync();
        }
    }
}
