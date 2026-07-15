using System;
using System.Collections.Generic;
using System.Text;

namespace Model.DTOs.UserDTOs.LogInDTOs
{
    public class LoginResponseDTO
    {
        public string Token { get; set; }

        public int UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }
    }
}
