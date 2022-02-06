using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using TicketManagement.Web.Models;

namespace TicketManagement.Web.Infrastructure
{
    /// <summary>
    /// Database initializer.
    /// </summary>
    public static class DbInitializer
    {
        /// <summary>
        /// Initialize database with admin account and base roles.
        /// </summary>
        /// <param name="context">HttpContext object.</param>
        /// <returns>Task.</returns>
        public static async Task Initialize(HttpContext context)
        {
            UserManager<User> userManager = context.RequestServices.GetRequiredService<UserManager<User>>();
            RoleManager<IdentityRole> roleManager = context.RequestServices.GetRequiredService<RoleManager<IdentityRole>>();
            string adminName = "admin@mail.ru";
            string password = "Qwer!1";
            if (await roleManager.FindByNameAsync("admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("admin"));
            }

            if (await roleManager.FindByNameAsync("user") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("user"));
            }

            if (await roleManager.FindByNameAsync("event manager") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("event manager"));
            }

            if (await roleManager.FindByNameAsync("venue manager") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("venue manager"));
            }

            if (await userManager.FindByNameAsync(adminName) == null)
            {
                User admin = new ()
                {
                    UserName = adminName,
                    Email = adminName,
                };
                IdentityResult result = await userManager.CreateAsync(admin, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "admin");
                }
            }
        }
    }
}
