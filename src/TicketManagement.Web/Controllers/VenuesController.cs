using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.BusinessLogic.ModelsDTO;

namespace TicketManagement.Web.Controllers
{
    [ResponseCache(CacheProfileName = "Caching")]
    public class VenuesController : Controller
    {
        private readonly IService<VenueDto> _service;

        public VenuesController(IService<VenueDto> service)
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

            var venue = await _service.GetByIdAsync((int)id);
            if (venue == null)
            {
                return NotFound();
            }

            return View(venue);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VenueDto venue)
        {
            if (ModelState.IsValid)
            {
                await _service.CreateAsync(venue);
                return RedirectToAction(nameof(Index));
            }

            return View(venue);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var updatingVenue = await _service.GetByIdAsync((int)id);
            if (updatingVenue == null)
            {
                return NotFound();
            }

            return View(updatingVenue);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, VenueDto venue)
        {
            if (id != venue.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _service.UpdateAsync(venue);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await VenueExists(venue.Id))
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

            return View(venue);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deletingVenue = await _service.GetByIdAsync((int)id);
            if (deletingVenue == null)
            {
                return NotFound();
            }

            return View(deletingVenue);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var venue = await _service.GetByIdAsync(id);
            await _service.DeleteAsync(venue);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> VenueExists(int id)
        {
            return await _service.GetByIdAsync(id) is not null;
        }
    }
}
