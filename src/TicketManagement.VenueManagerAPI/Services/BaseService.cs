using System.Collections.Generic;
using System.Threading.Tasks;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.VenueManagerAPI.Interfaces;

namespace TicketManagement.VenueManagerAPI.Services
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
            var models = await Repository.GetAllAsync();
            return await Converter.ConvertModelsRangeToDtos(models);
        }

        public async virtual Task<TDto> GetByIdAsync(int id)
        {
            var model = await Repository.GetByIdAsync(id);
            return await Converter.ConvertModelToDto(model);
        }

        public async virtual Task<TDto> CreateAsync(TDto obj)
        {
            var model = await Repository.CreateAsync(await Converter.ConvertDtoToModel(obj));
            return await Converter.ConvertModelToDto(model);
        }

        public async virtual Task<TDto> UpdateAsync(TDto obj)
        {
            var model = await Repository.UpdateAsync(await Converter.ConvertDtoToModel(obj));
            return await Converter.ConvertModelToDto(model);
        }

        public async virtual Task<TDto> DeleteAsync(TDto obj)
        {
            var model = await Repository.DeleteAsync(await Converter.ConvertDtoToModel(obj));
            return await Converter.ConvertModelToDto(model);
        }

        public async virtual Task<int> DeleteById(int id)
        {
            var model = await GetByIdAsync(id);
            await DeleteAsync(model);
            return id;
        }
    }
}
