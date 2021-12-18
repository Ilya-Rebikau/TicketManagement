using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Models;

namespace TicketManagement.BusinessLogic.Services
{
    /// <summary>
    /// Service with CRUD operations and validations for layout.
    /// </summary>
    internal class LayoutService : BaseService<Layout>, IService<Layout>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutService"/> class.
        /// </summary>
        /// <param name="repository">LayoutRepository object.</param>
        public LayoutService(IRepository<Layout> repository)
            : base(repository)
        {
        }

        public new Layout Create(Layout obj)
        {
            return Repository.Create(obj);
        }

        public new Layout Update(Layout obj)
        {
            return Repository.Update(obj);
        }
    }
}