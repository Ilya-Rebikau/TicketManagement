using System.ComponentModel.DataAnnotations;
using TicketManagement.Web.ModelsDTO;

namespace TicketManagement.Web.Models.EventAreas
{
    /// <summary>
    /// Event area view model.
    /// </summary>
    public class EventAreaViewModel
    {
        /// <summary>
        /// Gets or sets id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets event's id.
        /// </summary>
        [Required(ErrorMessage = "FieldRequired")]
        [Display(Name = "EventId")]
        public int EventId { get; set; }

        /// <summary>
        /// Gets or sets description.
        /// </summary>
        [Required(ErrorMessage = "FieldRequired")]
        [Display(Name = "Description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets X coordinate in layout.
        /// </summary>
        [Required(ErrorMessage = "FieldRequired")]
        [Display(Name = "CoordXInLayout")]
        public int CoordX { get; set; }

        /// <summary>
        /// Gets or sets Y coordinate in layout.
        /// </summary>
        [Required(ErrorMessage = "FieldRequired")]
        [Display(Name = "CoordYInLayout")]
        public int CoordY { get; set; }

        /// <summary>
        /// Gets or sets price.
        /// </summary>
        [Required(ErrorMessage = "FieldRequired")]
        [Display(Name = "Price")]
        public double Price { get; set; }
    }
}
