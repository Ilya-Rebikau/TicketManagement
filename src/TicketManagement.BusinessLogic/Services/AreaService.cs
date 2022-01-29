﻿using System;
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
    /// Service with CRUD operations and validations for area.
    /// </summary>
    internal class AreaService : BaseService<Area, AreaDto>, IService<AreaDto>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AreaService"/> class.
        /// </summary>
        /// <param name="repository">AreaRepository object.</param>
        /// <param name="converter">Converter object.</param>
        public AreaService(IRepository<Area> repository, IConverter<Area, AreaDto> converter)
            : base(repository, converter)
        {
        }

        public async override Task<AreaDto> CreateAsync(AreaDto obj)
        {
            CheckForPositiveCoords(obj);
            await CheckForUniqueDescription(obj);
            await CheckForUniqueCoordsInLayout(obj);
            return await base.CreateAsync(obj);
        }

        public async override Task<AreaDto> UpdateAsync(AreaDto obj)
        {
            CheckForPositiveCoords(obj);
            await CheckForUniqueDescription(obj);
            await CheckForUniqueCoordsInLayout(obj);
            return await base.UpdateAsync(obj);
        }

        /// <summary>
        /// Checking that all areas in layout have unique description.
        /// </summary>
        /// <param name="obj">Adding or updating area.</param>
        /// <exception cref="ArgumentException">Generates exception in case description is not unique.</exception>
        private async Task CheckForUniqueDescription(AreaDto obj)
        {
            IEnumerable<AreaDto> areas = await Converter.ConvertModelsRangeToDtos(await Repository.GetAllAsync());
            IEnumerable<AreaDto> areasInLayout = areas.Where(area => area.Description == obj.Description && area.LayoutId == obj.LayoutId && area.Id != obj.Id);
            if (areasInLayout.Any())
            {
                throw new ArgumentException("One of areas in this layout already has such description!");
            }
        }

        /// <summary>
        /// Checking that area has positive coords.
        /// </summary>
        /// <param name="obj">Adding or updating area.</param>
        /// <exception cref="ArgumentException">Generates exception in case coords aren't positive.</exception>
        private void CheckForPositiveCoords(AreaDto obj)
        {
            if (obj.CoordX <= 0 || obj.CoordY <= 0)
            {
                throw new ArgumentException("Coords can be only positive numbers!");
            }
        }

        /// <summary>
        /// Checking that area has unique coords in layout.
        /// </summary>
        /// <param name="obj">Adding or updating area.</param>
        /// <exception cref="ArgumentException">Generates exception in case coords aren't unique for layout.</exception>
        private async Task CheckForUniqueCoordsInLayout(AreaDto obj)
        {
            IEnumerable<AreaDto> areas = await Converter.ConvertModelsRangeToDtos(await Repository.GetAllAsync());
            IEnumerable<AreaDto> areasInLayout = areas.Where(area => area.LayoutId == obj.LayoutId && area.CoordX == obj.CoordX && area.CoordY == obj.CoordY && area.Id != obj.Id);
            if (areasInLayout.Any())
            {
                throw new ArgumentException("CoordX and CoordY must be unique for areas in one layout!");
            }
        }
    }
}
