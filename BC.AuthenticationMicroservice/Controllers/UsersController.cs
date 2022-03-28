using BC.AuthenticationMicroservice.Boundary.Request;
using BC.AuthenticationMicroservice.Interfaces;
using BC.AuthenticationMicroservice.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BC.AuthenticationMicroservice.Controllers
{
    //ToDo: 1. Add logger
    //2. add auth
    [ApiController]
    [Route("api/admin/users")]
    public class UsersController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly ILoggerManager _logger;

        public UsersController(UserManager<User> userManager, ILoggerManager logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        [HttpGet, Authorize(Roles = UserRoles.Admin)]
        public IActionResult GetUsers()
        {
            var users = _userManager.Users;

            return Ok(users);
        }

        [HttpGet("{id}"), Authorize(Roles = UserRoles.Admin)]//ToDo K: add authorize roles
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _userManager.FindByIdAsync(id).ConfigureAwait(false);

            if (user == null)
            {
                _logger.LogError($"User with id = {id} not found");
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost, Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> CreateUser(RegisterRequest userDto)//ToDo K: repeated logic
        {
            if (userDto == null)
            {
                _logger.LogWarn("UserDto can't be null");
                return BadRequest();
            }

            var user = new User()
            {
                Email = userDto.Email,
                UserName = $"{userDto.FirstName}_{userDto.SecondName}",
                FirstName = userDto.FirstName,
                SecondName = userDto.SecondName
            };

            var result = await _userManager.CreateAsync(user, userDto.Password).ConfigureAwait(false);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }
                return BadRequest(ModelState);
            }
            //ToDo K: add role when creating user
            //if (userDto.Roles != null && userDto.Roles.Count != 0) 
            //{
            //    await _userManager.AddToRolesAsync(user, userDto.Roles);
            //}
            return StatusCode(201);
        }

        //[HttpPut("{id}")]
        //public async Task<IActionResult> UpdateUser(string id, UserForUpdateDto userDto)
        //{
        //    var user = await _userManager.FindByIdAsync(id);

        //    if (user == null)
        //    {
        //        _logger.LogError($"User with id = {id} not found");
        //        return NotFound();
        //    }
        //    user.Email = userDto.Email;
        //    user.UserName = userDto.UserName;

        //    await _userManager.UpdateAsync(user);
        //    var userRoles = await _userManager.GetRolesAsync(user);

        //    var addedRoles = userDto.Roles.Except(userRoles);
        //    var removedRoles = userRoles.Except(userDto.Roles);

        //    await _userManager.AddToRolesAsync(user, addedRoles);
        //    await _userManager.RemoveFromRolesAsync(user, removedRoles);
        //    return Ok(user);
        //}

        //[HttpPut("{id}/password-change")]
        //[Authorize(Policy = PolicyTypes.Users.ChangePassword)]
        //public async Task<IActionResult> ChangeUsersPassword(string id, PasswordChangeDto passwordsDto)
        //{
        //    var user = await _userManager.FindByIdAsync(id);
        //    if (user == null)
        //    {
        //        _logger.LogError($"User with id = {id} not found");
        //        return NotFound();
        //    }
        //    if (!await _userManager.CheckPasswordAsync(user, passwordsDto.OldPassword))
        //    {
        //        _logger.LogError("Wrong old password");
        //        return BadRequest("Wrong old password");
        //    }
        //    var res = await _userManager.ChangePasswordAsync(user, passwordsDto.OldPassword, passwordsDto.NewPassword);
        //    return Ok(res);
        //}

        //[HttpDelete("{id}")]
        //[Authorize(Policy = PolicyTypes.Users.AddRemove)]
        //public async Task<IActionResult> DeleteUser(string id)
        //{
        //    var user = await _userManager.FindByIdAsync(id);

        //    if (user == null)
        //    {
        //        _logger.LogError($"User with id = {id} not found");
        //        return NotFound();
        //    }

        //    await _userManager.DeleteAsync(user);
        //    return Ok(user);
        //}

        //[HttpGet("{id}/roles")]
        //[Authorize(Policy = PolicyTypes.Users.ViewRoles)]
        //public async Task<IActionResult> GetRolesForUser(string id)
        //{
        //    var user = await _userManager.FindByIdAsync(id);
        //    if (user == null)
        //    {
        //        _logger.LogError($"User with id = {id} not found");
        //        return NotFound();
        //    }
        //    var userRoles = await _userManager.GetRolesAsync(user);
        //    return Ok(userRoles);
        //}

        //[HttpPut("{id}/roles")]
        //[Authorize(Policy = PolicyTypes.Users.EditRoles)]
        //public async Task<IActionResult> UpdateRolesForUser(string id, List<string> roles)
        //{
        //    var user = await _userManager.FindByIdAsync(id);
        //    if (user == null)
        //    {
        //        return NotFound("User not found");
        //    }
        //    var userRoles = await _userManager.GetRolesAsync(user);

        //    var addedRoles = roles.Except(userRoles);
        //    var removedRoles = userRoles.Except(roles);

        //    await _userManager.AddToRolesAsync(user, addedRoles);
        //    await _userManager.RemoveFromRolesAsync(user, removedRoles);
        //    return Ok();
        //}

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

        //[HttpGet("account/roles")]
        //[Authorize()]
        //public async Task<IActionResult> GetRolesForCurrentUser()
        //{
        //    var user = await _userManager.FindByNameAsync(User.Identity.Name);
        //    if (user == null)
        //    {
        //        _logger.LogError($"Current user not found");
        //        return NotFound();
        //    }
        //    var userRoles = await _userManager.GetRolesAsync(user);
        //    return Ok(userRoles);
        //}
    }
}
