using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using TicketManagement.Web.Infrastructure;

namespace TicketManagement.Web.Middlewares
{
    /// <summary>
    /// Middleware for db initializer.
    /// </summary>
    public class DbInitializerMiddleware
    {
        /// <summary>
        /// RequestDelegate object.
        /// </summary>
        private readonly RequestDelegate _next;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbInitializerMiddleware"/> class.
        /// </summary>
        /// <param name="next">Previous RequestDelegate object.</param>
        public DbInitializerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Invoke middleware and go to the next middleware.
        /// </summary>
        /// <param name="context">HttpContext object.</param>
        /// <returns>Task.</returns>
        public Task Invoke(HttpContext context)
        {
            DbInitializer.Initialize(context).Wait();
            return _next.Invoke(context);
        }
    }
}
