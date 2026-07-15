using Azure.Core;
using Model.DTOs.UserDTOs.RegisterDTOS;
using Model.Entity;
using RepositoryLayer.Context;
using RepositoryLayer.IRepository;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace RepositoryLayer.RepositoryImplementation
{
    public  class RegisterImpRL : IRegisterRL
    {

        private readonly NotesContext context;

        public RegisterImpRL(NotesContext context)
        {
            this.context = context;
        }

        public async Task<UserModel> Register(RegisterDTOS registerDTOS)
        {
            var existingUser = await context.UserTbl
                .FirstOrDefaultAsync(x => x.Email == registerDTOS.Email);

            if (existingUser != null)
            {
                return null;
            }

            UserModel user = new UserModel()
            {
                FirstName = registerDTOS.FirstName,
                LastName = registerDTOS.LastName,
                Email = registerDTOS.Email,
                Password = registerDTOS.Password
            };

            await context.UserTbl.AddAsync(user);

            await context.SaveChangesAsync();

            return user;
        }
    }
}


