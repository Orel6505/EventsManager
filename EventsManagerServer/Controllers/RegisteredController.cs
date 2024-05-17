using EventsManager.Data_Access_Layer;
using EventsManagerModels;
using PasswordManager;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Http;
using System.Web.UI.WebControls;

namespace EventsManager.Controllers
{
    public class RegisteredController : ApiController
    {
        [HttpPost]
        public bool Register(User user)
        {
            DbContext dbContext = OleDbContext.GetInstance();
            LibraryUnitOfWork libraryUnitOfWork = new LibraryUnitOfWork(dbContext);
            try
            {
                dbContext.OpenConnection();
                user.UserPassword = new Password(user.TempPassword);
                user.CreationDate = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
                return libraryUnitOfWork.UserRepository.Insert(user);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
            }
            finally
            {
                dbContext.CloseConnection();
            }
            return false;
        }

        [HttpGet]
        public bool IsAvailableUserName(string UserName)
        {
            DbContext dbContext = OleDbContext.GetInstance();
            LibraryUnitOfWork libraryUnitOfWork = new LibraryUnitOfWork(dbContext);
            try
            {
                dbContext.OpenConnection();
                return !libraryUnitOfWork.UserRepository.IsAvailableUserName(UserName);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
            }
            finally
            {
                dbContext.CloseConnection();
            }
            return false;
        }

        [HttpGet]
        public List<Order> GetOrders(int UserId)
        {
            DbContext dbContext = OleDbContext.GetInstance();
            LibraryUnitOfWork libraryUnitOfWork = new LibraryUnitOfWork(dbContext);
            try
            {
                List<Order> orders;
                dbContext.OpenConnection();
                orders = libraryUnitOfWork.OrderRepository.ReadByUserId(UserId);
                foreach (Order order in orders)
                {
                    order.menu = libraryUnitOfWork.MenuRepository.Read(order.MenuId);
                    order.EventType = libraryUnitOfWork.EventTypeRepository.Read(order.EventTypeId);
                }
                return orders;
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
            }
            finally
            {
                dbContext.CloseConnection();
            }
            return null;
        }
    }
}