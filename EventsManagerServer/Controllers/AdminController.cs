using EventsManager.Data_Access_Layer;
using EventsManagerModels;
using PasswordManager;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.UI.WebControls;
using Menu = EventsManagerModels.Menu;

namespace EventsManager.Controllers
{
    public class AdminController : ApiController
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

        [HttpPost]
        [ActionName("UpdateUser")]
        public bool Update(User user)
        {
            DbContext dbContext = OleDbContext.GetInstance();
            LibraryUnitOfWork libraryUnitOfWork = new LibraryUnitOfWork(dbContext);
            try
            {
                dbContext.OpenConnection();
                return libraryUnitOfWork.UserRepository.UpdateInfo(user);
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
        public List<User> GetUsers()
        {
            DbContext dbContext = OleDbContext.GetInstance();
            LibraryUnitOfWork libraryUnitOfWork = new LibraryUnitOfWork(dbContext);
            try
            {
                dbContext.OpenConnection();
                List<User> users = libraryUnitOfWork.UserRepository.ReadAll();
                foreach (User user in users)
                {
                    user.UserType = libraryUnitOfWork.UserRepository.UserTypeByUserId(user.UserId.ToString());
                }
                return users;
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

        [HttpGet]
        public User UserDetails(int id)
        {
            DbContext dbContext = OleDbContext.GetInstance();
            LibraryUnitOfWork libraryUnitOfWork = new LibraryUnitOfWork(dbContext);
            try
            {
                dbContext.OpenConnection();
                return libraryUnitOfWork.UserRepository.Read(id);
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

        [HttpGet]
        public List<Order> GetOrders()
        {
            DbContext dbContext = OleDbContext.GetInstance();
            LibraryUnitOfWork libraryUnitOfWork = new LibraryUnitOfWork(dbContext);
            try
            {
                dbContext.OpenConnection();
                List<Order> orders = libraryUnitOfWork.OrderRepository.ReadAll();
                foreach (Order order in orders) 
                { 
                    order.ChosenMenu = libraryUnitOfWork.MenuRepository.Read(order.MenuId);
                    order.ChosenHall = libraryUnitOfWork.HallRepository.Read(order.HallId);
                    order.ChosenUser = libraryUnitOfWork.UserRepository.Read(order.UserId);
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


        [HttpGet]
        [EnableCors(origins: "https://localhost:44365", headers: "*", methods: "*")]
        public List<Menu> GetMenus()
        {
            DbContext dbContext = OleDbContext.GetInstance();
            LibraryUnitOfWork libraryUnitOfWork = new LibraryUnitOfWork(dbContext);
            try
            {
                dbContext.OpenConnection();
                List<Menu> menus = libraryUnitOfWork.MenuRepository.ReadAll();
                foreach (Menu menu in menus)
                {
                    menu.ChosenHall = libraryUnitOfWork.HallRepository.Read(menu.HallId);
                }
                return menus;
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
                return null;
            }
            finally
            {
                dbContext.CloseConnection();
            }
        }

        [HttpPost]
        public bool NewOrder(Order order)
        {
            DbContext dbContext = OleDbContext.GetInstance();
            LibraryUnitOfWork libraryUnitOfWork = new LibraryUnitOfWork(dbContext);
            try
            {
                dbContext.OpenConnection();
                return libraryUnitOfWork.OrderRepository.Insert(order);
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
    }
}