using TicketManagement.EventManagerAPI.ModelsDTO;

namespace TicketManagement.EventManagerAPI.Models.EventAreas
{
    /// <summary>
    /// Event area model.
    /// </summary>
    public class EventAreaModel
    {
        /// <summary>
        /// Gets or sets id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets event's id.
        /// </summary>
        public int EventId { get; set; }

        /// <summary>
        /// Gets or sets description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets X coordinate in layout.
        /// </summary>
        public int CoordX { get; set; }

        /// <summary>
        /// Gets or sets Y coordinate in layout.
        /// </summary>
        public int CoordY { get; set; }

        /// <summary>
        /// Gets or sets price.
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// Convert event area dto to event area view model.
        /// </summary>
        /// <param name="eventArea">Event area dto.</param>
        public static implicit operator EventAreaModel(EventAreaDto eventArea)
        {
            return new EventAreaModel
            {
                Id = eventArea.Id,
                EventId = eventArea.EventId,
                Description = eventArea.Description,
                CoordX = eventArea.CoordX,
                CoordY = eventArea.CoordY,
                Price = eventArea.Price,
            };
        }

        /// <summary>
        /// Convert event area view model to event area dto.
        /// </summary>
        /// <param name="eventAreaVm">Event area view model.</param>
        public static implicit operator EventAreaDto(EventAreaModel eventAreaVm)
        {
            return new EventAreaDto
            {
                Id = eventAreaVm.Id,
                EventId = eventAreaVm.EventId,
                Description = eventAreaVm.Description,
                CoordX = eventAreaVm.CoordX,
                CoordY = eventAreaVm.CoordY,
                Price = eventAreaVm.Price,
            };
        }
    }
}
