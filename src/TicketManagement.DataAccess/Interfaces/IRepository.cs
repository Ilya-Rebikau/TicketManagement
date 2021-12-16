using System.Collections.Generic;

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
        IEnumerable<T> GetAll();

        /// <summary>
        /// Get one object from database by his id.
        /// </summary>
        /// <param name="id">id.</param>
        /// <returns>Object.</returns>
        T GetById(int id);

        /// <summary>
        /// Add new object to database.
        /// </summary>
        /// <param name="obj">Adding object.</param>
        T Create(T obj);

        /// <summary>
        /// Update one object in database.
        /// </summary>
        /// <param name="obj">Updating object.</param>
        /// <returns>Updated object.</returns>
        T Update(T obj);

        /// <summary>
        /// Delete object from database.
        /// </summary>
        /// <param name="obj">Deleting object.</param>
        /// <returns>Deleted object.</returns>
        T Delete(T obj);
    }
}
