using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using TicketManagement.BusinessLogic.ModelsDTO;
using TicketManagement.Web.Models.Events;
using TicketManagement.Web.Models.Tickets;

namespace TicketManagement.Web.Interfaces
{
    /// <summary>
    /// Web service for event controller.
    /// </summary>
    public interface IEventWebService
    {
        /// <summary>
        /// Get all event view models.
        /// </summary>
        /// <returns>Collection of event view models.</returns>
        Task<IEnumerable<EventViewModel>> GetAllEventViewModelsAsync();

        /// <summary>
        /// Get event view model for details method.
        /// </summary>
        /// <param name="event">Event dto model for convertations.</param>
        /// <param name="httpContext">HttpContext object.</param>
        /// <returns>Event view model.</returns>
        Task<EventViewModel> GetEventViewModelForDetailsAsync(EventDto @event, HttpContext httpContext);

        /// <summary>
        /// Get event view model for edit and delete methods.
        /// </summary>
        /// <param name="event">Event.</param>
        /// <param name="httpContext">HttpContext object.</param>
        /// <returns>Event view model.</returns>
        Task<EventViewModel> GetEventViewModelForEditAndDeleteAsync(EventDto @event, HttpContext httpContext);

        /// <summary>
        /// Get ticket view model for buy method.
        /// </summary>
        /// <param name="eventSeatId">EventSeat id.</param>
        /// <param name="price">Price for ticket.</param>
        /// <param name="httpContext">HttpContext object.</param>
        /// <returns>Ticket view model.</returns>
        Task<TicketViewModel> GetTicketViewModelForBuyAsync(int? eventSeatId, double? price, HttpContext httpContext);

        /// <summary>
        /// Update event seat state to occupied after buying ticket.
        /// </summary>
        /// <param name="ticketVm">Ticket view model.</param>
        /// <returns>True if state was changed and false if not because of low balance.</returns>
        Task<bool> UpdateEventSeatStateAfterBuyingTicket(TicketViewModel ticketVm);
    }
}
