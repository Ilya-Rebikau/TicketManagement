using TicketManagement.BusinessLogic.Interfaces;

namespace TicketManagement.BusinessLogic.ModelsDTO
{
    /// <summary>
    /// Represent venue's dto model.
    /// </summary>
    public class VenueDto : IEntityDto
    {
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

        /// <summary>
        /// Gets or sets name.
        /// </summary>
        public string Name { get; set; }
    }
}
