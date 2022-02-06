using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.BusinessLogic.ModelsDTO;

namespace TicketManagement.Web.Controllers
{
    /// <summary>
    /// Controller for tickets.
    /// </summary>
    [Authorize(Roles = "admin")]
    [ResponseCache(CacheProfileName = "Caching")]
    public class TicketsController : Controller
    {
        /// <summary>
        /// TicketService object.
        /// </summary>
        private readonly IService<TicketDto> _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="TicketsController"/> class.
        /// </summary>
        /// <param name="service">TicketService object.</param>
        public TicketsController(IService<TicketDto> service)
        {
            _service = service;
        }

        /// <summary>
        /// Get all tickets.
        /// </summary>
        /// <returns>Task with IActionResult.</returns>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _service.GetAllAsync());
        }

        /// <summary>
        /// Details about ticket.
        /// </summary>
        /// <param name="id">Id of ticket.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _service.GetByIdAsync((int)id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        /// <summary>
        /// Create ticket.
        /// </summary>
        /// <returns>IActionResult.</returns>
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Create ticket.
        /// </summary>
        /// <param name="ticket">Adding ticket.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TicketDto ticket)
        {
            if (ModelState.IsValid)
            {
                await _service.CreateAsync(ticket);
                return RedirectToAction(nameof(Index));
            }

            return View(ticket);
        }

        /// <summary>
        /// Edit ticket.
        /// </summary>
        /// <param name="id">Id of editing ticket.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var updatingTicket = await _service.GetByIdAsync((int)id);
            if (updatingTicket == null)
            {
                return NotFound();
            }

            return View(updatingTicket);
        }

        /// <summary>
        /// Edit ticket.
        /// </summary>
        /// <param name="id">Id of editing ticket.</param>
        /// <param name="ticket">Edited ticket.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TicketDto ticket)
        {
            if (id != ticket.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _service.UpdateAsync(ticket);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await TicketExists(ticket.Id))
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

            return View(ticket);
        }

        /// <summary>
        /// Delete ticket.
        /// </summary>
        /// <param name="id">Id of deleting ticket.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deletingTicket = await _service.GetByIdAsync((int)id);
            if (deletingTicket == null)
            {
                return NotFound();
            }

            return View(deletingTicket);
        }

        /// <summary>
        /// Delete confirmation.
        /// </summary>
        /// <param name="id">Id of deleting ticket.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ticket = await _service.GetByIdAsync(id);
            await _service.DeleteAsync(ticket);
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Check that ticket exists.
        /// </summary>
        /// <param name="id">Id of ticket.</param>
        /// <returns>True if exists and false if not.</returns>
        private async Task<bool> TicketExists(int id)
        {
            return await _service.GetByIdAsync(id) is not null;
        }
    }
}
