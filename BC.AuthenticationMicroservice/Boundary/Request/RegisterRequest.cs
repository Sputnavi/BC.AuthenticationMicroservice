using System.ComponentModel.DataAnnotations;

namespace BC.AuthenticationMicroservice.Boundary.Request
{
    public class RegisterRequest
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string SecondName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        [DataType(DataType.Password)]
        public string PasswordConfirm { get; set; }

        [Required]//ToDo k: add constraint to add only existring roles
        public string Role { get; set; }
    }
}
