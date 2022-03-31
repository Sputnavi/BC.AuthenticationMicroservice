﻿using BC.AuthenticationMicroservice.Boundary.Request;
using BC.AuthenticationMicroservice.CustomExceptions;
using BC.AuthenticationMicroservice.Interfaces;
using BC.AuthenticationMicroservice.Models;
using BC.AuthenticationMicroservice.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BC.AuthenticationMicroservice.Controllers
{
    //ToDo:  add auth
    [ApiController]
    [Route("api/admin/users")]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        private readonly ILoggerManager _logger;

        public UsersController(IUserService userService, ILoggerManager logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet]//, Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetUsersWithRolesAsync().ConfigureAwait(false);
            
            return Ok(users);
        }

        [HttpGet("{id}")]//, Authorize(Roles = UserRoles.Admin)]//ToDo K: add authorize roles
        public IActionResult GetUserById(string id)
        {
            var user = _userService.GetUserWithRoleById(id);

            if (user == null)
            {
                _logger.LogError($"User with id = {id} not found");
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost]//, Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> CreateUser(RegisterRequest userDto)//ToDo K: repeated logic
        {
            if (userDto == null)
            {
                _logger.LogWarn("UserDto can't be null");
                return BadRequest();
            }
            User createdUser = null;
            try
            {
                createdUser = await _userService.CreateUserAsync(userDto).ConfigureAwait(false);
            }
            catch (UserCreationException ucex)
            {
                _logger.LogError(ucex.Message);
                return BadRequest(ucex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500);
            }

            return new ObjectResult(createdUser) { StatusCode = StatusCodes.Status201Created };
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, UserUpdateDto userDto)
        {
            if (userDto == null)
            {
                _logger.LogWarn("UserDto can't be null");
                return BadRequest();
            }

            var updated = await _userService.UpdateUserAsync(id, userDto).ConfigureAwait(false);
            if (!updated)
            {
                return StatusCode(500);
            }
            return NoContent();
        }

        [HttpPut("{id}/password-change")]
        public async Task<IActionResult> ChangeUsersPassword(string id, PasswordChangeDto passwordsDto)
        {
            var passwordChanged = await _userService.ChangeUserPasswordAsync(id, passwordsDto).ConfigureAwait(false);//ToDo K: how to handle returns?
            if (!passwordChanged)
            {
                return StatusCode(500);
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)//ToDo K: flag deleted, not delete?
        {
            var deleted = await _userService.DeleteUserAsync(id).ConfigureAwait(false);
            if (!deleted)
            {
                return StatusCode(500);
            }
            return NoContent();
        }

        [HttpGet("account")]
        public async Task<IActionResult> GetCurrentUser()//ToDo K: need fix -> name is null
        {
            var user = await _userService.GetCurrentUserWithRole(User.Identity.Name);

            if (user == null)
            {
                _logger.LogError($"Current User not found");
                return NotFound();
            }
            return Ok(user);
        }
    }
}
