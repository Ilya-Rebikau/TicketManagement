using TicketManagement.EventManagerAPI.ModelsDTO;

namespace TicketManagement.EventManagerAPI.Models.EventAreas
{
    /// <summary>
    /// Event area model.
    /// </summary>
    public class EventAreaModel
    {
        /// <summary>
        /// Gets or sets id.
        /// </summary>
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
