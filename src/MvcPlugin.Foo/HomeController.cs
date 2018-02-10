using System;
using System.Web.Mvc;

namespace MvcSite.Areas.Admin.Controllers
{
    public class FooController : Controller
    {
        public string Hello()
        {
            //return "From Foo V1: " + DateTime.Now;
            //return "From Foo V2: " + DateTime.Now;
            return "From Foo V3: " + DateTime.Now;
        }
    }
}