using System;
using System.Collections.Generic;
using System.Text;

namespace Model.DTOs.PasswordDTOs
{
    public class ResetPasswordRequestDTO
    {

        public string NewPassword { get; set; }

        public string ConfirmPassword { get; set; }
    }
}
