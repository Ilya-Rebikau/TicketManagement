using System;
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
    [Route("[controller]")]
    [ApiController]
    [Authorize(Roles = "admin, event manager")]
    public class EventAreasController : ControllerBase
    {
        /// <summary>
        /// EventAreaService object.
        /// </summary>
        private readonly IService<EventAreaDto> _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventAreasController"/> class.
        /// </summary>
        /// <param name="service">EventAreaService object.</param>
        public EventAreasController(IService<EventAreaDto> service)
        {
            _service = service;
        }

        /// <summary>
        /// All event areas.
        /// </summary>
        /// <returns>Task with IActionResult.</returns>
        [HttpGet("getareas")]
        public async Task<IActionResult> GetEventAreaViewModels()
        {
            var eventAreas = await _service.GetAllAsync();
            var eventAreasVm = new List<EventAreaViewModel>();
            foreach (var eventArea in eventAreas)
            {
                eventAreasVm.Add(eventArea);
            }

            return Ok(eventAreasVm);
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
            if (eventArea == null)
            {
                return NotFound();
            }

            EventAreaViewModel eventAreaVm = eventArea;
            return Ok(eventAreaVm);
        }

        /// <summary>
        /// Create event area.
        /// </summary>
        /// <param name="eventAreaVm">Adding event area.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EventAreaViewModel eventAreaVm)
        {
            EventAreaDto eventArea = eventAreaVm;
            await _service.CreateAsync(eventArea);
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
            if (updatingEventArea == null)
            {
                return NotFound();
            }

            EventAreaViewModel eventAreaVm = updatingEventArea;
            return Ok(eventAreaVm);
        }

        /// <summary>
        /// Edit event area.
        /// </summary>
        /// <param name="eventAreaVm">Edited event area.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpPost("edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EventAreaViewModel eventAreaVm)
        {
            EventAreaDto eventArea = eventAreaVm;
            try
            {
                await _service.UpdateAsync(eventArea);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await EventAreaExists(eventArea.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Conflict();
        }

        /// <summary>
        /// Delete event area.
        /// </summary>
        /// <param name="id">Id of deleting event area.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deletingEventArea = await _service.GetByIdAsync((int)id);
            if (deletingEventArea == null)
            {
                return NotFound();
            }

            EventAreaViewModel eventAreaVm = deletingEventArea;
            return View(eventAreaVm);
        }

        /// <summary>
        /// Delete confirmation.
        /// </summary>
        /// <param name="id">Id of deleting event area.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _service.DeleteById(id);
            return RedirectToAction(nameof(GetEventAreaViewModels));
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
