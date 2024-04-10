using EventsManagerModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.DynamicData;
using System.Web.UI.WebControls;

namespace EventsManager.Data_Access_Layer
{
    public class FoodRepository : Repository, IRepository<Food>
    {

        public FoodRepository(DbContext dbContext) : base(dbContext) { }

        public bool Delete(int id)
        {
            string sql = $"DELETE FROM Foods WHERE FoodId=@FoodId";
            this.AddParameters("FoodId", id.ToString()); //prevents SQL Injection
            return this.dbContext.Delete(sql) > 0;
        }

        public bool Delete(string id)
        {
            return false;
        }

        public bool Delete(Food model)
        {
            string sql = $"DELETE FROM Foods WHERE FoodId=@FoodId";
            this.AddParameters("FoodId", model.FoodId.ToString()); //prevents SQL Injection
            return this.dbContext.Delete(sql) > 0;
        }

        public List<Food> ReadAll()
        {
            List<Food> Foods = new List<Food>();
            string sql = "SELECT * FROM Foods";
            using (IDataReader dataReader = this.dbContext.Read(sql))
                while (dataReader.Read() == true)
                    Foods.Add(this.modelFactory.FoodModelCreator.CreateModel(dataReader));
            return Foods;
        }

        public bool Insert(Food model)
        {
            string sql = $"INSERT INTO Foods(FoodName,FoodDesc,FoodImage,FoodPrice,FoodTypeId) VALUES(@FoodName,@FoodDesc,@FoodImage,@FoodPrice,@FoodTypeId)";
            this.AddParameters("FoodName", model.FoodName); //prevents SQL Injection
            this.AddParameters("FoodDesc", model.FoodDesc); //prevents SQL Injection
            this.AddParameters("FoodImage", model.FoodImage); //prevents SQL Injection
            this.AddParameters("FoodPrice", model.FoodPrice.ToString()); //prevents SQL Injection
            this.AddParameters("FoodTypeId", model.FoodPrice.ToString()); //prevents SQL Injection
            return this.dbContext.Create(sql) > 0;
        }

        public Food Read(object id)
        {
            string sql = $"SELECT * FROM Foods WHERE FoodId=@FoodId";
            this.AddParameters("FoodId", id.ToString()); //prevents SQL Injection
            using (IDataReader dataReader = this.dbContext.Read(sql))
            {
                dataReader.Read();
                return this.modelFactory.FoodModelCreator.CreateModel(dataReader);
            }
            //returns food
        }

        public List<Food> GetFoodsByMenuId(object id)
        {
            List<Food> Foods = new List<Food>();
            string sql = $"SELECT * FROM Foods INNER JOIN FoodMenu ON Foods.FoodId = FoodMenu.FoodId WHERE FoodMenu.MenuId=@MenuId;";
            this.AddParameters("MenuId", id.ToString()); //prevents SQL Injection
            using (IDataReader dataReader = this.dbContext.Read(sql))
                while (dataReader.Read() == true)
                    Foods.Add(this.modelFactory.FoodModelCreator.CreateModel(dataReader));
            return Foods;
            //returns food
        }

        public object ReadValue()
        {
            throw new NotImplementedException();
        }

        public bool Update(Food model)
        {
            string sql = "UPDATE Foods SET FoodName=@FoodName, FoodDesc=@FoodDesc, FoodImage=@FoodId, FoodTypeId=@FoodTypeId where FoodId=@FoodId";
            this.AddParameters("FoodId", model.FoodId.ToString()); //prevents SQL Injection
            this.AddParameters("FoodName", model.FoodName); //prevents SQL Injection
            this.AddParameters("FoodDesc", model.FoodDesc); //prevents SQL Injection
            this.AddParameters("FoodImage", model.FoodImage); //prevents SQL Injection
            this.AddParameters("FoodTypeId", model.FoodPrice.ToString()); //prevents SQL Injection
            return this.dbContext.Update(sql) > 0;
        }
    }
}