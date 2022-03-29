using System.ComponentModel.DataAnnotations;

namespace BC.AuthenticationMicroservice.Boundary.Request
{
    public class UserUpdateDto
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string SecondName { get; set; }
    }
}
