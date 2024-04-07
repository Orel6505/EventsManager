﻿using EventsManager.Data_Access_Layer;
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

namespace EventsManager.Controllers
{
    public class FoodController : ApiController
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
    }
}