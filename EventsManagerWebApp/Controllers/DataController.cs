

using System.Security.Claims;
using EventsManagerModels;
using EventsManagerWebService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace EventsManagerWeb.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [OutputCache(Duration = 0)]
    [Route("/api/[Action]")]
    //[EnableCors(origins: "https://localhost:44365", headers: "*", methods: "*")]
    public class DataController : Controller
    {
        [HttpGet]
        async public Task<MenuListVIewModel> GetMenus()
        {
            WebClient<MenuListVIewModel> client = new WebClient<MenuListVIewModel>
            {
                Server = Environment.GetEnvironmentVariable("WEBSERVICE_ENVIRONMENT"),
                Controller = "Visitor",
                Method = "GetMenus"
            };
            MenuListVIewModel Menus = await client.GetAsync();
            return Menus;
        }
        [HttpGet]
        async public Task<List<Hall>> GetHalls()
        {
            WebClient<List<Hall>> client = new WebClient<List<Hall>>
            {
                Server = Environment.GetEnvironmentVariable("WEBSERVICE_ENVIRONMENT"),
                Controller = "Visitor",
                Method = "GetHalls"
            }; 
            return await client.GetAsync();
        }
        [HttpGet]
        async public Task<bool> IsAvailableUserName(string UserName)
        {
            WebClient<bool> client = new WebClient<bool>
            {
                Server = Environment.GetEnvironmentVariable("WEBSERVICE_ENVIRONMENT"),
                Controller = "Registered",
                Method = "IsAvailableUserName"
            };
            client.AddKeyValue("UserName", UserName);
            return await client.GetAsync(); //TODO: remove this feature, doesn't follow security standards
        }
        [HttpGet]
        [Authorize(Roles = "Admin, User")]
        async public Task<bool> CheckPassword(string Password)
        {
            WebClient<bool> client = new WebClient<bool>
            {
                Server = Environment.GetEnvironmentVariable("WEBSERVICE_ENVIRONMENT"),
                Controller = "Registered",
                Method = "CheckPassword"
            };
            var claimsIdentity = User.Identity as ClaimsIdentity;
            try
            {
                client.AddKeyValue("UserId", claimsIdentity.FindFirst("UserId").Value);
                client.AddKeyValue("Password", Password);
                return await client.GetAsync();
            }
            catch (Exception) {
                return false;
            }

        }

        [Authorize(Roles = "Admin, User")]
        [HttpGet]
        async public Task<OrderListVIewModel> GetOrders()
        {
            WebClient<OrderListVIewModel> client = new WebClient<OrderListVIewModel>
            {
                Server = Environment.GetEnvironmentVariable("WEBSERVICE_ENVIRONMENT"),
                Controller = "Registered",
                Method = "GetOrders"
            };
            var claimsIdentity = User.Identity as ClaimsIdentity;
            client.AddKeyValue("UserId", claimsIdentity.FindFirst("UserId").Value);
            OrderListVIewModel Menus = await client.GetAsync();
            return Menus;
        }
        [HttpGet]
        async public Task<List<EventType>> HallAvailability(string EventDate, int HallId)
        {
            WebClient<List<EventType>> client = new WebClient<List<EventType>>
            {
                Server = Environment.GetEnvironmentVariable("WEBSERVICE_ENVIRONMENT"),
                Controller = "Registered",
                Method = "HallAvailability"
            };
            client.AddKeyValue("EventDate", EventDate);
            client.AddKeyValue("HallId", HallId.ToString());
            return await client.GetAsync(); 
        }
    }
}