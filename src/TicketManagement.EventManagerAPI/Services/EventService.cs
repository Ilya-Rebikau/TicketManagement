using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        /// EventAreaService object.
        /// </summary>
        private readonly IService<EventAreaDto> _eventAreaService;

        /// <summary>
        /// EventSeatService object.
        /// </summary>
        private readonly IService<EventSeatDto> _eventSeatService;

        /// <summary>
        /// Converter for time object.
        /// </summary>
        private readonly IUsersClient _usersClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventService"/> class.
        /// </summary>
        /// <param name="repository">EventRepository object.</param>
        /// <param name="converterDbDto">Converter for database and dto models.</param>
        /// <param name="converterDtoApi">Converter for dto and api models.</param>
        /// <param name="eventAreaRepository">EventAreaRepository object.</param>
        /// <param name="eventSeatRepository">EventSeatRepository object.</param>
        /// <param name="eventAreaService">EventAreaService object.</param>
        /// <param name="eventSeatService">EventSeatService object.</param>
        /// <param name="usersClient">ConverterForTime object.</param>
        public EventService(IRepository<Event> repository,
            IConverter<Event, EventDto> converterDbDto,
            IConverter<EventDto, EventModel> converterDtoApi,
            IRepository<EventArea> eventAreaRepository,
            IRepository<EventSeat> eventSeatRepository,
            IService<EventAreaDto> eventAreaService,
            IService<EventSeatDto> eventSeatService,
            IUsersClient usersClient)
            : base(repository, converterDbDto, eventAreaRepository, eventSeatRepository)
        {
            _converterDtoApi = converterDtoApi;
            _eventAreaService = eventAreaService;
            _eventSeatService = eventSeatService;
            _usersClient = usersClient;
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

        public async Task<IEnumerable<EventModel>> GetAllEventViewModelsAsync()
        {
            var events = await GetAllAsync();
            return await _converterDtoApi.ConvertSourceModelRangeToDestinationModelRange(events);
        }

        public async Task<EventModel> GetEventViewModelForDetailsAsync(EventDto @event, string token)
        {
            @event = await _usersClient.ConvertTimeFromUtcToUsers(token, @event);
            var eventAreas = await _eventAreaService.GetAllAsync();
            var eventAreasForEvent = eventAreas.Where(x => x.EventId == @event.Id);
            var eventSeats = await _eventSeatService.GetAllAsync();
            var eventAreaViewModels = new List<EventAreaModelInEvent>();
            foreach (var eventArea in eventAreasForEvent)
            {
                var eventSeatsInArea = eventSeats.Where(x => x.EventAreaId == eventArea.Id).ToList();
                var eventAreaViewModel = new EventAreaModelInEvent
                {
                    EventArea = eventArea,
                    EventSeats = eventSeatsInArea,
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
