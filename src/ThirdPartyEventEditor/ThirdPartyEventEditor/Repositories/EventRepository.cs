using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ThirdPartyEventEditor.Interfaces;
using ThirdPartyEventEditor.Models;

namespace ThirdPartyEventEditor.Repositories
{
    public class EventRepository : IRepository<ThirdPartyEvent>
    {
        private readonly IFilesConfig _filesConfig;

        public EventRepository(IFilesConfig filesConfig)
        {
            _filesConfig = filesConfig;
        }

        public ThirdPartyEvent Create(ThirdPartyEvent obj)
        {
            try  
            {  
                var json = File.ReadAllText(_filesConfig.FullPathToFile);
                if (string.IsNullOrWhiteSpace(json))
                {
                    string baseJson = "{\"events\": {}}";
                    File.WriteAllText(_filesConfig.FullPathToFile, baseJson);
                    json = File.ReadAllText(_filesConfig.FullPathToFile);
                }

                var jsonObj = JObject.Parse(json);
                var eventsArray = jsonObj.GetValue("events") as JArray;
                if (eventsArray is null)
                {
                    eventsArray = new JArray();
                }

                obj.Id = eventsArray.Count + 1;
                var newEventJson = JsonConvert.SerializeObject(obj);
                var newEvent = JObject.Parse(newEventJson);
                eventsArray.Add(newEvent);
                jsonObj["events"] = eventsArray;
                string newJsonResult = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
                File.WriteAllText(_filesConfig.FullPathToFile, newJsonResult);
                return obj;
            }
            catch
            {
                throw new ArgumentException("Wrong event data.");
            }
        }

        public ThirdPartyEvent Delete(ThirdPartyEvent obj)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ThirdPartyEvent> GetAll()
        {
            var json = File.ReadAllText(_filesConfig.FullPathToFile);
            try
            {
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
                        });
                    }
                }
                return events;
            }
            catch
            {
                throw new NullReferenceException("No events in file.");
            }
        }

        public ThirdPartyEvent GetById(int id)
        {
            throw new NotImplementedException();
        }

        public ThirdPartyEvent Update(ThirdPartyEvent obj)
        {
            throw new NotImplementedException();
        }

        private async Task<string> UploadSampleImage(string imageFileName)
        {
            var path = Path.Combine(_filesConfig.PathToDirectoryWithFile, imageFileName);
            using (var memoryStream = new MemoryStream())
            using (var fileStream = new FileStream(path, FileMode.Open))
            {
                await fileStream.CopyToAsync(memoryStream);
                return "data:image/png;base64," + Convert.ToBase64String(memoryStream.ToArray());
            }
        }
    }
}