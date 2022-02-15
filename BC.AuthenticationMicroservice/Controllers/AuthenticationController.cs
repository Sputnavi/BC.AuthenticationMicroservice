using BC.AuthenticationMicroservice.Models;
using BC.AuthenticationMicroservice.Boundary.Request;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BC.AuthenticationMicroservice.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<User> _userManager;

        public AuthenticationController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            User user = await _userManager.FindByEmailAsync(request.Email);
            if (user is not null && await _userManager.CheckPasswordAsync(user, request.Password))
            {
                return Ok();
            }

            return Unauthorized();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var user = new User()
            {
                Email = request.Email,
                UserName = $"{request.FirstName}_{request.SecondName}",
                FirstName = request.FirstName,
                SecondName = request.SecondName
            };

            IdentityResult result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                return BadRequest(string.Join(", ", result.Errors.Select(x => x.Description)));
            }

            return Ok();
        }
    }
}