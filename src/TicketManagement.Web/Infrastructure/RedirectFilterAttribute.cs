using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FeatureManagement;

namespace TicketManagement.Web.Infrastructure
{
    /// <summary>
    /// Filter for redirect to another page.
    /// </summary>
    public class RedirectFilterAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Redirect feature key.
        /// </summary>
        private const string RedirectFeature = "Redirect";

        /// <summary>
        /// Address in redirected app.
        /// </summary>
        private readonly string _address;

        /// <summary>
        /// Initializes a new instance of the <see cref="RedirectFilterAttribute"/> class.
        /// </summary>
        /// <param name="address">Address in redirected app.</param>
        public RedirectFilterAttribute(string address)
        {
            _address = address;
        }

        public override async void OnActionExecuting(ActionExecutingContext context)
        {
            var featureManager = context.HttpContext.RequestServices.GetService<IFeatureManager>();
            var config = context.HttpContext.RequestServices.GetService<IConfiguration>();
            if (await featureManager.IsEnabledAsync(RedirectFeature))
            {
                context.Result = new RedirectResult(config["ReactAppAddress"] + _address);
            }
        }
    }
}
