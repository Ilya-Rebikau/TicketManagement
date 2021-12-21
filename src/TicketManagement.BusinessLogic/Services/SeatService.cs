using System;
using System.Collections.Generic;
using System.Linq;
using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Models;

namespace TicketManagement.BusinessLogic.Services
{
    /// <summary>
    /// Service with CRUD operations and validations for seat.
    /// </summary>
    internal class SeatService : BaseService<Seat>, IService<Seat>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SeatService"/> class.
        /// </summary>
        /// <param name="repository">SeatRepository object.</param>
        public SeatService(IRepository<Seat> repository)
            : base(repository)
        {
        }

        public override Seat Create(Seat obj)
        {
            CheckForPositiveRowAndNumber(obj);
            CheckForUniqueRowAndNumber(obj);
            return Repository.Create(obj);
        }

        public override Seat Update(Seat obj)
        {
            CheckForPositiveRowAndNumber(obj);
            CheckForUniqueRowAndNumber(obj);
            return Repository.Update(obj);
        }

        /// <summary>
        /// Checking that all seats in area have unique row and number.
        /// </summary>
        /// <param name="obj">Adding or updating seat.</param>
        /// <exception cref="ArgumentException">Generates exception in case row and number are not unique.</exception>
        private void CheckForUniqueRowAndNumber(Seat obj)
        {
            IEnumerable<Seat> seats = Repository.GetAll();
            IEnumerable<Seat> seatsInArea = seats.Where(seat => seat.AreaId == obj.AreaId && seat.Row == obj.Row && seat.Number == obj.Number && seat.Id != obj.Id);
            if (seatsInArea.Any())
            {
                throw new ArgumentException("One of seats in this area already has such row and number!");
            }
        }

        private void CheckForPositiveRowAndNumber(Seat obj)
        {
            if (obj.Row <= 0 || obj.Number <= 0)
            {
                throw new ArgumentException("Row and number must be positive!");
            }
        }
    }
}
