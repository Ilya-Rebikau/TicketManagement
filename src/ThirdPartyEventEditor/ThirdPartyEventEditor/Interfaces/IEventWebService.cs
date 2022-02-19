using System.Threading.Tasks;
using ThirdPartyEventEditor.Models;

namespace ThirdPartyEventEditor.Interfaces
{
    /// <summary>
    /// Web service for events.
    /// </summary>
    public interface IEventWebService : IService<ThirdPartyEvent>
    {
        Task<string> UploadSampleImage(string imageFileName);
    }
}
