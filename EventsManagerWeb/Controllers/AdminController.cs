using EventsManagerModels;
using EventsManagerWebService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace EventsManagerWeb.Controllers
{
    [Authorize(Roles = "Admin")]
    [OutputCache(Duration = 0)]
    public class AdminController : Controller
    {
        public ActionResult Users()
        {
            WebClient<List<User>> client = new WebClient<List<User>>
            {
                Server = CommonParameters.Location.WebService,
                Controller = "Admin",
                Method = "GetUsers"
            };
            return View(client.Get());
        }
        public ActionResult Halls()
        {
            WebClient<List<Hall>> client = new WebClient<List<Hall>>
            {
                Server = CommonParameters.Location.WebService,
                Controller = "Visitor",
                Method = "GetHalls"
            };
            return View(client.Get());
        }
        public ActionResult Orders()
        {
            WebClient<List<Order>> client = new WebClient<List<Order>>
            {
                Server = CommonParameters.Location.WebService,
                Controller = "Admin",
                Method = "GetOrders"
            };
            return View(client.Get());
        }
        public ActionResult Menus()
        {
            WebClient<List<Menu>> client = new WebClient<List<Menu>>
            {
                Server = CommonParameters.Location.WebService,
                Controller = "Admin",
                Method = "GetMenus"
            };
            return View(client.Get());
        }
        [HttpPost]
        public ActionResult Users(User user)
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            WebClient<User> client = new WebClient<User>
            {
                Server = CommonParameters.Location.WebService,
            };
            if (user.FormAction == "Register")
            {
                client.Controller = "Registered";
                client.Method = "Register";
            }
            if (user.FormAction == "Update")
            {
                client.Controller = "Registered";
                client.Method = "UpdateUser";
            }
            user.UserId = Convert.ToInt16(claimsIdentity.FindFirst("UserId").Value.ToString());
            TempData["Update"] = client.Post(user);
            return Redirect(Request.UrlReferrer.ToString());
        }
    }
}