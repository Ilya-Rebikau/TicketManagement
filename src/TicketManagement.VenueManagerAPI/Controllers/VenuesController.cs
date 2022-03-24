using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketManagement.VenueManagerAPI.Interfaces;
using TicketManagement.VenueManagerAPI.Models.Venues;
using TicketManagement.VenueManagerAPI.ModelsDTO;

namespace TicketManagement.VenueManagerAPI.Controllers
{
    /// <summary>
    /// Controller for venue.
    /// </summary>
    [Authorize(Roles = "admin, venue manager")]
    [Route("[controller]")]
    [ApiController]
    public class VenuesController : Controller
    {
        /// <summary>
        /// VenueService object.
        /// </summary>
        private readonly IService<VenueDto> _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="VenuesController"/> class.
        /// </summary>
        /// <param name="service">VenueService object.</param>
        public VenuesController(IService<VenueDto> service)
        {
            _service = service;
        }

        /// <summary>
        /// Get all venues.
        /// </summary>
        /// <returns>Task with IActionResult.</returns>
        [HttpGet("getvenues")]
        public async Task<IActionResult> GetVenues()
        {
            var venues = await _service.GetAllAsync();
            var venueModels = new List<VenueModel>();
            foreach (var venue in venues)
            {
                venueModels.Add(venue);
            }

            return Ok(venueModels);
        }

        /// <summary>
        /// Details about venue.
        /// </summary>
        /// <param name="id">Id of venue.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpGet("details/{id}")]
        public async Task<IActionResult> Details([FromRoute] int id)
        {
            var venue = await _service.GetByIdAsync(id);
            return venue is null ? NotFound() : Ok(venue);
        }

        /// <summary>
        /// Create venue.
        /// </summary>
        /// <param name="venue">Adding venue.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] VenueModel venue)
        {
            await _service.CreateAsync(venue);
            return Ok();
        }

        /// <summary>
        /// Edit venue.
        /// </summary>
        /// <param name="id">Id of editing venue.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpGet("edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var updatingVenue = await _service.GetByIdAsync(id);
            return updatingVenue is null ? NotFound() : Ok(updatingVenue);
        }

        /// <summary>
        /// Edit venue.
        /// </summary>
        /// <param name="id">Id of editing venue.</param>
        /// <param name="venueVm">Edited venue.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpPut("edit/{id}")]
        public async Task<IActionResult> Edit([FromRoute] int id, [FromBody] VenueModel venueVm)
        {
            if (id != venueVm.Id)
            {
                return NotFound();
            }

            try
            {
                await _service.UpdateAsync(venueVm);
                return Ok();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await VenueExists(venueVm.Id))
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
        /// Delete venue.
        /// </summary>
        /// <param name="id">Id of deleting venue.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpGet("delete/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var deletingVenue = await _service.GetByIdAsync(id);
            return deletingVenue is null ? NotFound() : Ok(deletingVenue);
        }

        /// <summary>
        /// Delete confirmation.
        /// </summary>
        /// <param name="id">Id of deleting venue.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpDelete("delete/{id}")]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed([FromRoute] int id)
        {
            await _service.DeleteById(id);
            return Ok();
        }

        /// <summary>
        /// Check that venue exists.
        /// </summary>
        /// <param name="id">Id of venue.</param>
        /// <returns>True if exists and false if not.</returns>
        private async Task<bool> VenueExists(int id)
        {
            return await _service.GetByIdAsync(id) is not null;
        }
    }
}
