using Microsoft.Extensions.DependencyInjection;
using TicketManagement.DataAccess.Configuration;
using TicketManagement.DataAccess.Models;
using TicketManagement.EventManagerAPI.Interfaces;
using TicketManagement.EventManagerAPI.ModelsDTO;
using TicketManagement.EventManagerAPI.Services;

namespace TicketManagement.EventManagerAPI.Configuration
{
    /// <summary>
    /// Configure services from BLL.
    /// </summary>
    public static class ConfigureBllServices
    {
        /// <summary>
        /// Extension method for IServiceCollection to add services.
        /// </summary>
        /// <param name="services">Services.</param>
        /// <param name="connection">Connection string to database.</param>
        /// <returns>Added services.</returns>
        public static IServiceCollection AddBllServices(this IServiceCollection services, string connection)
        {
            services.AddDalServices(connection);
            services.AddConverters();
            services.AddScoped<IService<EventAreaDto>, EventAreaService>();
            services.AddScoped<IService<EventSeatDto>, EventSeatService>();
            services.AddScoped<IService<EventDto>, EventCrudService>();
            services.AddScoped<IEventService, EventService>();
            services.AddScoped<IReaderService<EventDto>, BaseReaderService<ThirdPartyEvent, EventDto>>();
            return services;
        }
    }
}
