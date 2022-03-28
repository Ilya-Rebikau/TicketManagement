using System.Collections.Generic;
using System.Threading.Tasks;

namespace TicketManagement.UserAPI.Interfaces
{
    /// <summary>
    /// Service to convert models and dto.
    /// </summary>
    /// <typeparam name="TSource">Model.</typeparam>
    /// <typeparam name="TDestination">Api model or dto.</typeparam>
    public interface IConverter<TSource, TDestination>
    {
        /// <summary>
        /// Convert source model to destination model.
        /// </summary>
        /// <param name="source">Source model.</param>
        /// <returns>Destination model.</returns>
        Task<TDestination> ConvertSourceToDestination(TSource source);

        /// <summary>
        /// Convert destination models to source model.
        /// </summary>
        /// <param name="destination">Destination model.</param>
        /// <returns>Source model.</returns>
        Task<TSource> ConvertDestinationToSource(TDestination destination);

        /// <summary>
        /// Convert range of source models to range of destination models.
        /// </summary>
        /// <param name="sourceModels">Range of source models.</param>
        /// <returns>Range of destination models.</returns>
        Task<IEnumerable<TDestination>> ConvertSourceModelRangeToDestinationModelRange(IEnumerable<TSource> sourceModels);

        /// <summary>
        /// Convert range of destination models to range of source models.
        /// </summary>
        /// <param name="destinationModels">Range of destination models.</param>
        /// <returns>Range of source models.</returns>
        Task<IEnumerable<TSource>> ConvertDestinationModelRangeToSourceModelRange(IEnumerable<TDestination> destinationModels);
    }
}
