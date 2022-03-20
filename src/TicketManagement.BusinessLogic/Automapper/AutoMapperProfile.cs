using AutoMapper;
using TicketManagement.BusinessLogic.ModelsDTO;
using TicketManagement.DataAccess.Models;

namespace TicketManagement.BusinessLogic.Automapper
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
        }
    }
}
