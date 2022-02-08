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
    /// Service with CRUD operations and validations for seat.
    /// </summary>
    internal class SeatService : BaseService<Seat, SeatDto>, IService<SeatDto>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SeatService"/> class.
        /// </summary>
        /// <param name="repository">SeatRepository object.</param>
        /// <param name="converter">Converter object.</param>
        public SeatService(IRepository<Seat> repository, IConverter<Seat, SeatDto> converter)
            : base(repository, converter)
        {
        }

        public async override Task<SeatDto> CreateAsync(SeatDto obj)
        {
            CheckForPositiveRowAndNumber(obj);
            await CheckForUniqueRowAndNumber(obj);
            return await base.CreateAsync(obj);
        }

        public async override Task<SeatDto> UpdateAsync(SeatDto obj)
        {
            CheckForPositiveRowAndNumber(obj);
            await CheckForUniqueRowAndNumber(obj);
            return await base.UpdateAsync(obj);
        }

        /// <summary>
        /// Checking that all seats in area have unique row and number.
        /// </summary>
        /// <param name="obj">Adding or updating seat.</param>
        /// <exception cref="ArgumentException">Generates exception in case row and number are not unique.</exception>
        private async Task CheckForUniqueRowAndNumber(SeatDto obj)
        {
            IEnumerable<SeatDto> seats = await Converter.ConvertModelsRangeToDtos(await Repository.GetAllAsync());
            IEnumerable<SeatDto> seatsInArea = seats.Where(seat => seat.AreaId == obj.AreaId && seat.Row == obj.Row && seat.Number == obj.Number && seat.Id != obj.Id);
            if (seatsInArea.Any())
            {
                throw new ArgumentException("One of seats in this area already has such row and number!");
            }
        }

        /// <summary>
        /// Checking that seat has positive row and number.
        /// </summary>
        /// <param name="obj">Adding or updating seat.</param>
        /// <exception cref="ArgumentException">Generates exception in case row or number are not positive.</exception>
        private void CheckForPositiveRowAndNumber(SeatDto obj)
        {
            if (obj.Row <= 0 || obj.Number <= 0)
            {
                throw new ArgumentException("Row and number must be positive!");
            }
        }
    }
}
