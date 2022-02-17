using System.Configuration;
using ThirdPartyEventEditor.Interfaces;

namespace ThirdPartyEventEditor.App_Start
{
    internal class JsonConfig : IJsonConfig
    {
        public string FileName => ConfigurationManager.AppSettings["JsonFileName"];
    }
}