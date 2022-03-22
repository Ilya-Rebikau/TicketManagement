namespace TicketManagement.UserAPI.Models
{
    /// <summary>
    /// Jwt token settings.
    /// </summary>
    public class JwtTokenSettings
    {
        /// <summary>
        /// Gets or sets jwt issuer.
        /// </summary>
        public string JwtIssuer { get; set; }

        /// <summary>
        /// Gets or sets jwt audience.
        /// </summary>
        public string JwtAudience { get; set; }

        /// <summary>
        /// Gets or sets jwt secret key.
        /// </summary>
        public string JwtSecretKey { get; set; }
    }
}
