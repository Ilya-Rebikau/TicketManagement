using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RestEase;
using TicketManagement.Web.Models.Areas;
using TicketManagement.Web.Models.Layouts;
using TicketManagement.Web.Models.Seats;
using TicketManagement.Web.Models.Venues;

namespace TicketManagement.Web.Interfaces.HttpClients
{
    /// <summary>
    /// Client for VenueManagerClient.
    /// </summary>
    public interface IVenueManagerClient
    {
        /// <summary>
        /// Key with authorization string in header.
        /// </summary>
        private const string AuthorizationKey = "Authorization";

        [Get("venues/getvenues")]
        public Task<IEnumerable<VenueViewModel>> GetVenueViewModels([Header(AuthorizationKey)] string token, CancellationToken cancellationToken = default);

        [Get("venues/details/{id}")]
        public Task<VenueViewModel> VenueDetails([Header(AuthorizationKey)] string token, [Path] int id, CancellationToken cancellationToken = default);

        [Post("venues/create")]
        public Task CreateVenue([Header(AuthorizationKey)] string token, [Body] VenueViewModel model, CancellationToken cancellationToken = default);

        [Get("venues/edit/{id}")]
        public Task<VenueViewModel> GetVenueViewModelForEdit([Header(AuthorizationKey)] string token, [Path] int id, CancellationToken cancellationToken = default);

        [Put("venues/edit/{id}")]
        public Task EditVenue([Header(AuthorizationKey)] string token, [Path] int id, [Body] VenueViewModel model, CancellationToken cancellationToken = default);

        [Get("venues/delete/{id}")]
        public Task<VenueViewModel> GetVenueViewModelForDelete([Header(AuthorizationKey)] string token, [Path] int id, CancellationToken cancellationToken = default);

        [Delete("venues/delete/{id}")]
        public Task DeleteVenue([Header(AuthorizationKey)] string token, [Path] int id, CancellationToken cancellationToken = default);

        [Get("seats/getseats")]
        public Task<IEnumerable<SeatViewModel>> GetSeatViewModels([Header(AuthorizationKey)] string token, CancellationToken cancellationToken = default);

        [Get("seats/details/{id}")]
        public Task<SeatViewModel> SeatDetails([Header(AuthorizationKey)] string token, [Path] int id, CancellationToken cancellationToken = default);

        [Post("seats/create")]
        public Task CreateSeat([Header(AuthorizationKey)] string token, [Body] SeatViewModel model, CancellationToken cancellationToken = default);

        [Get("seats/edit/{id}")]
        public Task<SeatViewModel> GetSeatViewModelForEdit([Header(AuthorizationKey)] string token, [Path] int id, CancellationToken cancellationToken = default);

        [Put("seats/edit/{id}")]
        public Task EditSeat([Header(AuthorizationKey)] string token, [Path] int id, [Body] SeatViewModel model, CancellationToken cancellationToken = default);

        [Get("seats/delete/{id}")]
        public Task<SeatViewModel> GetSeatViewModelForDelete([Header(AuthorizationKey)] string token, [Path] int id, CancellationToken cancellationToken = default);

        [Delete("seats/delete/{id}")]
        public Task DeleteSeat([Header(AuthorizationKey)] string token, [Path] int id, CancellationToken cancellationToken = default);

        [Get("layouts/getlayouts")]
        public Task<IEnumerable<LayoutViewModel>> GetLayoutViewModels([Header(AuthorizationKey)] string token, CancellationToken cancellationToken = default);

        [Get("layouts/details/{id}")]
        public Task<LayoutViewModel> LayoutDetails([Header(AuthorizationKey)] string token, [Path] int id, CancellationToken cancellationToken = default);

        [Post("layouts/create")]
        public Task CreateLayout([Header(AuthorizationKey)] string token, [Body] LayoutViewModel model, CancellationToken cancellationToken = default);

        [Get("layouts/edit/{id}")]
        public Task<LayoutViewModel> GetLayoutViewModelForEdit([Header(AuthorizationKey)] string token, [Path] int id, CancellationToken cancellationToken = default);

        [Put("layouts/edit/{id}")]
        public Task EditLayout([Header(AuthorizationKey)] string token, [Path] int id, [Body] LayoutViewModel model, CancellationToken cancellationToken = default);

        [Get("layouts/delete/{id}")]
        public Task<LayoutViewModel> GetLayoutViewModelForDelete([Header(AuthorizationKey)] string token, [Path] int id, CancellationToken cancellationToken = default);

        [Delete("layouts/delete/{id}")]
        public Task DeleteLayout([Header(AuthorizationKey)] string token, [Path] int id, CancellationToken cancellationToken = default);

        [Get("areas/getareas")]
        public Task<IEnumerable<AreaViewModel>> GetAreaViewModels([Header(AuthorizationKey)] string token, CancellationToken cancellationToken = default);

        [Get("areas/details/{id}")]
        public Task<AreaViewModel> AreaDetails([Header(AuthorizationKey)] string token, [Path] int id, CancellationToken cancellationToken = default);

        [Post("areas/create")]
        public Task CreateArea([Header(AuthorizationKey)] string token, [Body] AreaViewModel model, CancellationToken cancellationToken = default);

        [Get("areas/edit/{id}")]
        public Task<AreaViewModel> GetAreaViewModelForEdit([Header(AuthorizationKey)] string token, [Path] int id, CancellationToken cancellationToken = default);

        [Put("areas/edit/{id}")]
        public Task EditArea([Header(AuthorizationKey)] string token, [Path] int id, [Body] AreaViewModel model, CancellationToken cancellationToken = default);

        [Get("areas/delete/{id}")]
        public Task<AreaViewModel> GetAreaViewModelForDelete([Header(AuthorizationKey)] string token, [Path] int id, CancellationToken cancellationToken = default);

        [Delete("areas/delete/{id}")]
        public Task DeleteArea([Header(AuthorizationKey)] string token, [Path] int id, CancellationToken cancellationToken = default);
    }
}
