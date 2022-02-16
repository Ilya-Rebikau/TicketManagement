using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using TicketManagement.BusinessLogic.ModelsDTO;
using TicketManagement.Web.Models;

namespace TicketManagement.Web.WebServices
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
        /// <param name="httpContext">HttpContext object.</param>
        /// <returns>Task.</returns>
        public async Task ConvertTimeForUser(EventDto @event, HttpContext httpContext)
        {
            var user = await _userManager.GetUserAsync(httpContext.User);
            if (user is not null && !string.IsNullOrWhiteSpace(user.TimeZone))
            {
                var userTimeZone = TimeZoneInfo.FindSystemTimeZoneById(user.TimeZone);
                @event.TimeStart = TimeZoneInfo.ConvertTime(@event.TimeStart, TimeZoneInfo.Utc, userTimeZone);
                @event.TimeEnd = TimeZoneInfo.ConvertTime(@event.TimeEnd, TimeZoneInfo.Utc, userTimeZone);
            }
        }

        /// <summary>
        /// Convert time from UTC to user time zone.
        /// </summary>
        /// <param name="event">Event object.</param>
        /// <param name="user">User.</param>
        public void ConvertTimeForUser(EventDto @event, User user)
        {
            if (user is not null && !string.IsNullOrWhiteSpace(user.TimeZone))
            {
                var userTimeZone = TimeZoneInfo.FindSystemTimeZoneById(user.TimeZone);
                @event.TimeStart = TimeZoneInfo.ConvertTime(@event.TimeStart, TimeZoneInfo.Utc, userTimeZone);
                @event.TimeEnd = TimeZoneInfo.ConvertTime(@event.TimeEnd, TimeZoneInfo.Utc, userTimeZone);
            }
        }
    }
}
