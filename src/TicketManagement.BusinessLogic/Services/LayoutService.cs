using System;
using System.Collections.Generic;
using System.Linq;
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

        public override Layout Create(Layout obj)
        {
            CheckForUniqueNameInVenue(obj);
            return Repository.Create(obj);
        }

        public override Layout Update(Layout obj)
        {
            CheckForUniqueNameInVenue(obj);
            return Repository.Update(obj);
        }

        /// <summary>
        /// Checking that all layouts in venue have unique name.
        /// </summary>
        /// <param name="obj">Adding or updating layout.</param>
        /// <exception cref="ArgumentException">Generates exception in case there are layouts in venue with such name.</exception>
        private void CheckForUniqueNameInVenue(Layout obj)
        {
            IEnumerable<Layout> layouts = Repository.GetAll();
            IEnumerable<Layout> layoutsInVenue = layouts.Where(layout => layout.Name == obj.Name && layout.VenueId == obj.VenueId);
            if (layoutsInVenue.Any())
            {
                throw new ArgumentException("One of layouts in this venue already has such name!");
            }
        }
    }
}