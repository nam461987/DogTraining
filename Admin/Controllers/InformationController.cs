using DogTraining;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Admin.Controllers
{
    public class InformationController : Controller
    {
        // GET: Information
        public ActionResult Information()
        {
            return View();
        }
        public JsonResult GetInformationById(int id)
        {
            Information Record = new Information();
            string msg = string.Empty;

            try
            {
                Record = DogTraining.Information.SingleOrDefault("Where Id=@0", id);
            }
            catch (Exception ex)
            {
                msg = ex.ToString();
                return Json(new { Result = 0, Message = msg }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { Result = 1, Records = Record, Message = msg }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult UpdateInformation(Information md)
        {
            Information Record = DogTraining.Information.SingleOrDefault("where Id=@0", md.Id);
            string msg = string.Empty;
            try
            {
                Record.SiteName = md.SiteName;
                Record.Phone = md.Phone;
                Record.Email = md.Email;
                Record.Address = md.Address;
                Record.Facebook = md.Facebook;
                Record.Instagram = md.Instagram;
                Record.Twitter = md.Twitter;
                Record.Save();
            }
            catch (Exception ex)
            {
                msg = ex.ToString();
                return Json(new { Result = 0, Message = msg }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { Result = 1, Records = Record, Message = msg }, JsonRequestBehavior.AllowGet);
        }
    }
}