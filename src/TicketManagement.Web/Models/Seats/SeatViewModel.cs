using System.ComponentModel.DataAnnotations;

namespace TicketManagement.Web.Models.Seats
{
    /// <summary>
    /// Seat view model.
    /// </summary>
    public class SeatViewModel
    {
        /// <summary>
        /// Gets or sets id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets id of area.
        /// </summary>
        [Required(ErrorMessage = "FieldRequired")]
        [Display(Name = "AreaId")]
        [Range(1, int.MaxValue)]
        public int AreaId { get; set; }

        /// <summary>
        /// Gets or sets row number in area.
        /// </summary>
        [Required(ErrorMessage = "FieldRequired")]
        [Display(Name = "Row")]
        [Range(0, int.MaxValue)]
        public int Row { get; set; }

        /// <summary>
        /// Gets or sets number in row.
        /// </summary>
        [Required(ErrorMessage = "FieldRequired")]
        [Display(Name = "Number")]
        [Range(0, int.MaxValue)]
        public int Number { get; set; }
    }
}
