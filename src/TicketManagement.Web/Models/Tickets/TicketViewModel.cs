using System.ComponentModel.DataAnnotations;
using TicketManagement.Web.ModelsDTO;

namespace TicketManagement.Web.Models.Tickets
{
    /// <summary>
    /// Ticket view model for events.
    /// </summary>
    public class TicketViewModel
    {
        /// <summary>
        /// Gets or sets id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets event seat's id.
        /// </summary>
        [Required(ErrorMessage = "FieldRequired")]
        [Display(Name = "EventSeatId")]
        public int EventSeatId { get; set; }

        /// <summary>
        /// Gets or sets user's id.
        /// </summary>
        [Required(ErrorMessage = "FieldRequired")]
        [Display(Name = "UserId")]
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets price for ticket.
        /// </summary>
        public double Price { get; set; }
    }
}
