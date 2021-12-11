namespace TicketManagement.DataAccess.Models
{
    /// <summary>
    /// Represent venue's model.
    /// </summary>
    public class Venue
    {
        /// <summary>
        /// Gets or sets id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets ot sets address.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets phone.
        /// </summary>
        public string Phone { get; set; }
    }
}
