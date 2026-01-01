using EventsManagerModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;

namespace EventsManager.Data_Access_Layer
{
    public class FoodRepository : Repository, IRepository<Food>
    {
        public FoodRepository(DbContext dbContext, ILogger<FoodRepository> logger) : base(dbContext, logger) { }

        public bool Insert(Food model)
        {
            const string sql = """
                INSERT INTO Foods(FoodName,FoodDesc,FoodImage,FoodPrice,FoodTypeId)
                VALUES(@FoodName,@FoodDesc,@FoodImage,@FoodPrice,@FoodTypeId)
                """;

            try
            {
                using IDbCommand cmd = dbContext.CreateCommand(sql);

                AddParameter(cmd, "@FoodName", model.FoodName);
                AddParameter(cmd, "@FoodDesc", model.FoodDesc);
                AddParameter(cmd, "@FoodImage", model.FoodImage);
                AddParameter(cmd, "@FoodPrice", model.FoodPrice);
                AddParameter(cmd, "@FoodTypeId", model.FoodTypeId);

                return cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "INSERT Food failed {@Food}", model);
                throw;
            }
        }

        public Task<bool> InsertAsync(Food model) => InsertAsyncDefault(model, Insert);

        public Food? Read(object id)
        {
            const string sql = "SELECT * FROM Foods WHERE FoodId=@FoodId";

            try
            {
                using IDbCommand cmd = dbContext.CreateCommand(sql);
                AddParameter(cmd, "@FoodId", id);

                using IDataReader reader = cmd.ExecuteReader();

                if (!reader.Read())
                {
                    logger.LogWarning("Food with ID {Id} not found", id);
                    return null;
                }

                return modelFactory.FoodModelCreator.CreateModel(reader);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "READ Food failed for FoodId={Id}", id);
                throw;
            }
        }

        public Task<Food> ReadAsync(object id) => ReadAsyncDefault(id, Read);

        public List<Food> ReadAll()
        {
            const string sql = "SELECT * FROM Foods";
            List<Food> foods = new();

            using IDbCommand cmd = dbContext.CreateCommand(sql);
            using IDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                foods.Add(modelFactory.FoodModelCreator.CreateModel(reader));
            }

            return foods;
        }

        public Task<List<Food>> ReadAllAsync() => ReadAllAsyncDefault(ReadAll);

        public List<Food> GetFoodsByMenuId(object id)
        {
            const string sql = """
                SELECT *
                FROM Foods
                INNER JOIN FoodMenu ON Foods.FoodId = FoodMenu.FoodId
                WHERE FoodMenu.MenuId=@MenuId
                """;

            List<Food> foods = new();

            try
            {
                using IDbCommand cmd = dbContext.CreateCommand(sql);
                AddParameter(cmd, "@MenuId", id);

                using IDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    foods.Add(modelFactory.FoodModelCreator.CreateModel(reader));
                }

                return foods;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "GetFoodsByMenuId failed for MenuId={Id}", id);
                throw;
            }
        }

        public bool Update(Food model)
        {
            const string sql = """
                UPDATE Foods
                SET FoodName=@FoodName,
                    FoodDesc=@FoodDesc,
                    FoodImage=@FoodImage,
                    FoodPrice=@FoodPrice,
                    FoodTypeId=@FoodTypeId
                WHERE FoodId=@FoodId
                """;

            try
            {
                using IDbCommand cmd = dbContext.CreateCommand(sql);

                AddParameter(cmd, "@FoodId", model.FoodId);
                AddParameter(cmd, "@FoodName", model.FoodName);
                AddParameter(cmd, "@FoodDesc", model.FoodDesc);
                AddParameter(cmd, "@FoodImage", model.FoodImage);
                AddParameter(cmd, "@FoodPrice", model.FoodPrice);
                AddParameter(cmd, "@FoodTypeId", model.FoodTypeId);

                return cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "UPDATE Food failed {@Food}", model);
                throw;
            }
        }

        public Task<bool> UpdateAsync(Food model) => UpdateAsyncDefault(model, Update);

        public bool Delete(int id)
        {
            const string sql = "DELETE FROM Foods WHERE FoodId=@FoodId";

            using IDbCommand cmd = dbContext.CreateCommand(sql);
            AddParameter(cmd, "@FoodId", id);

            return cmd.ExecuteNonQuery() > 0;
        }

        public Task<bool> DeleteAsync(int id) => DeleteAsyncDefault(id, Delete);

        public bool Delete(string id)
        {
            return false;
        }

        public bool Delete(Food model)
        {
            const string sql = "DELETE FROM Foods WHERE FoodId=@FoodId";

            using IDbCommand cmd = dbContext.CreateCommand(sql);
            AddParameter(cmd, "@FoodId", model.FoodId);

            return cmd.ExecuteNonQuery() > 0;
        }

        public Task<bool> DeleteAsync(Food model) => DeleteAsyncDefault(model, Delete);

        public object ReadValue()
        {
            throw new NotImplementedException();
        }
    }
}