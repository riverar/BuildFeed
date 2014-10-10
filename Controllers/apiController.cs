using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BuildFeed.Models;

namespace BuildFeed.Controllers
{
    public class apiController : ApiController
    {
        public IEnumerable<Build> GetBuilds()
        {
            return Build.SelectInBuildOrder();
        }
    }
}
