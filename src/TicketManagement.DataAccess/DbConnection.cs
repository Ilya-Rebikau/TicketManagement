using Microsoft.Extensions.Configuration;

namespace TicketManagement.DataAccess
{
    /// <summary>
    /// Provides with string connection to MS SQL database.
    /// </summary>
    public static class DbConnection
    {
        /// <summary>
        /// Get connection string for access to database.
        /// </summary>
        /// <returns>String connection.</returns>
        public static string GetStringConnection()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
            return builder.Build().GetConnectionString("DefaultConnection");
        }
    }
}