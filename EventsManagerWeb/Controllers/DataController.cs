using EventsManagerModels;
using EventsManagerWebService;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Mvc;
using AuthorizeAttribute = System.Web.Http.AuthorizeAttribute;

namespace EventsManagerWeb.Controllers
{
    [System.Web.Http.AllowAnonymous]
    [OutputCache(Duration = 0)]
    [EnableCors(origins: "https://localhost:44365", headers: "*", methods: "*")]
    public class DataController : ApiController
    {
        [System.Web.Http.HttpGet]
        async public Task<MenuListVIewModel> GetMenus()
        {
            WebClient<MenuListVIewModel> client = new WebClient<MenuListVIewModel>
            {
                Server = CommonParameters.Location.WebService,
                Controller = "Visitor",
                Method = "GetMenus"
            };
            MenuListVIewModel Menus = await client.GetAsync();
            return Menus;
        }
        [System.Web.Http.HttpGet]
        async public Task<List<Hall>> GetHalls()
        {
            WebClient<List<Hall>> client = new WebClient<List<Hall>>
            {
                Server = CommonParameters.Location.WebService,
                Controller = "Visitor",
                Method = "GetHalls"
            }; 
            return await client.GetAsync();
        }
        [System.Web.Http.HttpGet]
        async public Task<bool> IsAvailableUserName(string UserName)
        {
            WebClient<bool> client = new WebClient<bool>
            {
                Server = CommonParameters.Location.WebService,
                Controller = "Registered",
                Method = "IsAvailableUserName"
            };
            client.AddKeyValue("UserName", UserName);
            return await client.GetAsync(); //TODO: remove this feature, doesn't follow security standarts
        }
        [System.Web.Http.HttpGet]
        [Authorize(Roles = "Admin, User")]
        async public Task<bool> CheckPassword(string Password)
        {
            WebClient<bool> client = new WebClient<bool>
            {
                Server = CommonParameters.Location.WebService,
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
        [System.Web.Http.HttpGet]
        async public Task<OrderListVIewModel> GetOrders()
        {
            WebClient<OrderListVIewModel> client = new WebClient<OrderListVIewModel>
            {
                Server = CommonParameters.Location.WebService,
                Controller = "Registered",
                Method = "GetOrders"
            };
            var claimsIdentity = User.Identity as ClaimsIdentity;
            client.AddKeyValue("UserId", claimsIdentity.FindFirst("UserId").Value);
            OrderListVIewModel Menus = await client.GetAsync();
            return Menus;
        }
    }
}