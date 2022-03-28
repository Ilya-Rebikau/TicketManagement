namespace TicketManagement.Web.Models
{
    /// <summary>
    /// View model for pagination.
    /// </summary>
    public static class PageViewModel
    {
        /// <summary>
        /// Gets or sets page number.
        /// </summary>
        public static int PageNumber { get; set; }

        /// <summary>
        /// Gets or sets next page condition.
        /// True if next page exist and false if not.
        /// </summary>
        public static bool NextPage { get; set; }
    }
}
