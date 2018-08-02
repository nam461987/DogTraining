using Admin.Common;
using Admin.Models.service;
using AutoMapper;
using DogTraining;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static Admin.Models.AjaxRequestData;

namespace Admin.Controllers
{
    [SessionAuthorize]
    public class AboutUsController : Controller
    {
        // GET: AboutUs
        public ActionResult AboutUs()
        {
            return View();
        }
        public JsonResult GetAboutUsById(int id)
        {
            News Record = new News();
            string msg = string.Empty;

            try
            {
                Record = DogTraining.News.SingleOrDefault("Where Id=@0 and Status<2", id);
            }
            catch (Exception ex)
            {
                msg = ex.ToString();
                return Json(new { Result = 0, Message = msg }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { Result = 1, Records = Record, Message = msg }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult UpdateAboutUs(News md)
        {
            News Record = DogTraining.News.SingleOrDefault("where Id=@0 And TypeId=3", md.Id);
            string msg = string.Empty;
            try
            {
                Record.Name = md.Name;
                Record.Description = md.Description;
                Record.NewsContent = md.NewsContent;
                Record.Tags = md.Tags;
                Record.Avatar = md.Avatar;
                Record.UpdatedDate = DateTime.Now;
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