using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager
{
    public class Password
    {
        readonly private SecurityHelper Instance;
        public string Salt { get; }
        public string HashPassword { get; }

        /// <summary> Creates object of password which contains <see cref="Salt"/> and <see cref="HashPassword"/></summary>
        public Password(SecurityHelper Instance, string EnteredPassword, int SaltLength)
        {
            this.Instance = Instance;
            this.Salt = Instance.GenerateSalt(SaltLength);
            this.HashPassword = Instance.GenerateHash(EnteredPassword, this.Salt);
        }

        /// <summary> Creates object of password which contains <see cref="Salt"/> and <see cref="HashPassword"/></summary>
        public Password(SecurityHelper Instance, string Salt, string HashPassword)
        {
            this.Instance = Instance;
            this.Salt = Salt;
            this.HashPassword = HashPassword;
        }

        public bool IsSamePassword(string EnteredPassword)
        {
            return Instance.IsSamePassword(this.HashPassword, this.Salt, EnteredPassword);
        }
    }
}
