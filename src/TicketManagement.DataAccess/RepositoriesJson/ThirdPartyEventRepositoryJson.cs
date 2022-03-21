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
        public Task<IQueryable<ThirdPartyEvent>> GetAllAsync(string json)
        {
            var events = new List<ThirdPartyEvent>();
            if (string.IsNullOrWhiteSpace(json))
            {
                return Task.FromResult(events.AsQueryable());
            }

            events = JsonConvert.DeserializeObject<List<ThirdPartyEvent>>(json);

            return Task.FromResult(events.AsQueryable());
        }

        public async Task<ThirdPartyEvent> GetByIdAsync(int id, string json)
        {
            var events = await GetAllAsync(json);
            var @event = events.SingleOrDefault(e => e.Id == id);
            return @event;
        }
    }
}
