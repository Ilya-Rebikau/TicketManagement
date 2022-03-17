using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using TicketManagement.EventManagerAPI.Interfaces;
using TicketManagement.EventManagerAPI.Models.Events;
using TicketManagement.EventManagerAPI.ModelsDTO;

namespace TicketManagement.EventManagerAPI.
    Services
{
    /// <summary>
    /// Service for event controller.
    /// </summary>
    internal class EventServiceWeb : IEventService
    {
        /// <summary>
        /// EventService object.
        /// </summary>
        private readonly IService<EventDto> _service;

        /// <summary>
        /// EventAreaService object.
        /// </summary>
        private readonly IService<EventAreaDto> _eventAreaService;

        /// <summary>
        /// EventSeatService object.
        /// </summary>
        private readonly IService<EventSeatDto> _eventSeatService;

        ///// <summary>
        ///// Converter for time object.
        ///// </summary>

#pragma warning disable S125 // Sections of code should not be commented out

                            // private readonly ConverterForTime _converter;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventServiceWeb"/> class.
        /// </summary>
        /// <param name="eventService">EventService object.</param>
        /// <param name="eventAreaService">EventAreaService object.</param>
        /// <param name="eventSeatService">EventSeatService object.</param>
        /*/// <param name="converter">ConverterForTime object.</param>*/
        public EventServiceWeb(IService<EventDto> eventService,
#pragma warning restore S125 // Sections of code should not be commented out
            IService<EventAreaDto> eventAreaService,
            IService<EventSeatDto> eventSeatService)/*,
            ConverterForTime converter)*/
        {
            _service = eventService;
            _eventAreaService = eventAreaService;
            _eventSeatService = eventSeatService;

#pragma warning disable S125 // Sections of code should not be commented out
            /*_converter = converter;*/
        }
#pragma warning restore S125 // Sections of code should not be commented out

        public async Task<IEnumerable<EventViewModel>> GetAllEventViewModelsAsync()
        {
            var events = await _service.GetAllAsync();
            var eventsVm = new List<EventViewModel>();
            foreach (var @event in events)
            {
                eventsVm.Add(@event);
            }

            return eventsVm;
        }

        public async Task<EventViewModel> GetEventViewModelForDetailsAsync(EventDto @event, HttpContext httpContext)
        {
#pragma warning disable S125 // Sections of code should not be commented out
                            // await _converter.ConvertTimeForUser(@event, httpContext);
            var eventAreas = await _eventAreaService.GetAllAsync();
#pragma warning restore S125 // Sections of code should not be commented out
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

#pragma warning disable CS1998 // В асинхронном методе отсутствуют операторы await, будет выполнен синхронный метод
        public async Task<EventViewModel> GetEventViewModelForEditAndDeleteAsync(EventDto @event, HttpContext httpContext)
#pragma warning restore CS1998 // В асинхронном методе отсутствуют операторы await, будет выполнен синхронный метод
        {
#pragma warning disable S125 // Sections of code should not be commented out
                            // await _converter.ConvertTimeForUser(@event, httpContext);
            EventViewModel eventVm = @event;
#pragma warning restore S125 // Sections of code should not be commented out
            return eventVm;
        }
    }
}
