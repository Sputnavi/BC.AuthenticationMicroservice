using BC.AuthenticationMicroservice.Boundary.Request;
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

            var created = await _userService.CreateUserAsync(userDto).ConfigureAwait(false);
            if (!created)
            {
                return StatusCode(500);
            }
            return StatusCode(201);
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
            return StatusCode(200);
        }

        //[HttpPut("{id}/password-change")]
        //public async Task<IActionResult> ChangeUsersPassword(string id, PasswordChangeDto passwordsDto)
        //{
        //    var user = await _userService.FindByIdAsync(id);
        //    if (user == null)
        //    {
        //        _logger.LogError($"User with id = {id} not found");
        //        return NotFound();
        //    }
        //    if (!await _userService.CheckPasswordAsync(user, passwordsDto.OldPassword))
        //    {
        //        _logger.LogError("Wrong old password");
        //        return BadRequest("Wrong old password");
        //    }
        //    var res = await _userService.ChangePasswordAsync(user, passwordsDto.OldPassword, passwordsDto.NewPassword);
        //    return Ok(res);
        //}

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)//ToDo K: flag deleted, not delete?
        {
            var deleted = await _userService.DeleteUserAsync(id).ConfigureAwait(false);
            if (!deleted)
            {
                return StatusCode(500);
            }
            return StatusCode(200);
        }

        //[HttpGet("account")]
        //[Authorize()]
        //public async Task<IActionResult> GetCurrentUser()
        //{
        //    var user = await _userManager.FindByNameAsync(User.Identity.Name);

        //    if (user == null)
        //    {
        //        _logger.LogError($"Current User not found");
        //        return NotFound();
        //    }
        //    return Ok(user);
        //}
    }
}
