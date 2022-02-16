using BC.AuthenticationMicroservice.Boundary.Request;
using BC.AuthenticationMicroservice.Interfaces;
using BC.AuthenticationMicroservice.Models;
using Microsoft.AspNetCore.Identity;

namespace BC.AuthenticationMicroservice.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<User> _userManager;

        public AuthenticationService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<User> AuthenticateAsync(LoginRequest request)
        {
            User user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null || !await _userManager.CheckPasswordAsync(user, request.Password))
            {
                return null;
            }

            return user;
        }
    }
}
