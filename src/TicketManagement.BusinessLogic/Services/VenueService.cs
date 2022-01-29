using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.BusinessLogic.ModelsDTO;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Models;

namespace TicketManagement.BusinessLogic.Services
{
    /// <summary>
    /// Service with CRUD operations and validations for venue.
    /// </summary>
    internal class VenueService : BaseService<Venue, VenueDto>, IService<VenueDto>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VenueService"/> class.
        /// </summary>
        /// <param name="repository">VenueRepository object.</param>
        /// <param name="converter">Converter object.</param>
        public VenueService(IRepository<Venue> repository, IConverter<Venue, VenueDto> converter)
            : base(repository, converter)
        {
        }
    }
}
