using EventsManagerModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventsManager.Data_Access_Layer
{
    public class MenuRepository : Repository, IRepository<Menu>
    {
        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public bool Delete(string id)
        {
            throw new NotImplementedException();
        }

        public bool Delete(EventsManagerModels.Menu model)
        {
            throw new NotImplementedException();
        }

        public bool Insert(EventsManagerModels.Menu model)
        {
            throw new NotImplementedException();
        }

        public EventsManagerModels.Menu Read(object id)
        {
            throw new NotImplementedException();
        }

        public List<EventsManagerModels.Menu> ReadAll()
        {
            throw new NotImplementedException();
        }

        public object ReadValue()
        {
            throw new NotImplementedException();
        }

        public bool Update(EventsManagerModels.Menu model)
        {
            throw new NotImplementedException();
        }
    }
}