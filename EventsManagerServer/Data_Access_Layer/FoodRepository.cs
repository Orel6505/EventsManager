using EventsManagerModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventsManager.Data_Access_Layer
{
    public class FoodRepository : Repository, IRepository<Food>
    {
        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public bool Delete(string id)
        {
            throw new NotImplementedException();
        }

        public List<Food> GetAll()
        {
            throw new NotImplementedException();
        }

        public bool Insert(Food model)
        {
            throw new NotImplementedException();
        }

        public Food Read(object id)
        {
            throw new NotImplementedException();
        }

        public object ReadValue()
        {
            throw new NotImplementedException();
        }

        public bool Update(Food model)
        {
            throw new NotImplementedException();
        }
    }
}