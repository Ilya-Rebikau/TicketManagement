using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Models;

namespace TicketManagement.BusinessLogic.Services
{
    /// <summary>
    /// Service with CRUD operations and validations for event.
    /// </summary>
    internal class EventService : BaseService<Event>, IService<Event>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventService"/> class.
        /// </summary>
        /// <param name="repository">EventRepository object.</param>
        public EventService(IRepository<Event> repository)
            : base(repository)
        {
        }

        public new Event Create(Event obj)
        {
            return Repository.Create(obj);
        }

        public new Event Update(Event obj)
        {
            return Repository.Update(obj);
        }
    }
}
