﻿using System.ComponentModel.DataAnnotations;

namespace BC.AuthenticationMicroservice.Boundary.Request
{
    public class LoginRequest
    {
        [Required]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
