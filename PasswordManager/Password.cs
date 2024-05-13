using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager
{
    [Serializable]
    [DataContract]
    public class Password : SecurityHelper
    {
        [DataMember]
        public string Salt { get; }
        [DataMember]
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

        public bool IsSamePassword(string EnteredPassword)
        {
            int count = 0;
            Byte[] EnteredBytes = Encoding.UTF8.GetBytes(GenerateHash(EnteredPassword, this.Salt));
            Byte[] PasswordBytes = Encoding.UTF8.GetBytes(this.HashPassword);
            for (int i=0; i<PasswordBytes.Length; i++)
            {
                if (PasswordBytes[i] != EnteredBytes[i])
                {
                    count++;
                }
            }
            return count == 0;
        }
    }
}
