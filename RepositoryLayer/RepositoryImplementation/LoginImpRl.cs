using Microsoft.EntityFrameworkCore;
using Model.DTOs.UserDTOs.LogInDTOs;
using Model.Entity;
using RepositoryLayer.Context;
using RepositoryLayer.IRepository;

namespace RepositoryLayer.RepositoryImplementation
{
    public class LoginImpRl : ILoginRL
    {
        private readonly NotesContext context;

        public LoginImpRl(NotesContext context)
        {
            this.context = context;
        }

        public async Task<UserModel> Login(LogInDto logInDto)
        {
            return await context.UserTbl
                .FirstOrDefaultAsync(
                    x => x.Email == logInDto.Email);
        }

        public async Task<UserModel> GetUserByEmail(string email)
        {
            return await context.UserTbl
                .FirstOrDefaultAsync(
                    x => x.Email == email);
        }

        public async Task<bool> UpdatePassword(
            int userId,
            string password)
        {
            var user = await context.UserTbl
                .FirstOrDefaultAsync(
                    x => x.UserId == userId);

            if (user == null)
            {
                return false;
            }

            user.Password = password;

            context.UserTbl.Update(user);

            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ForgotPassword(string email)
        {
            return await context.UserTbl
                .AnyAsync(x => x.Email == email);
        }

        public async Task<bool> ResetPassword(
            int userId,
            string newPassword)
        {
            return await UpdatePassword(
                userId,
                newPassword);
        }
    }
}