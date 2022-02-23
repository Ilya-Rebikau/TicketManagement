using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using TicketManagement.Web.Models.Events;

namespace TicketManagement.Web.Interfaces
{
    /// <summary>
    /// Web service for third party event controller.
    /// </summary>
    public interface IThirdPartyEventWebService
    {
        /// <summary>
        /// Get event view models from json file.
        /// </summary>
        /// <param name="file">Json file.</param>
        /// <returns>Collection of event view models.</returns>
        Task<IEnumerable<EventViewModel>> GetEventViewModelsFromJson(IFormFile file);
    }
}
