using EventsManagerModels;
using EventsManagerWebService;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Remoting.Contexts;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Razor.Generator;
using static System.Net.WebRequestMethods;

namespace EventsManagerWeb.Controllers
{
    [Authorize]
    [OutputCache(Duration = 0)]
    [Route("/Account/{action}")]
    public class RegisteredController : Controller
    {
        public ActionResult MyOrders()
        {
            WebClient<OrderListVIewModel> client = new WebClient<OrderListVIewModel>
            {
                Server = CommonParameters.Location.WebService,
                Controller = "Registered",
                Method = "GetOrders"
            };
            var claimsIdentity = User.Identity as ClaimsIdentity;
            client.AddKeyValue("UserId", claimsIdentity.FindFirst("UserId").Value);
            OrderListVIewModel Menus = client.Get();
            return View(Menus);
        }
        public ActionResult Order() 
        {
            return View();
        }
        public ActionResult Settings()
        {
            WebClient<User> client = new WebClient<User>
            {
                Server = CommonParameters.Location.WebService,
                Controller = "Registered",
                Method = "UserDetails"
            };
            var claimsIdentity = User.Identity as ClaimsIdentity;
            client.AddKeyValue("id", claimsIdentity.FindFirst("UserId").Value.ToString());
            User user = client.Get();
            return View(user);
        }

        [HttpPost]
        public ActionResult Update(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                if (!file.ContentType.StartsWith("image"))
                {
                    return RedirectToAction("Settings");
                }
                using (Image image = Image.FromStream(file.InputStream)) {
                    if (Session["UserName"] != null)
                        image.Save(Path.Combine($"{Server.MapPath("/")}Content\\Profile-img\\{Session["UserName"]}.png"), ImageFormat.Png);
                    else
                        ViewBag.Error = 1;
                }
            }
            Response.AddHeader("Cache-control", "no-cache");
            return RedirectToAction("Settings");
        }

        [HttpPost]
        [ActionName("UpdateUser")]
        public ActionResult Update(User user)
        {
            WebClient<User> client = new WebClient<User>
            {
                Server = CommonParameters.Location.WebService,
                Controller = "Registered",
                Method = "UpdateUser"
            };
            var claimsIdentity = User.Identity as ClaimsIdentity;
            user.UserId = Convert.ToInt16(claimsIdentity.FindFirst("UserId").Value.ToString());
            TempData["Update"] = client.Post(user);
            return RedirectToAction("Settings");
        }
    }
}