using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketManagement.UserAPI.Interfaces;
using TicketManagement.UserAPI.Models;
using TicketManagement.UserAPI.Models.Account;
using TicketManagement.UserAPI.Services;

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
        /// Key for jwt token in header.
        /// </summary>
        private const string AuthorizationKey = "Authorization";

        /// <summary>
        /// AccountWebService object.
        /// </summary>
        private readonly IAccountService _service;

        /// <summary>
        /// JwtTokenService object.
        /// </summary>
        private readonly JwtTokenService _jwtTokenService;

        /// <summary>
        /// ConverterForTime object.
        /// </summary>
        private readonly ConverterForTime _converterForTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController"/> class.
        /// </summary>
        /// <param name="service">AccountService object.</param>
        /// <param name="jwtTokenService">JwtTokenService object.</param>
        /// <param name="converterForTime">ConverterForTime object.</param>
        public AccountController(IAccountService service, JwtTokenService jwtTokenService, ConverterForTime converterForTime)
        {
            _service = service;
            _jwtTokenService = jwtTokenService;
            _converterForTime = converterForTime;
        }

        /// <summary>
        /// Register user.
        /// </summary>
        /// <param name="model">RegisterViewModel object.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var registerResult = await _service.RegisterUser(model);
            return registerResult.IdentityResult.Succeeded ? Ok(_jwtTokenService.GetToken(registerResult.User, registerResult.Roles))
                : BadRequest(registerResult.IdentityResult.Errors);
        }

        /// <summary>
        /// Login for user.
        /// </summary>
        /// <param name="model">LoginViewModel object.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var loginResult = await _service.Login(model);
            return loginResult.SignInResult.Succeeded ? Ok(_jwtTokenService.GetToken(loginResult.User, loginResult.Roles))
                : Forbid();
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
            var model = await _service.GetEditAccountViewModelForEdit(id);
            return model is null ? NotFound() : Ok(model);
        }

        /// <summary>
        /// Edit user.
        /// </summary>
        /// <param name="model">EditAccountViewModel object.</param>
        /// <returns>Task with IActionResult.</returns>
        [Authorize(Roles = "admin, user, event manager, venue manager")]
        [HttpPut("edit")]
        public async Task<IActionResult> Edit([FromBody] EditAccountModel model)
        {
            return Ok(await _service.UpdateUserInEdit(model));
        }

        /// <summary>
        /// Add balance on account.
        /// </summary>
        /// <param name="id">User id.</param>
        /// <returns>Task with IActionResult.</returns>
        [Authorize(Roles = "admin, user, event manager, venue manager")]
        [HttpGet("addbalance/{id}")]
        public async Task<IActionResult> AddBalance([FromRoute] string id)
        {
            var model = await _service.GetBalanceViewModel(id);
            return model is null ? NotFound() : Ok(model);
        }

        /// <summary>
        /// Add balance on account.
        /// </summary>
        /// <param name="model">AddBalanceViewModel object.</param>
        /// <returns>Task with IActionResult.</returns>
        [Authorize(Roles = "admin, user, event manager, venue manager")]
        [HttpPut("addbalance")]
        public async Task<IActionResult> AddBalance(AddBalanceModel model)
        {
            return Ok(await _service.AddBalance(model));
        }

        /// <summary>
        /// Validate jwt token.
        /// </summary>
        /// <param name="token">Jwt token.</param>
        /// <returns>True if token is valid and false if not.</returns>
        [Authorize(Roles = "admin, user, event manager, venue manager")]
        [HttpPost("validatetoken")]
        public IActionResult ValidateToken([FromHeader(Name = AuthorizationKey)] string token)
        {
            return Ok(_jwtTokenService.ValidateToken(token));
        }

        [Authorize(Roles = "admin, user, event manager, venue manager")]
        [HttpPost("converttime")]
        public async Task<IActionResult> ConvertTimeFromUtcToUsers([FromHeader(Name = AuthorizationKey)] string token, [FromBody] EventDto eventDto)
        {
            return _jwtTokenService.ValidateToken(token) ? Ok(await _converterForTime.ConvertTimeForUser(eventDto, token))
                : NotFound();
        }

        [Authorize(Roles = "admin, user, event manager, venue manager")]
        [HttpPost("getuserid")]
        public async Task<IActionResult> GetUserId([FromHeader(Name = AuthorizationKey)] string token)
        {
            return _jwtTokenService.ValidateToken(token) ? Ok(await _service.GetUserId(token)) : NotFound();
        }

        [Authorize(Roles = "admin, user, event manager, venue manager")]
        [HttpPost("changebalance")]
        public async Task<IActionResult> ChangeBalanceForUser([FromHeader(Name = AuthorizationKey)] string token, [FromBody] double price)
        {
            return Ok(await _service.ChangeBalanceForUser(token, price));
        }
    }
}
