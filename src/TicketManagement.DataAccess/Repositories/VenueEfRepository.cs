using System.Linq;
using System.Threading.Tasks;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Models;

namespace TicketManagement.DataAccess.Repositories
{
    internal class VenueEfRepository : EfRepository<Venue>, IRepository<Venue>
    {
        public VenueEfRepository(TicketManagementContext dbContext)
            : base(dbContext)
        {
        }

        public async override Task<Venue> DeleteAsync(Venue obj)
        {
            await DeleteLayoutsAreasAndSeatsAsync(obj);
            return await base.DeleteAsync(obj);
        }

        private async Task DeleteLayoutsAreasAndSeatsAsync(Venue obj)
        {
            var layouts = DbContext.Layouts;
            var layoutsInVenue = layouts.Where(l => l.VenueId == obj.Id).ToList();
            foreach (var layout in layoutsInVenue)
            {
                var areas = DbContext.Areas;
                var areasInLayout = areas.Where(a => a.LayoutId == layout.Id).ToList();
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
                DbContext.Layouts.Remove(layout);
            }

            await DbContext.SaveChangesAsync();
        }
    }
}
