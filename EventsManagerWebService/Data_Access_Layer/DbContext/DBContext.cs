using System;
using System.Data;
using System.Threading.Tasks;

namespace EventsManager.Data_Access_Layer
{
	public abstract class DbContext : IDbContext
	{
		protected IDbConnection connection;
		protected IDbTransaction transaction;

		public IDbCommand CreateCommand(string sql)
		{
			if (connection.State != ConnectionState.Open)
				OpenConnection();

			IDbCommand cmd = connection.CreateCommand();
			cmd.CommandText = sql;

			if (transaction != null)
				cmd.Transaction = transaction;

			return cmd;
		}

		public void BeginTransaction()
		{
			if (connection.State != ConnectionState.Open)
				connection.Open();

			transaction = connection.BeginTransaction();
		}

		public void EndTransaction()
		{
			transaction?.Commit();
			transaction?.Dispose();
			transaction = null;
		}

		public void RollbackTransaction()
		{
			transaction?.Rollback();
			transaction?.Dispose();
			transaction = null;
		}

		public void OpenConnection()
		{
			if (connection.State != ConnectionState.Open)
				connection.Open();
		}

		public async Task OpenConnectionAsync()
		{
			if (connection.State != ConnectionState.Open)
				await Task.Run(() => connection.Open());
		}

		public void CloseConnection()
		{
			if (connection.State != ConnectionState.Closed)
				connection.Close();
		}

		public async Task CloseConnectionAsync()
		{
			if (connection.State != ConnectionState.Closed)
				await Task.Run(() => connection.Close());
		}

		// ===== CRUD =====

		public int Create(string sql)
		{
			using IDbCommand cmd = CreateCommand(sql);
			return cmd.ExecuteNonQuery();
		}

		public async Task<int> CreateAsync(string sql)
		{
			using IDbCommand cmd = CreateCommand(sql);
			return await Task.Run(() => cmd.ExecuteNonQuery());
		}

		public int Update(string sql)
		{
			using IDbCommand cmd = CreateCommand(sql);
			return cmd.ExecuteNonQuery();
		}

		public async Task<int> UpdateAsync(string sql)
		{
			using IDbCommand cmd = CreateCommand(sql);
			return await Task.Run(() => cmd.ExecuteNonQuery());
		}

		public int Delete(string sql)
		{
			using IDbCommand cmd = CreateCommand(sql);
			return cmd.ExecuteNonQuery();
		}

		public async Task<int> DeleteAsync(string sql)
		{
			using IDbCommand cmd = CreateCommand(sql);
			return await Task.Run(() => cmd.ExecuteNonQuery());
		}

		public IDataReader Read(string sql)
		{
			IDbCommand cmd = CreateCommand(sql);
			// Reader owns the connection lifetime
			return cmd.ExecuteReader(CommandBehavior.CloseConnection);
		}

		public async Task<IDataReader> ReadAsync(string sql)
		{
			IDbCommand cmd = CreateCommand(sql);
			return await Task.Run(() => cmd.ExecuteReader(CommandBehavior.CloseConnection));
		}

		public object ReadValue(string sql)
		{
			using IDbCommand cmd = CreateCommand(sql);
			return cmd.ExecuteScalar();
		}

		public async Task<object> ReadValueAsync(string sql)
		{
			using IDbCommand cmd = CreateCommand(sql);
			return await Task.Run(() => cmd.ExecuteScalar());
		}

		// ===== Parameters =====

		public void AddParameters(IDbCommand cmd, IDataParameter param)
		{
			cmd.Parameters.Add(param);
		}
	}
}
