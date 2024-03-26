using EventsManagerModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EventsManagerWebService;

namespace EventsManagerWeb.Controllers
{
    public class FoodController : Controller
    {
        public ActionResult ViewFoods(int pageNumber=1, int itemsperPge=100)
        {
            WebClient<FoodListViewModel> client = new WebClient<FoodListViewModel>
            {
                Server = CommonParam.ServerLocation,
                Controller = "Visitor",
                Method = "GetFoods"
            };
            FoodListViewModel Foods = client.Get();
            if (Foods != null)
            {
                Foods.PageNumber = pageNumber;
                Foods.ItemsPerpage = itemsperPge;
            }
            return View(Foods);
        }

        public ActionResult ViewFood(int FoodId)
        {
            WebClient<Food> client = new WebClient<Food>
            {
                Server = CommonParam.ServerLocation,
                Controller = "Food",
                Method = "GetFoodById"
            };
            client.AddKeyValue("id", FoodId.ToString());
            Food food = client.Get();
            return View(food);
        }
    }
}