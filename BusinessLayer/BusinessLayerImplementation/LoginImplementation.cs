using BusinessLayer.Helper;
using BusinessLayer.IBusiness;
using Microsoft.Extensions.Logging;
using Model.DTOs.PasswordDTOs;
using Model.DTOs.UserDTOs.LogInDTOs;
using RepositoryLayer.IRepository;

namespace BusinessLayer.BusinessLayerImplementation
{
    public class LoginImplementationBL : IloginBL
    {
        private readonly ILoginRL loginRL;
        private readonly PasswordHelper passwordHelper;
        private readonly JwtTokenHelper jwtTokenHelper;
        private readonly ILogger<LoginImplementationBL> logger;
        private readonly IEmailService emailService;

        public LoginImplementationBL
        (
            ILoginRL loginRL,
            PasswordHelper passwordHelper,
            JwtTokenHelper jwtTokenHelper,
            ILogger<LoginImplementationBL> logger,
            IEmailService emailService
        )
        {
            this.loginRL = loginRL;
            this.passwordHelper = passwordHelper;
            this.jwtTokenHelper = jwtTokenHelper;
            this.logger = logger;
            this.emailService = emailService;
        }

        public async Task<LoginResponseDTO> Login(LogInDto logInDto)
        {
            try
            {
                logger.LogInformation(
                    "Login Attempt Started For Email : {Email}",
                    logInDto.Email);

                var user = await loginRL.Login(logInDto);

                if (user == null)
                {
                    logger.LogWarning(
                        "Login Failed - User Not Found : {Email}",
                        logInDto.Email);

                    throw new Exception("User Not Found");
                }

                bool isValid = passwordHelper.VerifyPassword(
                    logInDto.Password,
                    user.Password);

                if (!isValid)
                {
                    logger.LogWarning(
                        "Login Failed - Invalid Password : {Email}",
                        logInDto.Email);

                    throw new Exception("Invalid Password");
                }

                string token = jwtTokenHelper.GenerateToken(user);

                var response = new LoginResponseDTO
                {
                    Token = token,
                    UserId = user.UserId,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email
                };

                logger.LogInformation(
                    "Login Successful For User : {Email}",
                    logInDto.Email);

                return response;
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Exception Occurred During Login For Email : {Email}",
                    logInDto.Email);

                throw;
            }
        }

        public async Task<string> ForgotPassword(string email)
        {
            try
            {
                logger.LogInformation(
                    "Forgot Password Request Received For Email : {Email}",
                    email);

                var user = await loginRL.GetUserByEmail(email);

                if (user == null)
                {
                    logger.LogWarning(
                        "Forgot Password Failed - User Not Found : {Email}",
                        email);

                    return "User Not Found";
                }

                string token =
                    jwtTokenHelper.GenerateResetToken(user);

                string resetLink =$"http://localhost:4200/reset-password?token={token}";
                await emailService.SendEmail(
                    email,
                    "Fundoo Notes Password Reset",
                    $"Click the link below to reset your password:<br/><br/><a href='{resetLink}'>{resetLink}</a>");

                logger.LogInformation(
                    "Password Reset Email Sent Successfully To : {Email}",
                    email);

                return "Reset Password Link Sent Successfully";
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Exception Occurred During Forgot Password For Email : {Email}",
                    email);

                throw;
            }
        }

        public async Task<string> ResetPassword(
            int userId,
            ResetPasswordRequestDTO dto)
        {
            try
            {
                logger.LogInformation(
                    "Reset Password Started For UserId : {UserId}",
                    userId);

                if (dto.NewPassword != dto.ConfirmPassword)
                {
                    logger.LogWarning(
                        "Password Mismatch For UserId : {UserId}",
                        userId);

                    throw new Exception("Password and Confirm Password must match.");
                }

                string hashedPassword =
                    passwordHelper.HashPassword(dto.NewPassword);

                bool result =
                    await loginRL.UpdatePassword(
                        userId,
                        hashedPassword);

                if (!result)
                {
                    logger.LogWarning(
                        "User Not Found During Password Reset : {UserId}",
                        userId);

                    throw new Exception("User Not Found.");
                }

                logger.LogInformation(
                    "Password Reset Successful For UserId : {UserId}",
                    userId);

                return "Password Reset Successfully";
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Exception Occurred During Password Reset For UserId : {UserId}",
                    userId);

                throw;
            }
        }
    }
}
