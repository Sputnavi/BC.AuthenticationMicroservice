using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BC.AuthenticationMicroservice.Models
{
    public class User : IdentityUser
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string SecondName { get; set; }
    }
}
