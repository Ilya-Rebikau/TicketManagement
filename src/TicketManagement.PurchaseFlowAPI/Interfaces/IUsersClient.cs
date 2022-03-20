using System.Threading.Tasks;
using RestEase;
using TicketManagement.PurchaseFlowAPI.ModelsDTO;

namespace TicketManagement.PurchaseFlowAPI.Interfaces
{
    public interface IUsersClient
    {
        [Post("account/validatetoken")]
        public Task<bool> ValidateToken([Header("Authorization")] string token);

        [Post("account/converttime")]
        public Task<EventDto> ConvertTimeFromUtcToUsers([Header("Authorization")] string token, [Body] EventDto @event);

        [Post("account/getuserid")]
        public Task<string> GetUserId([Header("Authorization")] string token);

        [Post("account/changebalance")]
        public Task<bool> ChangeBalanceForUser([Header("Authorization")] string token, [Body] double price);
    }
}
