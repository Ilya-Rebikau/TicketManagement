using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Models;
using TicketManagement.VenueManagerAPI.Infrastructure;
using TicketManagement.VenueManagerAPI.Interfaces;
using TicketManagement.VenueManagerAPI.ModelsDTO;

namespace TicketManagement.VenueManagerAPI.Services
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
        /// <param name="confgiratuon">IConfiguration object.</param>
        public SeatService(IRepository<Seat> repository, IConverter<Seat, SeatDto> converter, IConfiguration confgiratuon)
            : base(repository, converter, confgiratuon)
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
        /// Checking that seat has positive row and number.
        /// </summary>
        /// <param name="obj">Adding or updating seat.</param>
        /// <exception cref="ValidationException">Generates exception in case row or number are not positive.</exception>
        private static void CheckForPositiveRowAndNumber(SeatDto obj)
        {
            if (obj.Row <= 0 || obj.Number <= 0)
            {
                throw new ValidationException("Row and number must be positive!");
            }
        }

        /// <summary>
        /// Checking that all seats in area have unique row and number.
        /// </summary>
        /// <param name="obj">Adding or updating seat.</param>
        /// <exception cref="ValidationException">Generates exception in case row and number are not unique.</exception>
        private async Task CheckForUniqueRowAndNumber(SeatDto obj)
        {
            var seats = await Repository.GetAllAsync();
            var seatsInArea = seats.Where(seat => seat.AreaId == obj.AreaId && seat.Row == obj.Row && seat.Number == obj.Number && seat.Id != obj.Id);
            if (seatsInArea.Any())
            {
                throw new ValidationException("One of seats in this area already has such row and number!");
            }
        }
    }
}
