using DogTraining;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Website.Controllers
{
    public class ShareController : Controller
    {
        // GET: Share
        [ChildActionOnly]
        public ActionResult _Header()
        {
            List<News> Records = News.Query("Where TypeId=2 And Status=1").ToList();

            ViewBag.Services = Records;

            return PartialView("~/Views/Shared/_Header.cshtml");
        }
    }
}