using BC.AuthenticationMicroservice.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BC.AuthenticationMicroservice.Repository
{
    public class ApplicationContext : IdentityDbContext<User, Role, string,
        IdentityUserClaim<string>, UserRole, IdentityUserLogin<string>,
        IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>(b =>
            {
                b.HasMany(e => e.UserRoles)
                .WithOne(e => e.User)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();
            });

            builder.Entity<Role>(b =>
            {
                b.HasMany(e => e.UserRoles)
                    .WithOne(e => e.Role)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();
            });

            SeedData(builder);
        }
      

        private static void SeedData(ModelBuilder builder)
        {
            IList<Role> roles = new List<Role>
            {
                new Role
                {
                    Id = "5E17EAB5-C0D7-45FE-AD03-774E657CEBF3",
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new Role
                {
                    Id = "A6BAD0EA-29F8-43A0-BB4B-B37077E16076",
                    Name = "Master",
                    NormalizedName = "MASTER"
                },
                new Role
                {
                    Id = "DD3E7256-D5EA-49A7-A390-2E6EEF3DFEB3",
                    Name = "User",
                    NormalizedName = "USER"
                },
            };

            builder.Entity<Role>().HasData(roles);

            SeedAnyUser(builder, "Admin", "5E17EAB5-C0D7-45FE-AD03-774E657CEBF3", "BB7EAB5-C0D7-45FE-AD03-774E657CEBF3");
            SeedAnyUser(builder, "Master", "A6BAD0EA-29F8-43A0-BB4B-B37077E16076", "ABBAD0EA-29F8-43A0-BB4B-B37077E16076");
            SeedAnyUser(builder, "User", "DD3E7256-D5EA-49A7-A390-2E6EEF3DFEB3", "BB3E7256-D5EA-49A7-A390-2E6EEF3DFEB3");
        }

        private static void SeedAnyUser(ModelBuilder builder, string role, string roleId, string userId)
        {
            var upperRole = role.ToUpper();
            var user = new User
            {
                Id = userId,
                Email = $"{role}@{role}.com",
                FirstName = role,
                SecondName = role,
                UserName = role,
                NormalizedEmail = $"{upperRole}@{upperRole}.COM",
                NormalizedUserName = upperRole,
            };

            var passwordHasher = new PasswordHasher<User>();
            user.PasswordHash = passwordHasher.HashPassword(user, $"{role}123!");

            builder.Entity<User>().HasData(user);

            builder.Entity<UserRole>()
                .HasData(new UserRole()
                {
                    UserId = userId,
                    RoleId = roleId,
                });
        }
    }
}
