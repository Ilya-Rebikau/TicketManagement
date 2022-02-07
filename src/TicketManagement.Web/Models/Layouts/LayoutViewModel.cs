using System.ComponentModel.DataAnnotations;
using TicketManagement.BusinessLogic.ModelsDTO;

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
        public static implicit operator LayoutViewModel(LayoutDto layout)
        {
            return new LayoutViewModel
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
        public static implicit operator LayoutDto(LayoutViewModel layoutVm)
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
