using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace EventsManager.Data_Access_Layer
{
    public abstract class DbContext : IDbContext
    {
        protected IDbConnection connection; // צינור לDATABASe
        protected IDbCommand command; // faucet
        protected IDbTransaction transaction; // send all sql command at once
        public void BeginTransaction()
        {
            this.transaction = this.connection.BeginTransaction();
        }

        public void CloseConnection()
        {
            if(this.connection.State == ConnectionState.Open)
                this.connection.Close(); //close connection
           // this.connection.Dispose(); //remove from memory
            this.command.Dispose(); //remove from memory
            this.transaction.Dispose(); //remove from memory
        }

        public bool Create(string sql)
        {
            throw new NotImplementedException();
        }

        public void CreateCommand()
        {
            this.command = this.connection.CreateCommand();
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
            if (this.connection.State == ConnectionState.Closed)
            {
                this.connection.Open();
            }
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