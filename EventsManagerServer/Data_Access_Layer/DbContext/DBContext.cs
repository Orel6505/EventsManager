using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace EventsManager.Data_Access_Layer
{
    public abstract class DbContext : IDbContext
    {
        protected IDbConnection connection; // צינור לDATABASE
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
           // this.connection.Dispose(); //removes from memory
            this.command.Dispose(); //removes from memory
            if(this.transaction != null )
                this.transaction.Dispose(); //removes from memory
        }

        public int Create(string sql)
        {
            return ChangeDb(sql);
        }

        private int ChangeDb(string sql)
        {
            this.command.CommandText = sql;
            return this.command.ExecuteNonQuery(); //Changes columms in the database, and returns the number of columms it changed
        }

        public void CreateCommand()
        {
            this.command = this.connection.CreateCommand();
        }

        public int Delete(string sql)
        {
            return ChangeDb(sql);
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
                this.connection.Open(); //if connection is closed, then open it
            }
            CreateCommand();
        }

        public IDataReader Read(string sql)
        {
            this.command.CommandText = sql;
            return this.command.ExecuteReader(); //collects the data
        }

        public object ReadValue(string sql)
        {
            this.command.CommandText = sql;
            return this.command.ExecuteScalar(); //collects the single "drop" of data
        }

        public int Update(string sql)
        {
            return ChangeDb(sql);

        }
    }
}