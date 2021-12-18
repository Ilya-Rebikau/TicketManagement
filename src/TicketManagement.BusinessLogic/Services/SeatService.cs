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

        public new Seat Create(Seat obj)
        {
            return Repository.Create(obj);
        }

        public new Seat Update(Seat obj)
        {
            return Repository.Update(obj);
        }
    }
}
