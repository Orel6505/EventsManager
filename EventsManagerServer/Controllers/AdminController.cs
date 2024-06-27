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
using System.Web.UI.WebControls;

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