using System.ComponentModel.DataAnnotations;
using TicketManagement.VenueManagerAPI.ModelsDTO;

namespace TicketManagement.VenueManagerAPI.Models.Layouts
{
    /// <summary>
    /// Layout model.
    /// </summary>
    public class LayoutModel
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

        /// <summary>
        /// Convert layout dto to layout view model.
        /// </summary>
        /// <param name="layout">Layout dto.</param>
        public static implicit operator LayoutModel(LayoutDto layout)
        {
            return new LayoutModel
            {
                Id = layout.Id,
                Name = layout.Name,
                Description = layout.Description,
                VenueId = layout.VenueId,
            };
        }

        /// <summary>
        /// Convert layout view model to layout dto.
        /// </summary>
        /// <param name="layoutVm">Layout view model.</param>
        public static implicit operator LayoutDto(LayoutModel layoutVm)
        {
            return new LayoutDto
            {
                Id = layoutVm.Id,
                Name = layoutVm.Name,
                Description = layoutVm.Description,
                VenueId = layoutVm.VenueId,
            };
        }
    }
}
