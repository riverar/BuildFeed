﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Http;

namespace BuildFeed
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.AppendTrailingSlash = true;

            routes.MapRoute(
                name: "Site Root",
                url: "",
                defaults: new { controller = "Build", action = "index", page = 1 }
            );

            routes.MapRoute(
                name: "Pagination",
                url: "page/{page}/",
                defaults: new { controller = "Build", action = "index", page = 1 }
            );

            routes.MapRoute(
                name: "Lab Root",
                url: "lab/{lab}/",
                defaults: new { controller = "Build", action = "lab", page = 1 }
            );

            routes.MapRoute(
                name: "Lab",
                url: "lab/{lab}/page/{page}/",
                defaults: new { controller = "Build", action = "lab", page = 1 }
            );

            routes.MapRoute(
                name: "Version Root",
                url: "version/{major}.{minor}/",
                defaults: new { controller = "Build", action = "version", page = 1 }
            );

            routes.MapRoute(
                name: "Version",
                url: "version/{major}.{minor}/page/{page}/",
                defaults: new { controller = "Build", action = "version", page = 1 }
            );

            routes.MapRoute(
                name: "Year Root",
                url: "year/{year}/",
                defaults: new { controller = "Build", action = "year", page = 1 }
            );

            routes.MapRoute(
                name: "Year",
                url: "year/{year}/page/{page}/",
                defaults: new { controller = "Build", action = "year", page = 1 }
            );

            routes.MapRoute(
                name: "Source Root",
                url: "source/{source}/",
                defaults: new { controller = "Build", action = "source", page = 1 }
            );

            routes.MapRoute(
                name: "Source",
                url: "source/{source}/page/{page}/",
                defaults: new { controller = "Build", action = "source", page = 1 }
            );

            routes.MapRoute(
                name: "RSS (with ID)",
                url: "rss/{action}/{id}/",
                defaults: new { controller = "rss", action = "index" }
            );

            routes.MapRoute(
                name: "RSS",
                url: "rss/{action}",
                defaults: new { controller = "rss", action = "index" }
            );

            routes.MapRoute(
                name: "Support",
                url: "support/{action}/",
                defaults: new { controller = "Support", action = "index" }
            );

            routes.MapHttpRoute(
                name: "API",
                routeTemplate: "api/{action}/{id}",
                defaults: new { controller = "api", action = "GetBuilds", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Actions",
                url: "actions/{action}/{id}",
                defaults: new { controller = "Build", action = "index", id = UrlParameter.Optional }
            );
        }
    }
}
