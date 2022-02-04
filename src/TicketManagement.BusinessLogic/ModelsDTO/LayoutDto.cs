using System.ComponentModel.DataAnnotations;
using TicketManagement.BusinessLogic.Interfaces;

namespace TicketManagement.BusinessLogic.ModelsDTO
{
    /// <summary>
    /// Represent layout's dto model.
    /// </summary>
    public class LayoutDto : IEntityDto
    {
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets venue's id.
        /// </summary>
        [Required(ErrorMessage = "FieldRequired")]
        [Display(Name = "VenueName")]
        public int VenueId { get; set; }

        /// <summary>
        /// Gets or sets description.
        /// </summary>
        [Required(ErrorMessage = "FieldRequired")]
        [Display(Name = "Description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets name of layout in venue.
        /// </summary>
        [Required(ErrorMessage = "FieldRequired")]
        [Display(Name = "Name")]
        public string Name { get; set; }
    }
}
