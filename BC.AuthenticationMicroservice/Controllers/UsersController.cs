using BC.AuthenticationMicroservice.Boundary.Request;
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

        [HttpGet, Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetUsersWithRolesAsync();
            
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
            User createdUser = createdUser = await _userService.CreateUserAsync(userDto);
            
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

            await _userService.UpdateUserAsync(id, userDto);
           
            return NoContent();
        }

        [HttpPut("{id}/password-change")]
        public async Task<IActionResult> ChangeUsersPassword(string id, PasswordChangeDto passwordsDto)
        {
            var passwordChanged = await _userService.ChangeUserPasswordAsync(id, passwordsDto);//ToDo K: how to handle returns?
            if (!passwordChanged)
            {
                return StatusCode(500);
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            await _userService.DeleteUserAsync(id);
            
            return NoContent();
        }

        [HttpGet("account")]
        public async Task<IActionResult> GetCurrentUser()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }
            if (User?.Identity?.Name == null)
            {
                return BadRequest("Name cannot be null");
            }
            var user = await _userService.GetCurrentUserWithRole(User.Identity.Name);

            return Ok(user);
        }
    }
}
