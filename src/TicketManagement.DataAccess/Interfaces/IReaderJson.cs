using System.Linq;
using System.Threading.Tasks;

namespace TicketManagement.DataAccess.Interfaces
{
    /// <summary>
    /// Work with json file with only read operations.
    /// </summary>
    /// <typeparam name="T">Type of model.</typeparam>
    public interface IReaderJson<T>
        where T : IEntity
    {
        /// <summary>
        /// Get all object from json file.
        /// </summary>
        /// <param name="json">Content of json file.</param>
        /// <returns>All objects.</returns>
        Task<IQueryable<T>> GetAllAsync(string json);

        /// <summary>
        /// Get object from json file by his id.
        /// </summary>
        /// <param name="id">Id of object.</param>
        /// <param name="json">Content of json file.</param>
        /// <returns>Object.</returns>
        Task<T> GetByIdAsync(int id, string json);
    }
}
