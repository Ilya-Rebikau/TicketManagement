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
    /// Service with CRUD operations and validations for ticket.
    /// </summary>
    internal class TicketService : BaseService<Ticket, TicketDto>, IService<TicketDto>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TicketService"/> class.
        /// </summary>
        /// <param name="repository">TicketRepository object.</param>
        /// <param name="converter">Converter object.</param>
        public TicketService(IRepository<Ticket> repository, IConverter<Ticket, TicketDto> converter)
            : base(repository, converter)
        {
        }
    }
}
