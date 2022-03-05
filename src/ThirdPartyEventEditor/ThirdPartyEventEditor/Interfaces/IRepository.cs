using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("ThirdPartyEventEditor.UnitTests")]

namespace ThirdPartyEventEditor.Interfaces
{
    /// <summary>
    /// Represent a repository pattern with CRUD operations.
    /// </summary>
    public interface IRepository<T>
        where T : IEntity
    {
        /// <summary>
        /// Get all objects from json file.
        /// </summary>
        /// <returns>All objects.</returns>
        Task<IQueryable<T>> GetAllAsync();

        /// <summary>
        /// Get one object from json file by his id.
        /// </summary>
        /// <param name="id">Id.</param>
        /// <returns>Object.</returns>
        Task<T> GetByIdAsync(int id);

        /// <summary>
        /// Add new object to json file.
        /// </summary>
        /// <param name="obj">Adding object.</param>
        /// <returns>Created object.</returns>
        Task<T> CreateAsync(T obj);

        /// <summary>
        /// Update one object in json file.
        /// </summary>
        /// <param name="obj">Updating object.</param>
        /// <returns>Updated object.</returns>
        Task<T> UpdateAsync(T obj);

        /// <summary>
        /// Delete object from json file.
        /// </summary>
        /// <param name="obj">Deleting object.</param>
        /// <returns>Deleted object.</returns>
        Task<T> DeleteAsync(T obj);
    }
}
