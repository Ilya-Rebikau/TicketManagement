using System.ComponentModel.DataAnnotations;
using TicketManagement.BusinessLogic.Interfaces;

namespace TicketManagement.BusinessLogic.ModelsDTO
{
    /// <summary>
    /// Represent seat's dto model during event.
    /// </summary>
    public class EventSeatDto : IEntityDto
    {
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets id of event area.
        /// </summary>
        [Required(ErrorMessage = "FieldRequired")]
        [Display(Name = "EventAreaId")]
        public int EventAreaId { get; set; }

        /// <summary>
        /// Gets or sets row number in area.
        /// </summary>
        [Required(ErrorMessage = "FieldRequired")]
        [Display(Name = "Row")]
        public int Row { get; set; }

        /// <summary>
        /// Gets or sets number in row.
        /// </summary>
        [Required(ErrorMessage = "FieldRequired")]
        [Display(Name = "Number")]
        public int Number { get; set; }

        /// <summary>
        /// Gets or sets state.
        /// </summary>
        [Required(ErrorMessage = "FieldRequired")]
        [Display(Name = "State")]
        public PlaceStatus State { get; set; }
    }
}
