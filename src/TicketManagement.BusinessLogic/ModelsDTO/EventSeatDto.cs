using TicketManagement.BusinessLogic.Interfaces;

namespace TicketManagement.BusinessLogic.ModelsDTO
{
    /// <summary>
    /// Represent seat's dto model during event.
    /// </summary>
    public class EventSeatDto : IEntityDto
    {
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
        public PlaceStatus State { get; set; }
    }
}
