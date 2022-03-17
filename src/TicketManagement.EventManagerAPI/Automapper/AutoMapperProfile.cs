using AutoMapper;
using TicketManagement.DataAccess.Models;
using TicketManagement.EventManagerAPI.ModelsDTO;

namespace TicketManagement.EventManagerAPI.Automapper
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
            CreateMap<ThirdPartyEvent, EventDto>().ConvertUsing<ConvertThirdPartyEventToDto>();
            CreateMap<EventDto, ThirdPartyEvent>().ConvertUsing<ConvertEventDtoToThirdParty>();
            CreateMap<EventArea, EventAreaDto>().ReverseMap();
            CreateMap<EventSeat, EventSeatDto>().ReverseMap();
            CreateMap<Event, EventDto>().ReverseMap();
        }
    }
}
