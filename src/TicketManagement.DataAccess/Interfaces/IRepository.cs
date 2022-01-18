using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("TicketManagement.IntegrationTests")]

namespace TicketManagement.DataAccess.Interfaces
{
    /// <summary>
    /// Represent a repository pattern with CRUD operations.
    /// </summary>
    public interface IRepository<T>
    {
        /// <summary>
        /// Get all objects from database.
        /// </summary>
        /// <returns>All objects.</returns>
        Task<IQueryable<T>> GetAllAsync();

        /// <summary>
        /// Get one object from database by his id.
        /// </summary>
        /// <param name="id">id.</param>
        /// <returns>Object.</returns>
        Task<T> GetByIdAsync(int id);

        /// <summary>
        /// Add new object to database.
        /// </summary>
        /// <param name="obj">Adding object.</param>
        Task<T> CreateAsync(T obj);

        /// <summary>
        /// Update one object in database.
        /// </summary>
        /// <param name="obj">Updating object.</param>
        /// <returns>Updated object.</returns>
        Task<T> UpdateAsync(T obj);

        /// <summary>
        /// Delete object from database.
        /// </summary>
        /// <param name="obj">Deleting object.</param>
        /// <returns>Deleted object.</returns>
        Task<T> DeleteAsync(T obj);
    }
}
