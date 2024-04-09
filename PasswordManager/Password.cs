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
        public int SaltLength { get; set; }

        /// <summary> Creates object of password which generates new Password and Salt from inserting new <see cref="EnteredPassword"/></summary>
        public Password(string EnteredPassword, int SaltLength=64)
        {
            this.SaltLength = SaltLength;
            this.Salt = GenerateSalt(SaltLength);
            this.HashPassword = GenerateHash(EnteredPassword, this.Salt);
        }

        /// <summary> Creates object of password which contains <see cref="Salt"/> and <see cref="HashPassword"/></summary>
        public Password(string HashPassword, string Salt, int SaltLength=64)
        {
            this.SaltLength = SaltLength;
            this.Salt = Salt;
            this.HashPassword = HashPassword;
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
