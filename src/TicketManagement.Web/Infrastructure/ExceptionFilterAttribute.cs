using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TicketManagement.Web.Infrastructure
{
    /// <summary>
    /// Exception filter attribute.
    /// </summary>
    public class ExceptionFilterAttribute : Attribute, IExceptionFilter
    {
        /// <summary>
        /// Action when you get exception.
        /// </summary>
        /// <param name="context">ExceptionContext object.</param>
        public void OnException(ExceptionContext context)
        {
            string exceptionMessage = context.Exception.Message;
            context.Result = new ContentResult
            {
                Content = exceptionMessage,
            };
            context.ExceptionHandled = true;
        }
    }
}
