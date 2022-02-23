using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
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
        private readonly object locker = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="EventRepository"/> class.
        /// </summary>
        /// <param name="filesConfig">IFilesConfig object.</param>
        public EventRepository(IFilesConfig filesConfig)
        {
            _filesConfig = filesConfig;
        }

        public async Task<ThirdPartyEvent> CreateAsync(ThirdPartyEvent obj)
        {
            return await Task.Run(() =>
            {
                var json = File.ReadAllText(_filesConfig.FullPathToJsonFile);
                if (string.IsNullOrWhiteSpace(json))
                {
                    string baseJson = "{\"events\": {}}";
                    lock (locker)
                    {
                        File.WriteAllText(_filesConfig.FullPathToJsonFile, baseJson);
                    }

                    json = File.ReadAllText(_filesConfig.FullPathToJsonFile);
                }

                var jsonObj = JObject.Parse(json);
                if (!(jsonObj.GetValue("events") is JArray eventsArray))
                {
                    eventsArray = new JArray();
                }

                obj.Id = eventsArray.Count + 1;
                var newEventJson = JsonConvert.SerializeObject(obj);
                var newEvent = JObject.Parse(newEventJson);
                eventsArray.Add(newEvent);
                jsonObj["events"] = eventsArray;
                string newJsonResult = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
                lock (locker)
                {
                    File.WriteAllText(_filesConfig.FullPathToJsonFile, newJsonResult);
                }

                return obj;
            });
        }

        public async Task<ThirdPartyEvent> DeleteAsync(ThirdPartyEvent obj)
        {
            return await Task.Run(() =>
            {
                var json = File.ReadAllText(_filesConfig.FullPathToJsonFile);
                try
                {
                    var jObject = JObject.Parse(json);
                    var eventsArray = jObject["events"] as JArray;
                    var deletingEvent = eventsArray.SingleOrDefault(e => e["Id"].Value<int>() == obj.Id);
                    eventsArray.Remove(deletingEvent);
                    string output = JsonConvert.SerializeObject(jObject, Formatting.Indented);
                    lock (locker)
                    {
                        File.WriteAllText(_filesConfig.FullPathToJsonFile, output);
                    }

                    return obj;
                }
                catch (Exception)
                {
                    throw new NullReferenceException("No such event in file!");
                }
            });
        }

        public async Task<IQueryable<ThirdPartyEvent>> GetAllAsync()
        {
            return await Task.Run(() =>
            {
                var json = File.ReadAllText(_filesConfig.FullPathToJsonFile);
                if (string.IsNullOrWhiteSpace(json))
                {
                    string baseJson = "{\"events\": {}}";
                    lock (locker)
                    {
                        File.WriteAllText(_filesConfig.FullPathToJsonFile, baseJson);
                    }

                    json = File.ReadAllText(_filesConfig.FullPathToJsonFile);
                }

                var jsonObj = JObject.Parse(json);
                var eventsArray = jsonObj.GetValue("events") as JArray;
                var events = new List<ThirdPartyEvent>();
                if (eventsArray != null)
                {
                    foreach (var jsonObject in eventsArray)
                    {
                        events.Add(new ThirdPartyEvent
                        {
                            Id = (int)jsonObject["Id"],
                            Name = jsonObject["Name"].ToString(),
                            Description = jsonObject["Description"].ToString(),
                            StartDate = (DateTime)jsonObject["StartDate"],
                            EndDate = (DateTime)jsonObject["EndDate"],
                            PosterImage = jsonObject["PosterImage"].ToString(),
                            LayoutId = (int)jsonObject["LayoutId"],
                        });
                    }
                }

                return events.AsQueryable();
            });
        }

        public async Task<ThirdPartyEvent> GetByIdAsync(int id)
        {
            return await Task.Run(() =>
            {
                var json = File.ReadAllText(_filesConfig.FullPathToJsonFile);
                try
                {
                    var jObject = JObject.Parse(json);
                    var eventsArray = jObject["events"] as JArray;
                    var jEvent = eventsArray.SingleOrDefault(e => e["Id"].Value<int>() == id);
                    var @event = jEvent.ToObject<ThirdPartyEvent>();
                    return @event;
                }
                catch
                {
                    throw new NullReferenceException("No such event in file.");
                }
            });
        }

        public async Task<ThirdPartyEvent> UpdateAsync(ThirdPartyEvent obj)
        {
            return await Task.Run(() =>
            {
                string json = File.ReadAllText(_filesConfig.FullPathToJsonFile);
                try
                {
                    var jObject = JObject.Parse(json);
                    var eventsArray = jObject["events"] as JArray;
                    foreach (var @event in eventsArray.Where(e => e["Id"].Value<int>() == obj.Id))
                    {
                        @event["Name"] = !string.IsNullOrWhiteSpace(obj.Name) ? obj.Name : "";
                        @event["Description"] = !string.IsNullOrWhiteSpace(obj.Description) ? obj.Description : "";
                        @event["StartDate"] = obj.StartDate;
                        @event["EndDate"] = obj.EndDate;
                        @event["PosterImage"] = !string.IsNullOrWhiteSpace(obj.PosterImage) ? obj.PosterImage : "";
                        @event["LayoutId"] = obj.LayoutId;
                    }

                    jObject["events"] = eventsArray;
                    string output = JsonConvert.SerializeObject(jObject, Formatting.Indented);
                    lock (locker)
                    {
                        File.WriteAllText(_filesConfig.FullPathToJsonFile, output);
                    }

                    return obj;
                }
                catch
                {
                    throw new NullReferenceException("No such event in file.");
                }
            });
        }
    }
}