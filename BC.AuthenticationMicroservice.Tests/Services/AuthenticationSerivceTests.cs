using BC.AuthenticationMicroservice.Boundary.Request;
using BC.AuthenticationMicroservice.Models;
using BC.AuthenticationMicroservice.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace BC.AuthenticationMicroservice.Tests.Services
{
    public class AuthenticationSerivceTests
    {
        private readonly Mock<UserManager<User>> _userManager;
        private readonly AuthenticationService _sut;

        public AuthenticationSerivceTests()
        {
            var store = new Mock<IUserStore<User>>();
            _userManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);

            _sut = new AuthenticationService(_userManager.Object);
        }

        [Fact]
        public async Task AuthenticateAsync_CorrectCredentials_ReturnsUser()
        {
            var user = new User()
            {
                Email = "someEmail",
                UserName = "someUserName",
                FirstName = "someFirstName",
                SecondName = "someSecondName",
            };
            _userManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(user);
            _userManager.Setup(x => x.CheckPasswordAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            var result = await _sut.AuthenticateAsync(new LoginRequest());
            
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(user);
        }

        [Fact]
        public async Task AuthenticateAsync_NoUser_ReturnsNull()
        {
            _userManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((User) null);

            var result = await _sut.AuthenticateAsync(new LoginRequest());

            result.Should().BeNull();
        }

        [Fact]
        public async Task AuthenticateAsync_IncorrectPassword_ReturnsNull()
        {
            _userManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(new User());
            _userManager.Setup(x => x.CheckPasswordAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(false);

            var result = await _sut.AuthenticateAsync(new LoginRequest());

            result.Should().BeNull();
        }
    }
}
