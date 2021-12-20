using System;
using System.Collections.Generic;
using System.Linq;
using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Models;

namespace TicketManagement.BusinessLogic.Services
{
    /// <summary>
    /// Service with CRUD operations and validations for area.
    /// </summary>
    internal class AreaService : BaseService<Area>, IService<Area>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AreaService"/> class.
        /// </summary>
        /// <param name="repository">AreaRepository object.</param>
        public AreaService(IRepository<Area> repository)
            : base(repository)
        {
        }

        public override Area Create(Area obj)
        {
            CheckForUniqueDescription(obj);
            return Repository.Create(obj);
        }

        public override Area Update(Area obj)
        {
            CheckForUniqueDescription(obj);
            return Repository.Update(obj);
        }

        /// <summary>
        /// Checking that all areas in layout have unique description.
        /// </summary>
        /// <param name="obj">Adding or updating area.</param>
        /// <exception cref="ArgumentException">Generates exception in case description is not unique.</exception>
        private void CheckForUniqueDescription(Area obj)
        {
            IEnumerable<Area> areas = Repository.GetAll();
            IEnumerable<Area> areasInLayout = areas.Where(area => area.Description == obj.Description && area.LayoutId == obj.LayoutId);
            if (areasInLayout.Any())
            {
                throw new ArgumentException("One of areas in this layout already has such description!");
            }
        }
    }
}
