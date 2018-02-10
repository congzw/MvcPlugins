using System;
using System.Web.Mvc;

namespace MvcSite.Controllers
{
    public class HomeController : Controller
    {
        public string Index()
        {
            return "Home Index: " + DateTime.Now;
        }
    }
}