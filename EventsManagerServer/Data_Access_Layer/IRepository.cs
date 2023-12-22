using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsManager.Data_Access_Layer
{
    public interface IRepository<T>
    {
        //this part is responsible for the logic part in Data access layer
        //CRUD - Create Read Update Delete

        //Create 
        bool Insert(T model);

        //Read
        List<T> GetAll(); //all sets that contains a specific value
        T Read(object id); //only a specific set
        object ReadValue(); //only a specific value

        //Update
        bool Update(T model);

        //Delete
        bool Delete(int id);
        bool Delete(string id);
    }
}
