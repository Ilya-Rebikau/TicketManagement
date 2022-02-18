using Ninject;
using Ninject.Modules;
using Ninject.Web.Mvc;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ThirdPartyEventEditor.App_Start;

namespace ThirdPartyEventEditor
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            NinjectModule services = new ServicesConfig();
            var kernel = new StandardKernel(services);
            DependencyResolver.SetResolver(new NinjectDependencyResolver(kernel));
        }
    }
}