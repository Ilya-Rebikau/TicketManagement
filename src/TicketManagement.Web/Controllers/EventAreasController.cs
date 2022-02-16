using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.BusinessLogic.ModelsDTO;
using TicketManagement.Web.Models.EventAreas;

namespace TicketManagement.Web.Controllers
{
    /// <summary>
    /// Controller for event areas.
    /// </summary>
    [Authorize(Roles = "admin, event manager")]
    [ResponseCache(CacheProfileName = "Caching")]
    public class EventAreasController : Controller
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
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var eventAreas = await _service.GetAllAsync();
            var eventAreasVm = new List<EventAreaViewModel>();
            foreach (var eventArea in eventAreas)
            {
                eventAreasVm.Add(eventArea);
            }

            return View(eventAreasVm);
        }

        /// <summary>
        /// Details about event area.
        /// </summary>
        /// <param name="id">Id of event area.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventArea = await _service.GetByIdAsync((int)id);
            if (eventArea == null)
            {
                return NotFound();
            }

            EventAreaViewModel eventAreaVm = eventArea;
            return View(eventAreaVm);
        }

        /// <summary>
        /// Create event area.
        /// </summary>
        /// <returns>IActionResult.</returns>
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Create event area.
        /// </summary>
        /// <param name="eventAreaVm">Adding event area.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EventAreaViewModel eventAreaVm)
        {
            if (!ModelState.IsValid)
            {
                return View(eventAreaVm);
            }

            EventAreaDto eventArea = eventAreaVm;
            await _service.CreateAsync(eventArea);
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Edit event area.
        /// </summary>
        /// <param name="id">Id of editing event area.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var updatingEventArea = await _service.GetByIdAsync((int)id);
            if (updatingEventArea == null)
            {
                return NotFound();
            }

            EventAreaViewModel eventAreaVm = updatingEventArea;
            return View(eventAreaVm);
        }

        /// <summary>
        /// Edit event area.
        /// </summary>
        /// <param name="id">Id of editing event area.</param>
        /// <param name="eventAreaVm">Edited event area.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EventAreaViewModel eventAreaVm)
        {
            if (id != eventAreaVm.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(eventAreaVm);
            }

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

            return RedirectToAction(nameof(Index));
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
            return RedirectToAction(nameof(Index));
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
