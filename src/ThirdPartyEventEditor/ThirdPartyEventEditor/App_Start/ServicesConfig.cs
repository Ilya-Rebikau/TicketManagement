using Ninject.Modules;
using ThirdPartyEventEditor.Interfaces;
using ThirdPartyEventEditor.Models;
using ThirdPartyEventEditor.Repositories;

namespace ThirdPartyEventEditor.App_Start
{
    /// <summary>
    /// Registrating services using DI container.
    /// </summary>
    public class ServicesConfig : NinjectModule
    {
        public override void Load()
        {
            Bind<IFilesConfig>().To<FilesConfig>();
            Bind<IRepository<ThirdPartyEvent>>().To<EventRepository>();
        }
    }
}