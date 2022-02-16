using BC.AuthenticationMicroservice.Models;
using BC.AuthenticationMicroservice.Boundary.Request;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BC.AuthenticationMicroservice.Interfaces;

namespace BC.AuthenticationMicroservice.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IAuthenticationService _authenticationService;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public AuthenticationController(UserManager<User> userManager, IAuthenticationService authenticationService, IJwtTokenGenerator jwtTokenGenerator)
        {
            _userManager = userManager;
            _authenticationService = authenticationService;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            User user = await _authenticationService.AuthenticateAsync(request);
            if (user is null)
            {
                return Unauthorized();
            }

            string token = await _jwtTokenGenerator.GenerateTokenAsync(user);

            return Ok(new { Token = token });
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