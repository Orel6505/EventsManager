using EventsManagerModels;
using EventsManagerWebService;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Win32;
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
            if (file == null || file.ContentLength <= 0)
            { //if file is null or empty, do not save the image
                return RedirectToAction("Settings");
            }
            if (!file.ContentType.StartsWith("image"))
            { //if file is not image, do not save image
                return RedirectToAction("Settings");
            }
            if (Session["UserName"] == null) 
            { //if user is null, do not save image
                return RedirectToAction("Settings");
            }
            using (Image image = Image.FromStream(file.InputStream))
                image.Save(Path.Combine($"{Server.MapPath("/Content/Profile-img")}\\{Session["UserName"]}.png"), ImageFormat.Png);

            //cache is set to don't save cache
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
        [HttpPost]
        [ActionName("UpdatePassword")]
        public ActionResult Update(string NewPassword)
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            WebClient<User> client = new WebClient<User>
            {
                Server = CommonParameters.Location.WebService,
                Controller = "Registered",
                Method = "UpdatePassword"
            };
            User user = new User()
            {
                UserId = Convert.ToInt16(claimsIdentity.FindFirst("UserId").Value.ToString()),
                TempPassword = NewPassword
            };
            TempData["Update"] = client.Post(user);
            return RedirectToAction("Settings");
        }

        [HttpPost]
        public ActionResult Rate(Rating rating, IEnumerable<HttpPostedFileBase> RatingImages)
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            List<string> images = new List<string>();
            WebClient<Rating> client = new WebClient<Rating>
            {
                Server = CommonParameters.Location.WebService,
                Controller = "Registered",
                Method = "NewRate"
            };
            if (Session["UserName"] == null)
            { //if user is null, do not save image
                return Redirect(Request.UrlReferrer.ToString());
            }
            rating.UserId = Convert.ToInt16(claimsIdentity.FindFirst("UserId").Value);
            rating.RatingDate = Convert.ToString(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            if (RatingImages != null)
            {
                int count = 0;
                foreach (HttpPostedFileBase file in RatingImages)
                {
                    if (file == null || file.ContentLength <= 0)
                    { //if file is null or empty, do not save the image
                        continue;
                    }
                    if (!file.ContentType.StartsWith("image"))
                    { //if file is not image, do not save image
                        continue;
                    }
                    count++;
                    try
                    {
                        using (Image image = Image.FromStream(file.InputStream))
                            image.Save(Path.Combine($"{Server.MapPath("/Content/Review-img")}\\Rating-{rating.RatingDate}-{count}.png"), ImageFormat.Png);
                        string ratingImage = $"/Content/Review-img/Rating-{rating.RatingDate}-{count}.png";
                        images.Add(ratingImage);
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                    
                }
                rating.RatingImagesLocation = images;
            }
            TempData["Error"] = client.Post(rating);
            return Redirect(Request.UrlReferrer.ToString());
        }
    }
}