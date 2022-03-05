using System;
using System.Linq;
using System.Threading.Tasks;
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

        public override async Task<ThirdPartyEvent> CreateAsync(ThirdPartyEvent obj)
        {
            ConvertTimeToUtc(obj);
            CheckEventForPastTime(obj);
            CheckForTimeBorders(obj);
            await CheckForSameLayoutInOneTime(obj);
            return await base.CreateAsync(obj);
        }

        public override async Task<ThirdPartyEvent> UpdateAsync(ThirdPartyEvent obj)
        {
            ConvertTimeToUtc(obj);
            CheckEventForPastTime(obj);
            CheckForTimeBorders(obj);
            await CheckForSameLayoutInOneTime(obj);
            return await base.UpdateAsync(obj);
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

        /// <summary>
        /// Checking that event can't be created in one time in one layout.
        /// </summary>
        /// <param name="obj">Adding or updating event.</param>
        /// <exception cref="ArgumentException">Generates exception in case event in this layout and time already exists.</exception>
        private async Task CheckForSameLayoutInOneTime(ThirdPartyEvent obj)
        {
            var events = await Repository.GetAllAsync();
            var eventsInLayout = events.Where(ev => ev.LayoutId == obj.LayoutId && obj.StartDate <= ev.StartDate && obj.EndDate >= ev.EndDate && ev.Id != obj.Id);
            if (eventsInLayout.Any())
            {
                throw new ArgumentException("You can't create event in one time in one layout!");
            }
        }
    }
}