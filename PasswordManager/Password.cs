using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager
{
    public class Password : SecurityHelper
    {
        public string Salt { get; }
        public string HashPassword { get; set; }
        public int SaltLength = 64;

        /// <summary> Creates object of password which generates new Password and Salt from inserting new <see cref="EnteredPassword"/></summary>
        public Password(string EnteredPassword)
        {
            this.Salt = GenerateSalt(SaltLength);
            this.HashPassword = GenerateHash(EnteredPassword, this.Salt);
        }

        /// <summary> Creates object of password which contains <see cref="Salt"/> and <see cref="HashPassword"/></summary>
        public Password(string HashPassword, string Salt)
        {
            this.HashPassword = HashPassword;
            this.Salt = Salt;
        }

        public void ApplyNewPassword(string EnteredPassword)
        {
            this.HashPassword = GenerateHash(EnteredPassword, this.Salt);
        }

        //TODO: Fix vulnerability to Timing attack
        public bool IsSamePassword(string EnteredPassword)
        {
            return this.HashPassword == GenerateHash(EnteredPassword, this.Salt);
        }
    }
}
