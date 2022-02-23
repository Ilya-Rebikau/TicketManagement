using System;
using System.ComponentModel.DataAnnotations;
using ThirdPartyEventEditor.Interfaces;

namespace ThirdPartyEventEditor.Models
{
    /// <summary>
    /// Represents third party event model.
    /// </summary>
    public class ThirdPartyEvent : IEntity
    {
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets name.
        /// </summary>
        [Required(ErrorMessage = "This field is required!")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets date of start event.
        /// </summary>
        [Required(ErrorMessage = "This field is required!")]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets date of end event.
        /// </summary>
        [Required(ErrorMessage = "This field is required!")]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Gets or sets description.
        /// </summary>
        [Required(ErrorMessage = "This field is required!")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets poster image.
        /// </summary>
        [Required(ErrorMessage = "This field is required!")]
        public string PosterImage { get; set; }

        /// <summary>
        /// Gets or sets id of layout for event.
        /// </summary>
        [Required(ErrorMessage = "This field is required!")]
        [Range(1, int.MaxValue, ErrorMessage = "Layout id can be only positive number!")]
        public int LayoutId { get; set; }
    }
}