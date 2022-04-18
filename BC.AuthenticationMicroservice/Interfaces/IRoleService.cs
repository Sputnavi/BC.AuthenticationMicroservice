using BC.AuthenticationMicroservice.Boundary.Response;

namespace BC.AuthenticationMicroservice.Interfaces
{
    public interface IRoleService
    {
        Task<List<GetRole>> GetRolesAsync();
        Task<List<UserDto>> GetUsersForRoleAsync(string roleName);
    }
}
