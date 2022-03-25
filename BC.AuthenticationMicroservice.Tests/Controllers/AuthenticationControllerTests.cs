using BC.AuthenticationMicroservice.Boundary.Request;
using BC.AuthenticationMicroservice.Boundary.Response;
using BC.AuthenticationMicroservice.Controllers;
using BC.AuthenticationMicroservice.Interfaces;
using BC.AuthenticationMicroservice.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace BC.AuthenticationMicroservice.Tests.Controllers
{
    public class AuthenticationControllerTests
    {
        private readonly AuthenticationController _sut;
        private readonly Mock<UserManager<User>> _userManager;
        private readonly Mock<IAuthenticationService> _authenticationService;
        private readonly Mock<IJwtTokenGenerator> _jwtTokenGenerator;

        public AuthenticationControllerTests()
        {
            var store = new Mock<IUserStore<User>>();
            _userManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            _authenticationService = new Mock<IAuthenticationService>();
            _jwtTokenGenerator = new Mock<IJwtTokenGenerator>();

            _sut = new AuthenticationController(_userManager.Object, _authenticationService.Object, _jwtTokenGenerator.Object);
        }

        [Fact]
        public async Task LoginAsync_CorrectCredenticals_ReturnsValidToken()
        {
            string token = "someTokenExample";
            _authenticationService.Setup(x => x.AuthenticateAsync(It.IsAny<LoginRequest>()))
                .ReturnsAsync(new User());
            _jwtTokenGenerator.Setup(x => x.GenerateTokenAsync(It.IsAny<User>()))
                .ReturnsAsync(token);

            var response = await _sut.LoginAsync(new LoginRequest());

            response.Should().BeOfType<OkObjectResult>();
            var result = response as OkObjectResult;
            result.Should().NotBeNull();
            result.Value.Should().NotBeNull();
            result.Value.Should().BeOfType<LoginResponse>();
            var loginResponse = result.Value as LoginResponse;
            loginResponse.Token.Should().Be(token);
        }

        [Fact]
        public async Task LoginAsync_IncorrectCredentials_ReturnsUnauthorizedResponse()
        {
            _authenticationService.Setup(x => x.AuthenticateAsync(It.IsAny<LoginRequest>()))
                .ReturnsAsync((User) null);

            var response = await _sut.LoginAsync(new LoginRequest());

            response.Should().BeOfType<UnauthorizedResult>();
        }

        [Fact]
        public async Task RegisterAsync_CorrectCredentials_ReturnsOkResponse()
        {
            _userManager.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            var response = await _sut.RegisterAsync(new RegisterRequest());

            response.Should().BeOfType<OkResult>();
        }

        [Fact]
        public async Task RegisterAsync_IncorrectCredentials_ReturnsOkResponse()
        {
            _userManager.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed());

            var response = await _sut.RegisterAsync(new RegisterRequest());

            response.Should().BeOfType<BadRequestObjectResult>();
        }
    }
}
