using Microsoft.Extensions.DependencyInjection;
using TicketManagement.DataAccess.Configuration;
using TicketManagement.PurchaseFlowAPI.Interfaces;
using TicketManagement.PurchaseFlowAPI.ModelsDTO;
using TicketManagement.PurchaseFlowAPI.Services;

namespace TicketManagement.PurchaseFlowAPI.Configuration
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
            services.AddScoped<IService<EventDto>, EventService>();
            services.AddScoped<IService<TicketDto>, TicketService>();
            services.AddScoped<IPurchaseService, PurchaseService>();
            return services;
        }
    }
}
