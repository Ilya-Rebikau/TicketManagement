using System.ComponentModel.DataAnnotations;
using TicketManagement.Web.ModelsDTO;

namespace TicketManagement.Web.Models.Account
{
    /// <summary>
    /// Account view model.
    /// </summary>
    public class AccountTicketViewModel
    {
        /// <summary>
        /// Gets or sets event.
        /// </summary>
        public EventDto Event { get; set; }

        /// <summary>
        /// Gets or sets price.
        /// </summary>
        [Display(Name="Price")]
        public double Price { get; set; }
    }
}
