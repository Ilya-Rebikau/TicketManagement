using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.BusinessLogic.ModelsDTO;
using TicketManagement.Web.Models.Layouts;

namespace TicketManagement.Web.Controllers
{
    /// <summary>
    /// Controller for layouts.
    /// </summary>
    [Authorize(Roles = "admin, venue manager")]
    [ResponseCache(CacheProfileName = "Caching")]
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
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var layouts = await _service.GetAllAsync();
            var layoutsVm = new List<LayoutViewModel>();
            foreach (var layout in layouts)
            {
                layoutsVm.Add(layout);
            }

            return View(layoutsVm);
        }

        /// <summary>
        /// Details about layout.
        /// </summary>
        /// <param name="id">Id of layout.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var layout = await _service.GetByIdAsync((int)id);
            if (layout == null)
            {
                return NotFound();
            }

            LayoutViewModel layoutVm = layout;
            return View(layoutVm);
        }

        /// <summary>
        /// Create layout.
        /// </summary>
        /// <returns>IActionResult.</returns>
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Create layout.
        /// </summary>
        /// <param name="layoutVm">Adding layout.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LayoutViewModel layoutVm)
        {
            if (!ModelState.IsValid)
            {
                return View(layoutVm);
            }

            LayoutDto layout = layoutVm;
            await _service.CreateAsync(layout);
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Edit layout.
        /// </summary>
        /// <param name="id">Id of editing layout.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var updatingLayout = await _service.GetByIdAsync((int)id);
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, LayoutViewModel layoutVm)
        {
            if (id != layoutVm.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(layoutVm);
            }

            LayoutDto layout = layoutVm;
            try
            {
                await _service.UpdateAsync(layout);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await LayoutExists(layout.Id))
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
        /// Delete layout.
        /// </summary>
        /// <param name="id">Id of deleting layout.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deletingLayout = await _service.GetByIdAsync((int)id);
            if (deletingLayout == null)
            {
                return NotFound();
            }

            LayoutViewModel layoutVm = deletingLayout;
            return View(layoutVm);
        }

        /// <summary>
        /// Delete confirmation.
        /// </summary>
        /// <param name="id">Id of deleting layout.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var layout = await _service.GetByIdAsync(id);
            await _service.DeleteAsync(layout);
            return RedirectToAction(nameof(Index));
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
