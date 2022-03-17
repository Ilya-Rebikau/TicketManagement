using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using TicketManagement.UserAPI.Services;

namespace TicketManagement.UserAPI.Middlewares
{
    public class JwtMiddleware
    {
        private const string AuthorizationKey = "Authorization";
        private readonly RequestDelegate _next;
        private readonly JwtTokenService _jwtTokenService;

        public JwtMiddleware(RequestDelegate next, JwtTokenService jwtTokenService)
        {
            _next = next;
            _jwtTokenService = jwtTokenService;
        }

        public async Task Invoke(HttpContext context)
        {
            context.Request.Headers.TryGetValue(AuthorizationKey, out var token);
            if (_jwtTokenService.ValidateToken(token))
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
