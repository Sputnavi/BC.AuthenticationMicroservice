using BC.AuthenticationMicroservice.Models;

namespace BC.AuthenticationMicroservice.Interfaces
{
    public interface IJwtTokenGenerator
    {
        Task<string> GenerateTokenAsync(User user);
    }
}
