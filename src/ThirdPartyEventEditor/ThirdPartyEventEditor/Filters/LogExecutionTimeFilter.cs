using Ninject;
using System.Diagnostics;
using System.IO;
using System.Web.Mvc;
using ThirdPartyEventEditor.Interfaces;

namespace ThirdPartyEventEditor.Filters
{
    /// <summary>
    /// Filter to log execution time of actions.
    /// </summary>
    public class LogExecutionTimeFilter : ActionFilterAttribute
    {
        /// <summary>
        /// IFilesConfig object
        /// </summary>
        [Inject]
        public IFilesConfig FilesConfig { get; set; }

        /// <summary>
        /// Stopwatch object.
        /// </summary>
        private Stopwatch _stopWatch;

        /// <summary>
        /// Name of controller.
        /// </summary>
        private string _controllerName;

        /// <summary>
        /// Name of action in controller.
        /// </summary>
        private string _actionName;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _stopWatch = new Stopwatch();
            _stopWatch.Start();
            _controllerName = nameof(filterContext.Controller);
            _actionName = filterContext.ActionDescriptor.ActionName;
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            _stopWatch.Stop();
            using (StreamWriter writer = new StreamWriter(FilesConfig.FullPathToLogsFile, true))
            {
                writer.WriteLine($"Action {_actionName} in controller {_controllerName} executed for: {_stopWatch.ElapsedMilliseconds} milliseconds");
                writer.WriteLine("______________________________________________________________________________________");
            }
        }
    }
}