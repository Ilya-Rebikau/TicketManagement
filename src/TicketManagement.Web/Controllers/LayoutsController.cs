﻿using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketManagement.Web.Extensions;
using TicketManagement.Web.Infrastructure;
using TicketManagement.Web.Interfaces.HttpClients;
using TicketManagement.Web.Models;
using TicketManagement.Web.Models.Layouts;
using TicketManagement.Web.ModelsDTO;

namespace TicketManagement.Web.Controllers
{
    /// <summary>
    /// Controller for layouts.
    /// </summary>
    [Authorize(Roles = "admin, venue manager")]
    [ResponseCache(CacheProfileName = "Caching")]
    [ExceptionFilter]
    public class LayoutsController : Controller
    {
        /// <summary>
        /// IVenueManagerClient object.
        /// </summary>
        private readonly IVenueManagerClient _venueManagerClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutsController"/> class.
        /// </summary>
        /// <param name="venueManagerClient">IVenueManagerClient object.</param>
        public LayoutsController(IVenueManagerClient venueManagerClient)
        {
            _venueManagerClient = venueManagerClient;
        }

        /// <summary>
        /// Get all layouts.
        /// </summary>
        /// <param name="pageNumber">Page number.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpGet]
        public async Task<IActionResult> Index(int pageNumber = 1)
        {
            var layouts = await _venueManagerClient.GetLayoutViewModels(HttpContext.GetJwtToken(), pageNumber);
            var nextLayouts = await _venueManagerClient.GetLayoutViewModels(HttpContext.GetJwtToken(), pageNumber + 1);
            PageViewModel.NextPage = nextLayouts is not null && nextLayouts.Any();
            PageViewModel.PageNumber = pageNumber;
            return View(layouts);
        }

        /// <summary>
        /// Details about layout.
        /// </summary>
        /// <param name="id">Id of layout.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            return id is null ? NotFound() : View(await _venueManagerClient.LayoutDetails(HttpContext.GetJwtToken(), (int)id));
        }

        /// <summary>
        /// Create layout.
        /// </summary>
        /// <returns>IActionResult.</returns>
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Create layout.
        /// </summary>
        /// <param name="layoutVm">Adding layout.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LayoutViewModel layoutVm)
        {
            if (!ModelState.IsValid)
            {
                return View(layoutVm);
            }

            await _venueManagerClient.CreateLayout(HttpContext.GetJwtToken(), layoutVm);
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Edit layout.
        /// </summary>
        /// <param name="id">Id of editing layout.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            return id is null ? NotFound() : View(await _venueManagerClient.GetLayoutViewModelForEdit(HttpContext.GetJwtToken(), (int)id));
        }

        /// <summary>
        /// Edit layout.
        /// </summary>
        /// <param name="id">Id of editing layout.</param>
        /// <param name="layoutVm">Edited layout.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, LayoutViewModel layoutVm)
        {
            if (id != layoutVm.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(layoutVm);
            }

            try
            {
                await _venueManagerClient.EditLayout(HttpContext.GetJwtToken(), id, layoutVm);
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict();
            }

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Delete layout.
        /// </summary>
        /// <param name="id">Id of deleting layout.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            return id is null ? NotFound() : View(await _venueManagerClient.GetLayoutViewModelForDelete(HttpContext.GetJwtToken(), (int)id));
        }

        /// <summary>
        /// Delete confirmation.
        /// </summary>
        /// <param name="id">Id of deleting layout.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _venueManagerClient.DeleteLayout(HttpContext.GetJwtToken(), id);
            return RedirectToAction(nameof(Index));
        }
    }
}
