using Admin.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DogTraining;

namespace Admin.Controllers
{
    [SessionAuthorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public JavaScriptResult GetUserPermissionJs()
        {
            List<Admin_Group_Permission_View00> perList = Admin_Group_Permission_View00.Query("Where GroupId=@0 And Status=1",
                Convert.ToInt32(Session["TypeId"])).ToList();
            var s = string.Format("var permissions = {0};", JsonConvert.SerializeObject(perList.Select(c => c.PermissionIdName)));
            return JavaScript(s);
        }
    }
}