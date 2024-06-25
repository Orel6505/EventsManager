using EventsManagerModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace EventsManager.Data_Access_Layer
{
    public class OrderRepository : Repository, IRepository<Order>
    {
        public OrderRepository(DbContext dbContext) : base(dbContext) { }

        public bool Delete(int id)
        {
            string sql = $"DELETE FROM Orders WHERE OrderId=@OrderId";
            this.AddParameters("OrderId", id.ToString()); //prevents SQL Injection
            return this.dbContext.Delete(sql) > 0;
        }

        public bool Delete(string id)
        {
            return false;
        }

        public bool Delete(Order model)
        {
            string sql = $"DELETE FROM Orders WHERE OrderId=@OrderId";
            this.AddParameters("OrderId", model.OrderId.ToString()); //prevents SQL Injection
            return this.dbContext.Delete(sql) > 0;
        }
        public bool Insert(Order model)
        {
            string sql = "INSERT INTO Orders (MenuId, UserId, EventTypeId, HallId, NumOfPeople, EventDate, IsPaid) VALUES (@MenuId, @UserId, @EventTypeId, @HallId, @NumOfPeople, @EventDate, @IsPaid)";
            this.AddParameters("MenuId", model.MenuId.ToString()); //prevents SQL Injection
            this.AddParameters("UserId", model.UserId.ToString()); //prevents SQL Injection
            this.AddParameters("EventTypeId", model.EventTypeId.ToString()); //prevents SQL Injection
            this.AddParameters("HallId", model.HallId.ToString()); //prevents SQL Injection
            this.AddParameters("NumOfPeople", model.NumOfPeople.ToString()); //prevents SQL Injection
            this.AddParameters("EventDate", model.EventDate.ToString()); //prevents SQL Injection
            this.AddParameters("IsPaid", model.IsPaid.ToString()); //prevents SQL Injection
            return this.dbContext.Create(sql) > 0;
        }

        public Order Read(object id)
        {
            string sql = $"SELECT * FROM Orders WHERE OrderId=@OrderId";
            this.AddParameters("OrderId", id.ToString()); //prevents SQL Injection
            using (IDataReader dataReader = this.dbContext.Read(sql))
            {
                dataReader.Read();
                return this.modelFactory.OrderModelCreator.CreateModel(dataReader);
            }
        }

        public List<Order> ReadAll()
        {
            List<Order> Orders = new List<Order>();
            string sql = "SELECT * FROM Orders";
            using (IDataReader dataReader = this.dbContext.Read(sql))
                while (dataReader.Read() == true)
                    Orders.Add(this.modelFactory.OrderModelCreator.CreateModel(dataReader));
            return Orders;
        }

        public List<Order> ReadByHallId(int id)
        {
            List<Order> Orders = new List<Order>();
            string sql = "SELECT * FROM Orders WHERE HallId=@HallId";
            this.AddParameters("HallId", id.ToString()); //prevents SQL Injection
            using (IDataReader dataReader = this.dbContext.Read(sql))
                while (dataReader.Read() == true)
                    Orders.Add(this.modelFactory.OrderModelCreator.CreateModel(dataReader));
            return Orders;
        }

        public List<Order> ReadByUserId(int id)
        {
            List<Order> Orders = new List<Order>();
            string sql = "SELECT * FROM Orders WHERE UserId=@UserId";
            this.AddParameters("UserId", id.ToString()); //prevents SQL Injection
            using (IDataReader dataReader = this.dbContext.Read(sql))
                while (dataReader.Read() == true)
                    Orders.Add(this.modelFactory.OrderModelCreator.CreateModel(dataReader));
            return Orders;
        }

        public object ReadValue()
        {
            throw new NotImplementedException();
        }

        public bool Update(Order model)
        {
            string sql = "UPDATE Orders SET NumOfPeople=@NumOfPeople, OrderDate=@OrderDate, MenuId=@MenuId, HallId=@HallId, UserId=@UserId, EventTypeId=@EventTypeId, EventDate=@EventDate, IsPaid=@IsPaid where OrderId=@OrderId";
            this.AddParameters("OrderId", model.OrderId.ToString()); //prevents SQL Injection
            this.AddParameters("NumOfPeople", model.NumOfPeople.ToString()); //prevents SQL Injection
            this.AddParameters("OrderDate", model.OrderDate); //prevents SQL Injection
            this.AddParameters("MenuId", model.MenuId.ToString()); //prevents SQL Injection
            this.AddParameters("HallId", model.HallId.ToString()); //prevents SQL Injection
            this.AddParameters("UserId", model.UserId.ToString()); //prevents SQL Injection
            this.AddParameters("IsPaid", model.IsPaid.ToString()); //prevents SQL Injection
            this.AddParameters("EventTypeId", model.EventTypeId.ToString()); //prevents SQL Injection
            this.AddParameters("EventDate", model.EventDate); //prevents SQL Injection
            return this.dbContext.Update(sql) > 0;
        }
    }
}