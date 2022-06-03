using BC.AuthenticationMicroservice.Boundary.Response;
using BC.AuthenticationMicroservice.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BC.AuthenticationMicroservice.Controllers
{
    [ApiController]
    [Route("api/admin/[controller]")]
    public class RolesController : Controller
    {
        private readonly IRoleService _roleService;
        private readonly ILoggerManager _logger;

        public RolesController(IRoleService roleService, ILoggerManager logger)
        {
            _roleService = roleService;
            _logger = logger;
        }

        /// <summary>
        /// Return a list of all roles.
        /// </summary>
        /// <response code="200">List of roles returned successfully</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetRole[]))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet(Name = "GetRoles")]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _roleService.GetRolesAsync();

            return Ok(roles);
        }

        /// <summary>
        /// Return users of the specified role.
        /// </summary>
        /// <response code="200">List of users for the specified role returned successfully</response>
        /// <response code="400">Role name can't be null.</response>
        /// <response code="404">Specified Role doesn't exist</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDto[]))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BaseErrorResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(BaseErrorResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(BaseErrorResponse))]
        [HttpGet("{roleName}/users", Name = "GetUsersForRoles")]
        public async Task<IActionResult> GetUsersForRoles(string roleName)
        {
            if (roleName == null)
            {
                return BadRequest("Role name can't be empty");
            }
            var users = await _roleService.GetUsersForRoleAsync(roleName);

            return Ok(users);
        }
    }
}
