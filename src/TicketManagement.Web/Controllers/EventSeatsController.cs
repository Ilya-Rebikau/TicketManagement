using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.BusinessLogic.ModelsDTO;

namespace TicketManagement.Web.Controllers
{
    /// <summary>
    /// Controller for event seats.
    /// </summary>
    [Authorize(Roles = "admin, event manager")]
    [ResponseCache(CacheProfileName = "Caching")]
    public class EventSeatsController : Controller
    {
        /// <summary>
        /// EventSeatService object.
        /// </summary>
        private readonly IService<EventSeatDto> _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventSeatsController"/> class.
        /// </summary>
        /// <param name="service">EventSeatService object.</param>
        public EventSeatsController(IService<EventSeatDto> service)
        {
            _service = service;
        }

        /// <summary>
        /// Get all event seats.
        /// </summary>
        /// <returns>Task with IActionResult.</returns>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _service.GetAllAsync());
        }

        /// <summary>
        /// Details about event seat.
        /// </summary>
        /// <param name="id">Id of event seat.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventSeat = await _service.GetByIdAsync((int)id);
            if (eventSeat == null)
            {
                return NotFound();
            }

            return View(eventSeat);
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
        /// <param name="eventSeat">Adding event seat.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EventSeatDto eventSeat)
        {
            if (ModelState.IsValid)
            {
                await _service.CreateAsync(eventSeat);
                return RedirectToAction(nameof(Index));
            }

            return View(eventSeat);
        }

        /// <summary>
        /// Edit event seat.
        /// </summary>
        /// <param name="id">Id of editing event seat.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var updatingEventSeat = await _service.GetByIdAsync((int)id);
            if (updatingEventSeat == null)
            {
                return NotFound();
            }

            return View(updatingEventSeat);
        }

        /// <summary>
        /// Edit event seat.
        /// </summary>
        /// <param name="id">Id of editing event seat.</param>
        /// <param name="eventSeat">Edited event seat.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EventSeatDto eventSeat)
        {
            if (id != eventSeat.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _service.UpdateAsync(eventSeat);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await EventSeatExists(eventSeat.Id))
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

            return View(eventSeat);
        }

        /// <summary>
        /// Delete event seat.
        /// </summary>
        /// <param name="id">Id of deleting event seat.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deletingEventSeat = await _service.GetByIdAsync((int)id);
            if (deletingEventSeat == null)
            {
                return NotFound();
            }

            return View(deletingEventSeat);
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
            var eventSeat = await _service.GetByIdAsync(id);
            await _service.DeleteAsync(eventSeat);
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Check that event seat exist.
        /// </summary>
        /// <param name="id">Id of event seat.</param>
        /// <returns>True if exist and false if not.</returns>
        private async Task<bool> EventSeatExists(int id)
        {
            return await _service.GetByIdAsync(id) is not null;
        }
    }
}
