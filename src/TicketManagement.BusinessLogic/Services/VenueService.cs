using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Models;

namespace TicketManagement.BusinessLogic.Services
{
    /// <summary>
    /// Service with CRUD operations and validations for venue.
    /// </summary>
    internal class VenueService : BaseService<Venue>, IService<Venue>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VenueService"/> class.
        /// </summary>
        /// <param name="repository">VenueRepository object.</param>
        public VenueService(IRepository<Venue> repository)
            : base(repository)
        {
        }

        public new Venue Create(Venue obj)
        {
            return Repository.Create(obj);
        }

        public new Venue Update(Venue obj)
        {
            return Repository.Update(obj);
        }
    }
}
