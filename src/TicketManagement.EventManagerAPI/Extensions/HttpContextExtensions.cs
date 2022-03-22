using Microsoft.AspNetCore.Http;

namespace TicketManagement.EventManagerAPI.Extensions
{
    /// <summary>
    /// Extensions for HttpContext.
    /// </summary>
    public static class HttpContextExtensions
    {
        /// <summary>
        /// Key with name of place in header with jwt token.
        /// </summary>
        private const string AuthorizationKey = "Authorization";

        /// <summary>
        /// Get jwt token from http context.
        /// </summary>
        /// <param name="httpContext">HttpContext object.</param>
        /// <returns>Jwt token.</returns>
        public static string GetJwtToken(this HttpContext httpContext)
        {
            httpContext.Request.Headers.TryGetValue(AuthorizationKey, out var token);
            return token;
        }
    }
}
