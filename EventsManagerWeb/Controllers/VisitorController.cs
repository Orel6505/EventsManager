using EventsManagerModels;
using EventsManagerWebService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EventsManagerWeb.Controllers
{
    public class VisitorController : Controller
    {
        public ActionResult ViewFoods(int PageNumber=1, int ItemsPerPage=3)
        {
            WebClient<FoodListViewModel> client = new WebClient<FoodListViewModel>
            {
                Server = CommonParameters.Location.WebService,
                Controller = "Visitor",
                Method = "GetFoods"
            };
            FoodListViewModel Foods = client.Get();
            if (Foods != null)
            {
                Foods.PageNumber = PageNumber;
                Foods.ItemsPerPage = ItemsPerPage;
            }
            return View(Foods);
        }

        public ActionResult ViewFood(int FoodId)
        {
            WebClient<Food> client = new WebClient<Food>
            {
                Server = CommonParameters.Location.WebService,
                Controller = "Visitor",
                Method = "GetFoodById"
            };
            client.AddKeyValue("id", FoodId.ToString());
            Food food = client.Get();
            return View(food);
        }

        public ActionResult ViewMenus(int PageNumber = 1, int ItemsPerPage = 3)
        {
            WebClient<MenuListVIewModel> client = new WebClient<MenuListVIewModel>
            {
                Server = CommonParameters.Location.WebService,
                Controller = "Visitor",
                Method = "GetMenus"
            };
            MenuListVIewModel Menus = client.Get();
            if (Menus != null)
            {
                Menus.PageNumber = PageNumber;
                Menus.ItemsPerPage = ItemsPerPage;
            }
            return View(Menus);
        }

        public ActionResult ViewMenues(int PageNumber = 1, int ItemsPerPage = 3)
        {
            WebClient<MenuListVIewModel> client = new WebClient<MenuListVIewModel>
            {
                Server = CommonParameters.Location.WebService,
                Controller = "Visitor",
                Method = "GetMenus"
            };
            MenuListVIewModel Menus = client.Get();
            if (Menus != null)
            {
                Menus.PageNumber = PageNumber;
                Menus.ItemsPerPage = ItemsPerPage;
            }
            return View(Menus);
        }

        public ActionResult ViewMenu(int id = 1)
        {
            WebClient<Menu> client = new WebClient<Menu>
            {
                Server = CommonParameters.Location.WebService,
                Controller = "Visitor",
                Method = "GetMenuById"
            };
            client.AddKeyValue("id", id.ToString());
            Menu menu = client.Get();
            return View(menu);
        }
    }
}