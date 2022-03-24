using TicketManagement.EventManagerAPI.ModelsDTO;

namespace TicketManagement.EventManagerAPI.Models.EventSeats
{
    /// <summary>
    /// Event seat model.
    /// </summary>
    public class EventSeatModel
    {
        /// <summary>
        /// Gets or sets id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets id of event area.
        /// </summary>
        public int EventAreaId { get; set; }

        /// <summary>
        /// Gets or sets row number in area.
        /// </summary>
        public int Row { get; set; }

        /// <summary>
        /// Gets or sets number in row.
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// Gets or sets state.
        /// </summary>
        public PlaceStatus State { get; set; }

        /// <summary>
        /// Convert event seat dto to event seat view model.
        /// </summary>
        /// <param name="eventSeat">Event seat dto.</param>
        public static implicit operator EventSeatModel(EventSeatDto eventSeat)
        {
            return new EventSeatModel
            {
                Id = eventSeat.Id,
                EventAreaId = eventSeat.EventAreaId,
                Row = eventSeat.Row,
                Number = eventSeat.Number,
                State = eventSeat.State,
            };
        }

        /// <summary>
        /// Convert event seat view model to event seat dto.
        /// </summary>
        /// <param name="eventSeatVm">Event seat view model.</param>
        public static implicit operator EventSeatDto(EventSeatModel eventSeatVm)
        {
            return new EventSeatDto
            {
                Id = eventSeatVm.Id,
                EventAreaId = eventSeatVm.EventAreaId,
                Row = eventSeatVm.Row,
                Number = eventSeatVm.Number,
                State = eventSeatVm.State,
            };
        }
    }
}
