using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketManagement.PurchaseFlowAPI.Interfaces;
using TicketManagement.PurchaseFlowAPI.Models;
using TicketManagement.PurchaseFlowAPI.ModelsDTO;

namespace TicketManagement.PurchaseFlowAPI.Services
{
    /// <summary>
    /// Service for purchase flow.
    /// </summary>
    public class PurchaseService : IPurchaseService
    {
        /// <summary>
        /// TicketService object.
        /// </summary>
        private readonly IService<TicketDto> _ticketService;

        /// <summary>
        /// EventService object.
        /// </summary>
        private readonly IService<EventDto> _eventService;

        /// <summary>
        /// EventAreaService object.
        /// </summary>
        private readonly IService<EventAreaDto> _eventAreaService;

        /// <summary>
        /// EventSeatService object.
        /// </summary>
        private readonly IService<EventSeatDto> _eventSeatService;

        /// <summary>
        /// IUsersClient object.
        /// </summary>
        private readonly IUsersClient _usersClient;

        /// <summary>
        /// Converter object.
        /// </summary>
        private readonly IConverter<TicketDto, TicketModel> _converter;

        /// <summary>
        /// Initializes a new instance of the <see cref="PurchaseService"/> class.
        /// </summary>
        /// <param name="ticketService">TicketService object.</param>
        /// <param name="eventService">EventService object.</param>
        /// <param name="eventAreaService">EventAreaService object.</param>
        /// <param name="eventSeatService">EventSeatService object.</param>
        /// <param name="usersClient">IUsersClient object.</param>
        /// <param name="converter">Converter object.</param>
        public PurchaseService(IService<TicketDto> ticketService,
            IService<EventDto> eventService,
            IService<EventAreaDto> eventAreaService,
            IService<EventSeatDto> eventSeatService,
            IUsersClient usersClient,
            IConverter<TicketDto, TicketModel> converter)
        {
            _ticketService = ticketService;
            _eventService = eventService;
            _eventAreaService = eventAreaService;
            _eventSeatService = eventSeatService;
            _usersClient = usersClient;
            _converter = converter;
        }

        public async Task<AccountModel> GetAccountViewModelForPersonalAccount(string token)
        {
            var tickets = await _ticketService.GetAllAsync();
            var userId = await _usersClient.GetUserId(token);
            var usersTickets = tickets.Where(t => t.UserId == userId).ToList();
            var eventSeats = await _eventSeatService.GetAllAsync();
            var eventAreas = await _eventAreaService.GetAllAsync();
            var ticketsVm = new List<AccountTicketModel>();
            foreach (var ticket in usersTickets)
            {
                var ticketEventSeat = eventSeats.FirstOrDefault(s => s.Id == ticket.EventSeatId);
                var eventArea = eventAreas.FirstOrDefault(a => a.Id == ticketEventSeat.EventAreaId);
                var @event = await _eventService.GetByIdAsync(eventArea.EventId);
                @event = await _usersClient.ConvertTimeFromUtcToUsers(token, @event);
                var ticketVm = new AccountTicketModel
                {
                    Price = eventArea.Price,
                    Event = @event,
                };
                ticketsVm.Add(ticketVm);
            }

            var accountVm = new AccountModel
            {
                JwtToken = token,
                Tickets = ticketsVm,
            };
            return accountVm;
        }

        public async Task<TicketModel> GetTicketViewModelForBuyAsync(int eventSeatId, double price, string token)
        {
            var userId = await _usersClient.GetUserId(token);
            var ticket = new TicketDto
            {
                UserId = userId,
                EventSeatId = eventSeatId,
            };

            var ticketVm = await _converter.ConvertSourceToDestination(ticket);
            ticketVm.Price = price;
            return ticketVm;
        }

        public async Task<bool> UpdateEventSeatStateAfterBuyingTicket(string token, TicketModel ticketVm)
        {
            if (await _usersClient.ChangeBalanceForUser(token, ticketVm.Price))
            {
                var seat = await _eventSeatService.GetByIdAsync(ticketVm.EventSeatId);
                seat.State = PlaceStatus.Occupied;
                await _eventSeatService.UpdateAsync(seat);
                await _ticketService.CreateAsync(await _converter.ConvertDestinationToSource(ticketVm));
                return true;
            }

            return false;
        }
    }
}
