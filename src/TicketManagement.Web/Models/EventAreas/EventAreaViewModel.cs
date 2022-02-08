using System.ComponentModel.DataAnnotations;
using TicketManagement.BusinessLogic.ModelsDTO;

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

        /// <summary>
        /// Convert event area dto to event area view model.
        /// </summary>
        /// <param name="eventArea">Event area dto.</param>
        public static implicit operator EventAreaViewModel(EventAreaDto eventArea)
        {
            return new EventAreaViewModel
            {
                Id = eventArea.Id,
                EventId = eventArea.EventId,
                Description = eventArea.Description,
                CoordX = eventArea.CoordX,
                CoordY = eventArea.CoordY,
                Price = eventArea.Price,
            };
        }

        /// <summary>
        /// Convert event area view model to event area dto.
        /// </summary>
        /// <param name="eventAreaVm">Event area view model.</param>
        public static implicit operator EventAreaDto(EventAreaViewModel eventAreaVm)
        {
            return new EventAreaDto
            {
                Id = eventAreaVm.Id,
                EventId = eventAreaVm.EventId,
                Description = eventAreaVm.Description,
                CoordX = eventAreaVm.CoordX,
                CoordY = eventAreaVm.CoordY,
                Price = eventAreaVm.Price,
            };
        }
    }
}
