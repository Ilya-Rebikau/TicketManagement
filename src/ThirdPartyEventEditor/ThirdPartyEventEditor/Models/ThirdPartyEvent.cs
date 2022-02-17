using System;
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
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets date of start event.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets date of end event.
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Gets or sets description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets poster image.
        /// </summary>
        public string PosterImage { get; set; }
    }
}