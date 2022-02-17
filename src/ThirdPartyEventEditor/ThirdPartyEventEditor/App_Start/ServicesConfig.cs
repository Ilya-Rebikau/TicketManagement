using Ninject.Modules;
using ThirdPartyEventEditor.Interfaces;

namespace ThirdPartyEventEditor.App_Start
{
    public class ServicesConfig : NinjectModule
    {
        public override void Load()
        {
            Bind<IJsonConfig>().To<JsonConfig>();
        }
    }
}