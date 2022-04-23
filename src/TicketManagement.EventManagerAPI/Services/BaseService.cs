using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.EventManagerAPI.Infrastructure;
using TicketManagement.EventManagerAPI.Interfaces;

namespace TicketManagement.EventManagerAPI.Services
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
        /// <param name="configuration">IConfiguration object.</param>
        protected BaseService(IRepository<TModel> repository, IConverter<TModel, TDto> converter, IConfiguration configuration)
        {
            Repository = repository;
            Converter = converter;
            CountOnPage = configuration.GetValue<int>("CountOnPage");
        }

        /// <summary>
        /// Gets or privately sets count of models on one page.
        /// </summary>
        protected int CountOnPage { get; private set; }

        /// <summary>
        /// Gets or privately sets IRepository object for CRUD operations in database.
        /// </summary>
        protected IRepository<TModel> Repository { get; private set; }

        /// <summary>
        /// Gets or privately sets IConverter object.
        /// </summary>
        protected IConverter<TModel, TDto> Converter { get; private set; }

        public async virtual Task<IEnumerable<TDto>> GetAllAsync(int pageNumber)
        {
            CheckForPageNumber(pageNumber);
            var models = await Repository.GetAllAsync();
            models = models.OrderBy(m => m.Id).Skip((pageNumber - 1) * CountOnPage).Take(CountOnPage);
            return await Converter.ConvertSourceModelRangeToDestinationModelRange(models);
        }

        public async virtual Task<TDto> GetByIdAsync(int id)
        {
            CheckForId(id);
            var model = await Repository.GetByIdAsync(id);
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
            CheckForId(id);
            var model = await GetByIdAsync(id);
            await DeleteAsync(model);
            return id;
        }

        /// <summary>
        /// Check that id is positive.
        /// </summary>
        /// <param name="id">Id.</param>
        /// <exception cref="ValidationException">Generates exception in case id isn't positive.</exception>
        private async void CheckForId(int id)
        {
            var allModels = await Repository.GetAllAsync();
            if (id <= 0)
            {
                throw new ValidationException("Id must be positive!");
            }

            if (!allModels.Any(m => m.Id == id))
            {
                throw new ValidationException("There is no such id!");
            }
        }

        /// <summary>
        /// Check that page number is positive.
        /// </summary>
        /// <param name="pageNumber">Page number.</param>
        /// <exception cref="ValidationException">Generates exception in case page number isn't positive.</exception>
        private async void CheckForPageNumber(int pageNumber)
        {
            if (pageNumber <= 0)
            {
                throw new ValidationException("Page number must be positive!");
            }

            var allModels = await Repository.GetAllAsync();
            var modelsOnPage = allModels.OrderBy(m => m.Id).Skip((pageNumber - 1) * CountOnPage).Take(CountOnPage);
            if (modelsOnPage is null || !modelsOnPage.Any())
            {
                throw new ValidationException("This page doesn't contain data.");
            }
        }
    }
}
