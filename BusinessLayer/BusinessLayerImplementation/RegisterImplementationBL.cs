using BusinessLayer.Helper;
using BusinessLayer.IBusiness;
using Microsoft.Extensions.Logging;
using Model.DTOs.UserDTOs.RegisterDTOS;
using RepositoryLayer.IRepository;
using Microsoft.Extensions.Logging;

namespace BusinessLayer.BusinessLayerImplementation
{
    public class RegisterImplementationBL : IRegisterBL
    {
        private readonly IRegisterRL registerRL;

        private readonly PasswordHelper passwordHelper;

        private readonly ILogger <RegisterImplementationBL> logger;

        public RegisterImplementationBL
        (
            IRegisterRL registerRL,
            PasswordHelper passwordHelper,
            ILogger <RegisterImplementationBL> logger
            
        )
        {
            this.registerRL = registerRL;
            this.passwordHelper = passwordHelper;
            this.logger = logger;
        }

        public async Task<string> Register(RegisterDTOS registerDTOS)
        {

            try
            {
                logger.LogInformation("Registration started for Email:{Email}", registerDTOS.Email);


            
            registerDTOS.Password =
                passwordHelper.HashPassword(registerDTOS.Password);


                logger.LogInformation("Paswrd hashed succesfuly");


              
            

            var result =
                await registerRL.Register(registerDTOS);

                if (result == null)
                {
                    logger.LogWarning("Email already exists : {Email}", registerDTOS.Email);

                    return "Email Already Exists";
                }

                logger.LogInformation("User Registered Successfully : {Email}", registerDTOS.Email);

            return "User Registered Successfully";
        }


            catch (Exception ex)
            {

                logger.LogError(ex, "Exception Occurred During Registration For Email", registerDTOS.Email);
                throw;
            }
}
    }
}