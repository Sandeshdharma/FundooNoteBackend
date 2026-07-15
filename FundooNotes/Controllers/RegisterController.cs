using BusinessLayer.IBusiness;
using Microsoft.AspNetCore.Mvc;
using Model.DTOs.UserDTOs.RegisterDTOS;
using Model.Responses;

namespace FundooNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly IRegisterBL registerBL;

        public RegisterController(IRegisterBL registerBL)
        {
            this.registerBL = registerBL;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDTOS registerDTOS)
        {
            var result = await registerBL.Register(registerDTOS);

            // Registration Failed
            if (result == "Email Already Exists")
            {
                return BadRequest(new ResponseModel<object>
                {
                    Success = false,
                    Message = result,
                    Data = null
                });
            }

            // Registration Successful
            return Ok(new ResponseModel<object>
            {
                Success = true,
                Message = result,
                Data = null
            });
        }
    }
}
