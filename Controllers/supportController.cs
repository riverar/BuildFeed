using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using BuildFeed.Models;
using BuildFeed.Models.ViewModel;

namespace BuildFeed.Controllers
{
    public class supportController : Controller
    {
        // GET: support
        public ActionResult index()
        {
            return View();
        }

        public ActionResult login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult login(LoginUser ru)
        {
            if (ModelState.IsValid)
            {
                bool isAuthenticated = Membership.ValidateUser(ru.UserName, ru.Password);

                if (isAuthenticated)
                {
                    FormsAuthentication.SetAuthCookie(ru.UserName, ru.RememberMe);

                    string returnUrl = string.IsNullOrEmpty(Request.QueryString["ReturnUrl"]) ? "/" : Request.QueryString["ReturnUrl"];

                    return Redirect(returnUrl);
                }
            }

            ViewData["ErrorMessage"] = "The username and password are not valid.";
            return View(ru);
        }

        [Authorize]
        public ActionResult password()
        {
            return View();
        }

        [HttpPost]
        public ActionResult password(ChangePassword cp)
        {
            if (ModelState.IsValid)
            {
                var user = Membership.GetUser();
                bool success = user.ChangePassword(cp.OldPassword, cp.NewPassword);

                if (success)
                {
                    return Redirect("/");
                }
            }

            ViewData["ErrorMessage"] = "There was an error changing your password.";
            return View(cp);
        }

        public ActionResult logout()
        {
            FormsAuthentication.SignOut();
            return Redirect("/");
        }

        public ActionResult register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult register(RegistrationUser ru)
        {
            if (ModelState.IsValid)
            {
                MembershipCreateStatus status;
                Membership.CreateUser(ru.UserName, ru.Password, ru.EmailAddress, "THIS WILL BE IGNORED", "I WILL BE IGNORED", false, out status);

                switch (status)
                {
                    case MembershipCreateStatus.Success:
                        return RedirectToAction("thanks_register");
                    case MembershipCreateStatus.InvalidPassword:
                        ViewData["ErrorMessage"] = "The password is invalid.";
                        break;
                    case MembershipCreateStatus.DuplicateEmail:
                        ViewData["ErrorMessage"] = "A user account with this email address already exists.";
                        break;
                    case MembershipCreateStatus.DuplicateUserName:
                        ViewData["ErrorMessage"] = "A user account with this user name already exists.";
                        break;
                    default:
                        ViewData["ErrorMessage"] = "Unspecified error.";
                        break;
                }
            }

            return View(ru);
        }

        public ActionResult thanks_register()
        {
            return View();
        }

        public ActionResult rss()
        {
            return View();
        }

        public ActionResult sitemap()
        {
            IEnumerable<Build> builds = Build.SelectInVersionOrder();
            Dictionary<string, SitemapPagedAction[]> actions = new Dictionary<string, SitemapPagedAction[]>();

            actions.Add("Pages", new SitemapPagedAction[] { new SitemapPagedAction()
            {
                UrlParams = new RouteValueDictionary(new {
                    controller = "build",
                    action = "index",
                    page = 1
                }),
                Pages = (builds.Count() + (buildController.pageSize - 1)) / buildController.pageSize
            } });

            actions.Add("Versions", (from b in builds
                                     group b by new BuildVersion() { Major = b.MajorVersion, Minor = b.MinorVersion } into bv
                                     orderby bv.Key.Major descending,
                                             bv.Key.Minor descending
                                     select new SitemapPagedAction()
                                     {
                                         Name = string.Format("Windows NT {0}.{1}", bv.Key.Major, bv.Key.Minor),
                                         UrlParams = new RouteValueDictionary(new
                                         {
                                             controller = "build",
                                             action = "version",
                                             major = bv.Key.Major,
                                             minor = bv.Key.Minor,
                                             page = 1
                                         }),
                                         Pages = (bv.Count() + (buildController.pageSize - 1)) / buildController.pageSize
                                     }).ToArray());

            actions.Add("Labs", (from b in builds
                                 where !string.IsNullOrEmpty(b.Lab)
                                 group b by b.Lab into bv
                                 orderby bv.Key
                                 select new SitemapPagedAction()
                                 {
                                     Name = bv.Key,
                                     UrlParams = new RouteValueDictionary(new
                                     {
                                         controller = "build",
                                         action = "lab",
                                         lab = bv.Key,
                                         page = 1
                                     }),
                                     Pages = (bv.Count() + (buildController.pageSize - 1)) / buildController.pageSize
                                 }).ToArray());

            actions.Add("Years", (from b in builds
                                 where b.BuildTime.HasValue
                                 group b by b.BuildTime.Value.Year into bv
                                 orderby bv.Key
                                 select new SitemapPagedAction()
                                 {
                                     Name = bv.Key.ToString(),
                                     UrlParams = new RouteValueDictionary(new
                                     {
                                         controller = "build",
                                         action = "year",
                                         year = bv.Key,
                                         page = 1
                                     }),
                                     Pages = (bv.Count() + (buildController.pageSize - 1)) / buildController.pageSize
                                 }).ToArray());

            actions.Add("Sources", (from b in builds
                                 group b by b.SourceType into bv
                                 orderby bv.Key
                                 select new SitemapPagedAction()
                                 {
                                     Name = DisplayHelpers.GetDisplayTextForEnum(bv.Key),
                                     UrlParams = new RouteValueDictionary(new
                                     {
                                         controller = "build",
                                         action = "source",
                                         source = bv.Key,
                                         page = 1
                                     }),
                                     Pages = (bv.Count() + (buildController.pageSize - 1)) / buildController.pageSize
                                 }).ToArray());

            SitemapData model = new SitemapData()
            {
                Builds = (from b in builds
                          select new SitemapDataBuild()
                          {
                              Id = b.Id,
                              Name = b.FullBuildString
                          }).ToArray(),

                Actions = actions
            };

            return View(model);
        }
    }
}