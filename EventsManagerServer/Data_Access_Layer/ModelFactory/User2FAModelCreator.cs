using EventsManagerModels;
using PasswordManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EventsManager.Data_Access_Layer
{
    public class User2FAModelCreator : IOleDbModelCreator<User>
    {
        public User CreateModel(IDataReader source)
        {
            User User = new User()
            {
                FirstName = Convert.ToString(source["FirstName"]),
                LastName = Convert.ToString(source["LastName"]),
                UserName = Convert.ToString(source["UserName"]),
                Email = Convert.ToString(source["Email"]),
                UserPassword = null
            };
            return User;
        }
    }
}
