using BC.AuthenticationMicroservice.Boundary.Request;
using BC.AuthenticationMicroservice.Boundary.Response;
using BC.AuthenticationMicroservice.Models;

namespace BC.AuthenticationMicroservice.Interfaces
{
    public interface IUserService
    {
        Task<List<UserWithRole>> GetUsersWithRolesAsync();
        UserWithRole GetUserWithRoleById(string id);
        Task<User> CreateUserAsync(RegisterRequest userDto);
        Task<bool> UpdateUserAsync(string id, UserUpdateDto userDto);
        Task<bool> DeleteUserAsync(string id);
        Task<string> GetUserRoleAsync(string userId);
    }
}
