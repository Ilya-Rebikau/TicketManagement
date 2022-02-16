using Microsoft.Extensions.DependencyInjection;
using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.BusinessLogic.ModelsDTO;
using TicketManagement.BusinessLogic.Services;
using TicketManagement.DataAccess.Models;

namespace TicketManagement.BusinessLogic.Configuration
{
    /// <summary>
    /// Configure converters from BLL.
    /// </summary>
    public static class ConfigureConverters
    {
        /// <summary>
        /// Extension method for IServiceCollection to add services.
        /// </summary>
        /// <param name="services">Services.</param>
        /// <returns>Added services.</returns>
        public static IServiceCollection AddConverters(this IServiceCollection services)
        {
            services.AddScoped(typeof(IConverter<Area, AreaDto>), typeof(BaseConverter<Area, AreaDto>));
            services.AddScoped(typeof(IConverter<EventArea, EventAreaDto>), typeof(BaseConverter<EventArea, EventAreaDto>));
            services.AddScoped(typeof(IConverter<EventSeat, EventSeatDto>), typeof(BaseConverter<EventSeat, EventSeatDto>));
            services.AddScoped(typeof(IConverter<Event, EventDto>), typeof(BaseConverter<Event, EventDto>));
            services.AddScoped(typeof(IConverter<Layout, LayoutDto>), typeof(BaseConverter<Layout, LayoutDto>));
            services.AddScoped(typeof(IConverter<Seat, SeatDto>), typeof(BaseConverter<Seat, SeatDto>));
            services.AddScoped(typeof(IConverter<Venue, VenueDto>), typeof(BaseConverter<Venue, VenueDto>));
            services.AddScoped(typeof(IConverter<Ticket, TicketDto>), typeof(BaseConverter<Ticket, TicketDto>));
            return services;
        }
    }
}
