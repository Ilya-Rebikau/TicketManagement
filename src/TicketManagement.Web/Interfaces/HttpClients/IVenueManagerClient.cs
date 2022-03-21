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
    public interface IVenueManagerClient
    {
        [Get("venues/getvenues")]
        public Task<IEnumerable<VenueViewModel>> GetVenueViewModels([Header("Authorization")] string token, CancellationToken cancellationToken = default);

        [Get("venues/details/{id}")]
        public Task<VenueViewModel> VenueDetails([Header("Authorization")] string token, [Path] int id, CancellationToken cancellationToken = default);

        [Post("venues/create")]
        public Task CreateVenue([Header("Authorization")] string token, [Body] VenueViewModel model, CancellationToken cancellationToken = default);

        [Get("venues/edit/{id}")]
        public Task<VenueViewModel> GetVenueViewModelForEdit([Header("Authorization")] string token, [Path] int id, CancellationToken cancellationToken = default);

        [Put("venues/edit/{id}")]
        public Task EditVenue([Header("Authorization")] string token, [Path] int id, [Body] VenueViewModel model, CancellationToken cancellationToken = default);

        [Get("venues/delete/{id}")]
        public Task<VenueViewModel> GetVenueViewModelForDelete([Header("Authorization")] string token, [Path] int id, CancellationToken cancellationToken = default);

        [Delete("venues/delete/{id}")]
        public Task DeleteVenue([Header("Authorization")] string token, [Path] int id, CancellationToken cancellationToken = default);

        [Get("seats/getseats")]
        public Task<IEnumerable<SeatViewModel>> GetSeatViewModels([Header("Authorization")] string token, CancellationToken cancellationToken = default);

        [Get("seats/details/{id}")]
        public Task<SeatViewModel> SeatDetails([Header("Authorization")] string token, [Path] int id, CancellationToken cancellationToken = default);

        [Post("seats/create")]
        public Task CreateSeat([Header("Authorization")] string token, [Body] SeatViewModel model, CancellationToken cancellationToken = default);

        [Get("seats/edit/{id}")]
        public Task<SeatViewModel> GetSeatViewModelForEdit([Header("Authorization")] string token, [Path] int id, CancellationToken cancellationToken = default);

        [Put("seats/edit/{id}")]
        public Task EditSeat([Header("Authorization")] string token, [Path] int id, [Body] SeatViewModel model, CancellationToken cancellationToken = default);

        [Get("seats/delete/{id}")]
        public Task<SeatViewModel> GetSeatViewModelForDelete([Header("Authorization")] string token, [Path] int id, CancellationToken cancellationToken = default);

        [Delete("seats/delete/{id}")]
        public Task DeleteSeat([Header("Authorization")] string token, [Path] int id, CancellationToken cancellationToken = default);

        [Get("layouts/getlayouts")]
        public Task<IEnumerable<LayoutViewModel>> GetLayoutViewModels([Header("Authorization")] string token, CancellationToken cancellationToken = default);

        [Get("layouts/details/{id}")]
        public Task<LayoutViewModel> LayoutDetails([Header("Authorization")] string token, [Path] int id, CancellationToken cancellationToken = default);

        [Post("layouts/create")]
        public Task CreateLayout([Header("Authorization")] string token, [Body] LayoutViewModel model, CancellationToken cancellationToken = default);

        [Get("layouts/edit/{id}")]
        public Task<LayoutViewModel> GetLayoutViewModelForEdit([Header("Authorization")] string token, [Path] int id, CancellationToken cancellationToken = default);

        [Put("layouts/edit/{id}")]
        public Task EditLayout([Header("Authorization")] string token, [Path] int id, [Body] LayoutViewModel model, CancellationToken cancellationToken = default);

        [Get("layouts/delete/{id}")]
        public Task<LayoutViewModel> GetLayoutViewModelForDelete([Header("Authorization")] string token, [Path] int id, CancellationToken cancellationToken = default);

        [Delete("layouts/delete/{id}")]
        public Task DeleteLayout([Header("Authorization")] string token, [Path] int id, CancellationToken cancellationToken = default);

        [Get("areas/getareas")]
        public Task<IEnumerable<AreaViewModel>> GetAreaViewModels([Header("Authorization")] string token, CancellationToken cancellationToken = default);

        [Get("areas/details/{id}")]
        public Task<AreaViewModel> AreaDetails([Header("Authorization")] string token, [Path] int id, CancellationToken cancellationToken = default);

        [Post("areas/create")]
        public Task CreateArea([Header("Authorization")] string token, [Body] AreaViewModel model, CancellationToken cancellationToken = default);

        [Get("areas/edit/{id}")]
        public Task<AreaViewModel> GetAreaViewModelForEdit([Header("Authorization")] string token, [Path] int id, CancellationToken cancellationToken = default);

        [Put("areas/edit/{id}")]
        public Task EditArea([Header("Authorization")] string token, [Path] int id, [Body] AreaViewModel model, CancellationToken cancellationToken = default);

        [Get("areas/delete/{id}")]
        public Task<AreaViewModel> GetAreaViewModelForDelete([Header("Authorization")] string token, [Path] int id, CancellationToken cancellationToken = default);

        [Delete("areas/delete/{id}")]
        public Task DeleteArea([Header("Authorization")] string token, [Path] int id, CancellationToken cancellationToken = default);
    }
}
