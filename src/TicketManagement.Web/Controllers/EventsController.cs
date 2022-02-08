using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.BusinessLogic.ModelsDTO;
using TicketManagement.Web.Models;
using TicketManagement.Web.Models.Events;
using TicketManagement.Web.Models.Tickets;

namespace TicketManagement.Web.Controllers
{
    /// <summary>
    /// Controller for events.
    /// </summary>
    [ResponseCache(CacheProfileName = "Caching")]
    public class EventsController : Controller
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
        /// Localizer object.
        /// </summary>
        private readonly IStringLocalizer<EventsController> _localizer;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventsController"/> class.
        /// </summary>
        /// <param name="service">EventService object.</param>
        /// <param name="ticketService">TicketService object.</param>
        /// <param name="eventAreaService">EventAreaService object.</param>
        /// <param name="eventSeatService">EventSeatService object.</param>
        /// <param name="userManager">UserManager object.</param>
        /// <param name="localizer">Localizer object.</param>
        public EventsController(IService<EventDto> service, IService<TicketDto> ticketService, IService<EventAreaDto> eventAreaService,
            IService<EventSeatDto> eventSeatService, UserManager<User> userManager, IStringLocalizer<EventsController> localizer)
        {
            _service = service;
            _ticketService = ticketService;
            _eventAreaService = eventAreaService;
            _eventSeatService = eventSeatService;
            _userManager = userManager;
            _localizer = localizer;
        }

        /// <summary>
        /// Get all events.
        /// </summary>
        /// <returns>Task with IActionResult.</returns>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var events = await _service.GetAllAsync();
            var eventsVm = new List<EventViewModel>();
            foreach (var @event in events)
            {
                eventsVm.Add(@event);
            }

            return View(eventsVm);
        }

        /// <summary>
        /// Details about event.
        /// </summary>
        /// <param name="id">Id of event.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _service.GetByIdAsync((int)id);
            if (@event == null)
            {
                return NotFound();
            }

            await ConvertTimeForUser(@event);
            var eventAreas = await _eventAreaService.GetAllAsync();
            var eventAreasForEvent = eventAreas.Where(x => x.EventId == (int)id);
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
            return View(eventViewModel);
        }

        /// <summary>
        /// Create event.
        /// </summary>
        /// <returns>IActionResult.</returns>
        [Authorize(Roles = "admin, event manager")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Create event.
        /// </summary>
        /// <param name="eventVm">Adding event.</param>
        /// <returns>Task with IActionResult.</returns>
        [Authorize(Roles = "admin, event manager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EventViewModel eventVm)
        {
            if (ModelState.IsValid)
            {
                EventDto @event = eventVm;
                await _service.CreateAsync(@event);
                return RedirectToAction(nameof(Index));
            }

            return View(eventVm);
        }

        /// <summary>
        /// Edit event.
        /// </summary>
        /// <param name="id">Id of editing event.</param>
        /// <returns>Task with IActionResult.</returns>
        [Authorize(Roles = "admin, event manager")]
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var updatingEvent = await _service.GetByIdAsync((int)id);
            if (updatingEvent == null)
            {
                return NotFound();
            }

            await ConvertTimeForUser(updatingEvent);
            EventViewModel eventVm = updatingEvent;
            return View(eventVm);
        }

        /// <summary>
        /// Edit event.
        /// </summary>
        /// <param name="id">Id of editing event.</param>
        /// <param name="eventVm">Edited event.</param>
        /// <returns>Task with IActionResult.</returns>
        [Authorize(Roles = "admin, event manager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EventViewModel eventVm)
        {
            if (id != eventVm.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                EventDto @event = eventVm;
                try
                {
                    await _service.UpdateAsync(@event);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await EventExists(@event.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            return View(eventVm);
        }

        /// <summary>
        /// Delete event.
        /// </summary>
        /// <param name="id">Id of deleting event.</param>
        /// <returns>Task with IActionResult.</returns>
        [Authorize(Roles = "admin, event manager")]
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deletingEvent = await _service.GetByIdAsync((int)id);
            if (deletingEvent == null)
            {
                return NotFound();
            }

            await ConvertTimeForUser(deletingEvent);
            EventViewModel eventVm = deletingEvent;
            return View(eventVm);
        }

        /// <summary>
        /// Delete confirmation.
        /// </summary>
        /// <param name="id">Id of deleting event.</param>
        /// <returns>Task with IActionResult.</returns>
        [Authorize(Roles = "admin, event manager")]
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @event = await _service.GetByIdAsync(id);
            await _service.DeleteAsync(@event);
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Buy ticket.
        /// </summary>
        /// <param name="eventSeatId">EventSeat id.</param>
        /// <param name="price">Price for ticket.</param>
        /// <returns>Task with IActionResult.</returns>
        [Authorize(Roles = "admin, user, event manager, venue manager")]
        [HttpGet]
        public async Task<IActionResult> Buy(int? eventSeatId, double? price)
        {
            if (eventSeatId == null || price == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(HttpContext.User);
            var ticket = new TicketDto
            {
                UserId = user.Id,
                EventSeatId = (int)eventSeatId,
            };

            TicketViewModel ticketVm = ticket;
            ticketVm.Price = (double)price;

            return View(ticketVm);
        }

        /// <summary>
        /// Buy confirmation.
        /// </summary>
        /// <param name="ticketVm">TicketViewModel object.</param>
        /// <returns>Task with IActionResult.</returns>
        [Authorize(Roles = "admin, user, event manager, venue manager")]
        [HttpPost]
        [ActionName("Buy")]
        public async Task<IActionResult> BuyConfirmed(TicketViewModel ticketVm)
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
            }
            else
            {
                return ValidationProblem($"{_localizer["NoBalance"]}");
            }

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Check that event exist.
        /// </summary>
        /// <param name="id">Id of event.</param>
        /// <returns>True if exist and false if not.</returns>
        private async Task<bool> EventExists(int id)
        {
            return await _service.GetByIdAsync(id) is not null;
        }

        /// <summary>
        /// Convert time from UTC to user time zone.
        /// </summary>
        /// <param name="event">Event object.</param>
        /// <returns>Task.</returns>
        private async Task ConvertTimeForUser(EventDto @event)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user is not null && !string.IsNullOrWhiteSpace(user.TimeZone))
            {
                var userTimeZone = TimeZoneInfo.FindSystemTimeZoneById(user.TimeZone);
                @event.TimeStart = TimeZoneInfo.ConvertTime(@event.TimeStart, TimeZoneInfo.Utc, userTimeZone);
                @event.TimeEnd = TimeZoneInfo.ConvertTime(@event.TimeEnd, TimeZoneInfo.Utc, userTimeZone);
            }
        }
    }
}
