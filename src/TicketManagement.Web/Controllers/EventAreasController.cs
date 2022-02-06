using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.BusinessLogic.ModelsDTO;

namespace TicketManagement.Web.Controllers
{
    [ResponseCache(CacheProfileName = "Caching")]
    public class EventAreasController : Controller
    {
        private readonly IService<EventAreaDto> _service;

        public EventAreasController(IService<EventAreaDto> service)
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

            var eventArea = await _service.GetByIdAsync((int)id);
            if (eventArea == null)
            {
                return NotFound();
            }

            return View(eventArea);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

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

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var eventArea = await _service.GetByIdAsync(id);
            await _service.DeleteAsync(eventArea);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> EventAreaExists(int id)
        {
            return await _service.GetByIdAsync(id) is not null;
        }
    }
}
