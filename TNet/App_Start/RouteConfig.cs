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

            routes.MapRoute(
                "Manage_MercList",
                "{controller}/{action}/{pageIndex}",
                new { controller = "Manage", action = "MercList", pageIndex = UrlParameter.Optional }
                );
            routes.MapRoute(
                "Manage_MercEdit",
                "{controller}/{action}/{idmerc}",
                new { controller = "Manage", action = "MercEdit", idmerc = UrlParameter.Optional }
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
