using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsManager.Data_Access_Layer
{
    public interface IDbContext
    {
        //Data Access Layer
        //The layer that gets data from the Database 
        //CRUD = Create Read Update Delete

        //Create
        int Create(string sql);
        Task<int> CreateAsync(string sql);

        //Read
        IDataReader Read(string sql);
        Task<IDataReader> ReadAsync(string sql);
        object ReadValue(string sql);
        Task<object> ReadValueAsync(string sql);

        //Update
        int Update(string sql);
        Task<int> UpdateAsync(string sql);

        //Delete
        int Delete(string sql);
        Task<int> DeleteAsync(string sql);

        //
        void OpenConnection();
        Task OpenConnectionAsync();
        void CloseConnection();
        Task CloseConnectionAsync();

        void BeginTransaction();
        void EndTransaction();
        void RollbackTransaction();
    }
}
