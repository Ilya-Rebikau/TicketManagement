namespace TicketManagement.UserAPI.Models.Account
{
    /// <summary>
    /// Add balance to account model.
    /// </summary>
    public class AddBalanceModel
    {
        /// <summary>
        /// Gets or sets account id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets balance for account.
        /// </summary>
        public double Balance { get; set; }
    }
}
