using Microsoft.Extensions.DependencyInjection;
using TicketManagement.DataAccess.Models;
using TicketManagement.PurchaseFlowAPI.Automapper;
using TicketManagement.PurchaseFlowAPI.Interfaces;
using TicketManagement.PurchaseFlowAPI.ModelsDTO;

namespace TicketManagement.PurchaseFlowAPI.Configuration
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
            services.AddScoped<IConverter<Ticket, TicketDto>, ModelsConverter<Ticket, TicketDto>>();
            return services;
        }
    }
}
