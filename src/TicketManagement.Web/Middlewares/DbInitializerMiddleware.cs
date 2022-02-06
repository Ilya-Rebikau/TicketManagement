using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using TicketManagement.Web.Infrastructure;

namespace TicketManagement.Web.Middlewares
{
    public class DbInitializerMiddleware
    {
        private readonly RequestDelegate _next;

        public DbInitializerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext context)
        {
            DbInitializer.Initialize(context).Wait();
            return _next.Invoke(context);
        }
    }
}
