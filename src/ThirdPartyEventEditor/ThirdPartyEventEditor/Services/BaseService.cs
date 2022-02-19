using System.Collections.Generic;
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

        public virtual T Create(T obj)
        {
            return Repository.Create(obj);
        }

        public virtual T Delete(T obj)
        {
            return Repository.Delete(obj);
        }

        public virtual int DeleteById(int id)
        {
            return Repository.DeleteById(id);
        }

        public virtual IEnumerable<T> GetAll()
        {
            return Repository.GetAll();
        }

        public virtual T GetById(int id)
        {
            return Repository.GetById(id);
        }

        public virtual T Update(T obj)
        {
            return Repository.Update(obj);
        }
    }
}