using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketManagement.VenueManagerAPI.Interfaces;
using TicketManagement.VenueManagerAPI.Models.Areas;
using TicketManagement.VenueManagerAPI.ModelsDTO;

namespace TicketManagement.VenueManagerAPI.Controllers
{
    /// <summary>
    /// Controller for areas.
    /// </summary>
    [Authorize(Roles = "admin, venue manager")]
    [Route("[controller]")]
    [ApiController]
    public class AreasController : Controller
    {
        /// <summary>
        /// AreaService object.
        /// </summary>
        private readonly IService<AreaDto> _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="AreasController"/> class.
        /// </summary>
        /// <param name="service">AreaService object.</param>
        public AreasController(IService<AreaDto> service)
        {
            _service = service;
        }

        /// <summary>
        /// All areas.
        /// </summary>
        /// <returns>Task with IActionResult.</returns>
        [HttpGet("getareas")]
        public async Task<IActionResult> GetAreas()
        {
            var areas = await _service.GetAllAsync();
            var areaModels = new List<AreaModel>();
            foreach (var area in areas)
            {
                areaModels.Add(area);
            }

            return Ok(areaModels);
        }

        /// <summary>
        /// Details about chosen area.
        /// </summary>
        /// <param name="id">Id of chosen area.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpGet("details/{id}")]
        public async Task<IActionResult> Details([FromRoute] int id)
        {
            var area = await _service.GetByIdAsync(id);
            return area is null ? NotFound() : Ok(area);
        }

        /// <summary>
        /// Create area.
        /// </summary>
        /// <param name="areaVm">Creating area.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] AreaModel areaVm)
        {
            await _service.CreateAsync(areaVm);
            return Ok();
        }

        /// <summary>
        /// Edit area.
        /// </summary>
        /// <param name="id">Id of editing area.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpGet("edit/{id}")]
        public async Task<IActionResult> Edit([FromRoute] int id)
        {
            var updatingArea = await _service.GetByIdAsync(id);
            return updatingArea is null ? NotFound() : Ok(updatingArea);
        }

        /// <summary>
        /// Edit area.
        /// </summary>
        /// <param name="id">Id of editing area.</param>
        /// <param name="areaVm">Editing area.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpPut("edit/{id}")]
        public async Task<IActionResult> Edit([FromRoute] int id, [FromBody] AreaModel areaVm)
        {
            if (id != areaVm.Id)
            {
                return NotFound();
            }

            try
            {
                await _service.UpdateAsync(areaVm);
                return Ok();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await AreaExists(areaVm.Id))
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
        /// Delete area.
        /// </summary>
        /// <param name="id">Id of deleting area.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpGet("delete/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var deletingArea = await _service.GetByIdAsync(id);
            return deletingArea is null ? NotFound() : Ok(deletingArea);
        }

        /// <summary>
        /// Delete confirmation.
        /// </summary>
        /// <param name="id">Id of deleting area.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpDelete("delete/{id}")]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed([FromRoute] int id)
        {
            await _service.DeleteById(id);
            return Ok();
        }

        /// <summary>
        /// Check that area exist.
        /// </summary>
        /// <param name="id">Id of deleting area.</param>
        /// <returns>True if exists and false if not.</returns>
        private async Task<bool> AreaExists(int id)
        {
            return await _service.GetByIdAsync(id) is not null;
        }
    }
}
