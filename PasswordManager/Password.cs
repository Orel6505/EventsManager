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
        public string HashPassword { get; }

        /// <summary> Creates object of password which contains <see cref="Salt"/> and <see cref="HashPassword"/></summary>
        public Password(string EnteredPassword, int SaltLength)
        {
            this.Salt = this.GenerateSalt(SaltLength);
            this.HashPassword = this.GenerateHash(EnteredPassword, this.Salt);
        }

        /// <summary> Creates object of password which contains <see cref="Salt"/> and <see cref="HashPassword"/></summary>
        public Password(string HashPassword, string Salt)
        {
            this.HashPassword = HashPassword;
            this.Salt = Salt;
        }

        public bool IsSamePassword(string EnteredPassword)
        {
            return this.IsSamePassword(this.HashPassword, this.Salt, EnteredPassword);
        }
    }
}
