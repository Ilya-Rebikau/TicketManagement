using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.BusinessLogic.ModelsDTO;

namespace TicketManagement.Web.Controllers
{
    [Authorize(Roles = "admin, venue manager")]
    [ResponseCache(CacheProfileName = "Caching")]
    public class LayoutsController : Controller
    {
        private readonly IService<LayoutDto> _service;

        public LayoutsController(IService<LayoutDto> service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _service.GetAllAsync());
        }

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

            return View(layout);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LayoutDto layout)
        {
            if (ModelState.IsValid)
            {
                await _service.CreateAsync(layout);
                return RedirectToAction(nameof(Index));
            }

            return View(layout);
        }

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

            return View(updatingLayout);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, LayoutDto layout)
        {
            if (id != layout.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
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

            return View(layout);
        }

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

            return View(deletingLayout);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var layout = await _service.GetByIdAsync(id);
            await _service.DeleteAsync(layout);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> LayoutExists(int id)
        {
            return await _service.GetByIdAsync(id) is not null;
        }
    }
}
