using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.BusinessLogic.ModelsDTO;
using TicketManagement.Web.Interfaces;
using TicketManagement.Web.Models;
using TicketManagement.Web.Models.Events;
using TicketManagement.Web.Models.Tickets;

namespace TicketManagement.Web.WebServices
{
    /// <summary>
    /// Web service for event controller.
    /// </summary>
    internal class EventWebService : IEventWebService
    {
        /// <summary>
        /// EventService object.
        /// </summary>
        private readonly IService<EventDto> _service;

        /// <summary>
        /// TicketService object.
        /// </summary>
        private readonly IService<TicketDto> _ticketService;

        /// <summary>
        /// EventAreaService object.
        /// </summary>
        private readonly IService<EventAreaDto> _eventAreaService;

        /// <summary>
        /// EventSeatService object.
        /// </summary>
        private readonly IService<EventSeatDto> _eventSeatService;

        /// <summary>
        /// UserManager object.
        /// </summary>
        private readonly UserManager<User> _userManager;

        /// <summary>
        /// Converter for time object.
        /// </summary>
        private readonly ConverterForTime _converter;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventWebService"/> class.
        /// </summary>
        /// <param name="eventService">EventService object.</param>
        /// <param name="ticketService">TicketService object.</param>
        /// <param name="eventAreaService">EventAreaService object.</param>
        /// <param name="eventSeatService">EventSeatService object.</param>
        /// <param name="userManager">UserManager object.</param>
        /// <param name="converter">ConverterForTime object.</param>
        public EventWebService(IService<EventDto> eventService,
            IService<TicketDto> ticketService,
            IService<EventAreaDto> eventAreaService,
            IService<EventSeatDto> eventSeatService,
            UserManager<User> userManager,
            ConverterForTime converter)
        {
            _service = eventService;
            _ticketService = ticketService;
            _eventAreaService = eventAreaService;
            _eventSeatService = eventSeatService;
            _userManager = userManager;
            _converter = converter;
        }

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
            await _converter.ConvertTimeForUser(@event, httpContext);
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

        public async Task<EventViewModel> GetEventViewModelForEditAndDeleteAsync(EventDto @event, HttpContext httpContext)
        {
            await _converter.ConvertTimeForUser(@event, httpContext);
            EventViewModel eventVm = @event;
            return eventVm;
        }

        public async Task<TicketViewModel> GetTicketViewModelForBuyAsync(int? eventSeatId, double? price, HttpContext httpContext)
        {
            var user = await _userManager.GetUserAsync(httpContext.User);
            var ticket = new TicketDto
            {
                UserId = user.Id,
                EventSeatId = (int)eventSeatId,
            };

            TicketViewModel ticketVm = ticket;
            ticketVm.Price = (double)price;
            return ticketVm;
        }

        public async Task<bool> UpdateEventSeatStateAfterBuyingTicket(TicketViewModel ticketVm)
        {
            TicketDto ticket = ticketVm;
            var user = await _userManager.FindByIdAsync(ticket.UserId);
            if (user.Balance >= ticketVm.Price)
            {
                user.Balance -= ticketVm.Price;
                var seat = await _eventSeatService.GetByIdAsync(ticket.EventSeatId);
                seat.State = PlaceStatus.Occupied;
                await _eventSeatService.UpdateAsync(seat);
                await _ticketService.CreateAsync(ticket);
                return true;
            }

            return false;
        }
    }
}
