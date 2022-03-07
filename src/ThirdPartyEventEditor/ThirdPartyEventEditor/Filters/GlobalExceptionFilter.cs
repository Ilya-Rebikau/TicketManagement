using Ninject;
using System.IO;
using System.Web.Mvc;
using ThirdPartyEventEditor.Interfaces;

namespace ThirdPartyEventEditor.Filters
{
    /// <summary>
    /// IFilesConfig object
    /// </summary>
    public class GlobalExceptionFilter : FilterAttribute, IExceptionFilter
    {
        [Inject]
        public IFilesConfig FilesConfig { get; set; }

        public void OnException(ExceptionContext filterContext)
        {
            if (!filterContext.ExceptionHandled)
            {
                filterContext.Result = new ViewResult
                {
                    ViewName = "Error",
                    ViewData = new ViewDataDictionary<string>(filterContext.Exception.Message),
                };

                using (StreamWriter writer = new StreamWriter(FilesConfig.FullPathToLogsFile, true))
                {
                    writer.WriteLine(filterContext.Exception.Message);
                    writer.WriteLine(filterContext.Exception.StackTrace);
                    writer.WriteLine($"Method with exception: {filterContext.Exception.TargetSite}");
                    writer.WriteLine($"Source: {filterContext.Exception.Source}");
                    writer.WriteLine("______________________________________________________________________________________");
                }

                filterContext.ExceptionHandled = true;
            }
        }
    }
}