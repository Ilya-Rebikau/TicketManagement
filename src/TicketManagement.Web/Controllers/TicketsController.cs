﻿using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketManagement.Web.Extensions;
using TicketManagement.Web.Infrastructure;
using TicketManagement.Web.Interfaces.HttpClients;
using TicketManagement.Web.Models;
using TicketManagement.Web.Models.Tickets;

namespace TicketManagement.Web.Controllers
{
    /// <summary>
    /// Controller for tickets.
    /// </summary>
    [Authorize(Roles = "admin, event manager")]
    [ResponseCache(CacheProfileName = "Caching")]
    [ExceptionFilter]
    public class TicketsController : Controller
    {
        /// <summary>
        /// IEventManagerClient object.
        /// </summary>
        private readonly IEventManagerClient _eventManagerClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="TicketsController"/> class.
        /// </summary>
        /// <param name="eventManagerClient">IEventManagerClient object.</param>
        public TicketsController(IEventManagerClient eventManagerClient)
        {
            _eventManagerClient = eventManagerClient;
        }

        /// <summary>
        /// Get all tickets.
        /// </summary>
        /// <param name="pageNumber">Page number.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpGet]
        [RedirectFilter("tickets")]
        public async Task<IActionResult> Index(int pageNumber = 1)
        {
            var tickets = await _eventManagerClient.GetTicketViewModels(HttpContext.GetJwtToken(), pageNumber);
            var nextTickets = await _eventManagerClient.GetTicketViewModels(HttpContext.GetJwtToken(), pageNumber + 1);
            PageViewModel.NextPage = nextTickets is not null && nextTickets.Any();
            PageViewModel.PageNumber = pageNumber;
            return View(tickets);
        }

        /// <summary>
        /// Details about ticket.
        /// </summary>
        /// <param name="id">Id of ticket.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            return id is null ? NotFound() : View(await _eventManagerClient.TicketDetails(HttpContext.GetJwtToken(), (int)id));
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
        /// <param name="ticketVm">Adding ticket.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TicketViewModel ticketVm)
        {
            if (!ModelState.IsValid)
            {
                return View(ticketVm);
            }

            await _eventManagerClient.CreateTicket(HttpContext.GetJwtToken(), ticketVm);
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Edit ticket.
        /// </summary>
        /// <param name="id">Id of editing ticket.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            return id is null ? NotFound() : View(await _eventManagerClient.GetTicketViewModelForEdit(HttpContext.GetJwtToken(), (int)id));
        }

        /// <summary>
        /// Edit ticket.
        /// </summary>
        /// <param name="id">Id of editing ticket.</param>
        /// <param name="ticketVm">Edited ticket.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TicketViewModel ticketVm)
        {
            if (id != ticketVm.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(ticketVm);
            }

            try
            {
                await _eventManagerClient.EditTicket(HttpContext.GetJwtToken(), id, ticketVm);
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict();
            }

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Delete ticket.
        /// </summary>
        /// <param name="id">Id of deleting ticket.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            return id is null ? NotFound() : View(await _eventManagerClient.GetTicketViewModelForDelete(HttpContext.GetJwtToken(), (int)id));
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
            await _eventManagerClient.DeleteTicket(HttpContext.GetJwtToken(), id);
            return RedirectToAction(nameof(Index));
        }
    }
}
