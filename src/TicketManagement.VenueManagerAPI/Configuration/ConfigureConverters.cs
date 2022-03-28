using Microsoft.Extensions.DependencyInjection;
using TicketManagement.DataAccess.Models;
using TicketManagement.VenueManagerAPI.Automapper;
using TicketManagement.VenueManagerAPI.Interfaces;
using TicketManagement.VenueManagerAPI.Models.Areas;
using TicketManagement.VenueManagerAPI.Models.Layouts;
using TicketManagement.VenueManagerAPI.Models.Seats;
using TicketManagement.VenueManagerAPI.Models.Venues;
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
            services.AddScoped<IConverter<Area, AreaDto>, ModelsConverter<Area, AreaDto>>();
            services.AddScoped<IConverter<Layout, LayoutDto>, ModelsConverter<Layout, LayoutDto>>();
            services.AddScoped<IConverter<Seat, SeatDto>, ModelsConverter<Seat, SeatDto>>();
            services.AddScoped<IConverter<Venue, VenueDto>, ModelsConverter<Venue, VenueDto>>();
            services.AddScoped<IConverter<VenueDto, VenueModel>, ModelsConverter<VenueDto, VenueModel>>();
            services.AddScoped<IConverter<AreaDto, AreaModel>, ModelsConverter<AreaDto, AreaModel>>();
            services.AddScoped<IConverter<LayoutDto, LayoutModel>, ModelsConverter<LayoutDto, LayoutModel>>();
            services.AddScoped<IConverter<SeatDto, SeatModel>, ModelsConverter<SeatDto, SeatModel>>();
            return services;
        }
    }
}
