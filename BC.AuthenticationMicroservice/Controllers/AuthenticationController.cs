using BC.AuthenticationMicroservice.Models;
using BC.AuthenticationMicroservice.Boundary.Request;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BC.AuthenticationMicroservice.Interfaces;
using BC.AuthenticationMicroservice.Services;

namespace BC.AuthenticationMicroservice.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IAuthenticationService _authenticationService;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;

        public AuthenticationController(UserManager<User> userManager, IAuthenticationService authenticationService, IJwtTokenGenerator jwtTokenGenerator, 
            IConfiguration configuration, IUserService userService)
        {
            _userManager = userManager;
            _authenticationService = authenticationService;
            _jwtTokenGenerator = jwtTokenGenerator;
            _configuration = configuration;
            _userService = userService;
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
            IConfigurationSection jwtSettings = _configuration.GetSection("JwtSettings");

            var response = new
            {
                token = token,
                minutesToExpire = Convert.ToDouble(jwtSettings["minutesToExpire"]),
                role = await _userService.GetUserRoleAsync(user.Id)
            };
            return Ok(response);
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