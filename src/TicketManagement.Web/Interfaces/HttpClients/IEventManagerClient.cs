using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using RestEase;
using TicketManagement.Web.Models.EventAreas;
using TicketManagement.Web.Models.Events;
using TicketManagement.Web.Models.EventSeats;
using TicketManagement.Web.Models.ThirdPartyEvents;
using TicketManagement.Web.Models.Tickets;

namespace TicketManagement.Web.Interfaces.HttpClients
{
    /// <summary>
    /// Client for EventManagerAPI.
    /// </summary>
    public interface IEventManagerClient
    {
        /// <summary>
        /// Key with authorization string in header.
        /// </summary>
        private const string AuthorizationKey = "Authorization";

        [Get("eventareas/getareas")]
        public Task<IEnumerable<EventAreaViewModel>> GetEventAreaViewModels([Header(AuthorizationKey)] string token, CancellationToken cancellationToken = default);

        [Get("eventareas/details/{id}")]
        public Task<EventAreaViewModel> EventAreaDetails([Header(AuthorizationKey)] string token, [Path] int id, CancellationToken cancellationToken = default);

        [Post("eventareas/create")]
        public Task CreateEventArea([Header(AuthorizationKey)] string token, [Body] EventAreaViewModel model, CancellationToken cancellationToken = default);

        [Get("eventareas/edit/{id}")]
        public Task<EventAreaViewModel> GetEventAreaViewModelForEdit([Header(AuthorizationKey)] string token, [Path] int id, CancellationToken cancellationToken = default);

        [Put("eventareas/edit/{id}")]
        public Task EditEventArea([Header(AuthorizationKey)] string token, [Path] int id, [Body] EventAreaViewModel model, CancellationToken cancellationToken = default);

        [Get("eventareas/delete/{id}")]
        public Task<EventAreaViewModel> GetEventAreaViewModelForDelete([Header(AuthorizationKey)] string token, [Path] int id, CancellationToken cancellationToken = default);

        [Delete("eventareas/delete/{id}")]
        public Task DeleteEventArea([Header(AuthorizationKey)] string token, [Path] int id, CancellationToken cancellationToken = default);

        [Get("eventseats/geteventseats")]
        public Task<IEnumerable<EventSeatViewModel>> GetEventSeatsViewModels([Header(AuthorizationKey)] string token, CancellationToken cancellationToken = default);

        [Get("eventseats/details/{id}")]
        public Task<EventSeatViewModel> EventSeatDetails([Header(AuthorizationKey)] string token, [Path] int id, CancellationToken cancellationToken = default);

        [Post("eventseats/create")]
        public Task CreateEventSeat([Header(AuthorizationKey)] string token, [Body] EventSeatViewModel model, CancellationToken cancellationToken = default);

        [Get("eventseats/edit/{id}")]
        public Task<EventSeatViewModel> GetEventSeatViewModelForEdit([Header(AuthorizationKey)] string token, [Path] int id, CancellationToken cancellationToken = default);

        [Put("eventseats/edit/{id}")]
        public Task EditEventSeat([Header(AuthorizationKey)] string token, [Path] int id, [Body] EventSeatViewModel model, CancellationToken cancellationToken = default);

        [Get("eventseats/delete/{id}")]
        public Task<EventSeatViewModel> GetEventSeatViewModelForDelete([Header(AuthorizationKey)] string token, [Path] int id, CancellationToken cancellationToken = default);

        [Delete("eventseats/delete/{id}")]
        public Task DeleteEventSeat([Header(AuthorizationKey)] string token, [Path] int id, CancellationToken cancellationToken = default);

        [Get("events/getevents")]
        public Task<IEnumerable<EventViewModel>> GetEventViewModels([Header(AuthorizationKey)] string token, CancellationToken cancellationToken = default);

        [Get("events/details/{id}")]
        public Task<EventViewModel> EventDetails([Header(AuthorizationKey)] string token, [Path] int id, CancellationToken cancellationToken = default);

        [Post("events/create")]
        public Task CreateEvent([Header(AuthorizationKey)] string token, [Body] EventViewModel model, CancellationToken cancellationToken = default);

        [Get("events/edit/{id}")]
        public Task<EventViewModel> GetEventViewModelForEdit([Header(AuthorizationKey)] string token, [Path] int id, CancellationToken cancellationToken = default);

        [Put("events/edit/{id}")]
        public Task EditEvent([Header(AuthorizationKey)] string token, [Path] int id, [Body] EventViewModel model, CancellationToken cancellationToken = default);

        [Get("events/delete/{id}")]
        public Task<EventViewModel> GetEventViewModelForDelete([Header(AuthorizationKey)] string token, [Path] int id, CancellationToken cancellationToken = default);

        [Delete("events/delete/{id}")]
        public Task DeleteEvent([Header(AuthorizationKey)] string token, [Path] int id, CancellationToken cancellationToken = default);

        [Get("tickets/gettickets")]
        public Task<IEnumerable<TicketViewModel>> GetTicketViewModels([Header(AuthorizationKey)] string token, CancellationToken cancellationToken = default);

        [Get("tickets/details/{id}")]
        public Task<TicketViewModel> TicketDetails([Header(AuthorizationKey)] string token, [Path] int id, CancellationToken cancellationToken = default);

        [Post("tickets/create")]
        public Task CreateTicket([Header(AuthorizationKey)] string token, [Body] TicketViewModel model, CancellationToken cancellationToken = default);

        [Get("tickets/edit/{id}")]
        public Task<TicketViewModel> GetTicketViewModelForEdit([Header(AuthorizationKey)] string token, [Path] int id, CancellationToken cancellationToken = default);

        [Put("tickets/edit/{id}")]
        public Task EditTicket([Header(AuthorizationKey)] string token, [Path] int id, [Body] TicketViewModel model, CancellationToken cancellationToken = default);

        [Get("tickets/delete/{id}")]
        public Task<TicketViewModel> GetTicketViewModelForDelete([Header(AuthorizationKey)] string token, [Path] int id, CancellationToken cancellationToken = default);

        [Delete("tickets/delete/{id}")]
        public Task DeleteTicket([Header(AuthorizationKey)] string token, [Path] int id, CancellationToken cancellationToken = default);

        [Post("thirdpartyevents/getevents")]
        public Task<IEnumerable<EventViewModel>> GetThirdPartyEventViewModels([Header(AuthorizationKey)] string token, [Body] ThirdPartyEventData data,
            CancellationToken cancellationToken = default);

        [Post("thirdpartyevents/save")]
        public Task SaveThirdPartyEventsToDatabase([Header(AuthorizationKey)] string token, [Body] IEnumerable<EventViewModel> events, CancellationToken cancellationToken = default);
    }
}
