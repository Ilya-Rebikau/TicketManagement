namespace TicketManagement.DataAccess.Models
{
    /// <summary>
    /// Represent layout's model.
    /// </summary>
    public class Layout
    {
        /// <summary>
        /// Gets or sets id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets venue's id.
        /// </summary>
        public int VenueId { get; set; }

        /// <summary>
        /// Gets or sets description.
        /// </summary>
        public string Description { get; set; }
    }
}
