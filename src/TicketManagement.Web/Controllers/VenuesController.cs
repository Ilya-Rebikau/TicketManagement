using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.BusinessLogic.ModelsDTO;

namespace TicketManagement.Web.Controllers
{
    /// <summary>
    /// Controller for venue.
    /// </summary>
    [Authorize(Roles = "admin, venue manager")]
    [ResponseCache(CacheProfileName = "Caching")]
    public class VenuesController : Controller
    {
        /// <summary>
        /// VenueService object.
        /// </summary>
        private readonly IService<VenueDto> _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="VenuesController"/> class.
        /// </summary>
        /// <param name="service">VenueService object.</param>
        public VenuesController(IService<VenueDto> service)
        {
            _service = service;
        }

        /// <summary>
        /// Get all venues.
        /// </summary>
        /// <returns>Task with IActionResult.</returns>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _service.GetAllAsync());
        }

        /// <summary>
        /// Details about venue.
        /// </summary>
        /// <param name="id">Id of venue.</param>
        /// <returns>Task with IActionResult.</returns>
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

        /// <summary>
        /// Create venue.
        /// </summary>
        /// <returns>IActionResult.</returns>
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Create venue.
        /// </summary>
        /// <param name="venue">Adding venue.</param>
        /// <returns>Task with IActionResult.</returns>
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

        /// <summary>
        /// Edit venue.
        /// </summary>
        /// <param name="id">Id of editing venue.</param>
        /// <returns>Task with IActionResult.</returns>
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

        /// <summary>
        /// Edit venue.
        /// </summary>
        /// <param name="id">Id of editing venue.</param>
        /// <param name="venue">Edited venue.</param>
        /// <returns>Task with IActionResult.</returns>
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

        /// <summary>
        /// Delete venue.
        /// </summary>
        /// <param name="id">Id of deleting venue.</param>
        /// <returns>Task with IActionResult.</returns>
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

        /// <summary>
        /// Delete confirmation.
        /// </summary>
        /// <param name="id">Id of deleting venue.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var venue = await _service.GetByIdAsync(id);
            await _service.DeleteAsync(venue);
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Check that venue exists.
        /// </summary>
        /// <param name="id">Id of venue.</param>
        /// <returns>True if exists and false if not.</returns>
        private async Task<bool> VenueExists(int id)
        {
            return await _service.GetByIdAsync(id) is not null;
        }
    }
}
