﻿using EventsManager.Data_Access_Layer;
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
                    order.menu = libraryUnitOfWork.MenuRepository.Read(order.MenuId);
                    order.EventType = libraryUnitOfWork.EventTypeRepository.Read(order.EventTypeId);
                }
                model.Menus = libraryUnitOfWork.MenuRepository.ReadAll();
                model.Foods = libraryUnitOfWork.FoodRepository.ReadAll();
                model.FoodTypes = libraryUnitOfWork.FoodTypeRepository.ReadAll();
                model.Halls = libraryUnitOfWork.HallRepository.ReadAll();
                foreach (EventsManagerModels.Menu menu in model.Menus)
                {
                    menu.FoodIds = libraryUnitOfWork.MenuRepository.GetFoodIdsBy(menu.MenuId);
                    menu.FoodTypeIds = libraryUnitOfWork.MenuRepository.GetFoodTypeIdsBy(menu.MenuId);
                }
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
    }
}