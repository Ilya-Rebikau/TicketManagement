using System;
using TicketManagement.DataAccess.Interfaces;

namespace TicketManagement.DataAccess.Models
{
    /// <summary>
    /// Represent event's model.
    /// </summary>
    public class Event : IEntity
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
