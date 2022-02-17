using BC.AuthenticationMicroservice.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BC.AuthenticationMicroservice.Repository
{
    public class ApplicationContext : IdentityDbContext<User>
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            SeedData(builder);
        }

        private static void SeedData(ModelBuilder builder)
        {
            IList<IdentityRole> roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = "5E17EAB5-C0D7-45FE-AD03-774E657CEBF3",
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole
                {
                    Id = "A6BAD0EA-29F8-43A0-BB4B-B37077E16076",
                    Name = "Master",
                    NormalizedName = "MASTER"
                },
                new IdentityRole
                {
                    Id = "DD3E7256-D5EA-49A7-A390-2E6EEF3DFEB3",
                    Name = "User",
                    NormalizedName = "USER"
                },
            };

            builder.Entity<IdentityRole>().HasData(roles);

            var adminUser = new User
            {
                Id = "5D5E025D-85FB-46DE-8FE4-6A7686981027",
                Email = "admin@admin.com",
                FirstName = "admin",
                SecondName = "admin",
                UserName = "admin",
                NormalizedEmail = "ADMIN@ADMIN.COM",
                NormalizedUserName = "ADMIN",
            };

            var passwordHasher = new PasswordHasher<User>();
            adminUser.PasswordHash = passwordHasher.HashPassword(adminUser, "Admin123!");

            builder.Entity<User>().HasData(adminUser);

            builder.Entity<IdentityUserRole<string>>()
                .HasData(new IdentityUserRole<string>()
                {
                    UserId = "5D5E025D-85FB-46DE-8FE4-6A7686981027",
                    RoleId = "5E17EAB5-C0D7-45FE-AD03-774E657CEBF3"
                });
        }
    }
}
