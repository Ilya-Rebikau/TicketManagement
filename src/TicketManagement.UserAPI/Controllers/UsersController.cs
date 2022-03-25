using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketManagement.UserAPI.Interfaces;
using TicketManagement.UserAPI.Models.Users;

namespace TicketManagement.UserAPI.Controllers
{
    /// <summary>
    /// Controller for users.
    /// </summary>
    [Authorize(Roles = "admin")]
    [Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        /// <summary>
        /// UsersService object.
        /// </summary>
        private readonly IUsersService _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersController"/> class.
        /// </summary>
        /// <param name="service">UsersService object.</param>
        public UsersController(IUsersService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get all users.
        /// </summary>
        /// <param name="pageNumber">Page number.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpGet("getusers")]
        public async Task<IActionResult> GetUsers([FromBody] int pageNumber)
        {
            return Ok(await _service.GetUsers(pageNumber));
        }

        /// <summary>
        /// Create user.
        /// </summary>
        /// <param name="model">CreateUserViewModel object.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateUserModel model)
        {
            return Ok(await _service.CreateUser(model));
        }

        /// <summary>
        /// Edit user.
        /// </summary>
        /// <param name="id">Id of editing user.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpGet("edit/{id}")]
        public async Task<IActionResult> Edit([FromRoute] string id)
        {
            var model = await _service.GetEditUserViewModel(id);
            return model is null ? NotFound() : Ok(model);
        }

        /// <summary>
        /// Edit user.
        /// </summary>
        /// <param name="model">EditUserViewModel object.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpPut("edit")]
        public async Task<IActionResult> Edit([FromBody] EditUserModel model)
        {
            return Ok(await _service.UpdateUserInEditAsync(model));
        }

        /// <summary>
        /// Delete user.
        /// </summary>
        /// <param name="id">Id of deleting object.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> Delete([FromRoute] string id)
        {
            var deletedId = await _service.DeleteUserAsync(id);
            return string.IsNullOrEmpty(deletedId) ? NotFound() : Ok(deletedId);
        }

        /// <summary>
        /// Change passowrd for user.
        /// </summary>
        /// <param name="id">Id of user.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpGet("changepassword/{id}")]
        public async Task<IActionResult> ChangePassword([FromRoute] string id)
        {
            var model = await _service.GetChangePasswordViewModel(id);
            return model is null ? NotFound() : Ok(model);
        }

        /// <summary>
        /// Change password for user.
        /// </summary>
        /// <param name="model">ChangePasswordViewModel object.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpPut("changepassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordModel model)
        {
            return Ok(await _service.ChangePassword(model, HttpContext));
        }

        /// <summary>
        /// Edit roles.
        /// </summary>
        /// <param name="id">User id.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpGet("editroles/{id}")]
        public async Task<IActionResult> EditRoles([FromRoute] string id)
        {
            var model = await _service.GetChangeRoleViewModel(id);
            return model is null ? NotFound() : Ok(model);
        }

        /// <summary>
        /// Edit roles.
        /// </summary>
        /// <param name="model">ChangeRoleViewModel.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpPut("editroles")]
        public async Task<IActionResult> EditRoles([FromBody] ChangeRoleModel model)
        {
            await _service.EditRoles(model);
            return Ok();
        }
    }
}
