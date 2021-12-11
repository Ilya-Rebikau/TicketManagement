namespace TicketManagement.DataAccess.Models
{
    /// <summary>
    /// Represent seat's model during event.
    /// </summary>
    public class EventSeat
    {
        /// <summary>
        /// Gets or sets id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets id of event area.
        /// </summary>
        public int EventAreaId { get; set; }

        /// <summary>
        /// Gets or sets row number in area.
        /// </summary>
        public int Row { get; set; }

        /// <summary>
        /// Gets or sets number in row.
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// Gets or sets state.
        /// </summary>
        public bool State { get; set; }
    }
}
