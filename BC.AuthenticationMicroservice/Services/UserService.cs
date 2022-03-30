using AutoMapper;
using BC.AuthenticationMicroservice.Boundary.Request;
using BC.AuthenticationMicroservice.Boundary.Response;
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
        private readonly IMapper _mapper;

        public UserService(UserManager<User> userManager, RoleManager<Role> roleManager, ApplicationContext context,
            IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<UserWithRole>> GetUsersWithRolesAsync()
        {
            var domainUsers = await _context.Users
                .Include(x => x.UserRoles)
                    .ThenInclude(x => x.Role)
                .AsNoTracking()
                .ToListAsync()
                .ConfigureAwait(false);

            var users = _mapper.Map<List<UserWithRole>>(domainUsers);
            return users;
        }
        
        public UserWithRole GetUserWithRoleById(string id)
        {
            var domainUser = _context.Users
                .Include(x => x.UserRoles)
                    .ThenInclude(x => x.Role)
                    .FirstOrDefault(u => u.Id == id);
            
            var user = _mapper.Map<UserWithRole>(domainUser);
            return user;
        }

        public async Task<User> CreateUserAsync(RegisterRequest userDto)
        {
            var roleExists = await RoleExistsAsync(userDto.Role).ConfigureAwait(false); 
            if (!roleExists)
            {
                return null;
            }
            var user = _mapper.Map<User>(userDto);

            var userResult = await _userManager.CreateAsync(user, userDto.Password).ConfigureAwait(false);
            if (!userResult.Succeeded)
            {
                return null;//ToDo K:return smth normal
            }
            
            var roleResult = await _userManager.AddToRoleAsync(user, userDto.Role).ConfigureAwait(false);
            return user;
        }

        public async Task<bool> UpdateUserAsync(string id, UserUpdateDto userDto)
        {
            var user = await _userManager.FindByIdAsync(id).ConfigureAwait(false);
            if (user == null)
            {
                return false;
            }

            var updatedUser = _mapper.Map(userDto, user);

            var result = await _userManager.UpdateAsync(updatedUser).ConfigureAwait(false);
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
