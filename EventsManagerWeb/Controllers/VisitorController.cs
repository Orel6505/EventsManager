﻿using EventsManagerModels;
using EventsManagerWebService;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.Remoting.Contexts;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace EventsManagerWeb.Controllers
{
    [OutputCache(Duration = 0)]
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

        public ActionResult Halls()
        {
            WebClient<List<Hall>> client = new WebClient<List<Hall>>
            {
                Server = CommonParameters.Location.WebService,
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
                Server = CommonParameters.Location.WebService,
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
                Session["UserName"] = user.UserName;
                Session["Email"] = user.Email;
                string token = GetToken(user);
                HttpCookie httpOnlyCookie = new HttpCookie("Token", token)
                {
                    SameSite = SameSiteMode.Lax,
                    HttpOnly = true,
                    Secure = true,
                    Expires = DateTime.MinValue,
                };
                Response.SetCookie(httpOnlyCookie);
                return RedirectToAction("Home", "Visitor");
            }
            return Redirect(Request.UrlReferrer.ToString());
        }
        [HttpPost]
        public ActionResult LogOut()
        {
            Session.Clear();
            Session.Abandon();
            Response.Cookies["Token"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", ""));
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
                Server = CommonParameters.Location.WebService,
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
            return Redirect(Request.UrlReferrer.ToString());
        }
        public ActionResult Login2FA()
        {
            return View();
        }

        public ActionResult NotFound()
        {
            return View();
        }

        public string GetToken(User user)
        {
            var key = ConfigurationManager.AppSettings["JwtKey"];
            var issuer = ConfigurationManager.AppSettings["JwtIssuer"];

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //Create a List of Claims, Keep claims name short    
            var permClaims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("UserId", user.UserId.ToString()),
            };

            //Create Security Token object by giving required parameters    
            JwtSecurityToken token = new JwtSecurityToken(issuer, //Issuer
                issuer,  //Audience
                permClaims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials);
            string jwt_token = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt_token;
        }
    }
}