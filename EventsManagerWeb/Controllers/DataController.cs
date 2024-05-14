using EventsManagerModels;
using EventsManagerWebService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Mvc;
using AuthorizeAttribute = System.Web.Http.AuthorizeAttribute;

namespace EventsManagerWeb.Controllers
{
    [OutputCache(Duration = 0)]
    [EnableCors(origins: "https://localhost:44365", headers: "*", methods: "*")]
    public class DataController : ApiController
    {
        [Authorize]
        [System.Web.Http.HttpGet]
        async public Task<MenuListVIewModel> GetMenus()
        {
            WebClient<MenuListVIewModel> client = new WebClient<MenuListVIewModel>
            {
                Server = CommonParameters.Location.WebService,
                Controller = "Visitor",
                Method = "GetMenus"
            };
            if (Request.Headers.GetCookies("Token").Count <= 0)
            {
            }
            MenuListVIewModel Menus = await client.GetAsync();
            return Menus;
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
            return await client.GetAsync();
        }
    }
}