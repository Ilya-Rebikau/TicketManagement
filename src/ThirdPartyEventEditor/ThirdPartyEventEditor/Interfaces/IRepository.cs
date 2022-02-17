using System.Collections.Generic;

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
        IEnumerable<T> GetAll();

        /// <summary>
        /// Get one object from json file by his id.
        /// </summary>
        /// <param name="id">Id.</param>
        /// <returns>Object.</returns>
        T GetById(int id);

        /// <summary>
        /// Add new object to json file.
        /// </summary>
        /// <param name="obj">Adding object.</param>
        T Create(T obj);

        /// <summary>
        /// Update one object in json file.
        /// </summary>
        /// <param name="obj">Updating object.</param>
        /// <returns>Updated object.</returns>
        T Update(T obj);

        /// <summary>
        /// Delete object from json file.
        /// </summary>
        /// <param name="obj">Deleting object.</param>
        /// <returns>Deleted object.</returns>
        T Delete(T obj);
    }
}
