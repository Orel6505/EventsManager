using EventsManagerModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace EventsManager.Data_Access_Layer
{
    public class FoodTypeRepository : Repository, IRepository<FoodType>
    {
        public FoodTypeRepository(DbContext dbContext, ILogger<FoodTypeRepository> logger) : base(dbContext, logger) { }

        public bool Insert(FoodType model)
        {
            const string sql = """
                INSERT INTO FoodTypes(FoodTypeId,FoodTypeName)
                VALUES(@FoodTypeId,@FoodTypeName)
                """;

            try
            {
                using IDbCommand cmd = dbContext.CreateCommand(sql);

                AddParameter(cmd, "@FoodTypeId", model.FoodTypeId);
                AddParameter(cmd, "@FoodTypeName", model.FoodTypeName);

                return cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "INSERT FoodType failed {@FoodType}", model);
                throw;
            }
        }

        public Task<bool> InsertAsync(FoodType model) => InsertAsyncDefault(model, Insert);

        public FoodType? Read(object id)
        {
            const string sql = "SELECT * FROM FoodTypes WHERE FoodTypeId=@FoodTypeId";

            try
            {
                using IDbCommand cmd = dbContext.CreateCommand(sql);
                AddParameter(cmd, "@FoodTypeId", id);

                using IDataReader reader = cmd.ExecuteReader();

                if (!reader.Read())
                {
                    logger.LogWarning("FoodType with ID {Id} not found", id);
                    return null;
                }

                return modelFactory.FoodTypeModelCreator.CreateModel(reader);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "READ FoodType failed for FoodTypeId={Id}", id);
                throw;
            }
        }

        public Task<FoodType> ReadAsync(object id) => ReadAsyncDefault(id, Read);

        public List<FoodType> ReadAll()
        {
            const string sql = "SELECT * FROM FoodTypes";
            List<FoodType> foodTypes = new();

            using IDbCommand cmd = dbContext.CreateCommand(sql);
            using IDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                foodTypes.Add(modelFactory.FoodTypeModelCreator.CreateModel(reader));
            }

            return foodTypes;
        }

        public Task<List<FoodType>> ReadAllAsync() => ReadAllAsyncDefault(ReadAll);

        public bool Update(FoodType model)
        {
            const string sql = """
                UPDATE FoodTypes
                SET FoodTypeName=@FoodTypeName
                WHERE FoodTypeId=@FoodTypeId
                """;

            try
            {
                using IDbCommand cmd = dbContext.CreateCommand(sql);

                AddParameter(cmd, "@FoodTypeId", model.FoodTypeId);
                AddParameter(cmd, "@FoodTypeName", model.FoodTypeName);

                return cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "UPDATE FoodType failed {@FoodType}", model);
                throw;
            }
        }

        public Task<bool> UpdateAsync(FoodType model) => UpdateAsyncDefault(model, Update);

        public bool Delete(int id)
        {
            const string sql = "DELETE FROM FoodTypes WHERE FoodTypeId=@FoodTypeId";

            using IDbCommand cmd = dbContext.CreateCommand(sql);
            AddParameter(cmd, "@FoodTypeId", id);

            return cmd.ExecuteNonQuery() > 0;
        }

        public Task<bool> DeleteAsync(int id) => DeleteAsyncDefault(id, Delete);

        public bool Delete(string id)
        {
            return false;
        }

        public bool Delete(FoodType model)
        {
            const string sql = "DELETE FROM FoodTypes WHERE FoodTypeId=@FoodTypeId";

            using IDbCommand cmd = dbContext.CreateCommand(sql);
            AddParameter(cmd, "@FoodTypeId", model.FoodTypeId);

            return cmd.ExecuteNonQuery() > 0;
        }

        public Task<bool> DeleteAsync(FoodType model) => DeleteAsyncDefault(model, Delete);

        public object ReadValue()
        {
            throw new NotImplementedException();
        }
    }
}