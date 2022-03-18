using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using TicketManagement.UserAPI.Models;

namespace TicketManagement.UserAPI.Services
{
    public class ConverterForTime
    {
        /// <summary>
        /// UserManager object.
        /// </summary>
        private readonly UserManager<User> _userManager;

        /// <summary>
        /// JwtTokenService object.
        /// </summary>
        private readonly JwtTokenService _jwtTokenService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConverterForTime"/> class.
        /// </summary>
        /// <param name="userManager">UserManager object.</param>
        /// <param name="jwtTokenService">JwtTokenService object.</param>
        public ConverterForTime(UserManager<User> userManager, JwtTokenService jwtTokenService)
        {
            _userManager = userManager;
            _jwtTokenService = jwtTokenService;
        }

        /// <summary>
        /// Convert time from UTC to user time zone.
        /// </summary>
        /// <param name="event">Event object.</param>
        /// <param name="token">Jwt token of user.</param>
        /// <returns>Task.</returns>
        public async Task<EventDto> ConvertTimeForUser(EventDto @event, string token)
        {
            if (_jwtTokenService.ValidateToken(token))
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtSecurityToken = handler.ReadJwtToken(token);
                var claims = jwtSecurityToken.Claims;
                var identity = new ClaimsIdentity(claims);
                var principalClaims = new ClaimsPrincipal(identity);
                var email = principalClaims.FindFirst(JwtRegisteredClaimNames.Sub).Value;
                var user = await _userManager.FindByEmailAsync(email);
                if (user is not null && !string.IsNullOrWhiteSpace(user.TimeZone))
                {
                    var userTimeZone = TimeZoneInfo.FindSystemTimeZoneById(user.TimeZone);
                    @event.TimeStart = TimeZoneInfo.ConvertTime(@event.TimeStart, TimeZoneInfo.Utc, userTimeZone);
                    @event.TimeEnd = TimeZoneInfo.ConvertTime(@event.TimeEnd, TimeZoneInfo.Utc, userTimeZone);
                }
            }

            return @event;
        }
    }
}
