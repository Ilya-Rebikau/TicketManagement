using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.BusinessLogic.ModelsDTO;
using TicketManagement.Web.Infrastructure;
using TicketManagement.Web.Interfaces;
using TicketManagement.Web.Models.Events;
using TicketManagement.Web.Models.Tickets;

namespace TicketManagement.Web.Controllers
{
    /// <summary>
    /// Controller for events.
    /// </summary>
    [ResponseCache(CacheProfileName = "Caching")]
    [ExceptionFilter]
    public class EventsController : Controller
    {
        /// <summary>
        /// Const for showing error with low balance for buying ticket from resource file.
        /// </summary>
        private const string NoBalance = "NoBalance";

        /// <summary>
        /// EventService object.
        /// </summary>
        private readonly IService<EventDto> _eventService;

        /// <summary>
        /// EventWebService object.
        /// </summary>
        private readonly IEventWebService _eventWebService;

        /// <summary>
        /// Localizer object.
        /// </summary>
        private readonly IStringLocalizer<EventsController> _localizer;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventsController"/> class.
        /// </summary>
        /// <param name="eventService">EventService object.</param>
        /// <param name="eventWebService">EventWebService object.</param>
        /// <param name="localizer">Localizer object.</param>
        public EventsController(IService<EventDto> eventService, IEventWebService eventWebService, IStringLocalizer<EventsController> localizer)
        {
            _eventService = eventService;
            _eventWebService = eventWebService;
            _localizer = localizer;
        }

        /// <summary>
        /// Get all events.
        /// </summary>
        /// <returns>Task with IActionResult.</returns>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _eventWebService.GetAllEventViewModelsAsync());
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

            var @event = await _eventService.GetByIdAsync((int)id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(await _eventWebService.GetEventViewModelForDetailsAsync(@event, HttpContext));
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
            if (!ModelState.IsValid)
            {
                return View(eventVm);
            }

            EventDto @event = eventVm;
            await _eventService.CreateAsync(@event);
            return RedirectToAction(nameof(Index));
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

            var updatingEvent = await _eventService.GetByIdAsync((int)id);
            if (updatingEvent == null)
            {
                return NotFound();
            }

            return View(_eventWebService.GetEventViewModelForEditAndDeleteAsync(updatingEvent, HttpContext));
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

            if (!ModelState.IsValid)
            {
                return View(eventVm);
            }

            EventDto @event = eventVm;
            try
            {
                await _eventService.UpdateAsync(@event);
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

            var deletingEvent = await _eventService.GetByIdAsync((int)id);
            if (deletingEvent == null)
            {
                return NotFound();
            }

            return View(_eventWebService.GetEventViewModelForEditAndDeleteAsync(deletingEvent, HttpContext));
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
            await _eventService.DeleteById(id);
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

            return View(await _eventWebService.GetTicketViewModelForBuyAsync(eventSeatId, price, HttpContext));
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
            if (await _eventWebService.UpdateEventSeatStateAfterBuyingTicket(ticketVm))
            {
                return RedirectToAction(nameof(Index));
            }

            throw new InvalidOperationException(_localizer[NoBalance]);
        }

        /// <summary>
        /// Check that event exist.
        /// </summary>
        /// <param name="id">Id of event.</param>
        /// <returns>True if exist and false if not.</returns>
        private async Task<bool> EventExists(int id)
        {
            return await _eventService.GetByIdAsync(id) is not null;
        }
    }
}
