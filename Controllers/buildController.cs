using BuildFeed.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BuildFeed.Controllers
{
    public class buildController : Controller
    {
        public static int pageSize { get { return 15; } }
        //
        // GET: /build/

        public ActionResult index(int page = 1)
        {
            var builds = Build.SelectInBuildOrder();
            var pageBuilds = builds.Skip((page - 1) * pageSize).Take(pageSize);

            ViewBag.PageNumber = page;
            ViewBag.PageCount = Math.Ceiling(Convert.ToDouble(builds.Count()) / Convert.ToDouble(pageSize));

            return View(pageBuilds);
        }

        public ActionResult year(int year, int page = 1)
        {
            var builds = Build.SelectInBuildOrder().Where(b => b.BuildTime.HasValue && b.BuildTime.Value.Year == year);
            var pageBuilds = builds.Skip((page - 1) * pageSize).Take(pageSize);

            ViewBag.PageNumber = page;
            ViewBag.PageCount = Math.Ceiling(Convert.ToDouble(builds.Count()) / Convert.ToDouble(pageSize));

            return View("index", pageBuilds);
        }

        public ActionResult lab(string lab, int page = 1)
        {
            var builds = Build.SelectInBuildOrder().Where(b => b.Lab != null && (b.Lab.ToLower() == lab.ToLower()));
            var pageBuilds = builds.Skip((page - 1) * pageSize).Take(pageSize);

            ViewBag.PageNumber = page;
            ViewBag.PageCount = Math.Ceiling(Convert.ToDouble(builds.Count()) / Convert.ToDouble(pageSize));

            return View("index", pageBuilds);
        }

        public ActionResult version(int major, int minor, int page = 1)
        {
            var builds = Build.SelectInBuildOrder().Where(b => b.MajorVersion == major && b.MinorVersion == minor);
            var pageBuilds = builds.Skip((page - 1) * pageSize).Take(pageSize);

            ViewBag.PageNumber = page;
            ViewBag.PageCount = Math.Ceiling(Convert.ToDouble(builds.Count()) / Convert.ToDouble(pageSize));

            return View("index", pageBuilds);
        }

        public ActionResult source(TypeOfSource source, int page = 1)
        {
            var builds = Build.SelectInBuildOrder().Where(b => b.SourceType == source);
            var pageBuilds = builds.Skip((page - 1) * pageSize).Take(pageSize);

            ViewBag.PageNumber = page;
            ViewBag.PageCount = Math.Ceiling(Convert.ToDouble(builds.Count()) / Convert.ToDouble(pageSize));

            return View("index", pageBuilds);
        }

        //
        // GET: /build/Info/5

        public ActionResult info(int id)
        {
            Build b = Build.SelectById(id);

            if(b == null)
            {
                return new HttpNotFoundResult();
            }

            return View(b);
        }

        //
        // GET: /build/Create
        [Authorize]
        public ActionResult create()
        {
            return View();
        }

        //
        // POST: /build/Create
        [Authorize]
        [HttpPost]
        public ActionResult create(Build build)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    build.Added = DateTime.Now;
                    build.Modified = DateTime.Now;
                    Build.Insert(build);
                }
                catch
                {
                    return View(build);
                }
                return RedirectToAction("index");
            }
            else
            {
                return View(build);
            }
        }

        //
        // GET: /build/Edit/5
        [Authorize]
        public ActionResult edit(long id)
        {
            Build b = Build.SelectById(id);
            return View("create", b);
        }

        //
        // POST: /build/Edit/5
        [Authorize]
        [HttpPost]
        public ActionResult edit(long id, Build build)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Build.Update(build);
                }
                catch
                {
                    return View();
                }

                return RedirectToAction("index");
            }
            else
            {
                return View("create", build);
            }
        }

        [Authorize(Users = "hounsell")]
        public ActionResult delete(long id)
        {
            Build.DeleteById(id);
            return Redirect("/");
        }
    }
}
