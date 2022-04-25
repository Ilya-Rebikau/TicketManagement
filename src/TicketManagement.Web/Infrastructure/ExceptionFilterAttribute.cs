using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using TicketManagement.Web.Models;

namespace TicketManagement.Web.Infrastructure
{
    /// <summary>
    /// Exception filter attribute.
    /// </summary>
    public class ExceptionFilterAttribute : Attribute, IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var exceptionType = context.Exception.GetType();
            var content = exceptionType.GetProperty("Content");
            string exceptionMessage = content.GetValue(context.Exception).ToString();
            var viewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), context.ModelState)
            {
                Model = new ErrorViewModel { Message = exceptionMessage },
            };
            context.Result = new ViewResult
            {
                ViewName = "Error",
                ViewData = viewData,
            };

            context.ExceptionHandled = true;
        }
    }
}
