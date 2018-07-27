using Admin.Common;
using Admin.Models.video;
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
    public class VideoController : Controller
    {
        // GET: Video
        public ActionResult Video()
        {
            return View();
        }
        public JsonResult GetAllVideo(DataModel obj)
        {
            string order = string.Empty;
            int totalRecords = int.MinValue;

            if (obj._od != null)
            {
                order = " Order By " + obj._od.FirstOrDefault().Key + " " + obj._od.FirstOrDefault().Value;
            }

            List<Video> Records_source = new List<Video>();
            List<Video_Config> Records = new List<Video_Config>();
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
                    Records_source = DogTraining.Video.Query("Where Status<2 AND " + querystring + order + "").ToList();
                }
                else
                {
                    Records_source = DogTraining.Video.Query("Where Status<2 " + order + "").ToList();
                }

                // Map du lieu sang Model khac
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Video, Video_Config>();
                });
                IMapper mapper = config.CreateMapper();
                Records = mapper.Map<List<Video>, List<Video_Config>>(Records_source);

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
        public JsonResult InsertVideo(Video md)
        {
            var msg = string.Empty;
            Video NewRecord = new Video();

            try
            {
                NewRecord.Name = md.Name;
                NewRecord.Description = md.Description;
                NewRecord.Url = md.Url;
                NewRecord.Status = 1;
                NewRecord.CreatedDate = DateTime.Now;
                NewRecord.Save();
            }
            catch(Exception ex)
            {
                msg = ex.ToString();
                return Json(new { Result = 0, Message = msg }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { Result = 1, Records = NewRecord, Message = msg }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetVideoById(int id)
        {
            Video Record = new Video();
            string msg = string.Empty;

            try
            {
                Record = DogTraining.Video.SingleOrDefault("Where Id=@0 and Status<2", id);
            }
            catch (Exception ex)
            {
                msg = ex.ToString();
                return Json(new { Result = 0, Message = msg }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { Result = 1, Records = Record, Message = msg }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult UpdateVideo(Video md)
        {
            Video Record = DogTraining.Video.SingleOrDefault("where Id=@0", md.Id);
            string msg = string.Empty;
            try
            {
                Record.Name = md.Name;
                Record.Description = md.Description;
                Record.Url = md.Url;
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

        public JsonResult DeleteVideo(int id, int status)
        {
            Video Record = DogTraining.Video.SingleOrDefault("where Id=@0", id);
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