using AutoMapper;
using TicketManagement.UserAPI.Models;
using TicketManagement.UserAPI.Models.Account;

namespace TicketManagement.UserAPI.Automapper
{
    /// <summary>
    /// Profile for automapper.
    /// </summary>
    internal class AutoMapperProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AutoMapperProfile"/> class.
        /// </summary>
        public AutoMapperProfile()
        {
            CreateMap<User, EditAccountModel>().ReverseMap();
        }
    }
}
