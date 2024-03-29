﻿using System.ComponentModel.DataAnnotations;
using TicketManagement.Web.ModelsDTO;

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
        [Range(1, int.MaxValue)]
        public int EventAreaId { get; set; }

        /// <summary>
        /// Gets or sets row number in area.
        /// </summary>
        [Required(ErrorMessage = "FieldRequired")]
        [Display(Name = "Row")]
        [Range(0, int.MaxValue)]
        public int Row { get; set; }

        /// <summary>
        /// Gets or sets number in row.
        /// </summary>
        [Required(ErrorMessage = "FieldRequired")]
        [Display(Name = "Number")]
        [Range(0, int.MaxValue)]
        public int Number { get; set; }

        /// <summary>
        /// Gets or sets state.
        /// </summary>
        [Required(ErrorMessage = "FieldRequired")]
        [Display(Name = "State")]
        public PlaceStatus State { get; set; }
    }
}
