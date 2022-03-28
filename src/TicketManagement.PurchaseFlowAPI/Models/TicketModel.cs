using System.ComponentModel.DataAnnotations;

namespace TicketManagement.PurchaseFlowAPI.Models
{
    /// <summary>
    /// Ticket model for events.
    /// </summary>
    public class TicketModel
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
