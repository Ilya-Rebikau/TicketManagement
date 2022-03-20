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
            services.AddScoped(typeof(IService<AreaDto>), typeof(AreaService));
            services.AddScoped(typeof(IService<LayoutDto>), typeof(LayoutService));
            services.AddScoped(typeof(IService<SeatDto>), typeof(SeatService));
            services.AddScoped(typeof(IService<VenueDto>), typeof(VenueService));
            return services;
        }
    }
}
