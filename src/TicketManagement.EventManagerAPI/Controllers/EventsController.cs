using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketManagement.EventManagerAPI.Interfaces;
using TicketManagement.EventManagerAPI.Models.Events;
using TicketManagement.EventManagerAPI.ModelsDTO;

namespace TicketManagement.EventManagerAPI.Controllers
{
    /// <summary>
    /// Controller for events.
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class EventsController : Controller
    {
        /// <summary>
        /// EventService object.
        /// </summary>
        private readonly IService<EventDto> _eventService;

        /// <summary>
        /// EventWebService object.
        /// </summary>
        private readonly IEventService _eventWebService;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventsController"/> class.
        /// </summary>
        /// <param name="eventService">EventService object.</param>
        /// <param name="eventWebService">EventWebService object.</param>
        public EventsController(IService<EventDto> eventService, IEventService eventWebService)
        {
            _eventService = eventService;
            _eventWebService = eventWebService;
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

            return View(await _eventWebService.GetEventViewModelForEditAndDeleteAsync(updatingEvent, HttpContext));
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

            return View(await _eventWebService.GetEventViewModelForEditAndDeleteAsync(deletingEvent, HttpContext));
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
