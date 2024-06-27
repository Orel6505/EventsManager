using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace EventsManagerWeb
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "DefaultAdmin",
                url: "Admin/{action}/{id}",
                defaults: new { controller = "Admin", action = "Users", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "DefaultRegistered",
                url: "Account/{action}/{id}",
                defaults: new { controller = "Registered", action = "MyOrders", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{action}/{id}",
                defaults: new { controller = "Visitor", action = "Home", id = UrlParameter.Optional }
            );
        }
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            config.EnableCors();

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{action}/{id}",
                defaults: new { controller = "Data", id = RouteParameter.Optional }
            );
        }
    }
}
