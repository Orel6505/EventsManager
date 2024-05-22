using EventsManagerModels;
using EventsManagerWebService;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace EventsManagerWeb.Controllers
{
    [OutputCache(Duration = 0)]
    [Route("/Account/{action}")]
    public class RegisteredController : Controller
    {
        public ActionResult MyOrders()
        {
            var handler = new JwtSecurityTokenHandler();
            WebClient<OrderListVIewModel> client = new WebClient<OrderListVIewModel>
            {
                Server = CommonParameters.Location.WebService,
                Controller = "Registered",
                Method = "GetOrders"
            };
            client.AddKeyValue("UserId", "1");
            OrderListVIewModel Menus = client.Get();
            return View(Menus);
        }
    }
}