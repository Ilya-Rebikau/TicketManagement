using System;
using System.Threading.Tasks;
using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Models;

namespace TicketManagement.BusinessLogic.Services
{
    /// <summary>
    /// Service with CRUD operations and validations for event area.
    /// </summary>
    internal class EventAreaService : BaseService<EventArea>, IService<EventArea>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventAreaService"/> class.
        /// </summary>
        /// <param name="repository">EventAreaRepository object.</param>
        public EventAreaService(IRepository<EventArea> repository)
            : base(repository)
        {
        }

        public async override Task<EventArea> CreateAsync(EventArea obj)
        {
            CheckForPositiveCoords(obj);
            CheckForPositivePrice(obj);
            return await Repository.CreateAsync(obj);
        }

        public async override Task<EventArea> UpdateAsync(EventArea obj)
        {
            CheckForPositiveCoords(obj);
            CheckForPositivePrice(obj);
            return await Repository.UpdateAsync(obj);
        }

        /// <summary>
        /// Checking that event area has positive coords.
        /// </summary>
        /// <param name="obj">Adding or updating event area.</param>
        /// <exception cref="ArgumentException">Generates exception in case coords aren't positive.</exception>
        private void CheckForPositiveCoords(EventArea obj)
        {
            if (obj.CoordX <= 0 || obj.CoordY <= 0)
            {
                throw new ArgumentException("Coords can be only positive numbers!");
            }
        }

        /// <summary>
        /// Checking that event area has positive price.
        /// </summary>
        /// <param name="obj">Adding or updating event area.</param>
        private void CheckForPositivePrice(EventArea obj)
        {
            if (obj.Price <= 0)
            {
                throw new ArgumentException("Price can be only positive number!");
            }
        }
    }
}
