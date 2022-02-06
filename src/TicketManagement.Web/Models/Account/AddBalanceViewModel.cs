using System.ComponentModel.DataAnnotations;

namespace TicketManagement.Web.Models.Account
{
    public class AddBalanceViewModel
    {
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets balance.
        /// </summary>
        [Range(0, double.MaxValue)]
        public double Balance { get; set; }
    }
}
