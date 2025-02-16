using System.Security.Claims;
using EventsManagerModels;
using EventsManagerWebApp;
using EventsManagerWebService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace EventsManagerWeb.Controllers
{
    [Authorize]
    [OutputCache(Duration = 0)]
    [Route("/Account/{action}")]
    [Controller]
    public class RegisteredController : Controller
    {
        public readonly JWTManager jwtManager;
        public RegisteredController(JWTManager jwtManager)
        {
            this.jwtManager = jwtManager;
        }
        public ActionResult MyOrders()
        {
            WebClient<OrderListVIewModel> client = new WebClient<OrderListVIewModel>
            {
                Server = Environment.GetEnvironmentVariable("WEBSERVICE_ENVIRONMENT"),
                Controller = "Registered",
                Method = "GetOrders"
            };
            var claimsIdentity = User.Identity as ClaimsIdentity;
            client.AddKeyValue("UserId", claimsIdentity.FindFirst("UserId").Value);
            OrderListVIewModel Menus = client.Get();
            return View(Menus);
        }
        public ActionResult Order(int id=1)
        {
            WebClient<NewOrderViewModel> client = new WebClient<NewOrderViewModel>
            {
                Server = Environment.GetEnvironmentVariable("WEBSERVICE_ENVIRONMENT"),
                Controller = "Registered",
                Method = "OrderMenus"
            };
            client.AddKeyValue("HallId", id.ToString());
            return View(client.Get());
        }
        public ActionResult Settings()
        {
            WebClient<User> client = new WebClient<User>
            {
                Server = Environment.GetEnvironmentVariable("WEBSERVICE_ENVIRONMENT"),
                Controller = "Registered",
                Method = "UserDetails"
            };
            var claimsIdentity = User.Identity as ClaimsIdentity;
            client.AddKeyValue("id", claimsIdentity.FindFirst("UserId").Value.ToString());
            User user = client.Get();
            return View(user);
        }

        /* [HttpPost]
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
            if (Context.Session.GetString("UserName") == null) 
            { //if user is null, do not save image
                return RedirectToAction("Settings");
            }
            using (Image image = Image.FromStream(file.InputStream))
                image.Save(Path.Combine($"{Server.MapPath("/Content/Profile-img")}\\{Context.Session.GetString("UserName")}.png"), ImageFormat.Png);

            //cache is set to don't save cache
            Response.AddHeader("Cache-control", "no-cache");
            return RedirectToAction("Settings");
        } */

        [HttpPost]
        [ActionName("UpdateUser")]
        public ActionResult Update(User user)
        {
            WebClient<User> client = new WebClient<User>
            {
                Server = Environment.GetEnvironmentVariable("WEBSERVICE_ENVIRONMENT"),
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
                Server = Environment.GetEnvironmentVariable("WEBSERVICE_ENVIRONMENT"),
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

        /* [HttpPost]
        public ActionResult Rate(Rating rating, IEnumerable<HttpPostedFileBase> RatingImages)
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            List<string> images = new List<string>();
            WebClient<Rating> client = new WebClient<Rating>
            {
                Server = Environment.GetEnvironmentVariable("WEBSERVICE_ENVIRONMENT"),
                Controller = "Registered",
                Method = "NewRate"
            };
            if (Context.Session.GetString("UserName") == null)
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
                        string ratingImage = $"/img/Review/Rating-{rating.RatingDate}-{count}.png";
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
        } */

        [HttpPost]
        public ActionResult NewOrder(Order order)
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            WebClient<Order> client = new WebClient<Order>
            {
                Server = Environment.GetEnvironmentVariable("WEBSERVICE_ENVIRONMENT"),
                Controller = "Registered",
                Method = "NewOrder"
            };
            if (HttpContext.Session.GetString("UserName") == null)
            { //if user is null, do not save image
                return Redirect(Request.Headers["Referer"].ToString());
            }
            order.UserId = Convert.ToInt16(claimsIdentity.FindFirst("UserId").Value);
            order.OrderDate = Convert.ToString(DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString());
            order.IsPaid = "true";
            TempData["Error"] = client.Post(order);
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}