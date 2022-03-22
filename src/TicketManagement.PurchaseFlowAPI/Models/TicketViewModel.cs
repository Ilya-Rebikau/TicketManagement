using System.ComponentModel.DataAnnotations;
using TicketManagement.PurchaseFlowAPI.ModelsDTO;

namespace TicketManagement.PurchaseFlowAPI.Models
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

        /// <summary>
        /// Convert ticket dto to ticket view model.
        /// </summary>
        /// <param name="ticket">ticket dto.</param>
        public static implicit operator TicketViewModel(TicketDto ticket)
        {
            return new TicketViewModel
            {
                Id = ticket.Id,
                EventSeatId = ticket.EventSeatId,
                UserId = ticket.UserId,
            };
        }

        /// <summary>
        /// Convert ticket view model to ticket dto.
        /// </summary>
        /// <param name="ticketVm">ticket view model.</param>
        public static implicit operator TicketDto(TicketViewModel ticketVm)
        {
            return new TicketDto
            {
                Id = ticketVm.Id,
                EventSeatId = ticketVm.EventSeatId,
                UserId = ticketVm.UserId,
            };
        }
    }
}
