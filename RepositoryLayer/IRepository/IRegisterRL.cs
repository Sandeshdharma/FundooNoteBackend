using Model.DTOs.UserDTOs.RegisterDTOS;
using Model.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.IRepository
{
    public interface IRegisterRL
    {

        Task<UserModel> Register(RegisterDTOS registerDTOS);

       
    }
}
