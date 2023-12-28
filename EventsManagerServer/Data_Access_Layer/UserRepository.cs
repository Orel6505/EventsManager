using EventsManagerModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventsManager.Data_Access_Layer
{
    public class UserRepository : Repository, IRepository<User>
    {
    }
}