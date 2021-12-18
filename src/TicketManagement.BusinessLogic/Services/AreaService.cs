using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Models;

namespace TicketManagement.BusinessLogic.Services
{
    /// <summary>
    /// Service with CRUD operations and validations for area.
    /// </summary>
    internal class AreaService : BaseService<Area>, IService<Area>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AreaService"/> class.
        /// </summary>
        /// <param name="repository">AreaRepository object.</param>
        public AreaService(IRepository<Area> repository)
            : base(repository)
        {
        }

        public new Area Create(Area obj)
        {
            return Repository.Create(obj);
        }

        public new Area Update(Area obj)
        {
            return Repository.Update(obj);
        }
    }
}
