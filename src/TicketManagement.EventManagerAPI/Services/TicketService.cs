using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Models;
using TicketManagement.EventManagerAPI.Interfaces;
using TicketManagement.EventManagerAPI.ModelsDTO;

namespace TicketManagement.EventManagerAPI.Services
{
    /// <summary>
    /// Service with CRUD operations and validations for ticket.
    /// </summary>
    internal class TicketService : BaseService<Ticket, TicketDto>, IService<TicketDto>
    {
        /// <summary>
        /// EventSeatRepository object.
        /// </summary>
        private readonly IRepository<EventSeat> _eventSeatRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="TicketService"/> class.
        /// </summary>
        /// <param name="repository">TicketRepository object.</param>
        /// <param name="converter">Converter object.</param>
        /// <param name="eventSeatRepository">EventSeatRepository object.</param>
        /// <param name="configuration">IConfiguration object.</param>
        public TicketService(IRepository<Ticket> repository,
            IConverter<Ticket, TicketDto> converter,
            IRepository<EventSeat> eventSeatRepository,
            IConfiguration configuration)
            : base(repository, converter, configuration)
        {
            _eventSeatRepository = eventSeatRepository;
        }

        public async override Task<TicketDto> DeleteAsync(TicketDto obj)
        {
            await DoPlaceFreeAsync(obj);
            return await base.DeleteAsync(obj);
        }

        /// <summary>
        /// Doing event seat state free.
        /// </summary>
        /// <param name="obj">Ticket with event seat.</param>
        /// <returns>Task.</returns>
        private async Task DoPlaceFreeAsync(TicketDto obj)
        {
            var eventSeats = _eventSeatRepository.GetAll();
            var eventSeat = eventSeats.Where(s => s.Id == obj.EventSeatId).First();
            eventSeat.State = (int)PlaceStatus.Free;
            await _eventSeatRepository.UpdateAsync(eventSeat);
        }
    }
}
