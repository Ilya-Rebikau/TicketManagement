using System;
using System.Linq;
using System.Threading.Tasks;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Models;
using TicketManagement.PurchaseFlowAPI.Infrastructure;
using TicketManagement.PurchaseFlowAPI.Interfaces;
using TicketManagement.PurchaseFlowAPI.ModelsDTO;

namespace TicketManagement.PurchaseFlowAPI.Services
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
        public EventAreaService(IRepository<EventArea> repository, IConverter<EventArea, EventAreaDto> converter, IRepository<EventSeat> eventSeatRepository)
            : base(repository, converter)
        {
            _eventSeatRepository = eventSeatRepository;
        }

        public async override Task<EventAreaDto> CreateAsync(EventAreaDto obj)
        {
            CheckForStringFileds(obj);
            CheckForEventId(obj);
            CheckForPositiveCoords(obj);
            CheckForPositivePrice(obj);
            return await base.CreateAsync(obj);
        }

        public async override Task<EventAreaDto> UpdateAsync(EventAreaDto obj)
        {
            CheckForStringFileds(obj);
            CheckForEventId(obj);
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
        /// Check that string fields aren't empty or white space.
        /// </summary>
        /// <param name="obj">Event area.</param>
        /// <exception cref="ValidationException">Generates exception in case string fields are empty or white space.</exception>
        private static void CheckForStringFileds(EventAreaDto obj)
        {
            if (string.IsNullOrWhiteSpace(obj.Description))
            {
                throw new ValidationException("Fields can't be empty or white space!");
            }
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
        /// Check that event id event is positive.
        /// </summary>
        /// <param name="obj">Event area.</param>
        /// <exception cref="ValidationException">Generates exception in case event id isn't positive.</exception>
        private static void CheckForEventId(EventAreaDto obj)
        {
            if (obj.EventId <= 0)
            {
                throw new ValidationException("Event id must be positive");
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
