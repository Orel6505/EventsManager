using EventsManagerModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EventsManager.Data_Access_Layer
{
    public class UserModelCreator : IOleDbModelCreator<User>
    {
        public User CreateModel(IDataReader source)
        {
            User user = new User()
            {
                UserId = Convert.ToInt16(source["UserId"]),
                FirstName = Convert.ToString(source["UserFirstName"]),
                LastName = Convert.ToString(source["UserLastName"]),
                UserName = Convert.ToString(source["UserName"]),
                PassWordHash = Convert.ToString(source["Password"]),
                Email = Convert.ToString(source["Email"]),
                Address = Convert.ToString(source["Address"]),
                PhoneNum = Convert.ToString(source["PhoneNum"]),
                CreationDate = Convert.ToString(source["CreationDate"]),
                UserType = null,
                Orders = null
            };
            return user;
        }
    }
}
