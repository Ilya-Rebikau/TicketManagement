using System.ComponentModel.DataAnnotations;
using TicketManagement.BusinessLogic.ModelsDTO;

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
        public int CoordX { get; set; }

        /// <summary>
        /// Gets or sets Y coordinate in layout.
        /// </summary>
        [Required(ErrorMessage = "FieldRequired")]
        [Display(Name = "CoordYInLayout")]
        public int CoordY { get; set; }

        /// <summary>
        /// Gets or sets base price for area.
        /// </summary>
        [Required(ErrorMessage = "FieldRequired")]
        [Display(Name = "Price")]
        public double BasePrice { get; set; }

        /// <summary>
        /// Convert area dto to area view model.
        /// </summary>
        /// <param name="area">Area dto.</param>
        public static implicit operator AreaViewModel(AreaDto area)
        {
            return new AreaViewModel
            {
                Id = area.Id,
                LayoutId = area.LayoutId,
                Description = area.Description,
                CoordX = area.CoordX,
                CoordY = area.CoordY,
                BasePrice = area.BasePrice,
            };
        }

        /// <summary>
        /// Convert area view model to area dto.
        /// </summary>
        /// <param name="areaVm">Area view model.</param>
        public static implicit operator AreaDto(AreaViewModel areaVm)
        {
            return new AreaDto
            {
                Id = areaVm.Id,
                LayoutId = areaVm.LayoutId,
                Description = areaVm.Description,
                CoordX = areaVm.CoordX,
                CoordY = areaVm.CoordY,
                BasePrice = areaVm.BasePrice,
            };
        }
    }
}
