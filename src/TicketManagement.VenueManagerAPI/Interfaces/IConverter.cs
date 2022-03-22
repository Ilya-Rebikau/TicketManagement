using System.Collections.Generic;
using System.Threading.Tasks;
using TicketManagement.DataAccess.Interfaces;

namespace TicketManagement.VenueManagerAPI.Interfaces
{
    /// <summary>
    /// Service to convert models and dto.
    /// </summary>
    /// <typeparam name="TModel">Model.</typeparam>
    /// <typeparam name="TDto">Dto.</typeparam>
    public interface IConverter<TModel, TDto>
        where TModel : IEntity
        where TDto : IEntityDto
    {
        /// <summary>
        /// Convert model object to dto.
        /// </summary>
        /// <param name="model">Model.</param>
        /// <returns>Dto.</returns>
        Task<TDto> ConvertModelToDto(TModel model);

        /// <summary>
        /// Convert dto to model object.
        /// </summary>
        /// <param name="dto">Dto.</param>
        /// <returns>Model.</returns>
        Task<TModel> ConvertDtoToModel(TDto dto);

        /// <summary>
        /// Convert range of models to range of dtos.
        /// </summary>
        /// <param name="models">Range of models.</param>
        /// <returns>Range of dtos.</returns>
        Task<IEnumerable<TDto>> ConvertModelsRangeToDtos(IEnumerable<TModel> models);

        /// <summary>
        /// Convert range of dtos to range of models.
        /// </summary>
        /// <param name="dtos">Range of dtos.</param>
        /// <returns>Range of models.</returns>
        Task<IEnumerable<TModel>> ConvertDtosRangeToModels(IEnumerable<TDto> dtos);
    }
}
