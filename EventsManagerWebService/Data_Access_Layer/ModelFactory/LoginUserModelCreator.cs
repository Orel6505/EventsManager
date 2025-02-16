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
    public class LoginUserModelCreator : IOleDbModelCreator<User>
    {
        public User CreateModel(IDataReader source)
        {
            User User = new User()
            {
                UserName = Convert.ToString(source["UserName"]),
                UserPassword = new Password(Convert.ToString(source["PasswordHash"]), Convert.ToString(source["Salt"]))
            };
            return User;
        }
    }
}
