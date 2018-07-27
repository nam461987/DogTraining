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
    public class ServiceController : Controller
    {
        // GET: News
        public ActionResult Service()
        {
            return View();
        }
        public JsonResult GetAllService(DataModel obj)
        {
            string order = string.Empty;
            int totalRecords = int.MinValue;

            if (obj._od != null)
            {
                order = " Order By " + obj._od.FirstOrDefault().Key + " " + obj._od.FirstOrDefault().Value;
            }

            List<News> Records_source = new List<News>();
            List<News_Config> Records = new List<News_Config>();
            string msg = string.Empty;
            try
            {
                if (obj._c != null)
                {
                    string querystring = "";
                    foreach (var k in obj._c)
                    {

                        switch (k.Key)
                        {

                            case "Name":
                                querystring += k.Value.ToString();
                                break;
                            default:
                                querystring += k.Key + "=" + k.Value.ToString();
                                break;
                        }
                        if (!k.Equals(obj._c.Last()))
                        {
                            querystring += " AND ";
                        }
                    }
                    Records_source = DogTraining.News.Query("Where Status<2 AND " + querystring + order + "").ToList();
                }
                else
                {
                    Records_source = DogTraining.News.Query("Where Status<2 " + order + "").ToList();
                }

                // Map du lieu sang Model khac
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<News, News_Config>();
                });
                IMapper mapper = config.CreateMapper();
                Records = mapper.Map<List<News>, List<News_Config>>(Records_source);

                int pSize = obj._lm;
                totalRecords = Records.Count();
                if (totalRecords > 1)
                {
                    Records = Records.Skip(obj._os).Take(pSize).ToList();
                }
            }
            catch (Exception ex)
            {
                msg = ex.ToString();
            }

            return Json(new { Result = 1, TotalRecordCount = totalRecords, Records = Records, Message = msg }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult InsertService(News md)
        {
            var msg = string.Empty;
            News NewRecord = new News();

            try
            {
                NewRecord.TypeId = 2;
                NewRecord.Name = md.Name;
                NewRecord.Description = md.Description;
                NewRecord.NewsContent = md.NewsContent;
                NewRecord.PostedDate = DateTime.Now;
                NewRecord.Tags = md.Tags;
                NewRecord.Avatar = md.Avatar;
                NewRecord.Status = 1;
                NewRecord.Save();
            }
            catch (Exception ex)
            {
                msg = ex.ToString();
                return Json(new { Result = 0, Message = msg }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { Result = 1, Records = NewRecord, Message = msg }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetServiceById(int id)
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
        public JsonResult UpdateService(News md)
        {
            News Record = DogTraining.News.SingleOrDefault("where Id=@0", md.Id);
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

        public JsonResult DeleteService(int id, int status)
        {
            News Record = DogTraining.News.SingleOrDefault("where Id=@0", id);
            string msg = string.Empty;
            try
            {
                Record.Status = status;
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