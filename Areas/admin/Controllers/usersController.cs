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
        public ActionResult index()
        {
            return View(Membership.GetAllUsers().Cast<MembershipUser>().OrderByDescending(m => m.IsApproved).ThenBy(m => m.UserName));
        }

        public ActionResult approve(Guid id)
        {
            var provider = (Membership.Provider as RedisMembershipProvider);
            provider.ChangeApproval(id, true);
            return RedirectToAction("Index");
        }

        public ActionResult unapprove(Guid id)
        {
            var provider = (Membership.Provider as RedisMembershipProvider);
            provider.ChangeApproval(id, false);
            return RedirectToAction("Index");
        }
    }
}