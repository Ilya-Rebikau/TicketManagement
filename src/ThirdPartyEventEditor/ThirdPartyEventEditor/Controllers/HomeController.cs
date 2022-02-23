using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using ThirdPartyEventEditor.Filters;
using ThirdPartyEventEditor.Interfaces;
using ThirdPartyEventEditor.Models;

namespace ThirdPartyEventEditor.Controllers
{
    [GlobalExceptionFilter]
    [LogExecutionTimeFilter]
    public class HomeController : Controller
    {
        /// <summary>
        /// IEventWebService object.
        /// </summary>
        private readonly IEventWebService _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController"/> class.
        /// </summary>
        /// <param name="service">IEventWebService object.</param>
        public HomeController(IEventWebService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get all events.
        /// </summary>
        /// <returns>ActionResult object.</returns>
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            return View(await _service.GetAllAsync());
        }

        /// <summary>
        /// Details about event.
        /// </summary>
        /// <param name="id">Id of event.</param>
        /// <returns>ActionResult object.</returns>
        [HttpGet]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("Event wasn't found.");
            }

            var @event = await _service.GetByIdAsync((int)id);
            if (@event == null)
            {
                throw new ArgumentNullException("Event wasn't found.");
            }

            return View(@event);
        }

        /// <summary>
        /// Create event.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Create event.
        /// </summary>
        /// <param name="event">Adding event.</param>
        /// <returns>Task with ActionResult.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ThirdPartyEvent @event)
        {
            if (!ModelState.IsValid)
            {
                return View(@event);
            }

            @event.PosterImage = await _service.UploadSampleImage(@event.PosterImage);
            await _service.CreateAsync(@event);
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Edit event.
        /// </summary>
        /// <param name="id">Id of editing event.</param>
        /// <returns>ActionResult object.</returns>
        [HttpGet]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("Event wasn't found.");
            }

            var updatingEvent = await _service.GetByIdAsync((int)id);
            if (updatingEvent == null)
            {
                throw new ArgumentNullException("Event wasn't found.");
            }

            return View(updatingEvent);
        }

        /// <summary>
        /// Edit event.
        /// </summary>
        /// <param name="id">Id of editing event.</param>
        /// <param name="event">Edited event.</param>
        /// <returns>Task with ActionResult.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, ThirdPartyEvent @event)
        {
            if (id != @event.Id)
            {
                throw new ArgumentNullException("Event wasn't found.");
            }

            if (!ModelState.IsValid)
            {
                return View(@event);
            }

            @event.PosterImage = await _service.UploadSampleImage(@event.PosterImage);
            await _service.UpdateAsync(@event);

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Delete event.
        /// </summary>
        /// <param name="id">Id of deleting event.</param>
        /// <returns>ActionResult object.</returns>
        [HttpGet]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("Event wasn't found.");
            }

            var deletingEvent = await _service.GetByIdAsync((int)id);
            if (deletingEvent == null)
            {
                throw new ArgumentNullException("Event wasn't found.");
            }

            return View(deletingEvent);
        }

        /// <summary>
        /// Delete confirmation.
        /// </summary>
        /// <param name="id">Id of deleting event.</param>
        /// <returns>ActionResult object.</returns>
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await _service.DeleteByIdAsync(id);
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Download file from server.
        /// </summary>
        /// <returns>File.</returns>
        [HttpGet]
        public FileResult SaveFile()
        {
            var file = _service.GetFileInfo();
            return File(file.FullPathWithName, file.Type, file.Name);
        }
    }
}