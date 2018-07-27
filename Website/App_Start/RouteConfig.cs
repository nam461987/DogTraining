using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Website
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Video",
                url: "videos.html",
                defaults: new { controller = "Home", action = "Video", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Service",
                url: "service/{name}-{id}.html",
                defaults: new { controller = "Home", action = "Service", id = UrlParameter.Optional, name = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "ContactUs",
                url: "contact-us.html",
                defaults: new { controller = "Home", action = "Contact", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "AboutUs",
                url: "about-us.html",
                defaults: new { controller = "Home", action = "About", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
