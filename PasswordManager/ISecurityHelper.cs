using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager
{
    public interface ISecurityHelper
    {
        string GenerateSalt(int Length);
        string GenerateHash(string Password, string Salt);
    }
}
