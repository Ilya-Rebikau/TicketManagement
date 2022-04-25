using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TicketManagement.PurchaseFlowAPI.Infrastructure
{
    /// <summary>
    /// Exception filter attribute.
    /// </summary>
    public class ExceptionFilterAttribute : Attribute, IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            string exceptionMessage = context.Exception.Message;
            if (context.Exception is ValidationException)
            {
                context.Result = new BadRequestObjectResult(context.Exception)
                {
                    Value = exceptionMessage,
                };
            }
            else
            {
                context.Result = new StatusCodeResult(500);
            }

            context.ExceptionHandled = true;
        }
    }
}
