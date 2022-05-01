using AutoMapper;
using BC.AuthenticationMicroservice.Boundary.Request;
using BC.AuthenticationMicroservice.Boundary.Response;
using BC.AuthenticationMicroservice.Interfaces;
using BC.AuthenticationMicroservice.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BC.AuthenticationMicroservice.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _logger;

        public AuthenticationController(IAuthenticationService authenticationService, IJwtTokenGenerator jwtTokenGenerator,
            IConfiguration configuration, IUserService userService, IMapper mapper, ILoggerManager logger)
        {
            _authenticationService = authenticationService;
            _jwtTokenGenerator = jwtTokenGenerator;
            _configuration = configuration;
            _userService = userService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(LoginRequest request)
        {
            User user = await _authenticationService.AuthenticateAsync(request);
            if (user is null)
            {
                return Unauthorized();
            }

            string token = await _jwtTokenGenerator.GenerateTokenAsync(user);
            IConfigurationSection jwtSettings = _configuration.GetSection("JwtSettings");

            var response = new LoginResponse
            {
                Token = token,
                MinutesToExpire = Convert.ToDouble(jwtSettings["minutesToExpire"]),
                Role = await _userService.GetUserRoleAsync(user.Id)
            };
            return Ok(response);
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(UserRegisterRequest userRequest)
        {
            if (userRequest == null)
            {
                _logger.LogWarn("User Request can't be null");
                return BadRequest();
            }
            var request = _mapper.Map<RegisterRequest>(userRequest);
            request.Role = UserRoles.User;

            User createdUser = await _userService.CreateUserAsync(request);

            return new ObjectResult(createdUser) { StatusCode = StatusCodes.Status201Created };

        }
    }
}