using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace EventsManager.Data_Access_Layer
{
    public class Repository
    {
        protected DbContext dbContext;
        protected ModelFactory modelFactory;
		protected readonly ILogger logger;

		public Repository(DbContext dbContext, ILogger logger)
		{
			this.dbContext = dbContext;
			this.modelFactory = new ModelFactory();
            this.logger = logger;
		}

		protected static void AddParameter(IDbCommand cmd, string name, object value)
		{
			IDbDataParameter param = cmd.CreateParameter();
			param.ParameterName = name;
			param.Value = value ?? DBNull.Value;
			cmd.Parameters.Add(param);
		}

		// Default async method implementations
		// Repositories can override these methods for specific behavior
		protected virtual Task<bool> InsertAsyncDefault<T>(T model, Func<T, bool> syncMethod)
		{
			return Task.Run(() => syncMethod(model));
		}

		protected virtual Task<T> ReadAsyncDefault<T>(object id, Func<object, T> syncMethod)
		{
			return Task.Run(() => syncMethod(id));
		}

		protected virtual Task<List<T>> ReadAllAsyncDefault<T>(Func<List<T>> syncMethod)
		{
			return Task.Run(() => syncMethod());
		}

		protected virtual Task<bool> UpdateAsyncDefault<T>(T model, Func<T, bool> syncMethod)
		{
			return Task.Run(() => syncMethod(model));
		}

		protected virtual Task<bool> DeleteAsyncDefault(int id, Func<int, bool> syncMethod)
		{
			return Task.Run(() => syncMethod(id));
		}

		protected virtual Task<bool> DeleteAsyncDefault<T>(T model, Func<T, bool> syncMethod)
		{
			return Task.Run(() => syncMethod(model));
		}
	}
}