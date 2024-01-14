using EventsManagerModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace EventsManager.Data_Access_Layer
{
    public class MenuRepository : Repository, IRepository<Menu>
    {
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
            string sql = $"INSERT INTO Menus(MenuName,MenuDesc,MenuImage,MenuPrice) VALUES(@MenuName,@MenuDesc,@MenuImage,@MenuPrice)";
            this.AddParameters("MenuName", model.MenuName); //prevents SQL Injection
            this.AddParameters("MenuDesc", model.MenuDesc); //prevents SQL Injection
            this.AddParameters("MenuImage", model.MenuImage); //prevents SQL Injection
            this.AddParameters("MenuPrice", model.MenuPrice.ToString()); //prevents SQL Injection
            return this.dbContext.Create(sql) > 0;
        }

        public Menu Read(object id)
        {
            string sql = $"SELECT FROM Menus WHERE MenuId=@MenuId";
            this.AddParameters("MenuId", id.ToString()); //prevents SQL Injection
            return this.modelFactory.MenuModelCreator.CreateModel(this.dbContext.Read(sql));
            //returns Menu
        }

        public List<Menu> ReadAll()
        {
            List<Menu> Menus = new List<Menu>();
            string sql = "SELECT * FROM Menus";
            IDataReader dataReader = this.dbContext.Read(sql);
            while (dataReader.Read() == true)
                Menus.Add(this.modelFactory.MenuModelCreator.CreateModel(dataReader));
            return Menus;
        }

        public object ReadValue()
        {
            throw new NotImplementedException();
        }

        public bool Update(Menu model)
        {
            string sql = "UPDATE Menus SET MenuName=@MenuName, MenuDesc=@MenuDesc, MenuImage=@MenuImage, MenuPrice=@MenuPrice where MenuId=@MenuId";
            this.AddParameters("MenuId", model.MenuId.ToString()); //prevents SQL Injection
            this.AddParameters("MenuName", model.MenuName); //prevents SQL Injection
            this.AddParameters("MenuDesc", model.MenuDesc); //prevents SQL Injection
            this.AddParameters("MenuImage", model.MenuImage); //prevents SQL Injection
            this.AddParameters("MenuPrice", model.MenuPrice.ToString()); //prevents SQL Injection
            return this.dbContext.Update(sql) > 0;
        }
    }
}