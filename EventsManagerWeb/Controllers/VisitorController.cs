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

        public ActionResult ViewMenus()
        {
            WebClient<MenuListVIewModel> client = new WebClient<MenuListVIewModel>
            {
                Server = CommonParameters.Location.WebService,
                Controller = "Visitor",
                Method = "GetMenus"
            };
            MenuListVIewModel Menus = client.Get();
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

        public ActionResult ViewHome()
        {
            return View();
        }
        public ActionResult ViewLogin()
        {
            return View();
        }
        public ActionResult ViewRegister()
        {
            return View();
        }
    }
}