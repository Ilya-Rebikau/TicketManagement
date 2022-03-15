using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketManagement.UserAPI.Interfaces;
using TicketManagement.UserAPI.Models.Account;
using UserAPI.Services;

namespace TicketManagement.UserAPI.Controllers
{
    /// <summary>
    /// Controller for user account.
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        /// <summary>
        /// AccountWebService object.
        /// </summary>
        private readonly IAccountService _service;

        /// <summary>
        /// JwtTokenService object.
        /// </summary>
        private readonly JwtTokenService _jwtTokenService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController"/> class.
        /// </summary>
        /// <param name="service">AccountService object.</param>
        /// <param name="jwtTokenService">JwtTokenService object.</param>
        public AccountController(IAccountService service, JwtTokenService jwtTokenService)
        {
            _service = service;
            _jwtTokenService = jwtTokenService;
        }

        /// <summary>
        /// Register user.
        /// </summary>
        /// <param name="model">RegisterViewModel object.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            var registerResult = await _service.RegisterUser(model);
            if (registerResult.IdentityResult.Succeeded)
            {
                return Ok(_jwtTokenService.GetToken(registerResult.User, registerResult.Roles));
            }

            return BadRequest(registerResult.IdentityResult.Errors);
        }

        /// <summary>
        /// Login for user.
        /// </summary>
        /// <param name="model">LoginViewModel object.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            var loginResult = await _service.Login(model);
            if (loginResult.SignInResult.Succeeded)
            {
                return Ok(_jwtTokenService.GetToken(loginResult.User, loginResult.Roles));
            }

            return Forbid();
        }

        /// <summary>
        /// Logout for user.
        /// </summary>
        /// <returns>Task with IActionResult.</returns>
        [Authorize(Roles = "admin, user, event manager, venue manager")]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _service.Logout();
            return Ok();
        }

        /// <summary>
        /// Edit account.
        /// </summary>
        /// <param name="id">User id.</param>
        /// <returns>Task with IActionResult.</returns>
        [Authorize(Roles = "admin, user, event manager, venue manager")]
        [HttpGet("edit/{id}")]
        public async Task<IActionResult> Edit([FromRoute] string id)
        {
            return Ok(await _service.GetEditAccountViewModelForEdit(id));
        }

        /// <summary>
        /// Edit user.
        /// </summary>
        /// <param name="model">EditAccountViewModel object.</param>
        /// <returns>Task with IActionResult.</returns>
        [Authorize(Roles = "admin, user, event manager, venue manager")]
        [HttpPost("edit")]
        public async Task<IActionResult> Edit([FromBody] EditAccountViewModel model)
        {
            return Ok(await _service.UpdateUserInEdit(model));
        }
    }
}
