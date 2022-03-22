using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketManagement.Web.Extensions;
using TicketManagement.Web.Infrastructure;
using TicketManagement.Web.Interfaces.HttpClients;
using TicketManagement.Web.Models.Venues;

namespace TicketManagement.Web.Controllers
{
    /// <summary>
    /// Controller for venue.
    /// </summary>
    [Authorize(Roles = "admin, venue manager")]
    [ResponseCache(CacheProfileName = "Caching")]
    [ExceptionFilter]
    public class VenuesController : Controller
    {
        /// <summary>
        /// IVenueManagerClient object.
        /// </summary>
        private readonly IVenueManagerClient _venueManagerClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="VenuesController"/> class.
        /// </summary>
        /// <param name="venueManagerClient">IVenueManagerClient object.</param>
        public VenuesController(IVenueManagerClient venueManagerClient)
        {
            _venueManagerClient = venueManagerClient;
        }

        /// <summary>
        /// Get all venues.
        /// </summary>
        /// <returns>Task with IActionResult.</returns>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var venuesVm = await _venueManagerClient.GetVenueViewModels(HttpContext.GetJwtToken());
            return View(venuesVm);
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

            var venueVm = await _venueManagerClient.VenueDetails(HttpContext.GetJwtToken(), (int)id);
            if (venueVm == null)
            {
                return NotFound();
            }

            return View(venueVm);
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
        /// <param name="venueVm">Adding venue.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VenueViewModel venueVm)
        {
            if (!ModelState.IsValid)
            {
                return View(venueVm);
            }

            await _venueManagerClient.CreateVenue(HttpContext.GetJwtToken(), venueVm);
            return RedirectToAction(nameof(Index));
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

            var venueVm = await _venueManagerClient.GetVenueViewModelForEdit(HttpContext.GetJwtToken(), (int)id);
            if (venueVm == null)
            {
                return NotFound();
            }

            return View(venueVm);
        }

        /// <summary>
        /// Edit venue.
        /// </summary>
        /// <param name="id">Id of editing venue.</param>
        /// <param name="venueVm">Edited venue.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, VenueViewModel venueVm)
        {
            if (id != venueVm.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(venueVm);
            }

            try
            {
                await _venueManagerClient.EditVenue(HttpContext.GetJwtToken(), id, venueVm);
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict();
            }

            return RedirectToAction(nameof(Index));
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

            var venueVm = await _venueManagerClient.GetVenueViewModelForDelete(HttpContext.GetJwtToken(), (int)id);
            if (venueVm == null)
            {
                return NotFound();
            }

            return View(venueVm);
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
            await _venueManagerClient.DeleteVenue(HttpContext.GetJwtToken(), id);
            return RedirectToAction(nameof(Index));
        }
    }
}
