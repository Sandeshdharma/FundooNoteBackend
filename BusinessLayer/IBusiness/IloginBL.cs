using Model.DTOs.PasswordDTOs;
using Model.DTOs.UserDTOs.LogInDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.IBusiness
{
    public  interface IloginBL
    {
        Task<LoginResponseDTO> Login(LogInDto logInDto);

        Task<string> ForgotPassword(string email);

        Task<string> ResetPassword(
            int userId,
            ResetPasswordRequestDTO dto);
    }
}
