using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketManagement.Web.Extensions;
using TicketManagement.Web.Infrastructure;
using TicketManagement.Web.Interfaces.HttpClients;
using TicketManagement.Web.Models;
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
        /// IEventManagerClient object.
        /// </summary>
        private readonly IEventManagerClient _eventManagerClient;

        /// <summary>
        /// IPurchaseClient object.
        /// </summary>
        private readonly IPurchaseFlowClient _purchaseClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventsController"/> class.
        /// </summary>
        /// <param name="eventManagerClient">IEventManagerClient object.</param>
        /// <param name="purchaseClient">IPurchaseClient object.</param>
        public EventsController(IEventManagerClient eventManagerClient, IPurchaseFlowClient purchaseClient)
        {
            _eventManagerClient = eventManagerClient;
            _purchaseClient = purchaseClient;
        }

        /// <summary>
        /// Get all events.
        /// </summary>
        /// <param name="pageNumber">Page number.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpGet]
        public async Task<IActionResult> Index(int pageNumber = 1)
        {
            var events = await _eventManagerClient.GetEventViewModels(HttpContext.GetJwtToken(), pageNumber);
            var nextEvents = await _eventManagerClient.GetEventViewModels(HttpContext.GetJwtToken(), pageNumber + 1);
            PageViewModel.NextPage = nextEvents is not null && nextEvents.Any();
            PageViewModel.PageNumber = pageNumber;
            return View(events);
        }

        /// <summary>
        /// Details about event.
        /// </summary>
        /// <param name="id">Id of event.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            return id is null ? NotFound() : View(await _eventManagerClient.EventDetails(HttpContext.GetJwtToken(), (int)id));
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

            await _eventManagerClient.CreateEvent(HttpContext.GetJwtToken(), eventVm);
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
            return id is null ? NotFound() : View(await _eventManagerClient.GetEventViewModelForEdit(HttpContext.GetJwtToken(), (int)id));
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

            try
            {
                await _eventManagerClient.EditEvent(HttpContext.GetJwtToken(), id, eventVm);
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict();
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
            return id is null ? NotFound() : View(await _eventManagerClient.GetEventViewModelForDelete(HttpContext.GetJwtToken(), (int)id));
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
            await _eventManagerClient.DeleteEvent(HttpContext.GetJwtToken(), id);
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Buy ticket.
        /// </summary>
        /// <param name="eventSeatId">Event seat id.</param>
        /// <param name="price">Price for place.</param>
        /// <returns>TicketViewModel.</returns>
        [HttpGet("events/buy")]
        public async Task<IActionResult> Buy(int eventSeatId, double price)
        {
            var eventSeatIdAndPrice = new Dictionary<int, double>
            {
                { eventSeatId, price },
            };
            return View(await _purchaseClient.GetTicketViewModelForBuy(HttpContext.GetJwtToken(), eventSeatIdAndPrice));
        }

        /// <summary>
        /// Buy ticket.
        /// </summary>
        /// <param name="ticketVm">Ticket view model.</param>
        /// <returns>IActionResult.</returns>
        [HttpPost("events/buy")]
        public async Task<IActionResult> Buy(TicketViewModel ticketVm)
        {
            await _purchaseClient.UpdateEventSeatStateAfterBuyingTicket(HttpContext.GetJwtToken(), ticketVm);
            return RedirectToAction(nameof(Index), typeof(EventsController).GetControllerName());
        }
    }
}
