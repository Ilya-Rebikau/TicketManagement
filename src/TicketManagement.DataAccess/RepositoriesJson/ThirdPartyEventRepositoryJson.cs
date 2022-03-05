using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
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
        public Task<IQueryable<ThirdPartyEvent>> GetAll(string json)
        {
            var events = new List<ThirdPartyEvent>();
            if (string.IsNullOrWhiteSpace(json))
            {
                return Task.FromResult(events.AsQueryable());
            }

            events = JsonConvert.DeserializeObject<List<ThirdPartyEvent>>(json);

            return Task.FromResult(events.AsQueryable());
        }

        public Task<ThirdPartyEvent> GetById(int id, string json)
        {
            var @event = GetAll(json).Result.SingleOrDefault(e => e.Id == id);
            return Task.FromResult(@event);
        }
    }
}
