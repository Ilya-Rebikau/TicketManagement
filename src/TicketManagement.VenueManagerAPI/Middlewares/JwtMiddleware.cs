using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using TicketManagement.VenueManagerAPI.Interfaces;

namespace TicketManagement.VenueManagerAPI.Middlewares
{
    /// <summary>
    /// Middleware for jwt authorize.
    /// </summary>
    public class JwtMiddleware
    {
        /// <summary>
        /// Key with authorization string in header.
        /// </summary>
        private const string AuthorizationKey = "Authorization";

        /// <summary>
        /// RequestDelegate object.
        /// </summary>
        private readonly RequestDelegate _next;

        /// <summary>
        /// IUsersClient object.
        /// </summary>
        private readonly IUsersClient _usersClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="JwtMiddleware"/> class.
        /// </summary>
        /// <param name="next">RequestDelegate object.</param>
        /// <param name="usersClient">IUsersClient object.</param>
        public JwtMiddleware(RequestDelegate next, IUsersClient usersClient)
        {
            _next = next;
            _usersClient = usersClient;
        }

        /// <summary>
        /// Invoke jwt authorization.
        /// </summary>
        /// <param name="context">HttpContext object.</param>
        /// <returns>Task.</returns>
        public async Task Invoke(HttpContext context)
        {
            context.Request.Headers.TryGetValue(AuthorizationKey, out var token);
            if (await _usersClient.ValidateToken(token))
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtSecurityToken = handler.ReadJwtToken(token);
                var claims = jwtSecurityToken.Claims;
                var identity = new ClaimsIdentity(claims);
                var principalClaims = new ClaimsPrincipal(identity);
                context.User = principalClaims;
            }

            await _next(context);
        }
    }
}
