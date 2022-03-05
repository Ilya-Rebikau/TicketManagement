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
                    events = eventsArray.ToObject<List<ThirdPartyEvent>>();
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
