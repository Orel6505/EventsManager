using EventsManagerModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;

namespace EventsManager.Data_Access_Layer
{
    public class MenuRepository : Repository, IRepository<Menu>
    {
        public MenuRepository(DbContext dbContext) : base(dbContext) { }

        public bool Delete(int id)
        {
            string sql = $"DELETE FROM Menus WHERE MenuId=@MenuId";
            this.AddParameters("MenuId", id.ToString()); //prevents SQL Injection
            return this.dbContext.Delete(sql) > 0;
        }

        public bool Delete(string id)
        {
            return false;
        }

        public bool Delete(Menu model)
        {
            string sql = $"DELETE FROM Menus WHERE MenuId=@MenuId";
            this.AddParameters("MenuId", model.MenuId.ToString()); //prevents SQL Injection
            return this.dbContext.Delete(sql) > 0;
        }

        public bool Insert(Menu model)
        {
            string sql = $"INSERT INTO Menus(MenuName,MenuDesc,MenuImage,HallId) VALUES(@MenuName,@MenuDesc,@MenuImage,@HallId)";
            this.AddParameters("MenuName", model.MenuName); //prevents SQL Injection
            this.AddParameters("MenuDesc", model.MenuDesc); //prevents SQL Injection
            this.AddParameters("MenuImage", model.MenuImage); //prevents SQL Injection
            this.AddParameters("HallId", model.HallId.ToString()); //prevents SQL Injection
            return this.dbContext.Create(sql) > 0;
        }

        public Menu Read(object id)
        {
            string sql = $"SELECT * FROM Menus WHERE MenuId=@MenuId";
            this.AddParameters("MenuId", id.ToString()); //prevents SQL Injection
            using (IDataReader dataReader = this.dbContext.Read(sql))
            {
                dataReader.Read();
                return this.modelFactory.MenuModelCreator.CreateModel(dataReader);
            }
            //returns Menu
        }

        public List<Menu> ReadAll()
        {
            List<Menu> Menus = new List<Menu>();
            string sql = "SELECT * FROM Menus";
            using (IDataReader dataReader = this.dbContext.Read(sql))
                while (dataReader.Read() == true)
                    Menus.Add(this.modelFactory.MenuModelCreator.CreateModel(dataReader));
            return Menus;
        }

        public List<int> GetFoodIdsBy(object id)
        {
            List<int> Foods = new List<int>();
            string sql = "SELECT FoodMenu.FoodId FROM Foods INNER JOIN FoodMenu ON Foods.FoodId = FoodMenu.FoodId WHERE FoodMenu.MenuId=@MenuId";
            this.AddParameters("MenuId", id.ToString()); //prevents SQL Injection
            using (IDataReader dataReader = this.dbContext.Read(sql))
                while (dataReader.Read() == true)
                    Foods.Add(Convert.ToInt16(dataReader["FoodId"]));
            return Foods;
        }
        public List<int> GetFoodTypeIdsBy(object id)
        {
            List<int> Foods = new List<int>();
            string sql = "SELECT FoodTypes.FoodTypeId\r\nFROM Menus INNER JOIN ((FoodTypes INNER JOIN Foods ON FoodTypes.FoodTypeId = Foods.FoodTypeId) INNER JOIN FoodMenu ON Foods.FoodId = FoodMenu.FoodId) ON Menus.MenuId = FoodMenu.MenuId\r\nWHERE (((Menus.MenuId)=[@MenuId]));\r\n";
            this.AddParameters("MenuId", id.ToString()); //prevents SQL Injection
            using (IDataReader dataReader = this.dbContext.Read(sql))
                while (dataReader.Read() == true)
                {
                    int tmp = Convert.ToInt16(dataReader["FoodTypeId"]);
                    if (Foods.IndexOf(tmp) < 0)
                    {
                        Foods.Add(tmp);
                    }
                }
            return Foods;
        }

        public object ReadValue()
        {
            throw new NotImplementedException();
        }

        public bool Update(Menu model)
        {
            string sql = "UPDATE Menus SET MenuName=@MenuName, MenuDesc=@MenuDesc, MenuImage=@MenuImage, HallId=@HallId where MenuId=@MenuId";
            this.AddParameters("MenuId", model.MenuId.ToString()); //prevents SQL Injection
            this.AddParameters("MenuName", model.MenuName); //prevents SQL Injection
            this.AddParameters("MenuDesc", model.MenuDesc); //prevents SQL Injection
            this.AddParameters("MenuImage", model.MenuImage); //prevents SQL Injection
            this.AddParameters("HallId", model.HallId.ToString()); //prevents SQL Injection
            return this.dbContext.Update(sql) > 0;
        }
    }
}