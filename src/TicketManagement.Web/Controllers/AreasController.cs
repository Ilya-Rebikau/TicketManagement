using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketManagement.Web.Infrastructure;
using TicketManagement.Web.Interfaces;
using TicketManagement.Web.Models.Areas;
using TicketManagement.Web.ModelsDTO;

namespace TicketManagement.Web.Controllers
{
    /// <summary>
    /// Controller for areas.
    /// </summary>
    [Authorize(Roles = "admin, venue manager")]
    [ResponseCache(CacheProfileName = "Caching")]
    [ExceptionFilter]
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
            var areas = await _service.GetAllAsync();
            var areasVm = new List<AreaViewModel>();
            foreach (var area in areas)
            {
                areasVm.Add(area);
            }

            return View(areasVm);
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

            AreaViewModel areaVm = area;
            return View(areaVm);
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
        /// <param name="areaVm">Creating area.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AreaViewModel areaVm)
        {
            if (!ModelState.IsValid)
            {
                return View(areaVm);
            }

            AreaDto area = areaVm;
            await _service.CreateAsync(area);
            return RedirectToAction(nameof(Index));
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

            AreaViewModel areaVm = updatingArea;
            return View(areaVm);
        }

        /// <summary>
        /// Edit area.
        /// </summary>
        /// <param name="id">Id of editing area.</param>
        /// <param name="areaVm">Editing area.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AreaViewModel areaVm)
        {
            if (id != areaVm.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(areaVm);
            }

            AreaDto area = areaVm;
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

            AreaViewModel areaVm = deletingArea;
            return View(areaVm);
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
            await _service.DeleteById(id);
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
