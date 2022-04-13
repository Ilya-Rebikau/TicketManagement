using Microsoft.AspNetCore.Http;

namespace TicketManagement.Web.Extensions
{
    /// <summary>
    /// Extensions for HttpContext.
    /// </summary>
    public static class HttpContextExtensions
    {
        /// <summary>
        /// Cookies name.
        /// </summary>
        private const string CookiesKey = "JwtTokenCookie";

        /// <summary>
        /// Get jwt token from http context.
        /// </summary>
        /// <param name="httpContext">HttpContext object.</param>
        /// <returns>Jwt token.</returns>
        public static string GetJwtToken(this HttpContext httpContext)
        {
            httpContext.Request.Cookies.TryGetValue(CookiesKey, out var token);
            return token;
        }

        /// <summary>
        /// Add jwt token to cookies.
        /// </summary>
        /// <param name="httpContext">HttpContext object.</param>
        /// <param name="token">Jwt token.</param>
        public static void AppendCookies(this HttpContext httpContext, string token)
        {
            httpContext.Response.Cookies.Append(CookiesKey, token, new CookieOptions
            {
                HttpOnly = false,
                SameSite = SameSiteMode.Strict,
            });
        }
    }
}
