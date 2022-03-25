using AutoMapper;
using TicketManagement.DataAccess.Models;
using TicketManagement.VenueManagerAPI.Models.Areas;
using TicketManagement.VenueManagerAPI.Models.Layouts;
using TicketManagement.VenueManagerAPI.Models.Seats;
using TicketManagement.VenueManagerAPI.Models.Venues;
using TicketManagement.VenueManagerAPI.ModelsDTO;

namespace TicketManagement.VenueManagerAPI.Automapper
{
    /// <summary>
    /// Profile for automapper.
    /// </summary>
    internal class AutoMapperProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AutoMapperProfile"/> class.
        /// </summary>
        public AutoMapperProfile()
        {
            CreateMap<Area, AreaDto>().ReverseMap();
            CreateMap<Layout, LayoutDto>().ReverseMap();
            CreateMap<Seat, SeatDto>().ReverseMap();
            CreateMap<Venue, VenueDto>().ReverseMap();
            CreateMap<VenueDto, VenueModel>().ReverseMap();
            CreateMap<AreaDto, AreaModel>().ReverseMap();
            CreateMap<LayoutDto, LayoutModel>().ReverseMap();
            CreateMap<SeatDto, SeatModel>().ReverseMap();
        }
    }
}
