using Microsoft.Extensions.DependencyInjection;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Models;
using TicketManagement.DataAccess.Repositories;

namespace TicketManagement.DataAccess.Configuration
{
    /// <summary>
    /// Configure repositories from DAL.
    /// </summary>
    public static class ConfigureRepositories
    {
        /// <summary>
        /// Extension method for IServiceCollection to add repositories.
        /// </summary>
        /// <param name="services">Services.</param>
        /// <returns>Added services.</returns>
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped(typeof(IRepository<Event>), typeof(EventEfRepository));
            return services;
        }
    }
}
