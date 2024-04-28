using EventsManagerModels;
using EventsManagerWebService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace EventsManagerWeb.Controllers
{
    public class VisitorController : Controller
    {
        public ActionResult Food(int FoodId)
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

        public ActionResult Menus()
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

        public ActionResult Menu(int id = 1)
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

        public ActionResult Home()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string UserName, string Password)
        {
            WebClient<User> client = new WebClient<User>
            {
                Server = CommonParameters.Location.WebService,
                Controller = "Visitor",
                Method = "CheckLogin"
            };
            client.AddKeyValue("userName", UserName);
            client.AddKeyValue("password", Password);
            User user = client.Get();
            if (user != null)
            {
                Session["UserName"] = "Orel6505";
                return RedirectToAction("Home", "Visitor");
            }
            else
                return RedirectToAction("Menus", "Visitor");
        }
        [HttpPost]
        public ActionResult LogOut()
        {
            Session["UserName"] = null;
            return RedirectToAction("Home", "Visitor");
        }
        public ActionResult Register()
        {
            return View();
        }
        public ActionResult Login2FA()
        {
            return View();
        }
    }
}