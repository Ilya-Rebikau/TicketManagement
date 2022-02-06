using System;
using System.ComponentModel.DataAnnotations;
using TicketManagement.BusinessLogic.Interfaces;

namespace TicketManagement.BusinessLogic.ModelsDTO
{
    /// <summary>
    /// Represent event's dto model.
    /// </summary>
    public class EventDto : IEntityDto
    {
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets name.
        /// </summary>
        [Required(ErrorMessage = "FieldRequired")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets description.
        /// </summary>
        [Required(ErrorMessage = "FieldRequired")]
        [Display(Name = "Description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets layout's id.
        /// </summary>
        [Required(ErrorMessage = "FieldRequired")]
        [Display(Name = "LayoutId")]
        public int LayoutId { get; set; }

        /// <summary>
        /// Gets or sets time when event starts.
        /// </summary>
        [Required(ErrorMessage = "FieldRequired")]
        [Display(Name = "TimeStart")]
        public DateTime TimeStart { get; set; }

        /// <summary>
        /// Gets or sets time when event ends.
        /// </summary>
        [Required(ErrorMessage = "FieldRequired")]
        [Display(Name = "TimeEnd")]
        public DateTime TimeEnd { get; set; }

        /// <summary>
        /// Gets or sets image URL.
        /// </summary>
        [Url(ErrorMessage = "WrongUrl")]
        [Display(Name = "ImageUrl")]
        public string ImageUrl { get; set; }
    }
}
