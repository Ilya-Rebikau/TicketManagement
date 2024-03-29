﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.PurchaseFlowAPI.Infrastructure;
using TicketManagement.PurchaseFlowAPI.Interfaces;

namespace TicketManagement.PurchaseFlowAPI.Services
{
    /// <summary>
    /// Service with base realization for CRUD operations for models.
    /// </summary>
    /// <typeparam name="TModel">Type of model for CRUD operations.</typeparam>
    /// <typeparam name="TDto">Type of dto for CRUD operations.</typeparam>
    internal class BaseService<TModel, TDto> : IService<TDto>
        where TModel : IEntity
        where TDto : IEntityDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseService{TModel, TDto}"/> class.
        /// </summary>
        /// <param name="repository">IRepository object with CRUD operations.</param>
        /// <param name="converter">IConverter object.</param>
        protected BaseService(IRepository<TModel> repository, IConverter<TModel, TDto> converter)
        {
            Repository = repository;
            Converter = converter;
        }

        /// <summary>
        /// Gets or privatly sets IRepository object for CRUD operations in database.
        /// </summary>
        protected IRepository<TModel> Repository { get; private set; }

        /// <summary>
        /// Gets or privatly sets IConverter object.
        /// </summary>
        protected IConverter<TModel, TDto> Converter { get; private set; }

        public async virtual Task<IEnumerable<TDto>> GetAllAsync()
        {
            var models = Repository.GetAll();
            return await Converter.ConvertSourceModelRangeToDestinationModelRange(models);
        }

        public async virtual Task<TDto> GetByIdAsync(int id)
        {
            var model = await CheckForIdAndGetModel(id);
            return await Converter.ConvertSourceToDestination(model);
        }

        public async virtual Task<TDto> CreateAsync(TDto obj)
        {
            var model = await Repository.CreateAsync(await Converter.ConvertDestinationToSource(obj));
            return await Converter.ConvertSourceToDestination(model);
        }

        public async virtual Task<TDto> UpdateAsync(TDto obj)
        {
            var model = await Repository.UpdateAsync(await Converter.ConvertDestinationToSource(obj));
            return await Converter.ConvertSourceToDestination(model);
        }

        public async virtual Task<TDto> DeleteAsync(TDto obj)
        {
            var model = await Repository.DeleteAsync(await Converter.ConvertDestinationToSource(obj));
            return await Converter.ConvertSourceToDestination(model);
        }

        public async virtual Task<int> DeleteById(int id)
        {
            var model = await CheckForIdAndGetModel(id);
            await DeleteAsync(await Converter.ConvertSourceToDestination(model));
            return id;
        }

        /// <summary>
        /// Check that id is positive.
        /// </summary>
        /// <param name="id">Id.</param>
        /// <exception cref="ValidationException">Generates exception in case id isn't positive.</exception>
        private async Task<TModel> CheckForIdAndGetModel(int id)
        {
            if (id <= 0)
            {
                throw new ValidationException("Id must be positive!");
            }

            var model = await Repository.GetByIdAsync(id);
            if (model is null)
            {
                throw new ValidationException("There is no such id!");
            }

            return model;
        }
    }
}
