using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.EventManagerAPI.Infrastructure;
using TicketManagement.EventManagerAPI.Interfaces;

namespace TicketManagement.EventManagerAPI.Services
{
    /// <summary>
    /// Base realization for reader service.
    /// </summary>
    /// <typeparam name="TModel">Type of model.</typeparam>
    /// <typeparam name="TDto">Type of dto.</typeparam>
    internal class BaseReaderService<TModel, TDto> : IReaderService<TDto>
        where TModel : IEntity
        where TDto : IEntityDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseReaderService{TModel, TDto}"/> class.
        /// </summary>
        /// <param name="reader">IReaderJson object.</param>
        /// <param name="converter">IConverter object.</param>
        public BaseReaderService(IReaderJson<TModel> reader, IConverter<TModel, TDto> converter)
        {
            Reader = reader;
            Converter = converter;
        }

        /// <summary>
        /// Gets or privately sets IReaderJson object for service.
        /// </summary>
        protected IReaderJson<TModel> Reader { get; private set; }

        /// <summary>
        /// Gets or privately sets IConverter object for service.
        /// </summary>
        protected IConverter<TModel, TDto> Converter { get; private set; }

        public virtual async Task<IEnumerable<TDto>> GetAllAsync(string json)
        {
            var models = await Reader.GetAllAsync(json);
            return await Converter.ConvertSourceModelRangeToDestinationModelRange(models);
        }

        public virtual async Task<TDto> GetByIdAsync(int id, string json)
        {
            CheckForId(id, json);
            var model = await Reader.GetByIdAsync(id, json);
            return await Converter.ConvertSourceToDestination(model);
        }

        /// <summary>
        /// Check that id is positive.
        /// </summary>
        /// <param name="id">Id.</param>
        /// <param name="json">Content of json file.</param>
        /// <exception cref="ValidationException">Generates exception in case id isn't positive.</exception>
        private async void CheckForId(int id, string json)
        {
            var allModels = await Reader.GetAllAsync(json);
            if (id <= 0)
            {
                throw new ValidationException("Id must be positive!");
            }

            if (!allModels.Any(m => m.Id == id))
            {
                throw new ValidationException("There is no such id!");
            }
        }
    }
}
