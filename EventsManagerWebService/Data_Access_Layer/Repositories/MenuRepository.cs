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
    public class MenuRepository : Repository, IRepository<Menu>
    {
        public MenuRepository(DbContext dbContext, ILogger<MenuRepository> logger) : base(dbContext, logger) { }

        public bool Insert(Menu model)
        {
            const string sql = """
                INSERT INTO Menus(MenuName,MenuDesc,MenuImage,HallId)
                VALUES(@MenuName,@MenuDesc,@MenuImage,@HallId)
                """;

            try
            {
                using IDbCommand cmd = dbContext.CreateCommand(sql);

                AddParameter(cmd, "@MenuName", model.MenuName);
                AddParameter(cmd, "@MenuDesc", model.MenuDesc);
                AddParameter(cmd, "@MenuImage", model.MenuImage);
                AddParameter(cmd, "@HallId", model.HallId);

                return cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "INSERT Menu failed {@Menu}", model);
                throw;
            }
        }

        public Task<bool> InsertAsync(Menu model) => InsertAsyncDefault(model, Insert);

        public Menu? Read(object id)
        {
            const string sql = "SELECT * FROM Menus WHERE MenuId=@MenuId";

            try
            {
                using IDbCommand cmd = dbContext.CreateCommand(sql);
                AddParameter(cmd, "@MenuId", id);

                using IDataReader reader = cmd.ExecuteReader();

                if (!reader.Read())
                {
                    logger.LogWarning("Menu with ID {Id} not found", id);
                    return null;
                }

                return modelFactory.MenuModelCreator.CreateModel(reader);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "READ Menu failed for MenuId={Id}", id);
                throw;
            }
        }

        public Task<Menu> ReadAsync(object id) => ReadAsyncDefault(id, Read);

        public List<Menu> ReadAll()
        {
            const string sql = "SELECT * FROM Menus";
            List<Menu> menus = new();

            using IDbCommand cmd = dbContext.CreateCommand(sql);
            using IDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                menus.Add(modelFactory.MenuModelCreator.CreateModel(reader));
            }

            return menus;
        }

        public Task<List<Menu>> ReadAllAsync() => ReadAllAsyncDefault(ReadAll);

        public List<Menu> GetAllByHallId(string HallId)
        {
            const string sql = """
                SELECT Menus.*
                FROM Menus
                WHERE HallId = @HallId
                  AND EXISTS (
                    SELECT 1
                    FROM FoodMenu
                    WHERE FoodMenu.MenuId = Menus.MenuId
                )
                """;

            List<Menu> menus = new();

            try
            {
                using IDbCommand cmd = dbContext.CreateCommand(sql);
                AddParameter(cmd, "@HallId", HallId);

                using IDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    menus.Add(modelFactory.MenuModelCreator.CreateModel(reader));
                }

                return menus;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "GetAllByHallId failed for HallId={HallId}", HallId);
                throw;
            }
        }

        public List<int> GetFoodIdsBy(object id)
        {
            const string sql = """
                SELECT FoodMenu.FoodId
                FROM Foods
                INNER JOIN FoodMenu ON Foods.FoodId = FoodMenu.FoodId
                WHERE FoodMenu.MenuId=@MenuId
                """;

            List<int> foods = new();

            try
            {
                using IDbCommand cmd = dbContext.CreateCommand(sql);
                AddParameter(cmd, "@MenuId", id);

                using IDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    foods.Add(Convert.ToInt32(reader["FoodId"]));
                }

                return foods;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "GetFoodIdsBy failed for MenuId={Id}", id);
                throw;
            }
        }

        public List<int> GetFoodTypeIdsBy(object id)
        {
            const string sql = """
                SELECT FoodTypes.FoodTypeId
                FROM Menus
                INNER JOIN ((FoodTypes INNER JOIN Foods ON FoodTypes.FoodTypeId = Foods.FoodTypeId)
                INNER JOIN FoodMenu ON Foods.FoodId = FoodMenu.FoodId) ON Menus.MenuId = FoodMenu.MenuId
                WHERE Menus.MenuId=@MenuId
                """;

            List<int> foodTypes = new();

            try
            {
                using IDbCommand cmd = dbContext.CreateCommand(sql);
                AddParameter(cmd, "@MenuId", id);

                using IDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    int tmp = Convert.ToInt32(reader["FoodTypeId"]);
                    if (!foodTypes.Contains(tmp))
                    {
                        foodTypes.Add(tmp);
                    }
                }

                return foodTypes;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "GetFoodTypeIdsBy failed for MenuId={Id}", id);
                throw;
            }
        }

        public bool Update(Menu model)
        {
            const string sql = """
                UPDATE Menus
                SET MenuName=@MenuName,
                    MenuDesc=@MenuDesc,
                    MenuImage=@MenuImage,
                    HallId=@HallId
                WHERE MenuId=@MenuId
                """;

            try
            {
                using IDbCommand cmd = dbContext.CreateCommand(sql);

                AddParameter(cmd, "@MenuId", model.MenuId);
                AddParameter(cmd, "@MenuName", model.MenuName);
                AddParameter(cmd, "@MenuDesc", model.MenuDesc);
                AddParameter(cmd, "@MenuImage", model.MenuImage);
                AddParameter(cmd, "@HallId", model.HallId);

                return cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "UPDATE Menu failed {@Menu}", model);
                throw;
            }
        }

        public Task<bool> UpdateAsync(Menu model) => UpdateAsyncDefault(model, Update);

        public bool Delete(int id)
        {
            const string sql = "DELETE FROM Menus WHERE MenuId=@MenuId";

            using IDbCommand cmd = dbContext.CreateCommand(sql);
            AddParameter(cmd, "@MenuId", id);

            return cmd.ExecuteNonQuery() > 0;
        }

        public Task<bool> DeleteAsync(int id) => DeleteAsyncDefault(id, Delete);

        public bool Delete(string id)
        {
            return false;
        }

        public bool Delete(Menu model)
        {
            const string sql = "DELETE FROM Menus WHERE MenuId=@MenuId";

            using IDbCommand cmd = dbContext.CreateCommand(sql);
            AddParameter(cmd, "@MenuId", model.MenuId);

            return cmd.ExecuteNonQuery() > 0;
        }

        public Task<bool> DeleteAsync(Menu model) => DeleteAsyncDefault(model, Delete);

        public object ReadValue()
        {
            throw new NotImplementedException();
        }
    }
}