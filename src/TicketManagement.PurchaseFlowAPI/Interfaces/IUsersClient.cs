using System.Threading.Tasks;
using RestEase;
using TicketManagement.PurchaseFlowAPI.ModelsDTO;

namespace TicketManagement.PurchaseFlowAPI.Interfaces
{
    /// <summary>
    /// Client for UsersAPI.
    /// </summary>
    public interface IUsersClient
    {
        /// <summary>
        /// Key with authorization string in header.
        /// </summary>
        private const string AuthorizationKey = "Authorization";

        /// <summary>
        /// Validate user jwt token.
        /// </summary>
        /// <param name="token">Jwt token.</param>
        /// <returns>True if token valid and false if not.</returns>
        [Post("account/validatetoken")]
        public Task<bool> ValidateToken([Header(AuthorizationKey)] string token);

        /// <summary>
        /// Convert time from utc to user timezone for event.
        /// </summary>
        /// <param name="token">Jwt token.</param>
        /// <param name="event">Event.</param>
        /// <returns>Updated event.</returns>
        [Post("account/converttime")]
        public Task<EventDto> ConvertTimeFromUtcToUsers([Header(AuthorizationKey)] string token, [Body] EventDto @event);

        /// <summary>
        /// Get user id.
        /// </summary>
        /// <param name="token">Jwt token.</param>
        /// <returns>User id.</returns>
        [Post("account/getuserid")]
        public Task<string> GetUserId([Header(AuthorizationKey)] string token);

        /// <summary>
        /// Change balance for user.
        /// </summary>
        /// <param name="token">Jwt token.</param>
        /// <param name="price">Price for place.</param>
        /// <returns>True if success and false if not.</returns>
        [Post("account/changebalance")]
        public Task<bool> ChangeBalanceForUser([Header(AuthorizationKey)] string token, [Body] double price);
    }
}
