using Microsoft.Extensions.DependencyInjection;
using TicketManagement.DataAccess.Models;
using TicketManagement.EventManagerAPI.Automapper;
using TicketManagement.EventManagerAPI.Interfaces;
using TicketManagement.EventManagerAPI.ModelsDTO;

namespace TicketManagement.EventManagerAPI.Configuration
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
            services.AddScoped<IConverter<EventArea, EventAreaDto>, ModelsConverter<EventArea, EventAreaDto>>();
            services.AddScoped<IConverter<EventSeat, EventSeatDto>, ModelsConverter<EventSeat, EventSeatDto>>();
            services.AddScoped<IConverter<Event, EventDto>, ModelsConverter<Event, EventDto>>();
            services.AddScoped<IConverter<ThirdPartyEvent, EventDto>, ModelsConverter<ThirdPartyEvent, EventDto>>();
            return services;
        }
    }
}
