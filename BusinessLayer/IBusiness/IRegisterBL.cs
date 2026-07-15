using Model.DTOs.UserDTOs.RegisterDTOS;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.IBusiness
{
    public interface IRegisterBL
    {

        Task<string> Register(RegisterDTOS RegisterDTOS);
    }
}
