using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketManagement.Web.Extensions;
using TicketManagement.Web.Infrastructure;
using TicketManagement.Web.Interfaces.HttpClients;
using TicketManagement.Web.Models.Areas;

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
        /// IVenueManagerClient object.
        /// </summary>
        private readonly IVenueManagerClient _venueManagerClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="AreasController"/> class.
        /// </summary>
        /// <param name="venueManagerClient">IVenueManagerClient object.</param>
        public AreasController(IVenueManagerClient venueManagerClient)
        {
            _venueManagerClient = venueManagerClient;
        }

        /// <summary>
        /// All areas.
        /// </summary>
        /// <param name="pageNumber">Page number.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpGet]
        public async Task<IActionResult> Index(int pageNumber = 1)
        {
            var areas = await _venueManagerClient.GetAreaViewModels(HttpContext.GetJwtToken(), pageNumber);
            return View(areas);
        }

        /// <summary>
        /// Details about chosen area.
        /// </summary>
        /// <param name="id">Id of chosen area.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            return id is null ? NotFound() : View(await _venueManagerClient.AreaDetails(HttpContext.GetJwtToken(), (int)id));
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

            await _venueManagerClient.CreateArea(HttpContext.GetJwtToken(), areaVm);
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
            return id is null ? NotFound() : View(await _venueManagerClient.GetAreaViewModelForEdit(HttpContext.GetJwtToken(), (int)id));
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

            try
            {
                await _venueManagerClient.EditArea(HttpContext.GetJwtToken(), id, areaVm);
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict();
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
            return id is null ? NotFound() : View(await _venueManagerClient.GetAreaViewModelForDelete(HttpContext.GetJwtToken(), (int)id));
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
            await _venueManagerClient.DeleteArea(HttpContext.GetJwtToken(), id);
            return RedirectToAction(nameof(Index));
        }
    }
}
