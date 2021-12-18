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
        /// IRepository object for CRUD operations in database.
        /// </summary>
        private readonly IRepository<T> _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseService{T}"/> class.
        /// </summary>
        /// <param name="repository">IRepository object with CRUD operations.</param>
        protected BaseService(IRepository<T> repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Gets or privatly sets IRepository object for CRUD operations in database.
        /// </summary>
        protected IRepository<T> Repository { get; private set; }

        public IEnumerable<T> GetAll()
        {
            return _repository.GetAll();
        }

        public T GetById(int id)
        {
            return _repository.GetById(id);
        }

        public T Create(T obj)
        {
            return _repository.Create(obj);
        }

        public T Update(T obj)
        {
            return _repository.Update(obj);
        }

        public T Delete(T obj)
        {
            return _repository.Delete(obj);
        }
    }
}
