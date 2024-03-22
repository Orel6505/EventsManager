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

namespace EventsManager.Controllers
{
    public class HallController : ApiController
    {
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