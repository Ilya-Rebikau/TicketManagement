﻿using TicketManagement.Web.Interfaces;

namespace TicketManagement.Web.ModelsDTO
{
    /// <summary>
    /// Represent area's dto model.
    /// </summary>
    public class AreaDto : IEntityDto
    {
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets layout's id.
        /// </summary>
        public int LayoutId { get; set; }

        /// <summary>
        /// Gets or sets description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets X coordinate in layout.
        /// </summary>
        public int CoordX { get; set; }

        /// <summary>
        /// Gets or sets Y coordinate in layout.
        /// </summary>
        public int CoordY { get; set; }

        /// <summary>
        /// Gets or sets base price for area.
        /// </summary>
        public double BasePrice { get; set; }
    }
}
