using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Models;
using TicketManagement.VenueManagerAPI.Infrastructure;
using TicketManagement.VenueManagerAPI.Interfaces;
using TicketManagement.VenueManagerAPI.ModelsDTO;

namespace TicketManagement.VenueManagerAPI.Services
{
    /// <summary>
    /// Service with CRUD operations and validations for area.
    /// </summary>
    internal class AreaService : BaseService<Area, AreaDto>, IService<AreaDto>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AreaService"/> class.
        /// </summary>
        /// <param name="repository">AreaRepository object.</param>
        /// <param name="converter">Converter object.</param>
        /// <param name="confgiratuon">IConfiguration object.</param>
        public AreaService(IRepository<Area> repository, IConverter<Area, AreaDto> converter, IConfiguration confgiratuon)
            : base(repository, converter, confgiratuon)
        {
        }

        public async override Task<AreaDto> CreateAsync(AreaDto obj)
        {
            CheckForStringFileds(obj);
            CheckForLayoutId(obj);
            CheckForPositivePrice(obj);
            CheckForPositiveCoords(obj);
            await CheckForUniqueDescription(obj);
            await CheckForUniqueCoordsInLayout(obj);
            return await base.CreateAsync(obj);
        }

        public async override Task<AreaDto> UpdateAsync(AreaDto obj)
        {
            CheckForStringFileds(obj);
            CheckForLayoutId(obj);
            CheckForPositivePrice(obj);
            CheckForPositiveCoords(obj);
            await CheckForUniqueDescription(obj);
            await CheckForUniqueCoordsInLayout(obj);
            return await base.UpdateAsync(obj);
        }

        /// <summary>
        /// Check that string fields aren't empty or white space.
        /// </summary>
        /// <param name="obj">Area.</param>
        /// <exception cref="ValidationException">Generates exception in case string fields are empty or white space.</exception>
        private static void CheckForStringFileds(AreaDto obj)
        {
            if (string.IsNullOrWhiteSpace(obj.Description))
            {
                throw new ValidationException("Fields can't be empty or white space!");
            }
        }

        /// <summary>
        /// Check that layout id is positive.
        /// </summary>
        /// <param name="obj">Area.</param>
        /// <exception cref="ValidationException">Generates exception in case layout id isn't positive.</exception>
        private static void CheckForLayoutId(AreaDto obj)
        {
            if (obj.LayoutId <= 0)
            {
                throw new ValidationException("Layout id must be positive");
            }
        }

        /// <summary>
        /// Checking that area has positive price.
        /// </summary>
        /// <param name="obj">Adding or updating area.</param>
        /// <exception cref="ValidationException">Generates exception in case positive isnt positive.</exception>
        private static void CheckForPositivePrice(AreaDto obj)
        {
            if (obj.BasePrice <= 0)
            {
                throw new ValidationException("Price must be positive!");
            }
        }

        /// <summary>
        /// Checking that area has positive coords.
        /// </summary>
        /// <param name="obj">Adding or updating area.</param>
        /// <exception cref="ValidationException">Generates exception in case coords aren't positive.</exception>
        private static void CheckForPositiveCoords(AreaDto obj)
        {
            if (obj.CoordX <= 0 || obj.CoordY <= 0)
            {
                throw new ValidationException("Coords can be only positive numbers!");
            }
        }

        /// <summary>
        /// Checking that all areas in layout have unique description.
        /// </summary>
        /// <param name="obj">Adding or updating area.</param>
        /// <exception cref="ValidationException">Generates exception in case description is not unique.</exception>
        private async Task CheckForUniqueDescription(AreaDto obj)
        {
            var areas = await Repository.GetAllAsync();
            var areasInLayout = areas.Where(area => area.Description == obj.Description && area.LayoutId == obj.LayoutId && area.Id != obj.Id);
            if (areasInLayout.Any())
            {
                throw new ValidationException("One of areas in this layout already has such description!");
            }
        }

        /// <summary>
        /// Checking that area has unique coords in layout.
        /// </summary>
        /// <param name="obj">Adding or updating area.</param>
        /// <exception cref="ValidationException">Generates exception in case coords aren't unique for layout.</exception>
        private async Task CheckForUniqueCoordsInLayout(AreaDto obj)
        {
            var areas = await Repository.GetAllAsync();
            var areasInLayout = areas.Where(area => area.LayoutId == obj.LayoutId && area.CoordX == obj.CoordX && area.CoordY == obj.CoordY && area.Id != obj.Id);
            if (areasInLayout.Any())
            {
                throw new ValidationException("CoordX and CoordY must be unique for areas in one layout!");
            }
        }
    }
}
