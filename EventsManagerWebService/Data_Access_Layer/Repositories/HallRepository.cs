using EventsManagerModels;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace EventsManager.Data_Access_Layer
{
	public class HallRepository : Repository, IRepository<Hall>
	{
		public HallRepository(
			DbContext dbContext,
			ILogger<HallRepository> logger)
			: base(dbContext, logger)
		{
		}

		public bool Insert(Hall model)
		{
			const string sql = """
                INSERT INTO Halls (HallName, HallDesc, HallImage, MaxPeople)
                VALUES (@HallName, @HallDesc, @HallImage, @MaxPeople)
                """;

			try
			{
				using IDbCommand cmd = dbContext.CreateCommand(sql);

				AddParameter(cmd, "@HallName", model.HallName);
				AddParameter(cmd, "@HallDesc", model.HallDesc);
				AddParameter(cmd, "@HallImage", model.HallImage);
				AddParameter(cmd, "@MaxPeople", model.MaxPeople);

				return cmd.ExecuteNonQuery() > 0;
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "INSERT Hall failed {@Hall}", model);
				throw;
			}
		}

		public async Task<bool> InsertAsync(Hall model)
		{
			const string sql = """
                INSERT INTO Halls (HallName, HallDesc, HallImage, MaxPeople)
                VALUES (@HallName, @HallDesc, @HallImage, @MaxPeople)
                """;

			try
			{
				using IDbCommand cmd = dbContext.CreateCommand(sql);

				AddParameter(cmd, "@HallName", model.HallName);
				AddParameter(cmd, "@HallDesc", model.HallDesc);
				AddParameter(cmd, "@HallImage", model.HallImage);
				AddParameter(cmd, "@MaxPeople", model.MaxPeople);

				int result = await Task.Run(() => cmd.ExecuteNonQuery());
				return result > 0;
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "INSERT Hall failed {@Hall}", model);
				throw;
			}
		}

		public Hall? Read(object id)
		{
			const string sql = "SELECT * FROM Halls WHERE HallId=@HallId";

			try
			{
				using IDbCommand cmd = dbContext.CreateCommand(sql);
				AddParameter(cmd, "@HallId", id);

				using IDataReader reader = cmd.ExecuteReader();

				if (!reader.Read())
				{
					logger.LogWarning("Hall with ID {Id} not found", id);
					return null;
				}

				return modelFactory.HallModelCreator.CreateModel(reader);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "READ Hall failed for HallId={Id}", id);
				throw;
			}
		}

		public async Task<Hall?> ReadAsync(object id)
		{
			const string sql = "SELECT * FROM Halls WHERE HallId=@HallId";

			try
			{
				using IDbCommand cmd = dbContext.CreateCommand(sql);
				AddParameter(cmd, "@HallId", id);

				using IDataReader reader = await Task.Run(() => cmd.ExecuteReader());

				if (!reader.Read())
				{
					logger.LogWarning("Hall with ID {Id} not found", id);
					return null;
				}

				return modelFactory.HallModelCreator.CreateModel(reader);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "READ Hall failed for HallId={Id}", id);
				throw;
			}
		}

		public List<Hall> ReadAll()
		{
			const string sql = "SELECT * FROM Halls";
			List<Hall> halls = new();

			using IDbCommand cmd = dbContext.CreateCommand(sql);
			using IDataReader reader = cmd.ExecuteReader();

			while (reader.Read())
			{
				halls.Add(modelFactory.HallModelCreator.CreateModel(reader));
			}

			return halls;
		}

		public async Task<List<Hall>> ReadAllAsync()
		{
			const string sql = "SELECT * FROM Halls";
			List<Hall> halls = new();

			using IDbCommand cmd = dbContext.CreateCommand(sql);
			using IDataReader reader = await Task.Run(() => cmd.ExecuteReader());

			while (reader.Read())
			{
				halls.Add(modelFactory.HallModelCreator.CreateModel(reader));
			}

			return halls;
		}

		public bool Update(Hall model)
		{
			const string sql = """
                UPDATE Halls
                SET HallName=@HallName,
                    HallDesc=@HallDesc,
                    HallImage=@HallImage,
                    MaxPeople=@MaxPeople
                WHERE HallId=@HallId
                """;

			try
			{
				using IDbCommand cmd = dbContext.CreateCommand(sql);

				AddParameter(cmd, "@HallId", model.HallId);
				AddParameter(cmd, "@HallName", model.HallName);
				AddParameter(cmd, "@HallDesc", model.HallDesc);
				AddParameter(cmd, "@HallImage", model.HallImage);
				AddParameter(cmd, "@MaxPeople", model.MaxPeople);

				return cmd.ExecuteNonQuery() > 0;
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "UPDATE Hall failed {@Hall}", model);
				throw;
			}
		}

		public async Task<bool> UpdateAsync(Hall model)
		{
			const string sql = """
                UPDATE Halls
                SET HallName=@HallName,
                    HallDesc=@HallDesc,
                    HallImage=@HallImage,
                    MaxPeople=@MaxPeople
                WHERE HallId=@HallId
                """;

			try
			{
				using IDbCommand cmd = dbContext.CreateCommand(sql);

				AddParameter(cmd, "@HallId", model.HallId);
				AddParameter(cmd, "@HallName", model.HallName);
				AddParameter(cmd, "@HallDesc", model.HallDesc);
				AddParameter(cmd, "@HallImage", model.HallImage);
				AddParameter(cmd, "@MaxPeople", model.MaxPeople);

				int result = await Task.Run(() => cmd.ExecuteNonQuery());
				return result > 0;
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "UPDATE Hall failed {@Hall}", model);
				throw;
			}
		}

		public bool Delete(int id)
		{
			const string sql = "DELETE FROM Halls WHERE HallId=@HallId";

			using IDbCommand cmd = dbContext.CreateCommand(sql);
			AddParameter(cmd, "@HallId", id);

			return cmd.ExecuteNonQuery() > 0;
		}

		public async Task<bool> DeleteAsync(int id)
		{
			const string sql = "DELETE FROM Halls WHERE HallId=@HallId";

			using IDbCommand cmd = dbContext.CreateCommand(sql);
			AddParameter(cmd, "@HallId", id);

			int result = await Task.Run(() => cmd.ExecuteNonQuery());
			return result > 0;
		}

		public bool Delete(string id)
		{
			return false;
		}

		public bool Delete(Hall model)
		{
			const string sql = "DELETE FROM Halls WHERE HallId=@HallId";

			using IDbCommand cmd = dbContext.CreateCommand(sql);
			AddParameter(cmd, "@HallId", model.HallId);

			return cmd.ExecuteNonQuery() > 0;
		}

		public async Task<bool> DeleteAsync(Hall model)
		{
			const string sql = "DELETE FROM Halls WHERE HallId=@HallId";

			using IDbCommand cmd = dbContext.CreateCommand(sql);
			AddParameter(cmd, "@HallId", model.HallId);

			int result = await Task.Run(() => cmd.ExecuteNonQuery());
			return result > 0;
		}

		public object ReadValue()
		{
			throw new NotImplementedException();
		}
	}
}
