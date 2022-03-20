using System.Globalization;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TicketManagement.BusinessLogic.Configuration;
using TicketManagement.Web.Infrastructure;
using TicketManagement.Web.Interfaces;
using TicketManagement.Web.Models;
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
            services.AddScoped(typeof(IAccountWebService), typeof(AccountWebService));
            services.AddScoped(typeof(IUsersWebService), typeof(UsersWebService));
            services.AddControllersWithViews(options =>
                options.CacheProfiles.Add("Caching",
                new CacheProfile
                {
                    Location = ResponseCacheLocation.Client,
                    Duration = 300,
                }))
            .AddViewLocalization()
            .AddDataAnnotationsLocalization(options =>
            {
                options.DataAnnotationLocalizerProvider = (type, factory) =>
                {
                    var assemblyName = new AssemblyName(typeof(Program).GetTypeInfo().Assembly.FullName);
                    return factory.Create("ModelsTranslations", assemblyName.Name);
                };
            });
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("en-US"),
                    new CultureInfo("ru-RU"),
                    new CultureInfo("be-BY"),
                };

                options.DefaultRequestCulture = new RequestCulture("ru-RU");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });
            services.AddDbContext<IdentityContext>(options => options.UseSqlServer(connection));
            services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<IdentityContext>();
            return services;
        }
    }
}
