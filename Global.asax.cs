using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Mvc
{
    public class MvcApplication : System.Web.HttpApplication
    {
         protected void Application_Start()
         {
             AreaRegistration.RegisterAllAreas();
             FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
             RouteConfig.RegisterRoutes(RouteTable.Routes);
             BundleConfig.RegisterBundles(BundleTable.Bundles);
         }
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                        new { controller = "Project", action = "ProjectList", id = UrlParameter.Optional }, // Parameter defaults
                         new { controller = "Details", action = "Details", id = UrlParameter.Optional }
            );

        }
    }
}
