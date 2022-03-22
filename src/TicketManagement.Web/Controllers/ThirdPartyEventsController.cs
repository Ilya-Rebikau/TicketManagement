using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TicketManagement.Web.Extensions;
using TicketManagement.Web.Infrastructure;
using TicketManagement.Web.Interfaces.HttpClients;
using TicketManagement.Web.Models.Events;
using TicketManagement.Web.Models.ThirdPartyEvents;

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
        /// IEventManagerClient object.
        /// </summary>
        private readonly IEventManagerClient _eventManagerClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="ThirdPartyEventsController"/> class.
        /// </summary>
        /// <param name="eventManagerClient">IEventManagerClient object.</param>
        public ThirdPartyEventsController(IEventManagerClient eventManagerClient)
        {
            _eventManagerClient = eventManagerClient;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ShowEvents(IFormFile file)
        {
            using var stream = file.OpenReadStream();
            byte[] fileData = new byte[stream.Length];
            await stream.ReadAsync(fileData);
            var data = new ThirdPartyEventData
            {
                BytesData = fileData,
            };
            return View(await _eventManagerClient.GetThirdPartyEventViewModels(HttpContext.GetJwtToken(), data));
        }

        [HttpPost]
        public async Task<IActionResult> SaveToDatabaseAsync(IEnumerable<EventViewModel> events)
        {
            await _eventManagerClient.SaveThirdPartyEventsToDatabase(HttpContext.GetJwtToken(), events);
            return RedirectToAction(nameof(Index), typeof(EventsController).GetControllerName());
        }
    }
}
