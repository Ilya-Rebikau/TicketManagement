using System.Threading.Tasks;
using RestEase;

namespace TicketManagement.VenueManagerAPI.Interfaces
{
    public interface IUsersClient
    {
        [Post("account/validatetoken")]
        public Task<bool> ValidateToken([Header("Authorization")] string token);
    }
}
