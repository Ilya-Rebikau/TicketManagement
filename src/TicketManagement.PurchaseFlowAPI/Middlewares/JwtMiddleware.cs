using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using TicketManagement.PurchaseFlowAPI.Interfaces;

namespace TicketManagement.PurchaseFlowAPI.Middlewares
{
    public class JwtMiddleware
    {
        private const string AuthorizationKey = "Authorization";
        private readonly RequestDelegate _next;
        private readonly IUsersClient _usersClient;

        public JwtMiddleware(RequestDelegate next, IUsersClient usersClient)
        {
            _next = next;
            _usersClient = usersClient;
        }

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
