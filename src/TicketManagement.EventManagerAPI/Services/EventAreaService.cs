using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Models;
using TicketManagement.EventManagerAPI.Infrastructure;
using TicketManagement.EventManagerAPI.Interfaces;
using TicketManagement.EventManagerAPI.ModelsDTO;

namespace TicketManagement.EventManagerAPI.Services
{
    /// <summary>
    /// Service with CRUD operations and validations for event area.
    /// </summary>
    internal class EventAreaService : BaseService<EventArea, EventAreaDto>, IService<EventAreaDto>
    {
        private readonly IRepository<EventSeat> _eventSeatRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventAreaService"/> class.
        /// </summary>
        /// <param name="repository">EventAreaRepository object.</param>
        /// <param name="converter">Converter object.</param>
        /// <param name="eventSeatRepository">EventSeatRepository object.</param>
        /// <param name="configuration">IConfiguration object.</param>
        public EventAreaService(IRepository<EventArea> repository,
            IConverter<EventArea, EventAreaDto> converter,
            IRepository<EventSeat> eventSeatRepository,
            IConfiguration configuration)
            : base(repository, converter, configuration)
        {
            _eventSeatRepository = eventSeatRepository;
        }

        public async override Task<EventAreaDto> CreateAsync(EventAreaDto obj)
        {
            CheckForPositiveCoords(obj);
            CheckForPositivePrice(obj);
            return await base.CreateAsync(obj);
        }

        public async override Task<EventAreaDto> UpdateAsync(EventAreaDto obj)
        {
            CheckForPositiveCoords(obj);
            CheckForPositivePrice(obj);
            return await base.UpdateAsync(obj);
        }

        public async override Task<EventAreaDto> DeleteAsync(EventAreaDto obj)
        {
            await CheckForTickets(obj);
            return await base.DeleteAsync(obj);
        }

        /// <summary>
        /// Checking that event area has positive coords.
        /// </summary>
        /// <param name="obj">Adding or updating event area.</param>
        /// <exception cref="ValidationException">Generates exception in case coords aren't positive.</exception>
        private static void CheckForPositiveCoords(EventAreaDto obj)
        {
            if (obj.CoordX <= 0 || obj.CoordY <= 0)
            {
                throw new ValidationException("Coords can be only positive numbers!");
            }
        }

        /// <summary>
        /// Checking that event area has positive price.
        /// </summary>
        /// <param name="obj">Adding or updating event area.</param>
        private static void CheckForPositivePrice(EventAreaDto obj)
        {
            if (obj.Price <= 0)
            {
                throw new ValidationException("Price can be only positive number!");
            }
        }

        /// <summary>
        /// Checking that there are no tickets in this event area.
        /// </summary>
        /// <param name="obj">Deleting event area.</param>
        /// <returns>Task.</returns>
        /// <exception cref="ValidationException">Generates exception in case there are tickets in this event area.</exception>
        private async Task CheckForTickets(EventAreaDto obj)
        {
            var allEventSeats = await _eventSeatRepository.GetAllAsync();
            var eventSeats = allEventSeats.Where(s => s.EventAreaId == obj.Id).Where(s => s.State == (int)PlaceStatus.Occupied).ToList();
            if (eventSeats.Any())
            {
                throw new ValidationException("Someone bought tickets in this event area already!");
            }
        }
    }
}
