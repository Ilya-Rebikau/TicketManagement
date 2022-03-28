using System.Collections.Generic;
using System.Threading.Tasks;

namespace TicketManagement.EventManagerAPI.Interfaces
{
    /// <summary>
    /// Service for work with json readers.
    /// </summary>
    /// <typeparam name="TDto">Type of model.</typeparam>
    public interface IReaderService<TDto>
        where TDto : IEntityDto
    {
        /// <summary>
        /// Get all object from json file.
        /// </summary>
        /// <param name="json">Content of json file.</param>
        /// <returns>All objects.</returns>
        Task<IEnumerable<TDto>> GetAllAsync(string json);

        /// <summary>
        /// Get object from json file by his id.
        /// </summary>
        /// <param name="id">Id of object.</param>
        /// <param name="json">Content of json file.</param>
        /// <returns>Object.</returns>
        Task<TDto> GetByIdAsync(int id, string json);
    }
}
