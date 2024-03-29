﻿using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ThirdPartyEventEditor.Interfaces;
using ThirdPartyEventEditor.Models;

namespace ThirdPartyEventEditor.Repositories
{
    /// <summary>
    /// Repository for third party events.
    /// </summary>
    internal class EventRepository : IRepository<ThirdPartyEvent>
    {
        /// <summary>
        /// IFilesConfig object.
        /// </summary>
        private readonly IFilesConfig _filesConfig;

        /// <summary>
        /// Object for lock.
        /// </summary>
        private readonly object _locker = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="EventRepository"/> class.
        /// </summary>
        /// <param name="filesConfig">IFilesConfig object.</param>
        public EventRepository(IFilesConfig filesConfig)
        {
            _filesConfig = filesConfig;
        }

        public Task<ThirdPartyEvent> CreateAsync(ThirdPartyEvent obj)
        {
            var json = File.ReadAllText(_filesConfig.FullPathToJsonFile);
            var events = JsonConvert.DeserializeObject<List<ThirdPartyEvent>>(json);
            if (events is null || events.Count == 0)
            {
                obj.Id = 1;
                events = new List<ThirdPartyEvent>();
            }
            else
            {
                obj.Id = events.Last().Id + 1;
            }

            events.Add(obj);
            var jsonEvents = JsonConvert.SerializeObject(events, Formatting.Indented);
            lock (_locker)
            {
                File.WriteAllText(_filesConfig.FullPathToJsonFile, jsonEvents);
            }

            return Task.FromResult(obj);
        }

        public Task<ThirdPartyEvent> DeleteAsync(ThirdPartyEvent obj)
        {
            var json = File.ReadAllText(_filesConfig.FullPathToJsonFile);
            var events = JsonConvert.DeserializeObject<List<ThirdPartyEvent>>(json);
            var deletingEvent = events.SingleOrDefault(e => e.Id == obj.Id);
            events.Remove(deletingEvent);
            string output = JsonConvert.SerializeObject(events, Formatting.Indented);
            lock (_locker)
            {
                File.WriteAllText(_filesConfig.FullPathToJsonFile, output);
            }

            return Task.FromResult(obj);
        }

        public Task<IQueryable<ThirdPartyEvent>> GetAllAsync()
        {
            var events = new List<ThirdPartyEvent>();
            var json = File.ReadAllText(_filesConfig.FullPathToJsonFile);
            if (string.IsNullOrWhiteSpace(json))
            {
                return Task.FromResult(events.AsQueryable());
            }
            events = JsonConvert.DeserializeObject<List<ThirdPartyEvent>>(json);

            return Task.FromResult(events.AsQueryable());
        }

        public async Task<ThirdPartyEvent> GetByIdAsync(int id)
        {
            var events = await GetAllAsync();
            var @event = events.SingleOrDefault(e => e.Id == id);
            return @event;
        }

        public async Task<ThirdPartyEvent> UpdateAsync(ThirdPartyEvent obj)
        {
            var events = await GetAllAsync();
            var @event = events.SingleOrDefault(e => e.Id == obj.Id);
            @event.LayoutId = obj.LayoutId;
            @event.Name = obj.Name;
            @event.PosterImage = obj.PosterImage;
            @event.StartDate = obj.StartDate;
            @event.EndDate = obj.EndDate;
            @event.Description = obj.Description;
            string output = JsonConvert.SerializeObject(events, Formatting.Indented);
            lock (_locker)
            {
                File.WriteAllText(_filesConfig.FullPathToJsonFile, output);
            }

            return obj;
        }
    }
}