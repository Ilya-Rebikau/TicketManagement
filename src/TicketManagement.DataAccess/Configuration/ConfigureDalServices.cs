using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Models;
using TicketManagement.DataAccess.RepositoriesEf;

namespace TicketManagement.DataAccess.Configuration
{
    /// <summary>
    /// Configure repositories from DAL.
    /// </summary>
    public static class ConfigureDalServices
    {
        /// <summary>
        /// Extension method for IServiceCollection to add dal services.
        /// </summary>
        /// <param name="services">Services.</param>
        /// <param name="connection">Connection string to database.</param>
        /// <returns>Added services.</returns>
        public static IServiceCollection AddDalServices(this IServiceCollection services, string connection)
        {
            services.AddDbContext<TicketManagementContext>(options =>
                options.UseSqlServer(connection));
            services.AddScoped(typeof(DbContext), typeof(TicketManagementContext));
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped(typeof(IRepository<Event>), typeof(EventEfRepository));
            services.AddScoped(typeof(IRepository<Area>), typeof(AreaEfRepository));
            services.AddScoped(typeof(IRepository<Layout>), typeof(LayoutEfRepository));
            services.AddScoped(typeof(IRepository<Venue>), typeof(VenueEfRepository));
            services.AddScoped(typeof(IRepository<EventArea>), typeof(EventAreaEfRepository));
            return services;
        }
    }
}
