namespace TicketManagement.Web.Models.Account
{
    public class EditAccountViewModel
    {
        public string Id { get; set; }

        public string Email { get; set; }

        /// <summary>
        /// Gets or sets first name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets surname.
        /// </summary>
        public string Surname { get; set; }
    }
}
