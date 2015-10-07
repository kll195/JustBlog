﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace JustBlog
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                "Tag",
                "Tag/{tag}",
                new { controller = "Blog", action = "Tag" }
);
            routes.MapRoute(
                "Category",
                "Category/{category}",
                new { controller = "Blog", action = "Category" }
            );


            routes.MapRoute(
                 "Login",
                 "Login",
                new { controller = "Admin", action = "Login" }
            );

            routes.MapRoute(
                "Logout",
                "Logout",
            new { controller = "Admin", action = "Logout" }
            );


            routes.MapRoute(
               "Manage",
               "Manage",
           new { controller = "Admin", action = "Manage" }
           );

            routes.MapRoute(
                "AdminAction",
                "Admin/{action}",
                new { controller = "Admin", action = "Login" }
            );
            routes.MapRoute(
                "Post",
                "Archive/{year}/{month}/{title}",
            new { controller = "Blog", action = "Post" }
            );

           


            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Blog", action = "Posts", id = UrlParameter.Optional }
            );

            

            
        }
    }
}