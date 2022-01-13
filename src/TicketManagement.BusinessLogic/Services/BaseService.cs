using System.Collections.Generic;
using System.Threading.Tasks;
using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.DataAccess.Interfaces;

namespace TicketManagement.BusinessLogic.Services
{
    /// <summary>
    /// Service with base realization for CRUD operations for models.
    /// </summary>
    /// <typeparam name="T">Type of model for CRUD operations.</typeparam>
    internal class BaseService<T> : IService<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseService{T}"/> class.
        /// </summary>
        /// <param name="repository">IRepository object with CRUD operations.</param>
        protected BaseService(IRepository<T> repository)
        {
            Repository = repository;
        }

        /// <summary>
        /// Gets or privatly sets IRepository object for CRUD operations in database.
        /// </summary>
        protected IRepository<T> Repository { get; private set; }

        public async virtual Task<IEnumerable<T>> GetAllAsync()
        {
            return await Repository.GetAllAsync();
        }

        public async virtual Task<T> GetByIdAsync(int id)
        {
            return await Repository.GetByIdAsync(id);
        }

        public async virtual Task<T> CreateAsync(T obj)
        {
            return await Repository.CreateAsync(obj);
        }

        public async virtual Task<T> UpdateAsync(T obj)
        {
            return await Repository.UpdateAsync(obj);
        }

        public async virtual Task<T> DeleteAsync(T obj)
        {
            return await Repository.DeleteAsync(obj);
        }
    }
}
