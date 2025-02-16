using EventsManager.Data_Access_Layer;
using EventsManagerModels;
using Microsoft.AspNetCore.Mvc;
using PasswordManager;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Web;

namespace EventsManager.Controllers
{
    [ApiController]
    [Route("[Controller]/[Action]")]
    public class RegisteredController : Controller
    {
        [HttpPost("[Controller]/Register")]
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

        [HttpPost("[Controller]/UpdateUser")]
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

        [HttpPost("[Controller]/UpdatePassword")]
        public bool UpdatePassword(User user)
        {
            DbContext dbContext = OleDbContext.GetInstance();
            LibraryUnitOfWork libraryUnitOfWork = new LibraryUnitOfWork(dbContext);
            try
            {
                dbContext.OpenConnection();
                user.UserPassword = new Password(user.TempPassword); 
                return libraryUnitOfWork.UserRepository.UpdatePassword(user);
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
        public bool CheckPassword(string UserId, string Password)
        {
            DbContext dbContext = OleDbContext.GetInstance();
            LibraryUnitOfWork libraryUnitOfWork = new LibraryUnitOfWork(dbContext);
            try
            {
                dbContext.OpenConnection();
                Password UserPassword = libraryUnitOfWork.UserRepository.GetPasswordByUserId(UserId);
                return UserPassword.IsSamePassword(Password);
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
        public OrderListVIewModel GetOrders(int UserId)
        {
            OrderListVIewModel model = new OrderListVIewModel();
            DbContext dbContext = OleDbContext.GetInstance();
            LibraryUnitOfWork libraryUnitOfWork = new LibraryUnitOfWork(dbContext);
            try
            {
                dbContext.OpenConnection();
                model.Orders = libraryUnitOfWork.OrderRepository.ReadByUserId(UserId);
                foreach (Order order in model.Orders)
                {
                    order.ChosenMenu = libraryUnitOfWork.MenuRepository.Read(order.MenuId);
                    order.EventType = libraryUnitOfWork.EventTypeRepository.Read(order.EventTypeId);
                }
                model.Halls = libraryUnitOfWork.HallRepository.ReadAll();
                return model;
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
        public bool NewRate(Rating rating)
        {
            DbContext dbContext = OleDbContext.GetInstance();
            LibraryUnitOfWork libraryUnitOfWork = new LibraryUnitOfWork(dbContext);
            try
            {
                dbContext.OpenConnection();

                // Insert the rating
                if (!libraryUnitOfWork.RatingRepository.Insert(rating))
                {
                    return false;
                }

                // Insert rating images (if any)
                if (rating.RatingImagesLocation.Count > 0)
                {
                    foreach (string file in rating.RatingImagesLocation)
                    {
                        RatingImage image = new RatingImage
                        {
                            ImageLocation = file,
                            RatingId = libraryUnitOfWork.RatingRepository.ReadByRatingDate(rating.RatingDate)
                        };

                        if (image.RatingId == 0)
                        {
                            // Handle the case when no valid RatingId is found
                            throw new InvalidOperationException("Invalid RatingId.");
                        }

                        libraryUnitOfWork.RatingImageRepository.Insert(image);
                    }
                }
                return true;
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
        public List<EventType> HallAvailability(string EventDate, string HallId)
        {
            DbContext dbContext = OleDbContext.GetInstance();
            LibraryUnitOfWork libraryUnitOfWork = new LibraryUnitOfWork(dbContext);
            try
            {
                dbContext.OpenConnection();
                return libraryUnitOfWork.EventTypeRepository.HallAvailability(EventDate, HallId);
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
        public NewOrderViewModel OrderMenus(string HallId)
        {
            NewOrderViewModel orderViewModel = new NewOrderViewModel();
            DbContext dbContext = OleDbContext.GetInstance();
            LibraryUnitOfWork libraryUnitOfWork = new LibraryUnitOfWork(dbContext);
            try
            {
                dbContext.OpenConnection();
                orderViewModel.Menus = libraryUnitOfWork.MenuRepository.GetAllByHallId(HallId);
                orderViewModel.OrderHall = libraryUnitOfWork.HallRepository.Read(HallId);
                return orderViewModel;
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