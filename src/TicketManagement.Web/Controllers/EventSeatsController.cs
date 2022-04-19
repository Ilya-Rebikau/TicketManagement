using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketManagement.Web.Extensions;
using TicketManagement.Web.Infrastructure;
using TicketManagement.Web.Interfaces.HttpClients;
using TicketManagement.Web.Models;
using TicketManagement.Web.Models.EventSeats;

namespace TicketManagement.Web.Controllers
{
    /// <summary>
    /// Controller for event seats.
    /// </summary>
    [Authorize(Roles = "admin, event manager")]
    [ResponseCache(CacheProfileName = "Caching")]
    [ExceptionFilter]
    public class EventSeatsController : Controller
    {
        /// <summary>
        /// IEventManagerClient object.
        /// </summary>
        private readonly IEventManagerClient _eventManagerClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventSeatsController"/> class.
        /// </summary>
        /// <param name="eventManagerClient">IEventManagerClient object.</param>
        public EventSeatsController(IEventManagerClient eventManagerClient)
        {
            _eventManagerClient = eventManagerClient;
        }

        /// <summary>
        /// Get all event seats.
        /// </summary>
        /// <param name="pageNumber">Page number.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpGet]
        [RedirectFilter("eventseats")]
        public async Task<IActionResult> Index(int pageNumber = 1)
        {
            var eventSeats = await _eventManagerClient.GetEventSeatsViewModels(HttpContext.GetJwtToken(), pageNumber);
            var nextEventSeats = await _eventManagerClient.GetEventSeatsViewModels(HttpContext.GetJwtToken(), pageNumber + 1);
            PageViewModel.NextPage = nextEventSeats is not null && nextEventSeats.Any();
            PageViewModel.PageNumber = pageNumber;
            return View(eventSeats);
        }

        /// <summary>
        /// Details about event seat.
        /// </summary>
        /// <param name="id">Id of event seat.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            return id is null ? NotFound() : View(await _eventManagerClient.EventSeatDetails(HttpContext.GetJwtToken(), (int)id));
        }

        /// <summary>
        /// Create event seat.
        /// </summary>
        /// <returns>IActionResult.</returns>
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Create event seat.
        /// </summary>
        /// <param name="eventSeatVm">Adding event seat.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EventSeatViewModel eventSeatVm)
        {
            if (!ModelState.IsValid)
            {
                return View(eventSeatVm);
            }

            await _eventManagerClient.CreateEventSeat(HttpContext.GetJwtToken(), eventSeatVm);
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Edit event seat.
        /// </summary>
        /// <param name="id">Id of editing event seat.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            return id is null ? NotFound() : View(await _eventManagerClient.GetEventSeatViewModelForEdit(HttpContext.GetJwtToken(), (int)id));
        }

        /// <summary>
        /// Edit event seat.
        /// </summary>
        /// <param name="id">Id of editing event seat.</param>
        /// <param name="eventSeatVm">Edited event seat.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EventSeatViewModel eventSeatVm)
        {
            if (id != eventSeatVm.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(eventSeatVm);
            }

            try
            {
                await _eventManagerClient.EditEventSeat(HttpContext.GetJwtToken(), id, eventSeatVm);
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict();
            }

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Delete event seat.
        /// </summary>
        /// <param name="id">Id of deleting event seat.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            return id is null ? NotFound() : View(await _eventManagerClient.GetEventSeatViewModelForDelete(HttpContext.GetJwtToken(), (int)id));
        }

        /// <summary>
        /// Delete confirmation.
        /// </summary>
        /// <param name="id">Id of deleting event seat.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _eventManagerClient.DeleteEventSeat(HttpContext.GetJwtToken(), id);
            return RedirectToAction(nameof(Index));
        }
    }
}
