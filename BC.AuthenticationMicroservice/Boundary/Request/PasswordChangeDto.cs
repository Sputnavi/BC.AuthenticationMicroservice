using System.ComponentModel.DataAnnotations;

namespace BC.AuthenticationMicroservice.Boundary.Request
{
    public class PasswordChangeDto
    {
        [Required]
        public string OldPassword { get; set; }

        [Required]
        public string NewPassword { get; set; }
    }
}
