using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;
using System.ServiceModel.Activation;
using TNet.Service.Merc;

namespace TNet.Service.Com
{
    public sealed class RouteService
    {
        public static void register()
        {
            RouteTable.Routes.Add(new ServiceRoute("Service/Merc/", new WebServiceHostFactory(), typeof(MercService)));
            RouteTable.Routes.Add(new ServiceRoute("MercService.svc", new WebServiceHostFactory(), typeof(MercService)));

            

        }
    }
}
