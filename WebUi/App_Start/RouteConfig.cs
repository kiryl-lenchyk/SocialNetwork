using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebUi
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");



            routes.MapRoute(
                name: "Login",
                url: "Login",
                defaults: new {controller = "Account", action = "Login"},
                namespaces: new string[] {"WebUi.Controllers"}
                );

            routes.MapRoute(
                name: "Register",
                url: "Register",
                defaults: new {controller = "Account", action = "Register"},
                namespaces: new string[] {"WebUi.Controllers"}
                );


            routes.MapRoute(
                name: "Dialog",
                url: "Dialog-{id}",
                defaults: new {controller = "Message", action = "Dialog"},
                namespaces: new string[] {"WebUi.Controllers"}
                );

            routes.MapRoute(
                name: "User",
                url: "User/id{id}",
                defaults: new {controller = "User", action = "Index"},
                namespaces: new string[] {"WebUi.Controllers"}
                );

            routes.MapRoute(
                name: "Error",
                url: "Error",
                defaults: new {controller = "Home", action = "Error"},
                namespaces: new string[] {"WebUi.Controllers"}
                );


            routes.MapRoute(
                name: "NotFound",
                url: "404",
                defaults: new {controller = "Home", action = "NotFound"},
                namespaces: new string[] {"WebUi.Controllers"}
                );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new {controller = "Home", action = "Index", id = UrlParameter.Optional},
                namespaces: new string[] {"WebUi.Controllers"}
                );


        }
    }
}