using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Models;

namespace TicketManagement.DataAccess.Repositories
{
    internal class SeatEfRepository : EfRepository<Seat>, IRepository<Seat>
    {
        public SeatEfRepository(TicketManagementContext dbContext)
            : base(dbContext)
        {
        }
    }
}
