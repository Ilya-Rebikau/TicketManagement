using TicketManagement.EventManagerAPI.Interfaces;

namespace TicketManagement.EventManagerAPI.ModelsDTO
{
    /// <summary>
    /// Represent area's model dto during event.
    /// </summary>
    public class EventAreaDto : IEntityDto
    {
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets event's id.
        /// </summary>
        public int EventId { get; set; }

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
        /// Gets or sets price.
        /// </summary>
        public double Price { get; set; }
    }
}
