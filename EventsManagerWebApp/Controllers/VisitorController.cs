using EventsManagerModels;
using EventsManagerWebService;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EventsManagerWebApp;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Newtonsoft.Json.Linq;
using System.Net;

namespace EventsManagerWeb.Controllers
{
    [OutputCache(Duration = 0)]
    [Route("[Action]")]
    [Controller]
    public class VisitorController : Controller
    {
        public readonly JWTManager jwtManager;
        public VisitorController(JWTManager jwtManager)
        {
            this.jwtManager = jwtManager;
        }
        public ActionResult Food(int FoodId)
        {
            WebClient<Food> client = new WebClient<Food>
            {
                Server = Environment.GetEnvironmentVariable("WEBSERVICE_ENVIRONMENT"),
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
                Server = Environment.GetEnvironmentVariable("WEBSERVICE_ENVIRONMENT"),
                Controller = "Visitor",
                Method = "GetMenus"
            };
            MenuListVIewModel Menus = client.Get();
            return View(Menus);
        }

        public ActionResult Halls()
        {
            WebClient<List<Hall>> client = new WebClient<List<Hall>>
            {
                Server = Environment.GetEnvironmentVariable("WEBSERVICE_ENVIRONMENT"),
                Controller = "Visitor",
                Method = "GetHalls"
            };
            List<Hall> Halls = client.Get();
            return View(Halls);
        }

        public ActionResult Hall(int id = 1)
        {
            WebClient<Hall> client = new WebClient<Hall>
            {
                Server = Environment.GetEnvironmentVariable("WEBSERVICE_ENVIRONMENT"),
                Controller = "Visitor",
                Method = "GetHallById"
            };
            client.AddKeyValue("id", id.ToString());
            Hall hall = client.Get();
            return View(hall);
        }

        public ActionResult Menu(int id = 1)
        {
            WebClient<Menu> client = new WebClient<Menu>
            {
                Server = Environment.GetEnvironmentVariable("WEBSERVICE_ENVIRONMENT"),
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
        public async Task<ActionResult> Login(string UserName, string Password)
        {
            WebClient<User> client = new WebClient<User>
            {
                Server = Environment.GetEnvironmentVariable("WEBSERVICE_ENVIRONMENT"),
                Controller = "Visitor",
                Method = "CheckLogin"
            };
            client.AddKeyValue("userName", UserName);
            client.AddKeyValue("password", Password);
            User user = await client.GetAsync();

            if (user == null)
            {
                return RedirectToAction("Login");
            }

            HttpContext.Session.SetString("UserName", user.UserName);
            HttpContext.Session.SetString("Email", user.Email);

            string token = jwtManager.GetToken(user);

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true, 
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddMinutes(30),
                Path = "/"
            };

            Response.Cookies.Append("Token", token, cookieOptions);

            return RedirectToAction("Home");
        }
        [HttpPost]
        public ActionResult LogOut()
        {
            HttpContext.Session.Clear();
            HttpContext.Session.Remove("Token");
            Response.Cookies.Append("Token", "");
            return RedirectToAction("Home", "Visitor");
        }
        public ActionResult Register()
        {
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Register(User user)
        {
            WebClient<User> client = new WebClient<User>
            {
                Server = Environment.GetEnvironmentVariable("WEBSERVICE_ENVIRONMENT"),
                Controller = "Registered",
                Method = "Register"
            };
            bool IsSuccess = client.Post(user);
            if (IsSuccess)
            {
                TempData["Login"] = true;
            }
            else
            {
                ViewBag["Login"] = false;
            }
            return Redirect(Request.Headers["Referer"].ToString());
        }
        public ActionResult Login2FA()
        {
            return View();
        }
    }
}