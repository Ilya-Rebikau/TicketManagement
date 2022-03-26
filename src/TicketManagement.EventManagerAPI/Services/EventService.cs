using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Models;
using TicketManagement.EventManagerAPI.Interfaces;
using TicketManagement.EventManagerAPI.Models.Events;
using TicketManagement.EventManagerAPI.ModelsDTO;

namespace TicketManagement.EventManagerAPI.
    Services
{
    /// <summary>
    /// Service for events.
    /// </summary>
    internal class EventService : EventCrudService, IEventService
    {
        /// <summary>
        /// Converter for dto and api models.
        /// </summary>
        private readonly IConverter<EventDto, EventModel> _converterDtoApi;

        /// <summary>
        /// Converter for time object.
        /// </summary>
        private readonly IUsersClient _usersClient;

        /// <summary>
        /// Converter for event area and event area dto.
        /// </summary>
        private readonly IConverter<EventArea, EventAreaDto> _converterForEventArea;

        /// <summary>
        /// Converter for event seat and event seat dto.
        /// </summary>
        private readonly IConverter<EventSeat, EventSeatDto> _converterForEventSeat;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventService"/> class.
        /// </summary>
        /// <param name="repository">EventRepository object.</param>
        /// <param name="converterDbDto">Converter for database and dto models.</param>
        /// <param name="converterDtoApi">Converter for dto and api models.</param>
        /// <param name="converterForEventArea">Converter for event area and event area dto.</param>
        /// <param name="converterForEventSeat">Converter for event seat and event seat dto.</param>
        /// <param name="eventAreaRepository">EventAreaRepository object.</param>
        /// <param name="eventSeatRepository">EventSeatRepository object.</param>
        /// <param name="usersClient">ConverterForTime object.</param>
        /// <param name="configuration">IConfiguration object.</param>
        public EventService(IRepository<Event> repository,
            IConverter<Event, EventDto> converterDbDto,
            IConverter<EventDto, EventModel> converterDtoApi,
            IConverter<EventArea, EventAreaDto> converterForEventArea,
            IConverter<EventSeat, EventSeatDto> converterForEventSeat,
            IRepository<EventArea> eventAreaRepository,
            IRepository<EventSeat> eventSeatRepository,
            IUsersClient usersClient,
            IConfiguration configuration)
            : base(repository, converterDbDto, eventAreaRepository, eventSeatRepository, configuration)
        {
            _converterDtoApi = converterDtoApi;
            _usersClient = usersClient;
            _converterForEventArea = converterForEventArea;
            _converterForEventSeat = converterForEventSeat;
        }

        public async Task<EventModel> UpdateAsync(EventModel obj)
        {
            var @event = await UpdateAsync(await _converterDtoApi.ConvertDestinationToSource(obj));
            return await _converterDtoApi.ConvertSourceToDestination(@event);
        }

        public async Task<EventModel> CreateAsync(EventModel obj)
        {
            var @event = await CreateAsync(await _converterDtoApi.ConvertDestinationToSource(obj));
            return await _converterDtoApi.ConvertSourceToDestination(@event);
        }

        public async Task<IEnumerable<EventModel>> GetAllEventViewModelsAsync(int pageNumber)
        {
            var events = await GetAllAsync(pageNumber);
            return await _converterDtoApi.ConvertSourceModelRangeToDestinationModelRange(events);
        }

        public async Task<EventModel> GetEventViewModelForDetailsAsync(EventDto @event, string token)
        {
            @event = await _usersClient.ConvertTimeFromUtcToUsers(token, @event);
            var eventAreas = await EventAreaRepository.GetAllAsync();
            var eventAreasForEvent = eventAreas.Where(x => x.EventId == @event.Id).ToList();
            var eventSeats = await EventSeatRepository.GetAllAsync();
            var eventAreaViewModels = new List<EventAreaModelInEvent>();
            foreach (var eventArea in eventAreasForEvent)
            {
                var eventSeatsInArea = eventSeats.Where(x => x.EventAreaId == eventArea.Id).ToList();
                var eventSeatsInEventArea = await _converterForEventSeat.ConvertSourceModelRangeToDestinationModelRange(eventSeatsInArea);
                var eventAreaViewModel = new EventAreaModelInEvent
                {
                    EventArea = await _converterForEventArea.ConvertSourceToDestination(eventArea),
                    EventSeats = eventSeatsInEventArea.ToList(),
                };

                eventAreaViewModels.Add(eventAreaViewModel);
            }

            EventModel eventViewModel = await _converterDtoApi.ConvertSourceToDestination(@event);
            eventViewModel.EventAreas = eventAreaViewModels;
            return eventViewModel;
        }

        public async Task<EventModel> GetEventViewModelForEditAndDeleteAsync(EventDto @event, string token)
        {
            @event = await _usersClient.ConvertTimeFromUtcToUsers(token, @event);
            return await _converterDtoApi.ConvertSourceToDestination(@event);
        }
    }
}
