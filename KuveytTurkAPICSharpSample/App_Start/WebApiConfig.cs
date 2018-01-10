using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace KuveytTurkAPICSharpSample
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);
            config.Routes.MapHttpRoute(
                name: "Generic",
                routeTemplate: "v1/{*url}",
                defaults: new { controller = "Values", action = "Default" }
            );
        }
    }
}
