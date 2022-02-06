using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.BusinessLogic.ModelsDTO;

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
            return View(await _service.GetAllAsync());
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

            return View(eventArea);
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
        /// <param name="eventArea">Adding event area.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EventAreaDto eventArea)
        {
            if (ModelState.IsValid)
            {
                await _service.CreateAsync(eventArea);
                return RedirectToAction(nameof(Index));
            }

            return View(eventArea);
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

            return View(updatingEventArea);
        }

        /// <summary>
        /// Edit event area.
        /// </summary>
        /// <param name="id">Id of editing event area.</param>
        /// <param name="eventArea">Edited event area.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EventAreaDto eventArea)
        {
            if (id != eventArea.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
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

            return View(eventArea);
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

            return View(deletingEventArea);
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
            var eventArea = await _service.GetByIdAsync(id);
            await _service.DeleteAsync(eventArea);
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
