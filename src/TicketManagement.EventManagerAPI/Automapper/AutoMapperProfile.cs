using AutoMapper;
using TicketManagement.DataAccess.Models;
using TicketManagement.EventManagerAPI.Models.EventAreas;
using TicketManagement.EventManagerAPI.Models.Events;
using TicketManagement.EventManagerAPI.Models.EventSeats;
using TicketManagement.EventManagerAPI.Models.Tickets;
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
            CreateMap<Ticket, TicketDto>().ReverseMap();
            CreateMap<Event, EventDto>().ReverseMap();
            CreateMap<EventAreaDto, EventAreaModel>().ReverseMap();
            CreateMap<EventDto, EventModel>().ReverseMap();
            CreateMap<EventSeatDto, EventSeatModel>().ReverseMap();
            CreateMap<TicketDto, TicketModel>().ReverseMap();
        }
    }
}
