using System;
using System.Globalization;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using RestEase;
using TicketManagement.Web.Infrastructure;
using TicketManagement.Web.Interfaces;
using TicketManagement.Web.Interfaces.HttpClients;
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
        /// <param name="configuration">IConfiguration object.</param>
        /// <returns>Added services.</returns>
        public static IServiceCollection AddWebServices(this IServiceCollection services, string connection, IConfiguration configuration)
        {
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
            services.AddHttpClient();
            services.AddScoped(scope =>
            {
                var baseUrl = configuration["UsersApiAddress"];
                return RestClient.For<IUsersClient>(baseUrl);
            });
            services.AddScoped(scope =>
            {
                var baseUrl = configuration["EventManagerApiAddress"];
                return RestClient.For<IEventManagerClient>(baseUrl);
            });
            services.AddScoped(scope =>
            {
                var baseUrl = configuration["PurchaseFlowApiAddress"];
                return RestClient.For<IPurchaseFlowClient>(baseUrl);
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MVC", Version = "v1" });
                var jwtSecurityScheme = new OpenApiSecurityScheme
                {
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Name = "JWT Authentication",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme,
                    },
                };
                c.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { jwtSecurityScheme, Array.Empty<string>() },
                });
            });
            return services;
        }
    }
}
