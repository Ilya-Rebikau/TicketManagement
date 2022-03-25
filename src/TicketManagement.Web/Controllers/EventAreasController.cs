using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketManagement.Web.Extensions;
using TicketManagement.Web.Infrastructure;
using TicketManagement.Web.Interfaces.HttpClients;
using TicketManagement.Web.Models.EventAreas;
using TicketManagement.Web.ModelsDTO;

namespace TicketManagement.Web.Controllers
{
    /// <summary>
    /// Controller for event areas.
    /// </summary>
    [Authorize(Roles = "admin, event manager")]
    [ExceptionFilter]
    public class EventAreasController : Controller
    {
        /// <summary>
        /// IEventManagerClient object.
        /// </summary>
        private readonly IEventManagerClient _eventManagerClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventAreasController"/> class.
        /// </summary>
        /// <param name="eventManagerClient">IEventManagerClient object.</param>
        public EventAreasController(IEventManagerClient eventManagerClient)
        {
            _eventManagerClient = eventManagerClient;
        }

        /// <summary>
        /// All event areas.
        /// </summary>
        /// <param name="pageNumber">Page number.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpGet]
        public async Task<IActionResult> Index(int pageNumber = 1)
        {
            var eventAreas = await _eventManagerClient.GetEventAreaViewModels(HttpContext.GetJwtToken(), pageNumber);
            return View(eventAreas);
        }

        /// <summary>
        /// Details about event area.
        /// </summary>
        /// <param name="id">Id of event area.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            return id is null ? NotFound() : View(await _eventManagerClient.EventAreaDetails(HttpContext.GetJwtToken(), (int)id));
        }

        /// <summary>
        /// Create event area.
        /// </summary>
        /// <returns>IActionResult.</returns>
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Create event area.
        /// </summary>
        /// <param name="eventAreaVm">Adding event area.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EventAreaViewModel eventAreaVm)
        {
            if (!ModelState.IsValid)
            {
                return View(eventAreaVm);
            }

            await _eventManagerClient.CreateEventArea(HttpContext.GetJwtToken(), eventAreaVm);
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Edit event area.
        /// </summary>
        /// <param name="id">Id of editing event area.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            return id is null ? NotFound() : View(await _eventManagerClient.GetEventAreaViewModelForEdit(HttpContext.GetJwtToken(), (int)id));
        }

        /// <summary>
        /// Edit event area.
        /// </summary>
        /// <param name="id">Id of editing event area.</param>
        /// <param name="eventAreaVm">Edited event area.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EventAreaViewModel eventAreaVm)
        {
            if (id != eventAreaVm.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(eventAreaVm);
            }

            try
            {
                await _eventManagerClient.EditEventArea(HttpContext.GetJwtToken(), id, eventAreaVm);
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict();
            }

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Delete event area.
        /// </summary>
        /// <param name="id">Id of deleting event area.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            return id is null ? NotFound() : View(await _eventManagerClient.GetEventAreaViewModelForDelete(HttpContext.GetJwtToken(), (int)id));
        }

        /// <summary>
        /// Delete confirmation.
        /// </summary>
        /// <param name="id">Id of deleting event area.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _eventManagerClient.DeleteEventArea(HttpContext.GetJwtToken(), id);
            return RedirectToAction(nameof(Index));
        }
    }
}
