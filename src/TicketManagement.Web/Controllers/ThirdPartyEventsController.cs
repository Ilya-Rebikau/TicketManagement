using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TicketManagement.Web.Extensions;
using TicketManagement.Web.Infrastructure;
using TicketManagement.Web.Interfaces;
using TicketManagement.Web.Models.Events;

namespace TicketManagement.Web.Controllers
{
    /// <summary>
    /// Controller for third party events.
    /// </summary>
    [Authorize(Roles = "admin, event manager")]
    [ResponseCache(CacheProfileName = "Caching")]
    [ExceptionFilter]
    public class ThirdPartyEventsController : Controller
    {
        /// <summary>
        /// IThirdPartyEventWebService object.
        /// </summary>
        private readonly IThirdPartyEventWebService _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="ThirdPartyEventsController"/> class.
        /// </summary>
        /// <param name="service">IThirdPartyEventWebService object.</param>
        public ThirdPartyEventsController(IThirdPartyEventWebService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ShowEvents(IFormFile file)
        {
            return View(await _service.GetEventViewModelsFromJson(file));
        }

        [HttpPost]
        public async Task<IActionResult> SaveToDatabaseAsync(IEnumerable<EventViewModel> events)
        {
            await _service.SaveToDatabase(events);
            return RedirectToAction(nameof(Index), typeof(EventsController).GetControllerName());
        }
    }
}
