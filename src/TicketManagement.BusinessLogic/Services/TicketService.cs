using System.Linq;
using System.Threading.Tasks;
using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.BusinessLogic.ModelsDTO;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Models;

namespace TicketManagement.BusinessLogic.Services
{
    /// <summary>
    /// Service with CRUD operations and validations for ticket.
    /// </summary>
    internal class TicketService : BaseService<Ticket, TicketDto>, IService<TicketDto>
    {
        private readonly IRepository<EventSeat> _eventSeatRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="TicketService"/> class.
        /// </summary>
        /// <param name="repository">TicketRepository object.</param>
        /// <param name="converter">Converter object.</param>
        /// <param name="eventSeatRepository">EventSeatRepository object.</param>
        public TicketService(IRepository<Ticket> repository, IConverter<Ticket, TicketDto> converter, IRepository<EventSeat> eventSeatRepository)
            : base(repository, converter)
        {
            _eventSeatRepository = eventSeatRepository;
        }

        public async override Task<TicketDto> DeleteAsync(TicketDto obj)
        {
            await DoPlaceFreeAsync(obj);
            return await base.DeleteAsync(obj);
        }

        private async Task DoPlaceFreeAsync(TicketDto obj)
        {
            var eventSeats = await _eventSeatRepository.GetAllAsync();
            var eventSeat = eventSeats.Where(s => s.Id == obj.EventSeatId).FirstOrDefault();
            eventSeat.State = (int)PlaceStatus.Free;
            await _eventSeatRepository.UpdateAsync(eventSeat);
        }
    }
}
