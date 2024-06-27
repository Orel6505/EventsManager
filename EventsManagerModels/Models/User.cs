using PasswordManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EventsManagerModels
{
    public class User
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string PhoneNum { get; set; }
        public string CreationDate { get; set; } //timestamp
        public UserType UserType { get; set; }
        public Password UserPassword { get; set; }
        public string TempPassword { get; set; }
        public int UserTypeId { get; set; }

        public List<Order> Orders { get; set; }
        public string FormAction { get; set; }
    }
}
