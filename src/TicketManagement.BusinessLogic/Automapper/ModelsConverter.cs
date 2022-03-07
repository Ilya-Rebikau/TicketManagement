using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.DataAccess.Interfaces;

namespace TicketManagement.BusinessLogic.Automapper
{
    /// <summary>
    /// Base converting dto to models and models to dto.
    /// </summary>
    /// <typeparam name="TModel">Model.</typeparam>
    /// <typeparam name="TDto">Dto.</typeparam>
    internal class ModelsConverter<TModel, TDto> : IConverter<TModel, TDto>
        where TModel : IEntity
        where TDto : IEntityDto
    {
        /// <summary>
        /// IMapper object.
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelsConverter{TModel, TDto}"/> class.
        /// </summary>
        /// <param name="mapper">IMapper object.</param>
        public ModelsConverter(IMapper mapper)
        {
            _mapper = mapper;
        }

        public virtual Task<TDto> ConvertModelToDto(TModel model)
        {
            var dto = _mapper.Map<TModel, TDto>(model);
            return Task.FromResult(dto);
        }

        public virtual Task<TModel> ConvertDtoToModel(TDto dto)
        {
            var model = _mapper.Map<TDto, TModel>(dto);
            return Task.FromResult(model);
        }

        public async Task<IEnumerable<TDto>> ConvertModelsRangeToDtos(IEnumerable<TModel> models)
        {
            var dtos = new List<TDto>();
            foreach (var model in models)
            {
                dtos.Add(await ConvertModelToDto(model));
            }

            return dtos;
        }

        public async Task<IEnumerable<TModel>> ConvertDtosRangeToModels(IEnumerable<TDto> dtos)
        {
            var models = new List<TModel>();
            foreach (var dto in dtos)
            {
                models.Add(await ConvertDtoToModel(dto));
            }

            return models;
        }
    }
}
