using System.ComponentModel.DataAnnotations;
using TicketManagement.BusinessLogic.ModelsDTO;

namespace TicketManagement.Web.Models.Events
{
    /// <summary>
    /// Ticket view model for events.
    /// </summary>
    public class TicketViewModel
    {
        /// <summary>
        /// Gets or sets ticket.
        /// </summary>
        public TicketDto Ticket { get; set; }

        /// <summary>
        /// Gets or sets price for ticket.
        /// </summary>
        public double Price { get; set; }
    }
}
