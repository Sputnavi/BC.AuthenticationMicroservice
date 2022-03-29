using Microsoft.AspNetCore.Identity;

namespace BC.AuthenticationMicroservice.Models
{
    public class Role : IdentityRole
    {
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
