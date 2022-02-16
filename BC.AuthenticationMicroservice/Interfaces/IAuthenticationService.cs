using BC.AuthenticationMicroservice.Boundary.Request;
using BC.AuthenticationMicroservice.Models;

namespace BC.AuthenticationMicroservice.Interfaces
{
    public interface IAuthenticationService
    {
        Task<User> AuthenticateAsync(LoginRequest loginRequest);
    }
}
