using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MaaAahwanam.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
               "viewservice",                                           // Route name
               "name/{name}/{type}",                                   // URL with parameters
               new { controller = "viewservice", action = "Index" }    // Parameter defaults
           );
            routes.MapRoute(
               "viewservice1",                                           // Route name
               "name/{name}",                                         // URL with parameters
               new { controller = "viewservice", action = "Index" }    // Parameter defaults
           );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "NHomePage", action = "Index", id = UrlParameter.Optional },
                 namespaces: new string[] { "MaaAahwanam.Web.Controllers" }
            );

        }
    }
}
