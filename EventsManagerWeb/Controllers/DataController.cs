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

namespace EventsManagerWeb.Controllers
{
    [OutputCache(Duration = 0)]
    public class DataController : ApiController
    {
        [System.Web.Http.HttpGet]
        [EnableCors(origins: "https://localhost:44365", headers: "*", methods: "*")]
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
    }
}