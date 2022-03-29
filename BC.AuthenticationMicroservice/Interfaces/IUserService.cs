using BC.AuthenticationMicroservice.Boundary.Request;
using BC.AuthenticationMicroservice.Models;

namespace BC.AuthenticationMicroservice.Interfaces
{
    public interface IUserService
    {
        Task<List<User>> GetUsersWithRolesAsync();
        User GetUserWithRoleById(string id);
        Task<bool> CreateUserAsync(RegisterRequest userDto);
        Task<bool> UpdateUserAsync(string id, UserUpdateDto userDto);
        Task<bool> DeleteUserAsync(string id);
    }
}
