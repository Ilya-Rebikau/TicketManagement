using System.IO;
using System.Web.Mvc;
using ThirdPartyEventEditor.App_Start;
using ThirdPartyEventEditor.Interfaces;

namespace ThirdPartyEventEditor.Filters
{
    /// <summary>
    /// Exception filter.
    /// </summary>
    public class GlobalExceptionFilter : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            if (!filterContext.ExceptionHandled)
            {
                filterContext.Result = new ViewResult
                {
                    ViewName = "Error",
                    ViewData = new ViewDataDictionary<string>(filterContext.Exception.Message),
                };

                IFilesConfig filesConfig = new FilesConfig();
                using (StreamWriter writer = new StreamWriter(filesConfig.FullPathToLogsFile, true))
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