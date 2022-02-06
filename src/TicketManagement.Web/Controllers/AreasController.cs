using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.BusinessLogic.ModelsDTO;

namespace TicketManagement.Web.Controllers
{
    /// <summary>
    /// Controller for areas.
    /// </summary>
    [Authorize(Roles = "admin, venue manager")]
    [ResponseCache(CacheProfileName = "Caching")]
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
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _service.GetAllAsync());
        }

        /// <summary>
        /// Details about chosen area.
        /// </summary>
        /// <param name="id">Id of chosen area.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var area = await _service.GetByIdAsync((int)id);
            if (area == null)
            {
                return NotFound();
            }

            return View(area);
        }

        /// <summary>
        /// Creat area.
        /// </summary>
        /// <returns>IActionResult.</returns>
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Create area.
        /// </summary>
        /// <param name="area">Creating area.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AreaDto area)
        {
            if (ModelState.IsValid)
            {
                await _service.CreateAsync(area);
                return RedirectToAction(nameof(Index));
            }

            return View(area);
        }

        /// <summary>
        /// Edit area.
        /// </summary>
        /// <param name="id">Id of editing area.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var updatingArea = await _service.GetByIdAsync((int)id);
            if (updatingArea == null)
            {
                return NotFound();
            }

            return View(updatingArea);
        }

        /// <summary>
        /// Edit area.
        /// </summary>
        /// <param name="id">Id of editing area.</param>
        /// <param name="area">Editing area.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AreaDto area)
        {
            if (id != area.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _service.UpdateAsync(area);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await AreaExists(area.Id))
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

            return View(area);
        }

        /// <summary>
        /// Delete area.
        /// </summary>
        /// <param name="id">Id of deleting area.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deletingArea = await _service.GetByIdAsync((int)id);
            if (deletingArea == null)
            {
                return NotFound();
            }

            return View(deletingArea);
        }

        /// <summary>
        /// Delete confirmation.
        /// </summary>
        /// <param name="id">Id of deleting area.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var area = await _service.GetByIdAsync(id);
            await _service.DeleteAsync(area);
            return RedirectToAction(nameof(Index));
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
