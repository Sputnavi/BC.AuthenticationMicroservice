using AutoMapper;
using BC.AuthenticationMicroservice.Boundary.Response;
using BC.AuthenticationMicroservice.Interfaces;
using BC.AuthenticationMicroservice.Models;
using BC.AuthenticationMicroservice.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BC.AuthenticationMicroservice.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly ApplicationContext _context;
        private readonly IMapper _mapper;

        public RoleService(RoleManager<Role> roleManager, ApplicationContext context,
            IMapper mapper)
        {
            _roleManager = roleManager;
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<GetRole>> GetRolesAsync()
        {
            var domainRoles = await _context.Roles
                .ToListAsync()
                .ConfigureAwait(false);

            var roles = _mapper.Map<List<GetRole>>(domainRoles);
            return roles;
        }
    }
}
