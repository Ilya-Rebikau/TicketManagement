using Microsoft.Extensions.DependencyInjection;
using TicketManagement.BusinessLogic.Configuration;
using TicketManagement.Web.Interfaces;
using TicketManagement.Web.WebServices;

namespace TicketManagement.Web.Configuration
{
    /// <summary>
    /// Configure services from presentation layer.
    /// </summary>
    public static class ConfigureWebServices
    {
        /// <summary>
        /// Extension method for IServiceCollection to add services.
        /// </summary>
        /// <param name="services">Services.</param>
        /// <param name="connection">Connection string to database.</param>
        /// <returns>Added services.</returns>
        public static IServiceCollection AddWebServices(this IServiceCollection services, string connection)
        {
            services.AddBllServices(connection);
            services.AddScoped(typeof(IEventWebService), typeof(EventWebService));
            services.AddScoped(typeof(IAccountWebService), typeof(AccountWebService));
            services.AddScoped(typeof(IUsersWebService), typeof(UsersWebService));
            services.AddScoped(typeof(ConverterForTime));
            return services;
        }
    }
}
