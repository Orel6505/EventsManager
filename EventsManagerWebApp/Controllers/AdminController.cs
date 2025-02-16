using EventsManagerModels;
using EventsManagerWebApp;
using EventsManagerWebService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using System.Security.Claims;

namespace EventsManagerWeb.Controllers
{
    [Authorize(Roles = "Admin")]
    [OutputCache(Duration = 0)]
    [Route("[Controller]/[Action]")]
    [Controller]
    public class AdminController : Controller
    {
        public readonly JWTManager jwtManager;
        public AdminController(JWTManager jwtManager) 
        { 
            this.jwtManager = jwtManager;
        }
        public ActionResult Users()
        {
            WebClient<List<User>> client = new WebClient<List<User>>
            {
                Server = Environment.GetEnvironmentVariable("WEBSERVICE_ENVIRONMENT"),
                Controller = "Admin",
                Method = "GetUsers"
            };
            return View(client.Get());
        }
        public ActionResult Halls()
        {
            WebClient<List<Hall>> client = new WebClient<List<Hall>>
            {
                Server = Environment.GetEnvironmentVariable("WEBSERVICE_ENVIRONMENT"),
                Controller = "Visitor",
                Method = "GetHalls"
            };
            return View(client.Get());
        }
        public ActionResult Orders()
        {
            WebClient<List<Order>> client = new WebClient<List<Order>>
            {
                Server = Environment.GetEnvironmentVariable("WEBSERVICE_ENVIRONMENT"),
                Controller = "Admin",
                Method = "GetOrders"
            };
            return View(client.Get());
        }
        public ActionResult Menus()
        {
            WebClient<List<Menu>> client = new WebClient<List<Menu>>
            {
                Server = Environment.GetEnvironmentVariable("WEBSERVICE_ENVIRONMENT"),
                Controller = "Admin",
                Method = "GetMenus"
            };
            return View(client.Get());
        }
        public ActionResult Foods()
        {
            WebClient<List<Food>> client = new WebClient<List<Food>>
            {
                Server = Environment.GetEnvironmentVariable("WEBSERVICE_ENVIRONMENT"),
                Controller = "Admin",
                Method = "GetFoods"
            };
            return View(client.Get());
        }
        [HttpPost]
        public ActionResult Users(User user)
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            WebClient<User> client = new WebClient<User>
            {
                Server = Environment.GetEnvironmentVariable("WEBSERVICE_ENVIRONMENT"),
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
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}