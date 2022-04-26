using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Models;

namespace TicketManagement.DataAccess.RepositoriesJson
{
    /// <summary>
    /// Repository for third party events with only read operations.
    /// </summary>
    internal class ThirdPartyEventRepositoryJson : IReaderJson<ThirdPartyEvent>
    {
        public IQueryable<ThirdPartyEvent> GetAll(string json)
        {
            var events = new List<ThirdPartyEvent>();
            if (string.IsNullOrWhiteSpace(json))
            {
                return events.AsQueryable();
            }

            events = JsonConvert.DeserializeObject<List<ThirdPartyEvent>>(json);

            return events.AsQueryable();
        }

        public ThirdPartyEvent GetById(int id, string json)
        {
            var events = GetAll(json);
            var @event = events.SingleOrDefault(e => e.Id == id);
            return @event;
        }
    }
}
