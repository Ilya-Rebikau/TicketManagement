using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.BusinessLogic.ModelsDTO;

namespace TicketManagement.Web.Controllers
{
    public class AreasController : Controller
    {
        private readonly IService<AreaDto> _service;

        public AreasController(IService<AreaDto> service)
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

            var area = await _service.GetByIdAsync((int)id);
            if (area == null)
            {
                return NotFound();
            }

            return View(area);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,LayoutId,Description,CoordX,CoordY")] AreaDto area)
        {
            if (ModelState.IsValid)
            {
                await _service.CreateAsync(area);
                return RedirectToAction(nameof(Index));
            }

            return View(area);
        }

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,LayoutId,Description,CoordX,CoordY")] AreaDto area)
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

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var area = await _service.GetByIdAsync(id);
            await _service.DeleteAsync(area);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> AreaExists(int id)
        {
            return await _service.GetByIdAsync(id) is not null;
        }
    }
}
