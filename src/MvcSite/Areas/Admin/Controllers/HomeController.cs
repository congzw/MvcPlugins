using System;
using System.Web.Mvc;

namespace MvcSite.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        public string Index()
        {
            return "Admin Home Index: " + DateTime.Now;
        }
    }
}