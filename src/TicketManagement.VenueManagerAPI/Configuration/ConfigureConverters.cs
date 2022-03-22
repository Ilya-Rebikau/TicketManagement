using Microsoft.Extensions.DependencyInjection;
using TicketManagement.DataAccess.Models;
using TicketManagement.VenueManagerAPI.Automapper;
using TicketManagement.VenueManagerAPI.Interfaces;
using TicketManagement.VenueManagerAPI.ModelsDTO;

namespace TicketManagement.VenueManagerAPI.Configuration
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
            services.AddScoped(typeof(IConverter<Area, AreaDto>), typeof(ModelsConverter<Area, AreaDto>));
            services.AddScoped(typeof(IConverter<Layout, LayoutDto>), typeof(ModelsConverter<Layout, LayoutDto>));
            services.AddScoped(typeof(IConverter<Seat, SeatDto>), typeof(ModelsConverter<Seat, SeatDto>));
            services.AddScoped(typeof(IConverter<Venue, VenueDto>), typeof(ModelsConverter<Venue, VenueDto>));
            return services;
        }
    }
}
