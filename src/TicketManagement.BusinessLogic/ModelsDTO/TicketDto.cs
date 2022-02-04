using System.ComponentModel.DataAnnotations;
using TicketManagement.BusinessLogic.Interfaces;

namespace TicketManagement.BusinessLogic.ModelsDTO
{
    /// <summary>
    /// Represent ticket's dto model.
    /// </summary>
    public class TicketDto : IEntityDto
    {
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets event seat's id.
        /// </summary>
        [Required(ErrorMessage = "FieldRequired")]
        [Display(Name = "SeatRowAndNumber")]
        public int EventSeatId { get; set; }

        /// <summary>
        /// Gets or sets user's id.
        /// </summary>
        [Required(ErrorMessage = "FieldRequired")]
        [Display(Name = "User")]
        public int UserId { get; set; }
    }
}
