using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.BusinessLogic.ModelsDTO;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Models;

namespace TicketManagement.BusinessLogic.Services
{
    /// <summary>
    /// Service with CRUD operations and validations for layout.
    /// </summary>
    internal class LayoutService : BaseService<Layout, LayoutDto>, IService<LayoutDto>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutService"/> class.
        /// </summary>
        /// <param name="repository">LayoutRepository object.</param>
        /// <param name="converter">Converter object.</param>
        public LayoutService(IRepository<Layout> repository, IConverter<Layout, LayoutDto> converter)
            : base(repository, converter)
        {
        }

        public async override Task<LayoutDto> CreateAsync(LayoutDto obj)
        {
            await CheckForUniqueNameInVenue(obj);
            return await base.CreateAsync(obj);
        }

        public async override Task<LayoutDto> UpdateAsync(LayoutDto obj)
        {
            await CheckForUniqueNameInVenue(obj);
            return await base.UpdateAsync(obj);
        }

        /// <summary>
        /// Checking that all layouts in venue have unique name.
        /// </summary>
        /// <param name="obj">Adding or updating layout.</param>
        /// <exception cref="ArgumentException">Generates exception in case there are layouts in venue with such name.</exception>
        private async Task CheckForUniqueNameInVenue(LayoutDto obj)
        {
            IEnumerable<LayoutDto> layouts = await Converter.ConvertModelsRangeToDtos(await Repository.GetAllAsync());
            IEnumerable<LayoutDto> layoutsInVenue = layouts.Where(layout => layout.Name == obj.Name && layout.VenueId == obj.VenueId && layout.Id != obj.Id);
            if (layoutsInVenue.Any())
            {
                throw new ArgumentException("One of layouts in this venue already has such name!");
            }
        }
    }
}