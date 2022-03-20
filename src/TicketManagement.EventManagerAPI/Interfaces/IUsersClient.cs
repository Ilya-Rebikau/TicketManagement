﻿using System.Threading.Tasks;
using RestEase;
using TicketManagement.EventManagerAPI.ModelsDTO;

namespace TicketManagement.EventManagerAPI.Interfaces
{
    public interface IUsersClient
    {
        [Post("account/validatetoken")]
        public Task<bool> ValidateToken([Header("Authorization")] string token);

        [Post("account/converttime")]
        public Task<EventDto> ConvertTimeFromUtcToUsers([Header("Authorization")] string token, [Body] EventDto @event);
    }
}
