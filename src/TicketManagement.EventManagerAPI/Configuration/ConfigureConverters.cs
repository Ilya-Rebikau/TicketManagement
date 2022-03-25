using Microsoft.Extensions.DependencyInjection;
using TicketManagement.DataAccess.Models;
using TicketManagement.EventManagerAPI.Automapper;
using TicketManagement.EventManagerAPI.Interfaces;
using TicketManagement.EventManagerAPI.Models.EventAreas;
using TicketManagement.EventManagerAPI.Models.Events;
using TicketManagement.EventManagerAPI.Models.EventSeats;
using TicketManagement.EventManagerAPI.Models.Tickets;
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
            services.AddScoped<IConverter<Ticket, TicketDto>, ModelsConverter<Ticket, TicketDto>>();
            services.AddScoped<IConverter<ThirdPartyEvent, EventDto>, ModelsConverter<ThirdPartyEvent, EventDto>>();
            services.AddScoped<IConverter<EventDto, EventModel>, ModelsConverter<EventDto, EventModel>>();
            services.AddScoped<IConverter<EventSeatDto, EventSeatModel>, ModelsConverter<EventSeatDto, EventSeatModel>>();
            services.AddScoped<IConverter<EventAreaDto, EventAreaModel>, ModelsConverter<EventAreaDto, EventAreaModel>>();
            services.AddScoped<IConverter<TicketDto, TicketModel>, ModelsConverter<TicketDto, TicketModel>>();
            return services;
        }
    }
}
