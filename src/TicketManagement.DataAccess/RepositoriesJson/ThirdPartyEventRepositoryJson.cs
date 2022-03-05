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
    internal class ThirdPartyEventRepositoryJson : IReaderJson<ThirdPartyEvent>
    {
        public async Task<IQueryable<ThirdPartyEvent>> GetAllAsync(string json)
        {
            return await Task.Run(() =>
            {
                if (string.IsNullOrWhiteSpace(json))
                {
                    json = "{\"events\": {}}";
                }

                var jsonObj = JObject.Parse(json);
                var events = new List<ThirdPartyEvent>();
                if (jsonObj.GetValue("events") is JArray eventsArray)
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

        public async Task<ThirdPartyEvent> GetByIdAsync(int id, string json)
        {
            return await Task.Run(() =>
            {
                var jsonObject = JObject.Parse(json);
                var eventsArray = jsonObject["events"] as JArray;
                var jsonEvent = eventsArray.SingleOrDefault(e => e["Id"].Value<int>() == id);
                var @event = jsonEvent.ToObject<ThirdPartyEvent>();
                return @event;
            });
        }
    }
}
