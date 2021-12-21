namespace TicketManagement.DataAccess.Models
{
    /// <summary>
    /// Represent seat's model.
    /// </summary>
    public class Seat
    {
        /// <summary>
        /// Gets or sets id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets area's id.
        /// </summary>
        public int AreaId { get; set; }

        /// <summary>
        /// Gets or sets row number in area.
        /// </summary>
        public int Row { get; set; }

        /// <summary>
        /// Gets or sets number in row.
        /// </summary>
        public int Number { get; set; }
    }
}
