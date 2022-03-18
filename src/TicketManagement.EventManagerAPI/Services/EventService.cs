using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
        /// <param name="converter">Converter object for events.</param>
        /// <param name="eventAreaRepository">EventAreaRepository object.</param>
        /// <param name="eventSeatRepository">EventSeatRepository object.</param>
        /// <param name="eventAreaService">EventAreaService object.</param>
        /// <param name="eventSeatService">EventSeatService object.</param>
        /// <param name="usersClient">ConverterForTime object.</param>
        public EventService(IRepository<Event> repository,
            IConverter<Event, EventDto> converter,
            IRepository<EventArea> eventAreaRepository,
            IRepository<EventSeat> eventSeatRepository,
            IService<EventAreaDto> eventAreaService,
            IService<EventSeatDto> eventSeatService,
            IUsersClient usersClient)
            : base(repository, converter, eventAreaRepository, eventSeatRepository)
        {
            _eventAreaService = eventAreaService;
            _eventSeatService = eventSeatService;
            _usersClient = usersClient;
        }

        public async Task<IEnumerable<EventViewModel>> GetAllEventViewModelsAsync()
        {
            var events = await GetAllAsync();
            var eventsVm = new List<EventViewModel>();
            foreach (var @event in events)
            {
                eventsVm.Add(@event);
            }

            return eventsVm;
        }

        public async Task<EventViewModel> GetEventViewModelForDetailsAsync(EventDto @event)
        {
            await _usersClient.ConvertTimeFromUtcToUsers(@event);
            var eventAreas = await _eventAreaService.GetAllAsync();
            var eventAreasForEvent = eventAreas.Where(x => x.EventId == @event.Id);
            var eventSeats = await _eventSeatService.GetAllAsync();
            var eventAreaViewModels = new List<EventAreaViewModelInEvent>();
            foreach (var eventArea in eventAreasForEvent)
            {
                var eventSeatsInArea = eventSeats.Where(x => x.EventAreaId == eventArea.Id).ToList();
                var eventAreaViewModel = new EventAreaViewModelInEvent
                {
                    EventArea = eventArea,
                    EventSeats = eventSeatsInArea,
                };

                eventAreaViewModels.Add(eventAreaViewModel);
            }

            EventViewModel eventViewModel = @event;
            eventViewModel.EventAreas = eventAreaViewModels;
            return eventViewModel;
        }

        public async Task<EventViewModel> GetEventViewModelForEditAndDeleteAsync(EventDto @event)
        {
            await _usersClient.ConvertTimeFromUtcToUsers(@event);
            EventViewModel eventVm = @event;
            return eventVm;
        }
    }
}
