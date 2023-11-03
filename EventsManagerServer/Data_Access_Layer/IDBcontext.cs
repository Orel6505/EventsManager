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
        void OpenConnection();
        void CloseConnection();

        void CreateCommand();

        void BeginTransaction();

        void DeleteCommand();

        //CRUD = Create Read Update Delete
        IDataReader Read(string sql);

        bool Create(string sql);

        bool update(string sql);

        bool delete(string sql);

    }
}
