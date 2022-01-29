using Microsoft.Extensions.DependencyInjection;
using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.BusinessLogic.ModelsDTO;
using TicketManagement.BusinessLogic.Services;
using TicketManagement.DataAccess.Configuration;
using TicketManagement.DataAccess.Models;

namespace TicketManagement.BusinessLogic.Configuration
{
    /// <summary>
    /// Configure services from BLL.
    /// </summary>
    public static class ConfigureServices
    {
        /// <summary>
        /// Extension method for IServiceCollection to add services.
        /// </summary>
        /// <param name="services">Services.</param>
        /// <returns>Added services.</returns>
        public static IServiceCollection AddBllServices(this IServiceCollection services)
        {
            services.AddRepositories();
            services.AddScoped(typeof(IConverter<Area, AreaDto>), typeof(BaseConverter<Area, AreaDto>));
            services.AddScoped(typeof(IConverter<EventArea, EventAreaDto>), typeof(BaseConverter<EventArea, EventAreaDto>));
            services.AddScoped(typeof(IConverter<EventSeat, EventSeatDto>), typeof(BaseConverter<EventSeat, EventSeatDto>));
            services.AddScoped(typeof(IConverter<Event, EventDto>), typeof(BaseConverter<Event, EventDto>));
            services.AddScoped(typeof(IConverter<Layout, LayoutDto>), typeof(BaseConverter<Layout, LayoutDto>));
            services.AddScoped(typeof(IConverter<Seat, SeatDto>), typeof(BaseConverter<Seat, SeatDto>));
            services.AddScoped(typeof(IConverter<Venue, VenueDto>), typeof(BaseConverter<Venue, VenueDto>));
            services.AddScoped(typeof(IConverter<Ticket, TicketDto>), typeof(BaseConverter<Ticket, TicketDto>));
            services.AddScoped(typeof(IService<AreaDto>), typeof(AreaService));
            services.AddScoped(typeof(IService<EventAreaDto>), typeof(EventAreaService));
            services.AddScoped(typeof(IService<EventSeatDto>), typeof(EventSeatService));
            services.AddScoped(typeof(IService<EventDto>), typeof(EventService));
            services.AddScoped(typeof(IService<LayoutDto>), typeof(LayoutService));
            services.AddScoped(typeof(IService<SeatDto>), typeof(SeatService));
            services.AddScoped(typeof(IService<VenueDto>), typeof(VenueService));
            services.AddScoped(typeof(IService<TicketDto>), typeof(TicketService));
            return services;
        }
    }
}
