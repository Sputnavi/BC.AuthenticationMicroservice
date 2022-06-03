using BC.AuthenticationMicroservice.Boundary.Request;
using BC.AuthenticationMicroservice.Boundary.Response;
using BC.AuthenticationMicroservice.Interfaces;
using BC.AuthenticationMicroservice.Models;
using Microsoft.AspNetCore.Mvc;

namespace BC.AuthenticationMicroservice.Controllers
{
    //ToDo: add auth
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

        /// <summary>
        /// Return all users.
        /// </summary>
        /// <response code="200">List of users returned successfully</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserWithRole[]))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(BaseErrorResponse))]
        [HttpGet(Name = "GetUsers")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetUsersWithRolesAsync();

            return Ok(users);
        }

        /// <summary>
        /// Return user by id.
        /// </summary>
        /// <response code="200">User returned successfully</response>
        /// <response code="404">User with the specified id doesn't exist</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserWithRole))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(BaseErrorResponse))]
        [HttpGet("{id}", Name = "GetUserById")]
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

        /// <summary>
        /// Create user with role.
        /// </summary>
        /// <param name="userDto">User data for registration.</param>
        /// <response code="201">Successfully created.</response> 
        /// <response code="400">Invalid body.</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RegisterRequest))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost(Name = "CreateUser")]
        public async Task<IActionResult> CreateUser(RegisterRequest userDto)
        {
            if (userDto == null)
            {
                _logger.LogWarn("UserDto can't be null");
                return BadRequest();
            }
            User createdUser = await _userService.CreateUserAsync(userDto);

            return new ObjectResult(createdUser) { StatusCode = StatusCodes.Status201Created };
        }

        /// <summary>
        /// Update user with the specified id.
        /// </summary>
        /// <param name="userDto">User data for update.</param>
        /// <response code="204">Successfully updated.</response> 
        /// <response code="400">Invalid body.</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(UserUpdateDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut("{id}", Name = "UpdateUser")]
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

        /// <summary>
        /// Change password of the user with the specified id.
        /// </summary>
        /// <param name="passwordsDto">Old and ned passwords.</param>
        /// <response code="204">Successfully updated.</response> 
        /// <response code="400">Invalid body.</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut("{id}/password-change", Name = "ChangePassword")]
        public async Task<IActionResult> ChangeUsersPassword(string id, PasswordChangeDto passwordsDto)
        {
            var passwordChanged = await _userService.ChangeUserPasswordAsync(id, passwordsDto);
            if (!passwordChanged)
            {
                return BadRequest();
            }
            return NoContent();
        }

        /// <summary>
        /// Delete user with the specified id.
        /// </summary>
        /// <response code="204">Successfully deleted.</response> 
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("{id}", Name = "DeleteUser")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            await _userService.DeleteUserAsync(id);

            return NoContent();
        }

        /// <summary>
        /// Get account information of the authorized user.
        /// </summary>
        /// <response code="200">Successfully get user account information.</response> 
        /// <response code="400">Invalid data.</response>
        /// <response code="401">Unauthorized user.</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("account", Name = "GetAccountInfo")]
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
