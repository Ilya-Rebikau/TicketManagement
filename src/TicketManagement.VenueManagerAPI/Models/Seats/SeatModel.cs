using System.ComponentModel.DataAnnotations;
using TicketManagement.VenueManagerAPI.ModelsDTO;

namespace TicketManagement.VenueManagerAPI.Models.Seats
{
    /// <summary>
    /// Seat model.
    /// </summary>
    public class SeatModel
    {
        /// <summary>
        /// Gets or sets id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets id of area.
        /// </summary>
        [Required(ErrorMessage = "FieldRequired")]
        [Display(Name = "AreaId")]
        public int AreaId { get; set; }

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
        /// Convert seat dto to seat view model.
        /// </summary>
        /// <param name="seat">Seat dto.</param>
        public static implicit operator SeatModel(SeatDto seat)
        {
            return new SeatModel
            {
                Id = seat.Id,
                AreaId = seat.AreaId,
                Row = seat.Row,
                Number = seat.Number,
            };
        }

        /// <summary>
        /// Convert seat view model to seat dto.
        /// </summary>
        /// <param name="seatVm">Seat view model.</param>
        public static implicit operator SeatDto(SeatModel seatVm)
        {
            return new SeatDto
            {
                Id = seatVm.Id,
                AreaId = seatVm.AreaId,
                Row = seatVm.Row,
                Number = seatVm.Number,
            };
        }
    }
}
