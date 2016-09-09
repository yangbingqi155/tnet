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
            //参与这种路由参数 兼容 WCF路由
            routes.MapRoute(
                 "User",
                 "User/",
                  new { controller = "User", action = "Index" },
                  new { controller = "^(?!Service).*" }
            );


            routes.MapRoute(
                 "Order_Pay",
                 "Order/Pay/{orderno}",
                  new { controller = "Order", action = "Pay", orderno = UrlParameter.Optional },
                  new { controller = "^(?!Service).*" }
              );

            routes.MapRoute(
                 "Manage_MercEdit",
                 "Manage/MercEdit/{idmerc}",
                  new { controller = "Manage", action = "MercEdit", idmerc = UrlParameter.Optional },
                  new { controller = "^(?!Service).*" }
            );
            
            routes.MapRoute(
               "Manage_MercList",
               "Manage/MercList/{pageIndex}",
                new { controller = "Manage", action = "MercList", pageIndex = UrlParameter.Optional },
                new { controller = "^(?!Service).*" }
            );

            routes.MapRoute(
               "Manage",
               "Manage/{action}/{id}",
                new { controller = "Manage", action = "Login", id = UrlParameter.Optional },
                new { controller = "^(?!Service).*" }
           );

            routes.MapRoute(
               "Default",
               "{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                new { controller = "^(?!Service).*" }
            );
            RouteService.register();
        }
    }
}
