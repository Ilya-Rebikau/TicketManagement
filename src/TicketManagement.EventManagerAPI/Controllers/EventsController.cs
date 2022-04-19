using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketManagement.EventManagerAPI.Extensions;
using TicketManagement.EventManagerAPI.Interfaces;
using TicketManagement.EventManagerAPI.Models.Events;

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
        /// <param name="pageNumber">Page number.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpGet("getevents")]
        public async Task<IActionResult> GetEvents([FromQuery] int pageNumber)
        {
            return Ok(await _eventService.GetAllEventViewModelsAsync(pageNumber));
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
            return @event is null ? NotFound() : Ok(await _eventService.GetEventViewModelForDetailsAsync(@event, HttpContext.GetJwtToken()));
        }

        /// <summary>
        /// Create event.
        /// </summary>
        /// <param name="event">Adding event.</param>
        /// <returns>Task with IActionResult.</returns>
        [Authorize(Roles = "admin, event manager")]
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] EventModel @event)
        {
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
            return updatingEvent is null ? NotFound() : Ok(await _eventService.GetEventViewModelForEditAndDeleteAsync(updatingEvent, HttpContext.GetJwtToken()));
        }

        /// <summary>
        /// Edit event.
        /// </summary>
        /// <param name="id">Id of editing event.</param>
        /// <param name="event">Edited event.</param>
        /// <returns>Task with IActionResult.</returns>
        [Authorize(Roles = "admin, event manager")]
        [HttpPut("edit/{id}")]
        public async Task<IActionResult> Edit([FromRoute] int id, [FromBody] EventModel @event)
        {
            if (id != @event.Id)
            {
                return NotFound();
            }

            try
            {
                await _eventService.UpdateAsync(@event);
                return Ok();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await EventExists(@event.Id))
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
            return deletingEvent is null ? NotFound() : Ok(await _eventService.GetEventViewModelForEditAndDeleteAsync(deletingEvent, HttpContext.GetJwtToken()));
        }

        /// <summary>
        /// Delete confirmation.
        /// </summary>
        /// <param name="id">Id of deleting event.</param>
        /// <returns>Task with IActionResult.</returns>
        [Authorize(Roles = "admin, event manager")]
        [HttpDelete("delete/{id}")]
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
