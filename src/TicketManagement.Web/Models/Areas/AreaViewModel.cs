using System.ComponentModel.DataAnnotations;

namespace TicketManagement.Web.Models.Areas
{
    /// <summary>
    /// Area view model.
    /// </summary>
    public class AreaViewModel
    {
        /// <summary>
        /// Gets or sets id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets layout's id.
        /// </summary>
        [Required(ErrorMessage = "FieldRequired")]
        [Display(Name = "LayoutId")]
        [Range(1, int.MaxValue)]
        public int LayoutId { get; set; }

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
        [Range(0, int.MaxValue)]
        public int CoordX { get; set; }

        /// <summary>
        /// Gets or sets Y coordinate in layout.
        /// </summary>
        [Required(ErrorMessage = "FieldRequired")]
        [Display(Name = "CoordYInLayout")]
        [Range(0, int.MaxValue)]
        public int CoordY { get; set; }

        /// <summary>
        /// Gets or sets base price for area.
        /// </summary>
        [Required(ErrorMessage = "FieldRequired")]
        [Display(Name = "Price")]
        [Range(0, double.MaxValue)]
        public double BasePrice { get; set; }
    }
}
