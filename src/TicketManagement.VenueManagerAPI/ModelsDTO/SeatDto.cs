using TicketManagement.VenueManagerAPI.Interfaces;

namespace TicketManagement.VenueManagerAPI.ModelsDTO
{
    /// <summary>
    /// Represent seat's dto model.
    /// </summary>
    public class SeatDto : IEntityDto
    {
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
