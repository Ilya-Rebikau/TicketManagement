using System;
using System.Collections.Generic;
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
    /// Service with CRUD operations and validations for event seat.
    /// </summary>
    internal class EventSeatService : BaseService<EventSeat, EventSeatDto>, IEventSeatService
    {
        /// <summary>
        /// EventAreaRepository object.
        /// </summary>
        private readonly IRepository<EventArea> _eventAreaRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventSeatService"/> class.
        /// </summary>
        /// <param name="repository">EventSeatRepository object.</param>
        /// <param name="converter">Converter object.</param>
        /// <param name="configuration">IConfiguration object.</param>
        /// <param name="eventAreaRepository">EventAreaRepository object.</param>
        public EventSeatService(IRepository<EventSeat> repository,
            IConverter<EventSeat, EventSeatDto> converter,
            IConfiguration configuration,
            IRepository<EventArea> eventAreaRepository)
            : base(repository, converter, configuration)
        {
            _eventAreaRepository = eventAreaRepository;
        }

        public async override Task<EventSeatDto> CreateAsync(EventSeatDto obj)
        {
            CheckForEventAreaId(obj);
            CheckForUniqueRowAndNumber(obj);
            CheckForPositiveRowAndNumber(obj);
            return await base.CreateAsync(obj);
        }

        public async override Task<EventSeatDto> UpdateAsync(EventSeatDto obj)
        {
            CheckForEventAreaId(obj);
            CheckForUniqueRowAndNumber(obj);
            CheckForPositiveRowAndNumber(obj);
            return await base.UpdateAsync(obj);
        }

        public async override Task<EventSeatDto> DeleteAsync(EventSeatDto obj)
        {
            CheckForTickets(obj);
            return await base.DeleteAsync(obj);
        }

        public async Task<IEnumerable<EventSeatDto>> GetFreeEventSeatsByEvent(int eventId)
        {
            var eventAreas = _eventAreaRepository.GetAll();
            var eventSeats = Repository.GetAll();
            var eventSeatsInAreas = eventAreas.Where(ea => ea.EventId == eventId).SelectMany(eventArea => eventSeats
            .Where(eventSeat => eventSeat.EventAreaId == eventArea.Id && eventSeat.State == (int)PlaceStatus.Free)).ToList();
            return await Converter.ConvertSourceModelRangeToDestinationModelRange(eventSeatsInAreas);
        }

        /// <summary>
        /// Check that event area id is positive.
        /// </summary>
        /// <param name="obj">Event seat.</param>
        /// <exception cref="ValidationException">Generates exception in case event area id isn't positive.</exception>
        private static void CheckForEventAreaId(EventSeatDto obj)
        {
            if (obj.EventAreaId <= 0)
            {
                throw new ValidationException("Event area id must be positive");
            }
        }

        /// <summary>
        /// Checking that seat has positive row and number.
        /// </summary>
        /// <param name="obj">Adding or updating seat.</param>
        /// <exception cref="ValidationException">Generates exception in case row or number are not positive.</exception>
        private static void CheckForPositiveRowAndNumber(EventSeatDto obj)
        {
            if (obj.Row <= 0 || obj.Number <= 0)
            {
                throw new ValidationException("Row and number must be positive!");
            }
        }

        /// <summary>
        /// Checking that there is no ticket in this event seat.
        /// </summary>
        /// <param name="obj">Deleting event seat.</param>
        /// <exception cref="ValidationException">Generates exception in case there is ticket in this event seat.</exception>
        private static void CheckForTickets(EventSeatDto obj)
        {
            if (obj.State == PlaceStatus.Occupied)
            {
                throw new ValidationException("Someone bought tickets for this event seat already!");
            }
        }

        /// <summary>
        /// Checking that all event seats in event area have unique row and number.
        /// </summary>
        /// <param name="obj">Adding or updating seat.</param>
        /// <exception cref="ValidationException">Generates exception in case row and number are not unique.</exception>
        private void CheckForUniqueRowAndNumber(EventSeatDto obj)
        {
            var eventSeats = Repository.GetAll();
            var eventSeatsInArea = eventSeats.Where(seat => seat.EventAreaId == obj.EventAreaId && seat.Row == obj.Row && seat.Number == obj.Number && seat.Id != obj.Id);
            if (eventSeatsInArea.Any())
            {
                throw new ValidationException("One of seats in this area already has such row and number!");
            }
        }
    }
}
