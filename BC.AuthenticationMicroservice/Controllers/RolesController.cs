using BC.AuthenticationMicroservice.Boundary.Request;
using BC.AuthenticationMicroservice.Interfaces;
using BC.AuthenticationMicroservice.Models;
using BC.AuthenticationMicroservice.Services;
using Microsoft.AspNetCore.Authorization;
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

        [HttpGet]//, Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _roleService.GetRolesAsync().ConfigureAwait(false);

            return Ok(roles);
        }
    }
}
