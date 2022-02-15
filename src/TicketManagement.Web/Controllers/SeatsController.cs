using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.BusinessLogic.ModelsDTO;
using TicketManagement.Web.Models.Seats;

namespace TicketManagement.Web.Controllers
{
    /// <summary>
    /// Controller for seats.
    /// </summary>
    [Authorize(Roles = "admin, venue manager")]
    [ResponseCache(CacheProfileName = "Caching")]
    public class SeatsController : Controller
    {
        /// <summary>
        /// SeatService object.
        /// </summary>
        private readonly IService<SeatDto> _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="SeatsController"/> class.
        /// </summary>
        /// <param name="service">SeatService object.</param>
        public SeatsController(IService<SeatDto> service)
        {
            _service = service;
        }

        /// <summary>
        /// Get all sets.
        /// </summary>
        /// <returns>Task with IActionResult.</returns>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var seats = await _service.GetAllAsync();
            var seatsVm = new List<SeatViewModel>();
            foreach (var seat in seats)
            {
                seatsVm.Add(seat);
            }

            return View(seatsVm);
        }

        /// <summary>
        /// Details about seat.
        /// </summary>
        /// <param name="id">Id of seat.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var seat = await _service.GetByIdAsync((int)id);
            if (seat == null)
            {
                return NotFound();
            }

            SeatViewModel seatVm = seat;
            return View(seatVm);
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

            SeatDto seat = seatVm;
            await _service.CreateAsync(seat);
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
            if (id == null)
            {
                return NotFound();
            }

            var updatingSeat = await _service.GetByIdAsync((int)id);
            if (updatingSeat == null)
            {
                return NotFound();
            }

            SeatViewModel seatVm = updatingSeat;
            return View(seatVm);
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

            SeatDto seat = seatVm;
            try
            {
                await _service.UpdateAsync(seat);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await SeatExists(seat.Id))
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
        /// Delete seat.
        /// </summary>
        /// <param name="id">Id of deleting seat.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deletingSeat = await _service.GetByIdAsync((int)id);
            if (deletingSeat == null)
            {
                return NotFound();
            }

            SeatViewModel seatVm = deletingSeat;
            return View(seatVm);
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
            var seat = await _service.GetByIdAsync(id);
            await _service.DeleteAsync(seat);
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Check that seat exists.
        /// </summary>
        /// <param name="id">Id of seat.</param>
        /// <returns>True if exists and false if not.</returns>
        private async Task<bool> SeatExists(int id)
        {
            return await _service.GetByIdAsync(id) is not null;
        }
    }
}
