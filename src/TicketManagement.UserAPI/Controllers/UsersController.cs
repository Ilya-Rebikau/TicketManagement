﻿using System.Threading.Tasks;
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
        /// <returns>Task with IActionResult.</returns>
        [HttpGet("getusers")]
        public async Task<IActionResult> GetUsers()
        {
            return Ok(await _service.GetUsers());
        }

        /// <summary>
        /// Create user.
        /// </summary>
        /// <param name="model">CreateUserViewModel object.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateUserViewModel model)
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
            if (model is not null)
            {
                return Ok(model);
            }

            return NotFound();
        }

        /// <summary>
        /// Edit user.
        /// </summary>
        /// <param name="model">EditUserViewModel object.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpPut("edit")]
        public async Task<IActionResult> Edit([FromBody] EditUserViewModel model)
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
            if (!string.IsNullOrEmpty(deletedId))
            {
                return Ok(deletedId);
            }

            return NotFound();
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
            if (model is not null)
            {
                return Ok(model);
            }

            return NotFound();
        }

        /// <summary>
        /// Change password for user.
        /// </summary>
        /// <param name="model">ChangePasswordViewModel object.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpPut("changepassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordViewModel model)
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
            if (model is not null)
            {
                return Ok(model);
            }

            return NotFound();
        }

        /// <summary>
        /// Edit roles.
        /// </summary>
        /// <param name="model">ChangeRoleViewModel.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpPut("editroles")]
        public async Task<IActionResult> EditRoles([FromBody] ChangeRoleViewModel model)
        {
            await _service.EditRoles(model);
            return Ok();
        }
    }
}
