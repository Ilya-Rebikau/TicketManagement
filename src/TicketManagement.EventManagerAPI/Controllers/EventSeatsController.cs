using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketManagement.EventManagerAPI.Interfaces;
using TicketManagement.EventManagerAPI.Models.EventSeats;
using TicketManagement.EventManagerAPI.ModelsDTO;

namespace TicketManagement.EventManagerAPI.Controllers
{
    /// <summary>
    /// Controller for event seats.
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    [Authorize(Roles = "admin, event manager")]
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
        [HttpGet("geteventseats")]
        public async Task<IActionResult> GetEventSeats()
        {
            var eventSeats = await _service.GetAllAsync();
            var eventSeatsModels = new List<EventSeatModel>();
            foreach (var eventSeat in eventSeats)
            {
                eventSeatsModels.Add(eventSeat);
            }

            return Ok(eventSeatsModels);
        }

        /// <summary>
        /// Details about event seat.
        /// </summary>
        /// <param name="id">Id of event seat.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpGet("details/{id}")]
        public async Task<IActionResult> Details([FromRoute] int id)
        {
            var eventSeat = await _service.GetByIdAsync(id);
            return eventSeat is null ? NotFound() : Ok(eventSeat);
        }

        /// <summary>
        /// Create event seat.
        /// </summary>
        /// <param name="eventSeat">Adding event seat.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] EventSeatModel eventSeat)
        {
            await _service.CreateAsync(eventSeat);
            return Ok();
        }

        /// <summary>
        /// Edit event seat.
        /// </summary>
        /// <param name="id">Id of editing event seat.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpGet("edit/{id}")]
        public async Task<IActionResult> Edit([FromRoute] int id)
        {
            var updatingEventSeat = await _service.GetByIdAsync(id);
            return updatingEventSeat is null ? NotFound() : Ok(updatingEventSeat);
        }

        /// <summary>
        /// Edit event seat.
        /// </summary>
        /// <param name="id">Id of editing event seat.</param>
        /// <param name="eventSeat">Edited event seat.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpPut("edit/{id}")]
        public async Task<IActionResult> Edit([FromRoute] int id, [FromBody] EventSeatModel eventSeat)
        {
            if (id != eventSeat.Id)
            {
                return NotFound();
            }

            try
            {
                await _service.UpdateAsync(eventSeat);
                return Ok();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await EventSeatExists(eventSeat.Id))
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
        /// Delete event seat.
        /// </summary>
        /// <param name="id">Id of deleting event seat.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpGet("delete/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var deletingEventSeat = await _service.GetByIdAsync(id);
            return deletingEventSeat is null ? NotFound() : Ok(deletingEventSeat);
        }

        /// <summary>
        /// Delete confirmation.
        /// </summary>
        /// <param name="id">Id of deleting event seat.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpDelete("delete/{id}")]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed([FromRoute] int id)
        {
            await _service.DeleteById(id);
            return Ok();
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
