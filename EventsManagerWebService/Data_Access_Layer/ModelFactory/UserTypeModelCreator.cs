using EventsManagerModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace EventsManager.Data_Access_Layer
{
    public class UserTypeModelCreator : IOleDbModelCreator<UserType>
    {
        public UserType CreateModel(IDataReader source)
        {
            UserType userType = new UserType()
            {
                UserTypeId = Convert.ToInt16(source["UserTypeId"]),
                UserTypeName = Convert.ToString(source["UserTypeName"]),
            };
            return userType;
        }
    }
}