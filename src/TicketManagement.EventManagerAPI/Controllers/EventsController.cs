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
    [Authorize(Roles = "admin, event manager")]
    [Route("[controller]")]
    [ApiController]
    public class EventsController : Controller
    {
        /// <summary>
        /// EventService object.
        /// </summary>
        private readonly IEventService _eventService;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventsController"/> class.
        /// </summary>
        /// <param name="eventService">EventService object.</param>
        public EventsController(IEventService eventService)
        {
            _eventService = eventService;
        }

        /// <summary>
        /// Get all events.
        /// </summary>
        /// <returns>Task with IActionResult.</returns>
        [HttpGet("getevents")]
        public async Task<IActionResult> GetEvents()
        {
            return Ok(await _eventService.GetAllEventViewModelsAsync());
        }

        /// <summary>
        /// Details about event.
        /// </summary>
        /// <param name="id">Id of event.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpGet("details/{id}")]
        public async Task<IActionResult> Details([FromRoute] int id)
        {
            var @event = await _eventService.GetByIdAsync(id);
            if (@event == null)
            {
                return NotFound();
            }

            return Ok(await _eventService.GetEventViewModelForDetailsAsync(@event));
        }

        /// <summary>
        /// Create event.
        /// </summary>
        /// <param name="eventVm">Adding event.</param>
        /// <returns>Task with IActionResult.</returns>
        [Authorize(Roles = "admin, event manager")]
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] EventViewModel eventVm)
        {
            if (!ModelState.IsValid)
            {
                return View(eventVm);
            }

            EventDto @event = eventVm;
            await _eventService.CreateAsync(@event);
            return Ok();
        }

        /// <summary>
        /// Edit event.
        /// </summary>
        /// <param name="id">Id of editing event.</param>
        /// <returns>Task with IActionResult.</returns>
        [Authorize(Roles = "admin, event manager")]
        [HttpGet("edit/{id}")]
        public async Task<IActionResult> Edit([FromRoute] int id)
        {
            var updatingEvent = await _eventService.GetByIdAsync(id);
            if (updatingEvent == null)
            {
                return NotFound();
            }

            return Ok(await _eventService.GetEventViewModelForEditAndDeleteAsync(updatingEvent));
        }

        /// <summary>
        /// Edit event.
        /// </summary>
        /// <param name="id">Id of editing event.</param>
        /// <param name="eventVm">Edited event.</param>
        /// <returns>Task with IActionResult.</returns>
        [Authorize(Roles = "admin, event manager")]
        [HttpPost("edit/{id}")]
        public async Task<IActionResult> Edit([FromRoute] int id, [FromBody] EventViewModel eventVm)
        {
            if (id != eventVm.Id)
            {
                return NotFound();
            }

            try
            {
                await _eventService.UpdateAsync(eventVm);
                return Ok();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await EventExists(eventVm.Id))
                {
                    return NotFound();
                }
                else
                {
                    return Conflict();
                }
            }
        }

        /// <summary>
        /// Delete event.
        /// </summary>
        /// <param name="id">Id of deleting event.</param>
        /// <returns>Task with IActionResult.</returns>
        [Authorize(Roles = "admin, event manager")]
        [HttpGet("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deletingEvent = await _eventService.GetByIdAsync(id);
            if (deletingEvent == null)
            {
                return NotFound();
            }

            return Ok(await _eventService.GetEventViewModelForEditAndDeleteAsync(deletingEvent));
        }

        /// <summary>
        /// Delete confirmation.
        /// </summary>
        /// <param name="id">Id of deleting event.</param>
        /// <returns>Task with IActionResult.</returns>
        [Authorize(Roles = "admin, event manager")]
        [HttpPost("delete/{id}")]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _eventService.DeleteById(id);
            return Ok();
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
