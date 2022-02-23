using System.Collections.Generic;
using System.Threading.Tasks;
using ThirdPartyEventEditor.Interfaces;

namespace ThirdPartyEventEditor.Services
{
    /// <summary>
    /// Base service for CRUD operations with models.
    /// </summary>
    /// <typeparam name="T">Type of model.</typeparam>
    internal class BaseService<T> : IService<T>
        where T : IEntity
    {
        /// <summary>
        /// Gets or sets IRepository object.
        /// </summary>
        protected IRepository<T> Repository { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseService{T}"/> class.
        /// </summary>
        /// <param name="repository">IRepository object with CRUD operations.</param>
        public BaseService(IRepository<T> repository)
        {
            Repository = repository;
        }

        public virtual async Task<T> CreateAsync(T obj)
        {
            return await Repository.CreateAsync(obj);
        }

        public virtual async Task<T> DeleteAsync(T obj)
        {
            return await Repository.DeleteAsync(obj);
        }

        public virtual async Task<int> DeleteByIdAsync(int id)
        {
            var model = await Repository.GetByIdAsync(id);
            await Repository.DeleteAsync(model);
            return id;
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await Repository.GetAllAsync();
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await Repository.GetByIdAsync(id);
        }

        public virtual async Task<T> UpdateAsync(T obj)
        {
            return await Repository.UpdateAsync(obj);
        }
    }
}