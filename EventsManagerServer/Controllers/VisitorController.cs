using EventsManager.Data_Access_Layer;
using EventsManagerModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.Http.Cors;
using PasswordManager;

namespace EventsManager.Controllers
{
    public class VisitorController : ApiController
    {
        [HttpGet]
        public List<Food> GetFoods()
        {
            List<Food> Foods;
            DbContext dbContext = OleDbContext.GetInstance();
            LibraryUnitOfWork libraryUnitOfWork = new LibraryUnitOfWork(dbContext);
            try
            {
                dbContext.OpenConnection();
                Foods = libraryUnitOfWork.FoodRepository.ReadAll();
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
            return Foods;
        }

        [HttpGet]
        public Food GetFoodById(int id)
        {
            DbContext dbContext = OleDbContext.GetInstance();
            LibraryUnitOfWork libraryUnitOfWork = new LibraryUnitOfWork(dbContext);
            Food food;
            try
            {
                dbContext.OpenConnection();
                food = libraryUnitOfWork.FoodRepository.Read(id);
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
            return food;
        }
        [HttpGet]
        public List<Hall> GetHalls()
        {
            List<Hall> Halls;
            DbContext dbContext = OleDbContext.GetInstance();
            LibraryUnitOfWork libraryUnitOfWork = new LibraryUnitOfWork(dbContext);
            try
            {
                dbContext.OpenConnection();
                Halls = libraryUnitOfWork.HallRepository.ReadAll();
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
            return Halls;
        }

        [HttpGet]
        public Hall GetHallById(int id)
        {
            DbContext dbContext = OleDbContext.GetInstance();
            LibraryUnitOfWork libraryUnitOfWork = new LibraryUnitOfWork(dbContext);
            Hall hall;
            try
            {
                dbContext.OpenConnection();
                hall = libraryUnitOfWork.HallRepository.Read(id);
                hall.Ratings = libraryUnitOfWork.RatingRepository.ReadRatingsByHallIdId(hall.HallId);
                foreach (Rating rating in hall.Ratings)
                {
                    rating.RatingImages = libraryUnitOfWork.RatingImageRepository.ReadByRatingId(rating.RatingId);
                }
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
            return hall;
        }

        [HttpGet]
        [EnableCors(origins: "https://localhost:44365", headers: "*", methods: "*")]
        public MenuListVIewModel GetMenus()
        {
            MenuListVIewModel model = new MenuListVIewModel();
            DbContext dbContext = OleDbContext.GetInstance();
            LibraryUnitOfWork libraryUnitOfWork = new LibraryUnitOfWork(dbContext);
            try
            {
                dbContext.OpenConnection();
                model.Menus = libraryUnitOfWork.MenuRepository.ReadAll();
                model.Foods = libraryUnitOfWork.FoodRepository.ReadAll();
                model.FoodTypes = libraryUnitOfWork.FoodTypeRepository.ReadAll();
                model.Halls = libraryUnitOfWork.HallRepository.ReadAll();
                foreach (EventsManagerModels.Menu menu in model.Menus)
                {
                    menu.FoodIds = libraryUnitOfWork.MenuRepository.GetFoodIdsBy(menu.MenuId);
                    menu.FoodTypeIds = libraryUnitOfWork.MenuRepository.GetFoodTypeIdsBy(menu.MenuId);
                }
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
            return model;
        }

        [HttpGet]
        public EventsManagerModels.Menu GetMenuById(int id = 1)
        {
            DbContext dbContext = OleDbContext.GetInstance();
            LibraryUnitOfWork libraryUnitOfWork = new LibraryUnitOfWork(dbContext);
            EventsManagerModels.Menu menu;
            try
            {
                dbContext.OpenConnection();
                menu = libraryUnitOfWork.MenuRepository.Read(id);
                menu.Foods = libraryUnitOfWork.FoodRepository.GetFoodsByMenuId(menu.MenuId);
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
            return menu;
        }
        [HttpGet]
        public User CheckLogin(string userName, string password)
        {
            DbContext dbContext = OleDbContext.GetInstance();
            LibraryUnitOfWork libraryUnitOfWork = new LibraryUnitOfWork(dbContext);
            User user;
            try
            {
                dbContext.OpenConnection();
                user = libraryUnitOfWork.UserRepository.GetLoginByUserName(userName);
                if (user == null)
                {
                    return null;
                }
                if (!user.UserPassword.IsSamePassword(password))
                {
                    return null;
                }
                user = libraryUnitOfWork.UserRepository.GetUser2FAByUserName(userName);
                return user;
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
        public UserType UserTypeByUserId(string UserId)
        {
            DbContext dbContext = OleDbContext.GetInstance();
            LibraryUnitOfWork libraryUnitOfWork = new LibraryUnitOfWork(dbContext);
            try
            {
                dbContext.OpenConnection();
                return libraryUnitOfWork.UserRepository.UserTypeByUserId(UserId);
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