using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
        /// Initializes a new instance of the <see cref="ConverterForTime"/> class.
        /// </summary>
        /// <param name="userManager">UserManager object.</param>
        public ConverterForTime(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        /// <summary>
        /// Convert time from UTC to user time zone.
        /// </summary>
        /// <param name="event">Event object.</param>
        /// <param name="userClaims">ClaimsPrincipal object with user claims.</param>
        /// <returns>Task.</returns>
        public async Task<EventDto> ConvertTimeForUser(EventDto @event, ClaimsPrincipal userClaims)
        {
            var user = await _userManager.GetUserAsync(userClaims);
            if (user is not null && !string.IsNullOrWhiteSpace(user.TimeZone))
            {
                var userTimeZone = TimeZoneInfo.FindSystemTimeZoneById(user.TimeZone);
                @event.TimeStart = TimeZoneInfo.ConvertTime(@event.TimeStart, TimeZoneInfo.Utc, userTimeZone);
                @event.TimeEnd = TimeZoneInfo.ConvertTime(@event.TimeEnd, TimeZoneInfo.Utc, userTimeZone);
            }

            return @event;
        }
    }
}
