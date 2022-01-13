using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Models;

namespace TicketManagement.BusinessLogic.Services
{
    /// <summary>
    /// Service with CRUD operations and validations for layout.
    /// </summary>
    internal class LayoutService : BaseService<Layout>, IService<Layout>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutService"/> class.
        /// </summary>
        /// <param name="repository">LayoutRepository object.</param>
        public LayoutService(IRepository<Layout> repository)
            : base(repository)
        {
        }

        public async override Task<Layout> CreateAsync(Layout obj)
        {
            await CheckForUniqueNameInVenue(obj);
            return await Repository.CreateAsync(obj);
        }

        public async override Task<Layout> UpdateAsync(Layout obj)
        {
            await CheckForUniqueNameInVenue(obj);
            return await Repository.UpdateAsync(obj);
        }

        /// <summary>
        /// Checking that all layouts in venue have unique name.
        /// </summary>
        /// <param name="obj">Adding or updating layout.</param>
        /// <exception cref="ArgumentException">Generates exception in case there are layouts in venue with such name.</exception>
        private async Task CheckForUniqueNameInVenue(Layout obj)
        {
            IEnumerable<Layout> layouts = await Repository.GetAllAsync();
            IEnumerable<Layout> layoutsInVenue = layouts.Where(layout => layout.Name == obj.Name && layout.VenueId == obj.VenueId && layout.Id != obj.Id);
            if (layoutsInVenue.Any())
            {
                throw new ArgumentException("One of layouts in this venue already has such name!");
            }
        }
    }
}