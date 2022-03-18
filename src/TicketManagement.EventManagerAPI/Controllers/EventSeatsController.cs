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
            var eventSeatsVm = new List<EventSeatViewModel>();
            foreach (var eventSeat in eventSeats)
            {
                eventSeatsVm.Add(eventSeat);
            }

            return Ok(eventSeatsVm);
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
            if (eventSeat == null)
            {
                return NotFound();
            }

            EventSeatViewModel eventSeatVm = eventSeat;
            return Ok(eventSeatVm);
        }

        /// <summary>
        /// Create event seat.
        /// </summary>
        /// <param name="eventSeatVm">Adding event seat.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] EventSeatViewModel eventSeatVm)
        {
            EventSeatDto eventSeat = eventSeatVm;
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
            if (updatingEventSeat == null)
            {
                return NotFound();
            }

            EventSeatViewModel eventSeatVm = updatingEventSeat;
            return Ok(eventSeatVm);
        }

        /// <summary>
        /// Edit event seat.
        /// </summary>
        /// <param name="id">Id of editing event seat.</param>
        /// <param name="eventSeatVm">Edited event seat.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpPost("edit/{id}")]
        public async Task<IActionResult> Edit([FromRoute] int id, [FromBody] EventSeatViewModel eventSeatVm)
        {
            if (id != eventSeatVm.Id)
            {
                return NotFound();
            }

            EventSeatDto eventSeat = eventSeatVm;
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
            if (deletingEventSeat == null)
            {
                return NotFound();
            }

            EventSeatViewModel eventSeatVm = deletingEventSeat;
            return Ok(eventSeatVm);
        }

        /// <summary>
        /// Delete confirmation.
        /// </summary>
        /// <param name="id">Id of deleting event seat.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpPost("delete/{id}")]
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
