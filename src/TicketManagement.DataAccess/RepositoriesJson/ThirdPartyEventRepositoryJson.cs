using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Models;

namespace TicketManagement.DataAccess.RepositoriesJson
{
    /// <summary>
    /// Repository for third party events with only read operations.
    /// </summary>
    internal class ThirdPartyEventRepositoryJson : IRepositoryJson<ThirdPartyEvent>
    {
        /// <summary>
        /// Object for lock.
        /// </summary>
        private readonly object _locker = new object();

        public async Task<IQueryable<ThirdPartyEvent>> GetAllAsync(string path)
        {
            return await Task.Run(() =>
            {
                var json = File.ReadAllText(path);
                if (string.IsNullOrWhiteSpace(json))
                {
                    string baseJson = "{\"events\": {}}";
                    lock (_locker)
                    {
                        File.WriteAllText(path, baseJson);
                    }

                    json = File.ReadAllText(path);
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

        /// <summary>
        /// Get third party event from json file by his id.
        /// </summary>
        /// <param name="id">Id of third party event.</param>
        /// <param name="path">Path to json file.</param>
        /// <returns>Third party event.</returns>
        public async Task<ThirdPartyEvent> GetByIdAsync(int id, string path)
        {
            return await Task.Run(() =>
            {
                var json = File.ReadAllText(path);
                var jsonObject = JObject.Parse(json);
                var eventsArray = jsonObject["events"] as JArray;
                var jsonEvent = eventsArray.SingleOrDefault(e => e["Id"].Value<int>() == id);
                var @event = jsonEvent.ToObject<ThirdPartyEvent>();
                return @event;
            });
        }
    }
}
