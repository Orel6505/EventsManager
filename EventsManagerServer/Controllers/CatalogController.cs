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
    public class CatalogController : ApiController
    {
        [HttpGet]
        public OrderCatalogViewModel GetCatalogViewModel()
        {
            OrderCatalogViewModel orderCatalogViewModel = new OrderCatalogViewModel();
            DbContext dbContext = OleDbContext.GetInstance();
            LibraryUnitOfWork libraryUnitOfWork = new LibraryUnitOfWork(dbContext);
            try
            {
                dbContext.OpenConnection();
                orderCatalogViewModel.Halls = libraryUnitOfWork.HallRepository.ReadAll();
                foreach(Hall hall in orderCatalogViewModel.Halls)
                {
                    hall.Ratings = libraryUnitOfWork.RatingRepository.ReadRatingsByHallIdId(hall.HallId);
                    foreach(Rating rating in hall.Ratings)
                    {
                        
                        rating.RatingImages = libraryUnitOfWork.RatingImageRepository.ReadByRatingId(rating.RatingId);
                    }
                    hall.Dates = libraryUnitOfWork.DateRepository.ReadByHallId(hall.HallId);
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
            return orderCatalogViewModel;
        }
    }
}