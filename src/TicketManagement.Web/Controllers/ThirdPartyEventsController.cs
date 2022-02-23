using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.BusinessLogic.ModelsDTO;
using TicketManagement.Web.Infrastructure;
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
        /// IReaderService object.
        /// </summary>
        private readonly IReaderService<EventDto> _reader;

        /// <summary>
        /// Initializes a new instance of the <see cref="ThirdPartyEventsController"/> class.
        /// </summary>
        /// <param name="reader">IReaderService object.</param>
        public ThirdPartyEventsController(IReaderService<EventDto> reader)
        {
            _reader = reader;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SaveEvents(IFormFile file)
        {
            var json = "";
            using (var stream = file.OpenReadStream())
            {
                byte[] buffer = new byte[stream.Length];
                await stream.ReadAsync(buffer, 0, buffer.Length);
                json = Encoding.Default.GetString(buffer);
            }

            var events = await _reader.GetAllAsync(json);
            var eventsVm = new List<EventViewModel>();
            foreach (var @event in events)
            {
                eventsVm.Add(@event);
            }

            return View(eventsVm);
        }
    }
}
