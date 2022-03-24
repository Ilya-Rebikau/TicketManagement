using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketManagement.VenueManagerAPI.Interfaces;
using TicketManagement.VenueManagerAPI.Models.Seats;
using TicketManagement.VenueManagerAPI.ModelsDTO;

namespace TicketManagement.VenueManagerAPI.Controllers
{
    /// <summary>
    /// Controller for seats.
    /// </summary>
    [Authorize(Roles = "admin, venue manager")]
    [Route("[controller]")]
    [ApiController]
    public class SeatsController : Controller
    {
        /// <summary>
        /// SeatService object.
        /// </summary>
        private readonly IService<SeatDto> _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="SeatsController"/> class.
        /// </summary>
        /// <param name="service">SeatService object.</param>
        public SeatsController(IService<SeatDto> service)
        {
            _service = service;
        }

        /// <summary>
        /// Get all sets.
        /// </summary>
        /// <returns>Task with IActionResult.</returns>
        [HttpGet("getseats")]
        public async Task<IActionResult> GetSeats()
        {
            var seats = await _service.GetAllAsync();
            var seatModels = new List<SeatModel>();
            foreach (var seat in seats)
            {
                seatModels.Add(seat);
            }

            return Ok(seatModels);
        }

        /// <summary>
        /// Details about seat.
        /// </summary>
        /// <param name="id">Id of seat.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpGet("details/{id}")]
        public async Task<IActionResult> Details([FromRoute] int id)
        {
            var seat = await _service.GetByIdAsync(id);
            return seat is null ? NotFound() : Ok(seat);
        }

        /// <summary>
        /// Create seat.
        /// </summary>
        /// <param name="seat">Adding seat.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] SeatModel seat)
        {
            await _service.CreateAsync(seat);
            return Ok();
        }

        /// <summary>
        /// Edit seat.
        /// </summary>
        /// <param name="id">Id of editing seat.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpGet("edit/{id}")]
        public async Task<IActionResult> Edit([FromRoute] int id)
        {
            var updatingSeat = await _service.GetByIdAsync(id);
            return updatingSeat is null ? NotFound() : Ok(updatingSeat);
        }

        /// <summary>
        /// Edit seat.
        /// </summary>
        /// <param name="id">Id of editing seat.</param>
        /// <param name="seatVm">Edited seat.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpPut("edit/{id}")]
        public async Task<IActionResult> Edit([FromRoute] int id, [FromBody] SeatModel seatVm)
        {
            if (id != seatVm.Id)
            {
                return NotFound();
            }

            try
            {
                await _service.UpdateAsync(seatVm);
                return Ok();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await SeatExists(seatVm.Id))
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
        /// Delete seat.
        /// </summary>
        /// <param name="id">Id of deleting seat.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpGet("delete/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var deletingSeat = await _service.GetByIdAsync(id);
            return deletingSeat is null ? NotFound() : Ok(deletingSeat);
        }

        /// <summary>
        /// Delete confirmation.
        /// </summary>
        /// <param name="id">Id of deleting seat.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpDelete("delete/{id}")]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed([FromRoute] int id)
        {
            await _service.DeleteById(id);
            return Ok();
        }

        /// <summary>
        /// Check that seat exists.
        /// </summary>
        /// <param name="id">Id of seat.</param>
        /// <returns>True if exists and false if not.</returns>
        private async Task<bool> SeatExists(int id)
        {
            return await _service.GetByIdAsync(id) is not null;
        }
    }
}
