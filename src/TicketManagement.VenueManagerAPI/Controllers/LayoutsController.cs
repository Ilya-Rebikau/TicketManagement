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
        /// IConverter object.
        /// </summary>
        private readonly IConverter<LayoutDto, LayoutModel> _converter;

        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutsController"/> class.
        /// </summary>
        /// <param name="service">LayoutService object.</param>
        /// <param name="converter">IConverter object.</param>
        public LayoutsController(IService<LayoutDto> service, IConverter<LayoutDto, LayoutModel> converter)
        {
            _service = service;
            _converter = converter;
        }

        /// <summary>
        /// Get all layouts.
        /// </summary>
        /// <param name="pageNumber">Page number.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpGet("getlayouts")]
        public async Task<IActionResult> GetLayouts([FromBody] int pageNumber)
        {
            var layouts = await _service.GetAllAsync(pageNumber);
            return Ok(await _converter.ConvertSourceModelRangeToDestinationModelRange(layouts));
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
            return layout is null ? NotFound() : Ok(layout);
        }

        /// <summary>
        /// Create layout.
        /// </summary>
        /// <param name="layoutVm">Adding layout.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] LayoutModel layoutVm)
        {
            await _service.CreateAsync(await _converter.ConvertDestinationToSource(layoutVm));
            return Ok();
        }

        /// <summary>
        /// Edit layout.
        /// </summary>
        /// <param name="id">Id of editing layout.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpGet("edit/{id}")]
        public async Task<IActionResult> Edit([FromRoute] int id)
        {
            var updatingLayout = await _service.GetByIdAsync(id);
            return updatingLayout is null ? NotFound() : Ok(updatingLayout);
        }

        /// <summary>
        /// Edit layout.
        /// </summary>
        /// <param name="id">Id of editing layout.</param>
        /// <param name="layoutVm">Edited layout.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpPut("edit/{id}")]
        public async Task<IActionResult> Edit([FromRoute] int id, [FromBody] LayoutModel layoutVm)
        {
            if (id != layoutVm.Id)
            {
                return NotFound();
            }

            try
            {
                await _service.UpdateAsync(await _converter.ConvertDestinationToSource(layoutVm));
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
            return deletingLayout is null ? NotFound() : Ok(deletingLayout);
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
