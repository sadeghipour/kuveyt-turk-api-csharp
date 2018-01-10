using KuveytTurkAPICSharp.Objects;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace KuveytTurkAPICSharpSample
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            string pemstr = File.ReadAllText(Server.MapPath("~/App_Data/private_key.pem")).Trim();
            JObject clientObject = JObject.Parse(File.ReadAllText(Server.MapPath("~/App_Data/client.json")));

            Application["Client"] = new Client()
            {
                Id = clientObject["ClientId"].ToString(),
                Secret = clientObject["ClientSecret"].ToString(),
                RedirectUri = clientObject["RedirectUri"].ToString(),
                CertificatePrivateKey = pemstr
            };
            KuveytTurkAPICSharp.Process sdk = new KuveytTurkAPICSharp.Process();
            Application["SDKInstance"] = sdk;

        }
    }
}
