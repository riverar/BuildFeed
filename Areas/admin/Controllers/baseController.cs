using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BuildFeed.Areas.admin.Controllers
{
    [Authorize(Users = "hounsell")]
    public class baseController : Controller
    {
        // GET: admin/base
        public ActionResult index()
        {
            return View();
        }
    }
}