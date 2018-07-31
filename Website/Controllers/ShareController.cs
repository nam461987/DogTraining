using DogTraining;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        public JavaScriptResult GetGlobalInfo()
        {
            //var str = String.Empty;
            var str = "var globalObj={";
            if (Session["GlobalObject"] == null)
            {
                Information Record = Information.SingleOrDefault("");

                //foreach (PropertyInfo prop in Record.GetType().GetProperties())
                //{
                //    if (prop.Name!="Id" && prop.GetValue(Record, null)!= null && prop.GetValue(Record, null).ToString().Replace(" ", String.Empty).Length > 0)
                //    {
                //        str += string.Format("var {0}='{1}';", "global" + prop.Name, prop.GetValue(Record, null).ToString());
                //    }
                //    else
                //    {
                //        str += string.Format("var {0}='{1}';", "global" + prop.Name, "null");
                //    }
                //}
                foreach (PropertyInfo prop in Record.GetType().GetProperties())
                {
                    if (prop.Name != "Id" && prop.GetValue(Record, null) != null && prop.GetValue(Record, null).ToString().Replace(" ", String.Empty).Length > 0)
                    {
                        str += string.Format("{0}:'{1}',", "global" + prop.Name, prop.GetValue(Record, null).ToString());
                    }
                    else
                    {
                        str += string.Format("{0}:'{1}',", "global" + prop.Name, null);
                    }
                }

                str = str.Remove(str.Length - 1);

                str += "}";
                Session["GlobalObject"] = str;
            }
            str = Session["GlobalObject"].ToString();

            return JavaScript(str);
        }
    }
}