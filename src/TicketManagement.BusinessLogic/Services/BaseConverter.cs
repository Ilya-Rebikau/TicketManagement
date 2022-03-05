using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.DataAccess.Interfaces;

namespace TicketManagement.BusinessLogic.Services
{
    /// <summary>
    /// Base converting dto to models and models to dto.
    /// </summary>
    /// <typeparam name="TModel">Model.</typeparam>
    /// <typeparam name="TDto">Dto.</typeparam>
    internal class BaseConverter<TModel, TDto> : IConverter<TModel, TDto>
        where TModel : IEntity
        where TDto : IEntityDto
    {
        public virtual async Task<TDto> ConvertModelToDto(TModel model)
        {
            return await Task.Run(() =>
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<TModel, TDto>());
                var mapper = config.CreateMapper();
                var dto = mapper.Map<TModel, TDto>(model);
                return dto;
            });
        }

        public virtual async Task<TModel> ConvertDtoToModel(TDto dto)
        {
            return await Task.Run(() =>
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<TDto, TModel>());
                var mapper = config.CreateMapper();
                var model = mapper.Map<TDto, TModel>(dto);
                return model;
            });
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
