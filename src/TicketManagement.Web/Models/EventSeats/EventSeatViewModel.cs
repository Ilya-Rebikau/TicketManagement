using System.ComponentModel.DataAnnotations;
using TicketManagement.BusinessLogic.ModelsDTO;

namespace TicketManagement.Web.Models.EventSeats
{
    /// <summary>
    /// Event seat view model.
    /// </summary>
    public class EventSeatViewModel
    {
        /// <summary>
        /// Gets or sets id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets id of event area.
        /// </summary>
        [Required(ErrorMessage = "FieldRequired")]
        [Display(Name = "EventAreaId")]
        public int EventAreaId { get; set; }

        /// <summary>
        /// Gets or sets row number in area.
        /// </summary>
        [Required(ErrorMessage = "FieldRequired")]
        [Display(Name = "Row")]
        public int Row { get; set; }

        /// <summary>
        /// Gets or sets number in row.
        /// </summary>
        [Required(ErrorMessage = "FieldRequired")]
        [Display(Name = "Number")]
        public int Number { get; set; }

        /// <summary>
        /// Gets or sets state.
        /// </summary>
        [Required(ErrorMessage = "FieldRequired")]
        [Display(Name = "State")]
        public PlaceStatus State { get; set; }

        /// <summary>
        /// Convert event seat dto to event seat view model.
        /// </summary>
        /// <param name="eventSeat">Event seat dto.</param>
        public static implicit operator EventSeatViewModel(EventSeatDto eventSeat)
        {
            return new EventSeatViewModel
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
        public static implicit operator EventSeatDto(EventSeatViewModel eventSeatVm)
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
