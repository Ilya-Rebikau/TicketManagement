using System.ComponentModel.DataAnnotations;
using TicketManagement.VenueManagerAPI.ModelsDTO;

namespace TicketManagement.VenueManagerAPI.Models.Venues
{
    /// <summary>
    /// Venue model.
    /// </summary>
    public class VenueModel
    {
        /// <summary>
        /// Gets or sets id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets description.
        /// </summary>
        [Required(ErrorMessage = "FieldRequired")]
        [Display(Name = "Description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets ot sets address.
        /// </summary>
        [Required(ErrorMessage = "FieldRequired")]
        [Display(Name = "Address")]
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets phone.
        /// </summary>
        [Display(Name = "Phone")]
        public string Phone { get; set; }

        /// <summary>
        /// Gets or sets name.
        /// </summary>
        [Required(ErrorMessage = "FieldRequired")]
        [Display(Name = "Name")]
        public string Name { get; set; }
    }
}
