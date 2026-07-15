using BusinessLayer.IBusiness;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model.DTOs.PasswordDTOs;
using Model.DTOs.UserDTOs.LogInDTOs;
using Model.Responses;
using System.Security.Claims;

namespace FundooNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IloginBL loginBL;
        private readonly ILogger<LoginController> logger;

        public LoginController
        (
            IloginBL loginBL,
            ILogger<LoginController> logger
        )
        {
            this.loginBL = loginBL;
            this.logger = logger;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LogInDto logInDto)
        {
            try
            {
                logger.LogInformation("Login Attenpt for  {Email}", logInDto.Email);
                var result = await loginBL.Login(logInDto);

                var response = new ResponseModel<LoginResponseDTO>
                {
                    Success = true,
                    Message = "Login successful",
                    Data = result
                };
                return Ok(response);
            }
            catch(Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return BadRequest(new ResponseModel<object>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                });
            
            }
        }


        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequestDTO dto)
        {
            try
            {
                logger.LogInformation( "Forgot Password Request Received For {Email}",dto.Email);

                var result =await loginBL.ForgotPassword(dto.Email);

                var response = new ResponseModel<object>
                    {
                        Success = true,
                        Message = result,
                        Data = null
                    };

                return Ok(response);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);

                return BadRequest(
                    new ResponseModel<object>
                    {
                        Success = false,
                        Message = ex.Message,
                        Data = null
                    });
            }
        }

        [Authorize]
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(
      ResetPasswordRequestDTO dto)
        {
            try
            {
                int userId = Convert.ToInt32(
                    User.FindFirst("UserId")?.Value);

                logger.LogInformation(
                    "Reset Password Request Received For UserId : {UserId}",
                    userId);

                var result =
                    await loginBL.ResetPassword(
                        userId,
                        dto);

                var response =
                    new ResponseModel<object>
                    {
                        Success = true,
                        Message = result,
                        Data = null
                    };

                return Ok(response);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);

                return BadRequest(
                    new ResponseModel<object>
                    {
                        Success = false,
                        Message = ex.Message,
                        Data = null
                    });
            }
        }
    }
}