using System.ComponentModel.DataAnnotations;
using TicketManagement.BusinessLogic.Interfaces;

namespace TicketManagement.BusinessLogic.ModelsDTO
{
    /// <summary>
    /// Represent seat's dto model.
    /// </summary>
    public class SeatDto : IEntityDto
    {
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets area's id.
        /// </summary>
        [Required(ErrorMessage = "FieldRequired")]
        [Display(Name = "AreaXAndY")]
        public int AreaId { get; set; }

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
    }
}
