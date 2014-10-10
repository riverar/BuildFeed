using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BuildFeed.Controllers
{
    public class pageController : Controller
    {
        public ActionResult Index()
        {
            return new RedirectResult("/", true);
        }
	}
}