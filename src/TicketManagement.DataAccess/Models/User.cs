using TicketManagement.DataAccess.Interfaces;

namespace TicketManagement.DataAccess.Models
{
    /// <summary>
    /// Represent user's model.
    /// </summary>
    public class User : IEntity
    {
        public int Id { get; set; }

        // TODO: TimeZone

        /// <summary>
        /// Gets or sets first name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets surname.
        /// </summary>
        public string Surname { get; set; }

        /// <summary>
        /// Gets or sets email.
        /// </summary>
        public string Email { get; set; }

        // TODO: purchase history

        /// <summary>
        /// Gets or sets balance.
        /// </summary>
        public double Balance { get; set; }
    }
}
