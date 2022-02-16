using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using TicketManagement.Web.Infrastructure;

namespace TicketManagement.Web.Controllers
{
    /// <summary>
    /// Home controller.
    /// </summary>
    [ResponseCache(CacheProfileName = "Caching")]
    [ExceptionFilter]
    public class HomeController : Controller
    {
        /// <summary>
        /// Change language for app.
        /// </summary>
        /// <param name="culture">New language.</param>
        /// <param name="returnUrl">Url for returning.</param>
        /// <returns>IActionResult.</returns>
        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddYears(1),
                });

            return LocalRedirect(returnUrl);
        }
    }
}
