using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using System.Web.Mvc;
using BuildFeed.Models;
using X.Web.RSS;
using X.Web.RSS.Enumerators;
using X.Web.RSS.Structure;
using X.Web.RSS.Structure.Validators;

namespace BuildFeed.Controllers
{
    public class rssController : Controller
    {
        //
        // GET: /rss/
        public async Task<ActionResult> index()
        {
            var builds = Build.SelectInBuildOrder().Take(20);

            RssDocument rdoc = new RssDocument()
            {
                Channel = new RssChannel()
                {
                    Title = "BuildFeed RSS - Recently Compiled",
                    Description = "",
                    Generator = "BuildFeed.net RSS Controller",
                    Link = new RssUrl(string.Format("{0}://{1}", Request.Url.Scheme, Request.Url.Authority)),
                    SkipHours = new List<Hour>(),
                    SkipDays = new List<Day>(),

                    Items = (from build in builds
                             select new RssItem()
                             {
                                 Title = build.FullBuildString,
                                 Link = new RssUrl(string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Action("info", new { controller = "Build", id = build.Id }))),
                                 Guid = new RssGuid() { IsPermaLink = true, Value = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Action("info", new { controller = "Build", id = build.Id })) },
                             }).ToList()
                }
            };

            Response.ContentType = "application/rss+xml";

            await Response.Output.WriteAsync(rdoc.ToXml());

            return new EmptyResult();
        }

        public async Task<ActionResult> added()
        {
            var builds = Build.Select().OrderByDescending(b => b.Added).Take(20);

            RssDocument rdoc = new RssDocument()
            {
                Channel = new RssChannel()
                {
                    Title = "BuildFeed RSS - Recently Added",
                    Description = "",
                    Generator = "BuildFeed.net RSS Controller",
                    Link = new RssUrl(string.Format("{0}://{1}", Request.Url.Scheme, Request.Url.Authority)),
                    SkipHours = new List<Hour>(),
                    SkipDays = new List<Day>(),

                    Items = (from build in builds
                             select new RssItem()
                             {
                                 Title = build.FullBuildString,
                                 Link = new RssUrl(string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Action("info", new { controller = "Build", id = build.Id }))),
                                 Guid = new RssGuid() { IsPermaLink = true, Value = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Action("info", new { controller = "Build", id = build.Id })) },
                                 PubDate = build.Added
                             }).ToList()
                }
            };

            Response.ContentType = "application/rss+xml";

            await Response.Output.WriteAsync(rdoc.ToXml());

            return new EmptyResult();
        }


        public async Task<ActionResult> version()
        {
            var builds = Build.SelectInVersionOrder()
                .Take(20);


            RssDocument rdoc = new RssDocument()
            {
                Channel = new RssChannel()
                {
                    Title = "BuildFeed RSS - Highest Version",
                    Description = "",
                    Generator = "BuildFeed.net RSS Controller",
                    Link = new RssUrl(string.Format("{0}://{1}", Request.Url.Scheme, Request.Url.Authority)),
                    SkipHours = new List<Hour>(),
                    SkipDays = new List<Day>(),

                    Items = (from build in builds
                             select new RssItem()
                             {
                                 Title = build.FullBuildString,
                                 Link = new RssUrl(string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Action("info", new { controller = "Build", id = build.Id }))),
                                 Guid = new RssGuid() { IsPermaLink = true, Value = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Action("info", new { controller = "Build", id = build.Id })) },
                             }).ToList()
                }
            };

            Response.ContentType = "application/rss+xml";

            await Response.Output.WriteAsync(rdoc.ToXml());

            return new EmptyResult();
        }


        public async Task<ActionResult> flight(LevelOfFlight id)
        {
            var builds = Build.SelectInBuildOrder()
                .Where(b => b.FlightLevel == id)
                .Take(20);


            RssDocument rdoc = new RssDocument()
            {
                Channel = new RssChannel()
                {
                    Title = string.Format("BuildFeed RSS - {0} Flight Level", id),
                    Description = "",
                    Generator = "BuildFeed.net RSS Controller",
                    Link = new RssUrl(string.Format("{0}://{1}", Request.Url.Scheme, Request.Url.Authority)),
                    SkipHours = new List<Hour>(),
                    SkipDays = new List<Day>(),

                    Items = (from build in builds
                             select new RssItem()
                             {
                                 Title = build.FullBuildString,
                                 Link = new RssUrl(string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Action("info", new { controller = "Build", id = build.Id }))),
                                 Guid = new RssGuid() { IsPermaLink = true, Value = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Action("info", new { controller = "Build", id = build.Id })) },
                             }).ToList()
                }
            };

            Response.ContentType = "application/rss+xml";

            await Response.Output.WriteAsync(rdoc.ToXml());

            return new EmptyResult();
        }
    }
}