namespace TicketManagement.Web.Models
{
    /// <summary>
    /// Error view model.
    /// </summary>
    public class ErrorViewModel
    {
        /// <summary>
        /// Gets or sets request id.
        /// </summary>
        public string RequestId { get; set; }

        /// <summary>
        /// Gets or sets expression to show or not request id.
        /// </summary>
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
