using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketManagement.Web.Extensions;
using TicketManagement.Web.Infrastructure;
using TicketManagement.Web.Interfaces.HttpClients;
using TicketManagement.Web.Models;
using TicketManagement.Web.Models.Seats;

namespace TicketManagement.Web.Controllers
{
    /// <summary>
    /// Controller for seats.
    /// </summary>
    [Authorize(Roles = "admin, venue manager")]
    [ResponseCache(CacheProfileName = "Caching")]
    [ExceptionFilter]
    public class SeatsController : Controller
    {
        /// <summary>
        /// IVenueManagerClient object.
        /// </summary>
        private readonly IVenueManagerClient _venueManagerClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="SeatsController"/> class.
        /// </summary>
        /// <param name="venueManagerClient">IVenueManagerClient object.</param>
        public SeatsController(IVenueManagerClient venueManagerClient)
        {
            _venueManagerClient = venueManagerClient;
        }

        /// <summary>
        /// Get all sets.
        /// </summary>
        /// <param name="pageNumber">Page number.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpGet]
        public async Task<IActionResult> Index(int pageNumber = 1)
        {
            var seats = await _venueManagerClient.GetSeatViewModels(HttpContext.GetJwtToken(), pageNumber);
            var nextSeats = await _venueManagerClient.GetSeatViewModels(HttpContext.GetJwtToken(), pageNumber + 1);
            PageViewModel.NextPage = nextSeats is not null && nextSeats.Any();
            PageViewModel.PageNumber = pageNumber;
            return View(seats);
        }

        /// <summary>
        /// Details about seat.
        /// </summary>
        /// <param name="id">Id of seat.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            return id is null ? NotFound() : View(await _venueManagerClient.SeatDetails(HttpContext.GetJwtToken(), (int)id));
        }

        /// <summary>
        /// Create seat.
        /// </summary>
        /// <returns>IActionResult.</returns>
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Create seat.
        /// </summary>
        /// <param name="seatVm">Adding seat.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SeatViewModel seatVm)
        {
            if (!ModelState.IsValid)
            {
                return View(seatVm);
            }

            await _venueManagerClient.CreateSeat(HttpContext.GetJwtToken(), seatVm);
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Edit seat.
        /// </summary>
        /// <param name="id">Id of editing seat.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            return id is null ? NotFound() : View(await _venueManagerClient.GetSeatViewModelForEdit(HttpContext.GetJwtToken(), (int)id));
        }

        /// <summary>
        /// Edit seat.
        /// </summary>
        /// <param name="id">Id of editing seat.</param>
        /// <param name="seatVm">Edited seat.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SeatViewModel seatVm)
        {
            if (id != seatVm.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(seatVm);
            }

            try
            {
                await _venueManagerClient.EditSeat(HttpContext.GetJwtToken(), id, seatVm);
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict();
            }

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Delete seat.
        /// </summary>
        /// <param name="id">Id of deleting seat.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            return id is null ? NotFound() : View(await _venueManagerClient.GetSeatViewModelForDelete(HttpContext.GetJwtToken(), (int)id));
        }

        /// <summary>
        /// Delete confirmation.
        /// </summary>
        /// <param name="id">Id of deleting seat.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _venueManagerClient.DeleteSeat(HttpContext.GetJwtToken(), id);
            return RedirectToAction(nameof(Index));
        }
    }
}
