using System.Collections.Generic;
using System.Threading.Tasks;

namespace ThirdPartyEventEditor.Interfaces
{
    /// <summary>
    /// Services with CRUD operations for models.
    /// </summary>
    public interface IService<T>
        where T : IEntity
    {
        /// <summary>
        /// Get all objects from json file.
        /// </summary>
        /// <returns>All objects.</returns>
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// Get one object from json file by his id.
        /// </summary>
        /// <param name="id">id.</param>
        /// <returns>Object.</returns>
        Task<T> GetByIdAsync(int id);

        /// <summary>
        /// Add new object to json file with validation logic.
        /// </summary>
        /// <param name="obj">Adding object.</param>
        /// <returns>Create object.</returns>
        Task<T> CreateAsync(T obj);

        /// <summary>
        /// Update one object in json file with validation logic.
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

        /// <summary>
        /// Delete object by id.
        /// </summary>
        /// <param name="id">Id of deleting object.</param>
        /// <returns>Id of deleted object.</returns>
        Task<int> DeleteByIdAsync(int id);
    }
}