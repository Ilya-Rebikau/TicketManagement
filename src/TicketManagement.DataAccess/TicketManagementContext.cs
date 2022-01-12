using Microsoft.EntityFrameworkCore;
using TicketManagement.DataAccess.Models;

namespace TicketManagement.DataAccess
{
    /// <summary>
    /// TicketManagementContext instance represents a session with database.
    /// </summary>
    public class TicketManagementContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TicketManagementContext"/> class.
        /// </summary>
        /// <param name="options">DbContextOptions object for TicketManagementContext.</param>
        public TicketManagementContext(DbContextOptions<TicketManagementContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TicketManagementContext"/> class.
        /// </summary>
        public TicketManagementContext()
        {
        }

        /// <summary>
        /// Gets or sets area models.
        /// </summary>
        public DbSet<Area> Areas { get; set; }

        /// <summary>
        /// Gets or sets event models.
        /// </summary>
        public DbSet<Event> Events { get; set; }

        /// <summary>
        /// Gets or sets event area models.
        /// </summary>
        public DbSet<EventArea> EventAreas { get; set; }

        /// <summary>
        /// Gets or sets event seat models.
        /// </summary>
        public DbSet<EventSeat> EventSeats { get; set; }

        /// <summary>
        /// Gets or sets layout models.
        /// </summary>
        public DbSet<Layout> Layouts { get; set; }

        /// <summary>
        /// Gets or sets seat models.
        /// </summary>
        public DbSet<Seat> Seats { get; set; }

        /// <summary>
        /// Gets or sets venue models.
        /// </summary>
        public DbSet<Venue> Venues { get; set; }
    }
}
