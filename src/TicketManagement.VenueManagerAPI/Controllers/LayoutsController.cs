using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketManagement.VenueManagerAPI.Interfaces;
using TicketManagement.VenueManagerAPI.Models.Layouts;
using TicketManagement.VenueManagerAPI.ModelsDTO;

namespace TicketManagement.VenueManagerAPI.Controllers
{
    /// <summary>
    /// Controller for layouts.
    /// </summary>
    [Authorize(Roles = "admin, venue manager")]
    [Route("[controller]")]
    [ApiController]
    public class LayoutsController : Controller
    {
        /// <summary>
        /// LayoutService object.
        /// </summary>
        private readonly IService<LayoutDto> _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutsController"/> class.
        /// </summary>
        /// <param name="service">LayoutService object.</param>
        public LayoutsController(IService<LayoutDto> service)
        {
            _service = service;
        }

        /// <summary>
        /// Get all layouts.
        /// </summary>
        /// <returns>Task with IActionResult.</returns>
        [HttpGet("getlayouts")]
        public async Task<IActionResult> GetLayouts()
        {
            var layouts = await _service.GetAllAsync();
            var layoutsVm = new List<LayoutViewModel>();
            foreach (var layout in layouts)
            {
                layoutsVm.Add(layout);
            }

            return Ok(layoutsVm);
        }

        /// <summary>
        /// Details about layout.
        /// </summary>
        /// <param name="id">Id of layout.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpGet("details/{id}")]
        public async Task<IActionResult> Details([FromRoute] int id)
        {
            var layout = await _service.GetByIdAsync(id);
            if (layout == null)
            {
                return NotFound();
            }

            LayoutViewModel layoutVm = layout;
            return Ok(layoutVm);
        }

        /// <summary>
        /// Create layout.
        /// </summary>
        /// <param name="layoutVm">Adding layout.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] LayoutViewModel layoutVm)
        {
            if (!ModelState.IsValid)
            {
                return View(layoutVm);
            }

            await _service.CreateAsync(layoutVm);
            return Ok();
        }

        /// <summary>
        /// Edit layout.
        /// </summary>
        /// <param name="id">Id of editing layout.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpGet("edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var updatingLayout = await _service.GetByIdAsync(id);
            if (updatingLayout == null)
            {
                return NotFound();
            }

            LayoutViewModel layoutVm = updatingLayout;
            return View(layoutVm);
        }

        /// <summary>
        /// Edit layout.
        /// </summary>
        /// <param name="id">Id of editing layout.</param>
        /// <param name="layoutVm">Edited layout.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpPut("edit/{id}")]
        public async Task<IActionResult> Edit([FromRoute] int id, [FromBody] LayoutViewModel layoutVm)
        {
            if (id != layoutVm.Id)
            {
                return NotFound();
            }

            try
            {
                await _service.UpdateAsync(layoutVm);
                return Ok();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await LayoutExists(layoutVm.Id))
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
        /// Delete layout.
        /// </summary>
        /// <param name="id">Id of deleting layout.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpGet("delete/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var deletingLayout = await _service.GetByIdAsync(id);
            if (deletingLayout == null)
            {
                return NotFound();
            }

            LayoutViewModel layoutVm = deletingLayout;
            return Ok(layoutVm);
        }

        /// <summary>
        /// Delete confirmation.
        /// </summary>
        /// <param name="id">Id of deleting layout.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpDelete("delete/{id}")]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed([FromRoute] int id)
        {
            await _service.DeleteById(id);
            return Ok();
        }

        /// <summary>
        /// Check that layout exist.
        /// </summary>
        /// <param name="id">Id of layout.</param>
        /// <returns>True if exists and false if not.</returns>
        private async Task<bool> LayoutExists(int id)
        {
            return await _service.GetByIdAsync(id) is not null;
        }
    }
}
