using System.Threading.Tasks;
using RestEase;

namespace TicketManagement.EventManagerAPI.Interfaces
{
    public interface IUsersClient
    {
        [Post("account/validatetoken")]
        public Task<bool> ValidateToken([Header("Authorization")] string token);
    }
}
