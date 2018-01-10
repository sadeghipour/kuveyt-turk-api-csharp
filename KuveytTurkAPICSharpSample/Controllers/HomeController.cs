using KuveytTurkAPICSharp;
using KuveytTurkAPICSharp.Objects;
using System.Collections.Generic;
using System.Web.Mvc;

namespace KuveytTurkAPICSharpSample.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
            return View();
        }
        public ActionResult OpenLoginUI()
        {
            ViewBag.Title = "Home Page";
            List<string> scopes = new List<string>()
                    {
                        "accounts",
                        "transfers",
                        "loans"
                    };
            Client client = System.Web.HttpContext.Current.Application["Client"] as Client;
            var uri = Process.GetKuveytTurkCustomerLoginUri(null, client, scopes);
            return Content($"<script>window.open('{uri.AbsoluteUri}', '_blank');</script>");
        }
    }
}
