using System.ComponentModel.DataAnnotations;
using TicketManagement.PurchaseFlowAPI.ModelsDTO;

namespace TicketManagement.PurchaseFlowAPI.Models
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
