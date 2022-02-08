using TicketManagement.DataAccess.Interfaces;

namespace TicketManagement.DataAccess.Models
{
    /// <summary>
    /// Represent layout's model.
    /// </summary>
    public class Layout : IEntity
    {
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets venue's id.
        /// </summary>
        public int VenueId { get; set; }

        /// <summary>
        /// Gets or sets description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets name of layout in venue.
        /// </summary>
        public string Name { get; set; }
    }
}
