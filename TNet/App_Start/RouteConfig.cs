using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using TNet.Service.Com;

namespace TNet
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            //默认导航到前端
            //
            //routes.MapRoute(
            //    "Manage_MercList",
            //    "{controller}/{action}/{pageIndex}",
            //    new { controller = "Manage", action = "MercList", pageIndex = UrlParameter.Optional }
            //    );

            routes.MapRoute(
                "User",
                "User",
                new { controller = "User", action = "Index" }
             );

            routes.MapRoute(
             "Default", // Route name
             "{controller}/{action}/{id}", // URL with parameters
             new { controller = "Home", action = "Index", id = UrlParameter.Optional },
             new { controller = "^(?!Service).*" }
             );

            RouteService.register();
        }
    }
}
