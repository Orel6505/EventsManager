using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsManager.Data_Access_Layer
{
    public interface IDBcontext
    {
        //Data Access Layer
        //The layer that gets data from the Database 
        //CRUD = Create Read Update Delete

        //Create
        bool Create(string sql); 

        //Read
        IDataReader Read(string sql);
        object ReadValue(string sql);

        //Update
        bool Update(string sql);

        //Delete
        bool Delete(string sql);

        //
        void OpenConnection();
        void CloseConnection();

        void CreateCommand();

        void BeginTransaction();
        void EndTransaction();

        void DeleteCommand();

    }
}
