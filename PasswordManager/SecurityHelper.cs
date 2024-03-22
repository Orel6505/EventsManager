using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager
{
    public class SecurityHelper : ISecurityHelper
    {
        /// <summary>Creates Random Salt for hashing password</summary>
        /// <returns>Returns random array of letters with length of the value given</returns>
        public string GenerateSalt(int Length)
        {            
            using (RandomNumberGenerator rnd = RandomNumberGenerator.Create())
            {
                byte[] sBytes = new byte[Length];
                rnd.GetBytes(sBytes);
                return BytesToString(sBytes);
            }
        }

        /// <summary>Takes <paramref name="Password"/> and <paramref name="Salt"/> and Hashes it with SHA256 Hash Algorithm</summary>
        /// <returns>Returns a hashed string</returns>
        public string GenerateHash(string Password, string Salt)
        {
            using (HashAlgorithm sha256 = SHA256.Create())
            {
                // ComputeHash - Generate hash from the provided Text and returns it as list of bytes
                byte[] bHash = sha256.ComputeHash(Encoding.UTF8.GetBytes(Password + Salt));
                // Convert byte[] to a string
                return BytesToString(bHash);
            }
        }

        public string BytesToString(byte[] Bytes)
        {
            string result = "";
            foreach (byte Byte in Bytes)
            {
                result += Byte.ToString("x2");
            }
            return result;
        }
    }
}
