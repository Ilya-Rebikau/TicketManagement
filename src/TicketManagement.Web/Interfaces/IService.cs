using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("TicketManagement.IntegrationTests")]
[assembly: InternalsVisibleTo("TicketManagement.UnitTests")]

namespace TicketManagement.Web.Interfaces
{
    /// <summary>
    /// Services with CRUD operations for models.
    /// </summary>
    public interface IService<TDto>
    {
        /// <summary>
        /// Get all objects from database.
        /// </summary>
        /// <returns>All objects.</returns>
        Task<IEnumerable<TDto>> GetAllAsync();

        /// <summary>
        /// Get one object from database by his id.
        /// </summary>
        /// <param name="id">id.</param>
        /// <returns>Object.</returns>
        Task<TDto> GetByIdAsync(int id);

        /// <summary>
        /// Add new object to database with validation logic.
        /// </summary>
        /// <param name="obj">Adding object.</param>
        /// <returns>Create object.</returns>
        Task<TDto> CreateAsync(TDto obj);

        /// <summary>
        /// Update one object in database with validation logic.
        /// </summary>
        /// <param name="obj">Updating object.</param>
        /// <returns>Updated object.</returns>
        Task<TDto> UpdateAsync(TDto obj);

        /// <summary>
        /// Delete object from database.
        /// </summary>
        /// <param name="obj">Deleting object.</param>
        /// <returns>Deleted object.</returns>
        Task<TDto> DeleteAsync(TDto obj);

        /// <summary>
        /// Delete object by id.
        /// </summary>
        /// <param name="id">Id of deleting object.</param>
        /// <returns>Id of deleted object.</returns>
        Task<int> DeleteById(int id);
    }
}
