using System.Collections.Generic;
using System.Threading.Tasks;
using TicketManagement.DataAccess.Interfaces;
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
            var model = await Reader.GetByIdAsync(id, json);
            return await Converter.ConvertSourceToDestination(model);
        }
    }
}
