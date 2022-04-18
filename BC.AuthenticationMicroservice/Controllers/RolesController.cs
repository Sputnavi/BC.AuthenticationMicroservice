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

        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _roleService.GetRolesAsync();

            return Ok(roles);
        }

        [HttpGet("{roleName}/users")]
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
