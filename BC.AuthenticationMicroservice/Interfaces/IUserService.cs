using BC.AuthenticationMicroservice.Boundary.Request;
using BC.AuthenticationMicroservice.Boundary.Response;
using BC.AuthenticationMicroservice.Models;

namespace BC.AuthenticationMicroservice.Interfaces
{
    public interface IUserService
    {
        Task<List<UserWithRole>> GetUsersWithRolesAsync();
        UserWithRole GetUserWithRoleById(string id);
        Task<UserWithRole> GetCurrentUserWithRole(string name);
        Task<User> CreateUserAsync(RegisterRequest userDto);
        Task UpdateUserAsync(string id, UserUpdateDto userDto);
        Task DeleteUserAsync(string id);
        Task<string> GetUserRoleAsync(string userId);
        Task<bool> ChangeUserPasswordAsync(string userId, PasswordChangeDto passwordsDto);
    }
}
