using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using BuildFeed.Auth;

namespace BuildFeed.Areas.admin.Controllers
{
    [Authorize(Users = "hounsell")]
    public class usersController : Controller
    {
        // GET: admin/users
        public ActionResult Index()
        {
            return View(Membership.GetAllUsers().Cast<MembershipUser>().OrderBy(m => m.IsApproved).ThenBy(m => m.UserName));
        }

        public ActionResult Approve(Guid id)
        {
            var provider = (Membership.Provider as RedisMembershipProvider);
            provider.ChangeApproval(id, true);
            return RedirectToAction("Index");
        }

        public ActionResult Unapprove(Guid id)
        {
            var provider = (Membership.Provider as RedisMembershipProvider);
            provider.ChangeApproval(id, false);
            return RedirectToAction("Index");
        }
    }
}