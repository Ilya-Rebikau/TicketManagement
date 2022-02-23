using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TicketManagement.Web.Infrastructure;
using TicketManagement.Web.Interfaces;

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

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ShowEvents(IFormFile file)
        {
            return View(await _service.GetEventViewModelsFromJson(file));
        }
    }
}
