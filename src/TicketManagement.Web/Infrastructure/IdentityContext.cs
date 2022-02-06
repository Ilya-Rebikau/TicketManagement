using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TicketManagement.Web.Models;

namespace TicketManagement.Web.Infrastructure
{
    /// <summary>
    /// DbContext for identity.
    /// Using model User.
    /// </summary>
    public class IdentityContext : IdentityDbContext<User>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityContext"/> class.
        /// </summary>
        /// <param name="options">DbContextOptions for IdentityContext object.</param>
        public IdentityContext(DbContextOptions<IdentityContext> options)
            : base(options)
        {
        }
    }
}
