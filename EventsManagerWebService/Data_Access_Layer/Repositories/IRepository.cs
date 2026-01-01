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
        Task<bool> InsertAsync(T model);

        //Read
        List<T> ReadAll(); //all sets that contains a specific value
        Task<List<T>> ReadAllAsync();
        T Read(object id); //only a specific set
        Task<T> ReadAsync(object id);
        object ReadValue(); //only a specific value

        //Update
        bool Update(T model);
        Task<bool> UpdateAsync(T model);

        //Delete
        bool Delete(int id);
        Task<bool> DeleteAsync(int id);
        bool Delete(string id);
        bool Delete(T model);
        Task<bool> DeleteAsync(T model);
    }
}
