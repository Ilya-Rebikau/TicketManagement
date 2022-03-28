using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketManagement.EventManagerAPI.Interfaces;
using TicketManagement.EventManagerAPI.Models.Events;
using TicketManagement.EventManagerAPI.Models.ThirdPartyEvents;

namespace TicketManagement.EventManagerAPI.Controllers
{
    [Authorize(Roles = "admin, event manager")]
    [Route("[controller]")]
    [ApiController]
    public class ThirdPartyEventsController : ControllerBase
    {
        /// <summary>
        /// IThirdPartyEventService object.
        /// </summary>
        private readonly IThirdPartyEventService _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="ThirdPartyEventsController"/> class.
        /// </summary>
        /// <param name="service">IThirdPartyEventService object.</param>
        public ThirdPartyEventsController(IThirdPartyEventService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get third party events.
        /// </summary>
        /// <param name="data">Data with third party events.</param>
        /// <returns>Third party events.</returns>
        [HttpPost("getevents")]
        public async Task<IActionResult> GetEvents([FromBody] ThirdPartyEventData data)
        {
            return Ok(await _service.GetEventViewModelsFromJson(data.BytesData));
        }

        /// <summary>
        /// Save third party events to database.
        /// </summary>
        /// <param name="events">Third party events.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpPost("save")]
        public async Task<IActionResult> SaveToDatabaseAsync([FromBody] IEnumerable<EventModel> events)
        {
            await _service.SaveToDatabase(events);
            return Ok();
        }
    }
}
