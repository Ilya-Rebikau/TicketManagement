using System.Collections.Generic;
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

        public virtual IEnumerable<T> GetAll()
        {
            return Repository.GetAll();
        }

        public virtual T GetById(int id)
        {
            return Repository.GetById(id);
        }

        public virtual T Create(T obj)
        {
            return Repository.Create(obj);
        }

        public virtual T Update(T obj)
        {
            return Repository.Update(obj);
        }

        public virtual T Delete(T obj)
        {
            return Repository.Delete(obj);
        }
    }
}
