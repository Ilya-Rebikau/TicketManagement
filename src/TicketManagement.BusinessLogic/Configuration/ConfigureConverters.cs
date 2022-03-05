using Microsoft.Extensions.DependencyInjection;
using TicketManagement.BusinessLogic.Automapper;
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
            services.AddAutoMapper(typeof(AutoMapperProfile));
            services.AddScoped(typeof(IConverter<Area, AreaDto>), typeof(ModelsConverter<Area, AreaDto>));
            services.AddScoped(typeof(IConverter<EventArea, EventAreaDto>), typeof(ModelsConverter<EventArea, EventAreaDto>));
            services.AddScoped(typeof(IConverter<EventSeat, EventSeatDto>), typeof(ModelsConverter<EventSeat, EventSeatDto>));
            services.AddScoped(typeof(IConverter<Event, EventDto>), typeof(ModelsConverter<Event, EventDto>));
            services.AddScoped(typeof(IConverter<Layout, LayoutDto>), typeof(ModelsConverter<Layout, LayoutDto>));
            services.AddScoped(typeof(IConverter<Seat, SeatDto>), typeof(ModelsConverter<Seat, SeatDto>));
            services.AddScoped(typeof(IConverter<Venue, VenueDto>), typeof(ModelsConverter<Venue, VenueDto>));
            services.AddScoped(typeof(IConverter<Ticket, TicketDto>), typeof(ModelsConverter<Ticket, TicketDto>));
            services.AddScoped(typeof(IConverter<ThirdPartyEvent, EventDto>), typeof(ModelsConverter<ThirdPartyEvent, EventDto>));
            return services;
        }
    }
}
