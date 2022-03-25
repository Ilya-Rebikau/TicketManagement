using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using TicketManagement.Web.ModelsDTO;

namespace TicketManagement.Web.Models.Events
{
    /// <summary>
    /// Event view model.
    /// </summary>
    public class EventViewModel
    {
        /// <summary>
        /// Gets or sets id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets name.
        /// </summary>
        [Required(ErrorMessage = "FieldRequired")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets description.
        /// </summary>
        [Required(ErrorMessage = "FieldRequired")]
        [Display(Name = "Description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets layout's id.
        /// </summary>
        [Required(ErrorMessage = "FieldRequired")]
        [Display(Name = "LayoutId")]
        public int LayoutId { get; set; }

        /// <summary>
        /// Gets or sets time when event starts.
        /// </summary>
        [Required(ErrorMessage = "FieldRequired")]
        [Display(Name = "TimeStart")]
        public DateTime TimeStart { get; set; }

        /// <summary>
        /// Gets or sets time when event ends.
        /// </summary>
        [Required(ErrorMessage = "FieldRequired")]
        [Display(Name = "TimeEnd")]
        public DateTime TimeEnd { get; set; }

        /// <summary>
        /// Gets or sets image URL.
        /// </summary>
        [Url(ErrorMessage = "WrongUrl")]
        [Display(Name = "ImageUrl")]
        public string ImageUrl { get; set; }

        /// <summary>
        /// Gets or sets state for saving third party event in database.
        /// </summary>
        public bool Checked { get; set; }

        /// <summary>
        /// Gets or sets list of EventAreaViewModel objects.
        /// EventAreaViewModel objects represents event areas.
        /// </summary>
        public IList<EventAreaViewModelInEvent> EventAreas { get; set; }

        /// <summary>
        /// Gets max X coordinate from event areas in event.
        /// If there are no event seats in event area, then will return 0.
        /// </summary>
        public int MaxXCoord
        {
            get
            {
                return EventAreas.Count > 0 && EventAreas is not null ? EventAreas.Max(x => x.EventArea.CoordX) : 0;
            }
        }

        /// <summary>
        /// Gets max Y coordinate from event areas in event.
        /// If there are no event seats in event area, then will return 0.
        /// </summary>
        public int MaxYCoord
        {
            get
            {
                return EventAreas.Count > 0 && EventAreas is not null ? EventAreas.Max(x => x.EventArea.CoordY) : 0;
            }
        }

        /// <summary>
        /// Sorting event areas by X and Y coordinates.
        /// </summary>
        /// <returns>Sorted event areas.</returns>
        public List<EventAreaViewModelInEvent> SortedEventAreas() => EventAreas.OrderBy(x => x.EventArea.CoordX).ThenBy(y => y.EventArea.CoordY).ToList();

        /// <summary>
        /// Check that event area with X and Y coordinate exist.
        /// </summary>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        /// <returns>True if exists and false if does not.</returns>
        public bool CheckAreaForExist(int x, int y)
        {
            var area = EventAreas.FirstOrDefault(e => e.EventArea.CoordX == x && e.EventArea.CoordY == y);
            if (area is not null && area.EventSeats.Count > 0)
            {
                return true;
            }

            return false;
        }
    }
}