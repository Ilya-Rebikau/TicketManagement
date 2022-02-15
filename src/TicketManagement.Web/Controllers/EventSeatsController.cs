using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.BusinessLogic.ModelsDTO;
using TicketManagement.Web.Models.EventSeats;

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
            var eventSeats = await _service.GetAllAsync();
            var eventSeatsVm = new List<EventSeatViewModel>();
            foreach (var eventSeat in eventSeats)
            {
                eventSeatsVm.Add(eventSeat);
            }

            return View(eventSeatsVm);
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

            EventSeatViewModel eventSeatVm = eventSeat;
            return View(eventSeatVm);
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

            EventSeatDto eventSeat = eventSeatVm;
            await _service.CreateAsync(eventSeat);
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
            if (id == null)
            {
                return NotFound();
            }

            var updatingEventSeat = await _service.GetByIdAsync((int)id);
            if (updatingEventSeat == null)
            {
                return NotFound();
            }

            EventSeatViewModel eventSeatVm = updatingEventSeat;
            return View(eventSeatVm);
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

            EventSeatDto eventSeat = eventSeatVm;
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

            EventSeatViewModel eventSeatVm = deletingEventSeat;
            return View(eventSeatVm);
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
