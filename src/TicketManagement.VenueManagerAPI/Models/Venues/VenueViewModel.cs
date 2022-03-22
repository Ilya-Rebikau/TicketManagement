using System.ComponentModel.DataAnnotations;
using TicketManagement.VenueManagerAPI.ModelsDTO;

namespace TicketManagement.VenueManagerAPI.Models.Venues
{
    /// <summary>
    /// Venue view model.
    /// </summary>
    public class VenueViewModel
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

        /// <summary>
        /// Convert venue dto to venue view model.
        /// </summary>
        /// <param name="venue">Venue dto.</param>
        public static implicit operator VenueViewModel(VenueDto venue)
        {
            return new VenueViewModel
            {
                Id = venue.Id,
                Description = venue.Description,
                Phone = venue.Phone,
                Address = venue.Address,
                Name = venue.Name,
            };
        }

        /// <summary>
        /// Convert venue view model to venue dto.
        /// </summary>
        /// <param name="venueVm">Venue view model.</param>
        public static implicit operator VenueDto(VenueViewModel venueVm)
        {
            return new VenueDto
            {
                Id = venueVm.Id,
                Description = venueVm.Description,
                Phone = venueVm.Phone,
                Address = venueVm.Address,
                Name = venueVm.Name,
            };
        }
    }
}
