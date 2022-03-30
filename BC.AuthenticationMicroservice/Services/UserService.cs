using BC.AuthenticationMicroservice.Boundary.Request;
using BC.AuthenticationMicroservice.Interfaces;
using BC.AuthenticationMicroservice.Models;
using BC.AuthenticationMicroservice.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BC.AuthenticationMicroservice.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly ApplicationContext _context;

        public UserService(UserManager<User> userManager, RoleManager<Role> roleManager, ApplicationContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public async Task<List<User>> GetUsersWithRolesAsync()
        {
            return await _context.Users
                .Include(x => x.UserRoles)
                    .ThenInclude(x => x.Role)
                .AsNoTracking()
                .ToListAsync()
                .ConfigureAwait(false);
        }
        
        public User GetUserWithRoleById(string id)
        {
            return _context.Users
                .Include(x => x.UserRoles)
                    .ThenInclude(x => x.Role)
                    .FirstOrDefault(u => u.Id == id);
        }

        public async Task<bool> CreateUserAsync(RegisterRequest userDto)
        {
            var roleExists = await RoleExistsAsync(userDto.Role).ConfigureAwait(false); 
            if (!roleExists)
            {
                return false;
            }
            var user = new User()
            {
                Email = userDto.Email,
                UserName = $"{userDto.FirstName}_{userDto.SecondName}",//ToDo K: overrise??
                FirstName = userDto.FirstName,
                SecondName = userDto.SecondName
            };

            var userResult = await _userManager.CreateAsync(user, userDto.Password).ConfigureAwait(false);
            if (!userResult.Succeeded)
            {
                return false;//ToDo K:return smth normal
            }
            
            var roleResult = await _userManager.AddToRoleAsync(user, userDto.Role).ConfigureAwait(false);
            return roleResult.Succeeded;
        }

        public async Task<bool> UpdateUserAsync(string id, UserUpdateDto userDto)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return false;
            }

            user.Email = userDto.Email;
            user.FirstName = userDto.FirstName;
            user.SecondName = userDto.SecondName;
            user.UserName = $"{userDto.FirstName}_{userDto.SecondName}";//ToDo K: overrise?? mapping?

            var result = await _userManager.UpdateAsync(user).ConfigureAwait(false);
            return result.Succeeded;
        }
        
        public async Task<bool> DeleteUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id).ConfigureAwait(false);

            if (user == null)
            {
                return false;
            }

            var result = await _userManager.DeleteAsync(user).ConfigureAwait(false);
            return result.Succeeded; 
        }
        
        private async Task<bool> RoleExistsAsync(string role)
        {
            return await _roleManager.RoleExistsAsync(role).ConfigureAwait(false);
        }

    }
}
