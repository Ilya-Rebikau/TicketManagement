using System;
using ThirdPartyEventEditor.Interfaces;
using ThirdPartyEventEditor.Models;

namespace ThirdPartyEventEditor.Services
{
    /// <summary>
    /// Service for events.
    /// </summary>
    internal class EventService : BaseService<ThirdPartyEvent>, IService<ThirdPartyEvent>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventService"/> class.
        /// </summary>
        /// <param name="repository"></param>
        public EventService(IRepository<ThirdPartyEvent> repository)
            : base(repository)
        {
            Repository = repository;
        }

        public override ThirdPartyEvent Create(ThirdPartyEvent obj)
        {
            ConvertTimeToUtc(obj);
            CheckEventForPastTime(obj);
            CheckForTimeBorders(obj);
            return base.Create(obj);
        }

        public override ThirdPartyEvent Update(ThirdPartyEvent obj)
        {
            ConvertTimeToUtc(obj);
            CheckEventForPastTime(obj);
            CheckForTimeBorders(obj);
            return base.Update(obj);
        }

        /// <summary>
        /// Convert time to UTC.
        /// </summary>
        /// <param name="obj">Creating or updating event.</param>
        private static void ConvertTimeToUtc(ThirdPartyEvent obj)
        {
            obj.StartDate = TimeZoneInfo.ConvertTimeToUtc(obj.StartDate);
            obj.EndDate = TimeZoneInfo.ConvertTimeToUtc(obj.EndDate);
        }

        /// <summary>
        /// Checking that event's time of end and time of start is not in past.
        /// </summary>
        /// <param name="obj">Adding or updating event.</param>
        /// <exception cref="ArgumentException">Generates exception in case StartDate or EndDate in past time.</exception>
        private void CheckEventForPastTime(ThirdPartyEvent obj)
        {
            if (obj.StartDate <= DateTime.Now || obj.EndDate <= DateTime.Now)
            {
                throw new ArgumentException("You can't create event in past!");
            }
        }

        /// <summary>
        /// Checking that event's time of end after time of start.
        /// </summary>
        /// <param name="obj">Adding or updating event.</param>
        /// <exception cref="ArgumentException">Generates exception in case StartDate after EndDate.</exception>
        private void CheckForTimeBorders(ThirdPartyEvent obj)
        {
            if (obj.StartDate >= obj.EndDate)
            {
                throw new ArgumentException("Time of start event can't be after event's time of end");
            }
        }
    }
}