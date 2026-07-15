using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Helper
{
    public class PasswordHelper
    {
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string password, string hashPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashPassword);
        }
    }
}
 