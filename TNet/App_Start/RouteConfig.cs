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
               "Default",
               "{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                new { controller = "^(?!Service).*" }
            );

            routes.MapRoute(
                 "Manage_MercEdit",
                 "Manage/{action}/{idmerc}",
                  new { controller = "Manage", action = "MercEdit", idmerc = UrlParameter.Optional },
                  new { controller = "^(?!Service).*" }
            );

            routes.MapRoute(
                "Manage_MercList",
               "Manage/{action}/{pageIndex}",
                new { controller = "Manage", action = "MercList", pageIndex = UrlParameter.Optional },
                new { controller = "^(?!Service).*" }
            );

            //参与这种路由参数 兼容 MCF路由
            routes.MapRoute(
                  "User",
                 "{User}",
                  new { controller = "User", action = "Index" },
                  new { controller = "^(?!Service).*" }
            );

            RouteService.register();
        }
    }
}
