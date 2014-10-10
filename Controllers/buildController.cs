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
        public int pageSize { get { return 12; } }
        //
        // GET: /build/

        public ActionResult Index(int page = 1)
        {
            var builds = Build.SelectInBuildOrder();
            var pageBuilds = builds.Skip((page - 1) * pageSize).Take(pageSize);

            ViewBag.PageNumber = page;
            ViewBag.PageCount = Math.Ceiling(Convert.ToDouble(builds.Count()) / Convert.ToDouble(pageSize));

            return View(pageBuilds);
        }

        public ActionResult Year(int year, int page = 1)
        {
            var builds = Build.SelectInBuildOrder().Where(b => b.BuildTime.HasValue && b.BuildTime.Value.Year == year);
            var pageBuilds = builds.Skip((page - 1) * pageSize).Take(pageSize);

            ViewBag.PageNumber = page;
            ViewBag.PageCount = Math.Ceiling(Convert.ToDouble(builds.Count()) / Convert.ToDouble(pageSize));

            return View("Index", pageBuilds);
        }

        public ActionResult Lab(string lab, int page = 1)
        {
            var builds = Build.SelectInBuildOrder().Where(b => b.Lab != null && (b.Lab.ToLower() == lab.ToLower()));
            var pageBuilds = builds.Skip((page - 1) * pageSize).Take(pageSize);

            ViewBag.PageNumber = page;
            ViewBag.PageCount = Math.Ceiling(Convert.ToDouble(builds.Count()) / Convert.ToDouble(pageSize));

            return View("Index", pageBuilds);
        }

        public ActionResult Version(int major, int minor, int page = 1)
        {
            var builds = Build.SelectInBuildOrder().Where(b => b.MajorVersion == major && b.MinorVersion == minor);
            var pageBuilds = builds.Skip((page - 1) * pageSize).Take(pageSize);

            ViewBag.PageNumber = page;
            ViewBag.PageCount = Math.Ceiling(Convert.ToDouble(builds.Count()) / Convert.ToDouble(pageSize));

            return View("Index", pageBuilds);
        }

        public ActionResult Source(TypeOfSource source, int page = 1)
        {
            var builds = Build.SelectInBuildOrder().Where(b => b.SourceType == source);
            var pageBuilds = builds.Skip((page - 1) * pageSize).Take(pageSize);

            ViewBag.PageNumber = page;
            ViewBag.PageCount = Math.Ceiling(Convert.ToDouble(builds.Count()) / Convert.ToDouble(pageSize));

            return View("Index", pageBuilds);
        }

        //
        // GET: /build/Info/5

        public ActionResult Info(int id)
        {
            Build b = Build.SelectById(id);
            return View(b);
        }

        //
        // GET: /build/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /build/Create
        [Authorize]
        [HttpPost]
        public ActionResult Create(Build build)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    build.Added = DateTime.Now;
                    Build.Insert(build);
                }
                catch
                {
                    return View(build);
                }
                return RedirectToAction("Index");
            }
            else
            {
                return View(build);
            }
        }

        //
        // GET: /build/Edit/5
        [Authorize]
        public ActionResult Edit(long id)
        {
            Build b = Build.SelectById(id);
            return View("Create", b);
        }

        //
        // POST: /build/Edit/5
        [Authorize]
        [HttpPost]
        public ActionResult Edit(long id, Build build)
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

                return RedirectToAction("Index");
            }
            else
            {
                return View("Create", build);
            }
        }

        [Authorize(Users = "hounsell")]
        public ActionResult Delete(long id)
        {
            Build.DeleteById(id);
            return Redirect("/");
        }
    }
}
