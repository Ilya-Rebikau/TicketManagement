using Ninject.Modules;
using System.Web.Mvc;
using ThirdPartyEventEditor.Interfaces;
using ThirdPartyEventEditor.Models;
using ThirdPartyEventEditor.Repositories;
using ThirdPartyEventEditor.Services;

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
            Bind<IService<ThirdPartyEvent>>().To<EventService>();
            Bind<IEventWebService>().To<EventWebService>();
            Unbind<ModelValidatorProvider>();
        }
    }
}