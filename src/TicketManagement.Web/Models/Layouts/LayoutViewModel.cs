using System.ComponentModel.DataAnnotations;
using TicketManagement.Web.ModelsDTO;

namespace TicketManagement.Web.Models.Layouts
{
    /// <summary>
    /// Layout view model.
    /// </summary>
    public class LayoutViewModel
    {
        /// <summary>
        /// Gets or sets id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets venue's id.
        /// </summary>
        [Required(ErrorMessage = "FieldRequired")]
        [Display(Name = "VenueId")]
        [Range(0, int.MaxValue)]
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
