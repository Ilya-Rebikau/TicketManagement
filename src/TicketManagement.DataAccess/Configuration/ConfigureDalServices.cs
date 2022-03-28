using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Models;
using TicketManagement.DataAccess.RepositoriesEf;
using TicketManagement.DataAccess.RepositoriesJson;

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
            services.AddScoped<DbContext, TicketManagementContext>();
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped<IRepository<Event>, EventEfRepository>();
            services.AddScoped<IRepository<Area>, AreaEfRepository>();
            services.AddScoped<IRepository<Layout>, LayoutEfRepository>();
            services.AddScoped<IRepository<Venue>, VenueEfRepository>();
            services.AddScoped<IRepository<EventArea>, EventAreaEfRepository>();
            services.AddScoped<IReaderJson<ThirdPartyEvent>, ThirdPartyEventRepositoryJson>();
            return services;
        }
    }
}
