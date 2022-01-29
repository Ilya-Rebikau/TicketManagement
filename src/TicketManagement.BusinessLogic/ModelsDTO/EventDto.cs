﻿using System;
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
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets layout's id.
        /// </summary>
        public int LayoutId { get; set; }

        /// <summary>
        /// Gets or sets time when event starts.
        /// </summary>
        public DateTime TimeStart { get; set; }

        /// <summary>
        /// Gets or sets time when event ends.
        /// </summary>
        public DateTime TimeEnd { get; set; }

        /// <summary>
        /// Gets or sets image.
        /// </summary>
        public string Image { get; set; }
    }
}
