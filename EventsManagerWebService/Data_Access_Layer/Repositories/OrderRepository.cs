using EventsManagerModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace EventsManager.Data_Access_Layer
{
    public class OrderRepository : Repository, IRepository<Order>
    {
        public OrderRepository(DbContext dbContext, ILogger<OrderRepository> logger) : base(dbContext, logger) { }

        public bool Insert(Order model)
        {
            const string sql = """
                INSERT INTO Orders (MenuId, UserId, EventTypeId, HallId, NumOfPeople, EventDate, IsPaid, OrderDate)
                VALUES (@MenuId, @UserId, @EventTypeId, @HallId, @NumOfPeople, @EventDate, @IsPaid, @OrderDate)
                """;

            try
            {
                using IDbCommand cmd = dbContext.CreateCommand(sql);

                AddParameter(cmd, "@MenuId", model.MenuId);
                AddParameter(cmd, "@UserId", model.UserId);
                AddParameter(cmd, "@EventTypeId", model.EventTypeId);
                AddParameter(cmd, "@HallId", model.HallId);
                AddParameter(cmd, "@NumOfPeople", model.NumOfPeople);
                AddParameter(cmd, "@EventDate", model.EventDate);
                AddParameter(cmd, "@IsPaid", model.IsPaid);
                AddParameter(cmd, "@OrderDate", model.OrderDate);

                return cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "INSERT Order failed {@Order}", model);
                throw;
            }
        }

        public Task<bool> InsertAsync(Order model) => InsertAsyncDefault(model, Insert);

        public Order? Read(object id)
        {
            const string sql = "SELECT * FROM Orders WHERE OrderId=@OrderId";

            try
            {
                using IDbCommand cmd = dbContext.CreateCommand(sql);
                AddParameter(cmd, "@OrderId", id);

                using IDataReader reader = cmd.ExecuteReader();

                if (!reader.Read())
                {
                    logger.LogWarning("Order with ID {Id} not found", id);
                    return null;
                }

                return modelFactory.OrderModelCreator.CreateModel(reader);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "READ Order failed for OrderId={Id}", id);
                throw;
            }
        }

        public Task<Order> ReadAsync(object id) => ReadAsyncDefault(id, Read);

        public List<Order> ReadAll()
        {
            const string sql = "SELECT * FROM Orders ORDER BY OrderId ASC";
            List<Order> orders = new();

            using IDbCommand cmd = dbContext.CreateCommand(sql);
            using IDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                orders.Add(modelFactory.OrderModelCreator.CreateModel(reader));
            }

            return orders;
        }

        public Task<List<Order>> ReadAllAsync() => ReadAllAsyncDefault(ReadAll);

        public List<Order> ReadByHallId(int id)
        {
            const string sql = "SELECT * FROM Orders WHERE HallId=@HallId";
            List<Order> orders = new();

            try
            {
                using IDbCommand cmd = dbContext.CreateCommand(sql);
                AddParameter(cmd, "@HallId", id);

                using IDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    orders.Add(modelFactory.OrderModelCreator.CreateModel(reader));
                }

                return orders;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "ReadByHallId failed for HallId={Id}", id);
                throw;
            }
        }

        public List<Order> ReadByUserId(int id)
        {
            const string sql = "SELECT * FROM Orders WHERE UserId=@UserId ORDER BY OrderId ASC";
            List<Order> orders = new();

            try
            {
                using IDbCommand cmd = dbContext.CreateCommand(sql);
                AddParameter(cmd, "@UserId", id);

                using IDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    orders.Add(modelFactory.OrderModelCreator.CreateModel(reader));
                }

                return orders;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "ReadByUserId failed for UserId={Id}", id);
                throw;
            }
        }

        public bool Update(Order model)
        {
            const string sql = """
                UPDATE Orders
                SET NumOfPeople=@NumOfPeople,
                    OrderDate=@OrderDate,
                    MenuId=@MenuId,
                    HallId=@HallId,
                    UserId=@UserId,
                    EventTypeId=@EventTypeId,
                    EventDate=@EventDate,
                    IsPaid=@IsPaid
                WHERE OrderId=@OrderId
                """;

            try
            {
                using IDbCommand cmd = dbContext.CreateCommand(sql);

                AddParameter(cmd, "@OrderId", model.OrderId);
                AddParameter(cmd, "@NumOfPeople", model.NumOfPeople);
                AddParameter(cmd, "@OrderDate", model.OrderDate);
                AddParameter(cmd, "@MenuId", model.MenuId);
                AddParameter(cmd, "@HallId", model.HallId);
                AddParameter(cmd, "@UserId", model.UserId);
                AddParameter(cmd, "@EventTypeId", model.EventTypeId);
                AddParameter(cmd, "@EventDate", model.EventDate);
                AddParameter(cmd, "@IsPaid", model.IsPaid);

                return cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "UPDATE Order failed {@Order}", model);
                throw;
            }
        }

        public Task<bool> UpdateAsync(Order model) => UpdateAsyncDefault(model, Update);

        public bool Delete(int id)
        {
            const string sql = "DELETE FROM Orders WHERE OrderId=@OrderId";

            using IDbCommand cmd = dbContext.CreateCommand(sql);
            AddParameter(cmd, "@OrderId", id);

            return cmd.ExecuteNonQuery() > 0;
        }

        public Task<bool> DeleteAsync(int id) => DeleteAsyncDefault(id, Delete);

        public bool Delete(string id)
        {
            return false;
        }

        public bool Delete(Order model)
        {
            const string sql = "DELETE FROM Orders WHERE OrderId=@OrderId";

            using IDbCommand cmd = dbContext.CreateCommand(sql);
            AddParameter(cmd, "@OrderId", model.OrderId);

            return cmd.ExecuteNonQuery() > 0;
        }

        public Task<bool> DeleteAsync(Order model) => DeleteAsyncDefault(model, Delete);

        public object ReadValue()
        {
            throw new NotImplementedException();
        }
    }
}