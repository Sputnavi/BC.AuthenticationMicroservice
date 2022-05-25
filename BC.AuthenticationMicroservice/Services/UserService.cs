using AutoMapper;
using BC.AuthenticationMicroservice.Boundary.Request;
using BC.AuthenticationMicroservice.Boundary.Response;
using BC.AuthenticationMicroservice.CustomExceptions;
using BC.AuthenticationMicroservice.Interfaces;
using BC.AuthenticationMicroservice.Models;
using BC.AuthenticationMicroservice.Models.Exceptions;
using BC.AuthenticationMicroservice.Repository;
using BC.Messaging;
using MassTransit;
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
        private readonly IPublishEndpoint _publishEndpoint;

        public UserService(UserManager<User> userManager, RoleManager<Role> roleManager, ApplicationContext context,
            IMapper mapper, IPublishEndpoint publishEndpoint)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _mapper = mapper;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<List<UserWithRole>> GetUsersWithRolesAsync()
        {
            var domainUsers = await _context.Users
                .Include(x => x.UserRoles)
                    .ThenInclude(x => x.Role)
                .AsNoTracking()
                .ToListAsync();

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

        public async Task<UserWithRole> GetCurrentUserWithRole(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            var user = await _userManager.FindByNameAsync(name);
            if (user == null)
            {
                throw new EntityNotFoundException(nameof(User), name);
            }

            var domainUser = _context.Users
                .Include(x => x.UserRoles)
                    .ThenInclude(x => x.Role)
                    .FirstOrDefault(u => u.Id == user.Id);

            var userWithRole = _mapper.Map<UserWithRole>(domainUser);
            return userWithRole;
        }

        public async Task<User> CreateUserAsync(RegisterRequest userDto)
        {
            if (userDto == null || userDto.Role == null)
            {
                throw new ArgumentNullException(nameof(userDto));
            }

            var roleExists = await RoleExistsAsync(userDto.Role);
            if (!roleExists)
            {
                throw new EntityNotFoundException(nameof(Role), userDto.Role);
            }
            var user = _mapper.Map<User>(userDto);

            var userResult = await _userManager.CreateAsync(user, userDto.Password);
            if (!userResult.Succeeded)
            {
                throw new UserCreationException(userResult.Errors);
            }

            await _userManager.AddToRoleAsync(user, userDto.Role);
            return user;
        }

        public async Task UpdateUserAsync(string id, UserUpdateDto userDto)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                throw new EntityNotFoundException(nameof(user), id);
            }

            var updatedUser = _mapper.Map(userDto, user);

            await _userManager.UpdateAsync(updatedUser);

            var message = _mapper.Map<UserUpdated>(updatedUser);
            await _publishEndpoint.Publish(message);
        }

        public async Task DeleteUserAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException(nameof(id));
            }
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                throw new EntityNotFoundException(nameof(User), id);
            }

            await _userManager.DeleteAsync(user);

            var message = _mapper.Map<UserDeleted>(user);
            await _publishEndpoint.Publish(message);
        }

        public async Task<string> GetUserRoleAsync(string userId)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);

            if (user == null)
            {
                return null;
            }

            var roleList = await _userManager.GetRolesAsync(user);
            return roleList.FirstOrDefault();
        }

        public async Task<bool> ChangeUserPasswordAsync(string userId, PasswordChangeDto passwordsDto)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return false;
            }
            if (!await _userManager.CheckPasswordAsync(user, passwordsDto.OldPassword))
            {
                return false;
            }
            var res = await _userManager.ChangePasswordAsync(user, passwordsDto.OldPassword, passwordsDto.NewPassword);
            return res.Succeeded;
        }

        private async Task<bool> RoleExistsAsync(string role)
        {
            return await _roleManager.RoleExistsAsync(role);
        }

    }
}
