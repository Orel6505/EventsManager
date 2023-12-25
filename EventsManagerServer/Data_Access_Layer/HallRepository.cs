using EventsManagerModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventsManager.Data_Access_Layer
{
    public class HallRepository : Repository, IRepository<Hall>
    {
        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public bool Delete(string id)
        {
            throw new NotImplementedException();
        }

        public bool Delete(Hall model)
        {
            throw new NotImplementedException();
        }

        public bool Insert(Hall model)
        {
            throw new NotImplementedException();
        }

        public Hall Read(object id)
        {
            throw new NotImplementedException();
        }

        public List<Hall> ReadAll()
        {
            throw new NotImplementedException();
        }

        public object ReadValue()
        {
            throw new NotImplementedException();
        }

        public bool Update(Hall model)
        {
            throw new NotImplementedException();
        }
    }
}