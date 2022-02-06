using System.ComponentModel.DataAnnotations;

namespace TicketManagement.Web.Models.Account
{
    /// <summary>
    /// Add balance to account view model.
    /// </summary>
    public class AddBalanceViewModel
    {
        /// <summary>
        /// Gets or sets account id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets balance for account.
        /// </summary>
        [Range(0, double.MaxValue)]
        [Display(Name = "Balance")]
        public double Balance { get; set; }
    }
}
