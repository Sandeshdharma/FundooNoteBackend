using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.IBusiness
{
    public interface IEmailService
    {

        Task SendEmail(string toEmail, string subject, string body);
    }
}
