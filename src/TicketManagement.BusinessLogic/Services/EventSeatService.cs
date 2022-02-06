using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.BusinessLogic.ModelsDTO;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Models;

namespace TicketManagement.BusinessLogic.Services
{
    /// <summary>
    /// Service with CRUD operations and validations for event seat.
    /// </summary>
    internal class EventSeatService : BaseService<EventSeat, EventSeatDto>, IService<EventSeatDto>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventSeatService"/> class.
        /// </summary>
        /// <param name="repository">EventSeatRepository object.</param>
        /// <param name="converter">Converter object.</param>
        public EventSeatService(IRepository<EventSeat> repository, IConverter<EventSeat, EventSeatDto> converter)
            : base(repository, converter)
        {
        }

        public async override Task<EventSeatDto> CreateAsync(EventSeatDto obj)
        {
            await CheckForUniqueRowAndNumber(obj);
            CheckForPositiveRowAndNumber(obj);
            return await base.CreateAsync(obj);
        }

        public async override Task<EventSeatDto> UpdateAsync(EventSeatDto obj)
        {
            await CheckForUniqueRowAndNumber(obj);
            CheckForPositiveRowAndNumber(obj);
            return await base.UpdateAsync(obj);
        }

        public async override Task<EventSeatDto> DeleteAsync(EventSeatDto obj)
        {
            CheckForTickets(obj);
            return await base.DeleteAsync(obj);
        }

        /// <summary>
        /// Checking that all event seats in event area have unique row and number.
        /// </summary>
        /// <param name="obj">Adding or updating seat.</param>
        /// <exception cref="ArgumentException">Generates exception in case row and number are not unique.</exception>
        private async Task CheckForUniqueRowAndNumber(EventSeatDto obj)
        {
            IEnumerable<EventSeatDto> eventSeats = await Converter.ConvertModelsRangeToDtos(await Repository.GetAllAsync());
            IEnumerable<EventSeatDto> eventSeatsInArea = eventSeats.Where(seat => seat.EventAreaId == obj.EventAreaId && seat.Row == obj.Row && seat.Number == obj.Number && seat.Id != obj.Id);
            if (eventSeatsInArea.Any())
            {
                throw new ArgumentException("One of seats in this area already has such row and number!");
            }
        }

        /// <summary>
        /// Checking that seat has positive row and number.
        /// </summary>
        /// <param name="obj">Adding or updating seat.</param>
        /// <exception cref="ArgumentException">Generates exception in case row or number are not positive.</exception>
        private void CheckForPositiveRowAndNumber(EventSeatDto obj)
        {
            if (obj.Row <= 0 || obj.Number <= 0)
            {
                throw new ArgumentException("Row and number must be positive!");
            }
        }

        private void CheckForTickets(EventSeatDto obj)
        {
            if (obj.State == PlaceStatus.Occupied)
            {
                throw new InvalidOperationException("Someone bought tickets for this event seat already!");
            }
        }
    }
}
