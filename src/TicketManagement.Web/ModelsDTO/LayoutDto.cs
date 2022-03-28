using TicketManagement.Web.Interfaces;

namespace TicketManagement.Web.ModelsDTO
{
    /// <summary>
    /// Represent layout's dto model.
    /// </summary>
    public class LayoutDto : IEntityDto
    {
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets venue's id.
        /// </summary>
        public int VenueId { get; set; }

        /// <summary>
        /// Gets or sets description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets name of layout in venue.
        /// </summary>
        public string Name { get; set; }
    }
}
