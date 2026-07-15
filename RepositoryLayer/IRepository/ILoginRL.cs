using Model.DTOs.UserDTOs.LogInDTOs;
using Model.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.IRepository
{
    public  interface ILoginRL
    {

        Task<UserModel> Login(LogInDto logInDto);

        Task<UserModel> GetUserByEmail(string email);

        Task<bool> UpdatePassword(
            int userId,
            string password);
    }
}
