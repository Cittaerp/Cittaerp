using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CittaErp
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            //help in solving could not load file ar10/ active reports
            routes.IgnoreRoute("{*allActiveReport}", new { allActiveReport = @".*\.ar10(/.*)?" });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Log_in", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
