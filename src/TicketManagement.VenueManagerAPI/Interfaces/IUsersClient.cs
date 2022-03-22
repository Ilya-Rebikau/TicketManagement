using System.Threading.Tasks;
using RestEase;

namespace TicketManagement.VenueManagerAPI.Interfaces
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
    }
}
