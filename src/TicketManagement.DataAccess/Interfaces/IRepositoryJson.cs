using System.Linq;
using System.Threading.Tasks;
using TicketManagement.DataAccess.Models;

namespace TicketManagement.DataAccess.Interfaces
{
    /// <summary>
    /// Repository for work with json file with only read operations.
    /// </summary>
    /// <typeparam name="T">Type of model.</typeparam>
    public interface IRepositoryJson<T>
        where T : IEntity
    {
        /// <summary>
        /// Get all object from json file.
        /// </summary>
        /// <param name="path">Path to json file.</param>
        /// <returns>All objects.</returns>
        Task<IQueryable<ThirdPartyEvent>> GetAllAsync(string path);

        /// <summary>
        /// Get object from json file by his id.
        /// </summary>
        /// <param name="id">Id of object.</param>
        /// <param name="path">Path to json file.</param>
        /// <returns>Object.</returns>
        Task<ThirdPartyEvent> GetByIdAsync(int id, string path);
    }
}
