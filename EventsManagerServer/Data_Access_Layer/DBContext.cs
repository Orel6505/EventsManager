using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace EventsManager.Data_Access_Layer
{
    public abstract class DbContext : IDbContext
    {
        protected IDbConnection connection;
        public void BeginTransaction()
        {
            throw new NotImplementedException();
        }

        public void CloseConnection()
        {
            throw new NotImplementedException();
        }

        public bool Create(string sql)
        {
            throw new NotImplementedException();
        }

        public void CreateCommand()
        {
            throw new NotImplementedException();
        }

        public bool Delete(string sql)
        {
            throw new NotImplementedException();
        }

        public void DeleteCommand()
        {
            throw new NotImplementedException();
        }

        public void EndTransaction()
        {
            throw new NotImplementedException();
        }

        public void OpenConnection()
        {
            throw new NotImplementedException();
        }

        public IDataReader Read(string sql)
        {
            throw new NotImplementedException();
        }

        public object ReadValue(string sql)
        {
            throw new NotImplementedException();
        }

        public bool Update(string sql)
        {
            throw new NotImplementedException();
        }
    }
}