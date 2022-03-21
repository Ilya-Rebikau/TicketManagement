using TicketManagement.Web.Interfaces;

namespace TicketManagement.Web.ModelsDTO
{
    /// <summary>
    /// Represent ticket's dto model.
    /// </summary>
    public class TicketDto : IEntityDto
    {
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets event seat's id.
        /// </summary>
        public int EventSeatId { get; set; }

        /// <summary>
        /// Gets or sets user's id.
        /// </summary>
        public string UserId { get; set; }
    }
}
