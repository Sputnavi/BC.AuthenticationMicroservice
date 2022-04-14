using BC.AuthenticationMicroservice.Models;
using BC.AuthenticationMicroservice.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace BC.AuthenticationMicroservice.Tests.Services
{
    public class JwtTokenGeneratorTests
    {
        private readonly JwtTokenGenerator _sut;
        private readonly Mock<UserManager<User>> _userManager;
        private readonly Mock<IConfiguration> _configuration;

        public JwtTokenGeneratorTests()
        {
            var store = new Mock<IUserStore<User>>();
            _userManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            _configuration = new Mock<IConfiguration>();

            _sut = new JwtTokenGenerator(_userManager.Object, _configuration.Object);
        }

        [Fact]
        public async Task GenerateTokenAsync_CorrectClaims_ReturnsToken()
        {
            var user = new User()
            {
                Email = "someEmail",
                UserName = "someUserName",
                FirstName = "someFirstName",
                SecondName = "someSecondName",
            };
            var section = new Mock<IConfigurationSection>();
            section.Setup(x => x["secretKey"]).Returns("testSecretKey123");
            section.Setup(x => x["validAudience"]).Returns("validAudience");
            section.Setup(x => x["validIssuer"]).Returns("validIssuer");
            section.Setup(x => x["minutesToExpire"]).Returns("10");

            _configuration.Setup(x => x.GetSection(It.IsAny<string>()))
                .Returns(section.Object);
            _userManager.Setup(x => x.GetRolesAsync(It.IsAny<User>()))
                .ReturnsAsync(Array.Empty<string>());

            var result = await _sut.GenerateTokenAsync(user);

            result.Should().NotBeNull();
            result.Should().NotBeEmpty();
        }

        [Fact]
        public async Task GenerateTokenAsync_ShortSecretKey_ThrowsOutOfRangeException()
        {
            var user = new User()
            {
                Email = "someEmail",
                UserName = "someUserName",
                FirstName = "someFirstName",
                SecondName = "someSecondName",
            };
            var section = new Mock<IConfigurationSection>();
            section.Setup(x => x["secretKey"]).Returns("short");
            section.Setup(x => x["validAudience"]).Returns("validAudience");
            section.Setup(x => x["validIssuer"]).Returns("validIssuer");
            section.Setup(x => x["minutesToExpire"]).Returns("10");

            _configuration.Setup(x => x.GetSection(It.IsAny<string>()))
                .Returns(section.Object);
            _userManager.Setup(x => x.GetRolesAsync(It.IsAny<User>()))
                .ReturnsAsync(Array.Empty<string>());

            var func = () => _sut.GenerateTokenAsync(user);

            await func.Should().ThrowAsync<ArgumentOutOfRangeException>();
        }

        [Fact]
        public async Task GenerateTokenAsync_NoSecretKey_ThrowsException()
        {
            var user = new User()
            {
                Email = "someEmail",
                UserName = "someUserName",
                FirstName = "someFirstName",
                SecondName = "someSecondName",
            };
            var section = new Mock<IConfigurationSection>();
            section.Setup(x => x["validAudience"]).Returns("validAudience");
            section.Setup(x => x["validIssuer"]).Returns("validIssuer");
            section.Setup(x => x["minutesToExpire"]).Returns("10");

            _configuration.Setup(x => x.GetSection(It.IsAny<string>()))
                .Returns(section.Object);
            _userManager.Setup(x => x.GetRolesAsync(It.IsAny<User>()))
                .ReturnsAsync(Array.Empty<string>());

            var func = () => _sut.GenerateTokenAsync(user);

            await func.Should().ThrowAsync<Exception>().WithMessage("No secret key provided from Configuration or Environment");
        }
    }
}
