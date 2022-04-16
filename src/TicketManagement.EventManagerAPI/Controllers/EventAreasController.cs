using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketManagement.EventManagerAPI.Interfaces;
using TicketManagement.EventManagerAPI.Models.EventAreas;
using TicketManagement.EventManagerAPI.ModelsDTO;

namespace TicketManagement.EventManagerAPI.Controllers
{
    /// <summary>
    /// Controller for event areas.
    /// </summary>
    [Authorize(Roles = "admin, event manager")]
    [Route("[controller]")]
    [ApiController]
    public class EventAreasController : ControllerBase
    {
        /// <summary>
        /// EventAreaService object.
        /// </summary>
        private readonly IService<EventAreaDto> _service;

        /// <summary>
        /// IConverter object.
        /// </summary>
        private readonly IConverter<EventAreaDto, EventAreaModel> _converter;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventAreasController"/> class.
        /// </summary>
        /// <param name="service">EventAreaService object.</param>
        /// <param name="converter">IConverter object.</param>
        public EventAreasController(IService<EventAreaDto> service, IConverter<EventAreaDto, EventAreaModel> converter)
        {
            _service = service;
            _converter = converter;
        }

        /// <summary>
        /// All event areas.
        /// </summary>
        /// <param name="pageNumber">Page number.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpGet("getareas")]
        public async Task<IActionResult> GetEventAreaViewModels([FromQuery] int pageNumber)
        {
            var eventAreas = await _service.GetAllAsync(pageNumber);
            return Ok(await _converter.ConvertSourceModelRangeToDestinationModelRange(eventAreas));
        }

        /// <summary>
        /// Details about event area.
        /// </summary>
        /// <param name="id">Id of event area.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpGet("details/{id}")]
        public async Task<IActionResult> Details([FromRoute] int id)
        {
            var eventArea = await _service.GetByIdAsync(id);
            return eventArea is null ? NotFound() : Ok(eventArea);
        }

        /// <summary>
        /// Create event area.
        /// </summary>
        /// <param name="eventArea">Adding event area.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] EventAreaModel eventArea)
        {
            await _service.CreateAsync(await _converter.ConvertDestinationToSource(eventArea));
            return Ok();
        }

        /// <summary>
        /// Edit event area.
        /// </summary>
        /// <param name="id">Id of editing event area.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpGet("edit/{id}")]
        public async Task<IActionResult> Edit([FromRoute] int id)
        {
            var updatingEventArea = await _service.GetByIdAsync(id);
            return updatingEventArea is null ? NotFound() : Ok(updatingEventArea);
        }

        /// <summary>
        /// Edit event area.
        /// </summary>
        /// <param name="id">Id of editing event area.</param>
        /// <param name="eventArea">Edited event area.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpPut("edit/{id}")]
        public async Task<IActionResult> Edit([FromRoute] int id, [FromBody] EventAreaModel eventArea)
        {
            if (id != eventArea.Id)
            {
                return NotFound();
            }

            try
            {
                await _service.UpdateAsync(await _converter.ConvertDestinationToSource(eventArea));
                return Ok();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await EventAreaExists(eventArea.Id))
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
        /// Delete event area.
        /// </summary>
        /// <param name="id">Id of deleting event area.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpGet("delete/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var deletingEventArea = await _service.GetByIdAsync(id);
            return deletingEventArea is null ? NotFound() : Ok(deletingEventArea);
        }

        /// <summary>
        /// Delete confirmation.
        /// </summary>
        /// <param name="id">Id of deleting event area.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpDelete("delete/{id}")]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed([FromRoute] int id)
        {
            await _service.DeleteById(id);
            return Ok();
        }

        /// <summary>
        /// Check that event area exist.
        /// </summary>
        /// <param name="id">Id of event area.</param>
        /// <returns>True if exist and false if not.</returns>
        private async Task<bool> EventAreaExists(int id)
        {
            return await _service.GetByIdAsync(id) is not null;
        }
    }
}
