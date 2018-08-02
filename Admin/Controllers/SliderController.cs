using Admin.Common;
using Admin.Models.slider;
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
    public class SliderController : Controller
    {
        // GET: Slider
        public ActionResult Slider()
        {
            return View();
        }
        public JsonResult GetAllSlider(DataModel obj)
        {
            string order = string.Empty;
            int totalRecords = int.MinValue;

            if (obj._od != null)
            {
                order = " Order By " + obj._od.FirstOrDefault().Key + " " + obj._od.FirstOrDefault().Value;
            }

            List<Slider> Records_source = new List<Slider>();
            List<Slider_Config> Records = new List<Slider_Config>();
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

                            case "Title":
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
                    Records_source = DogTraining.Slider.Query("Where Status<2 AND " + querystring + order + "").ToList();
                }
                else
                {
                    Records_source = DogTraining.Slider.Query("Where Status<2 " + order + "").ToList();
                }

                // Map du lieu sang Model khac
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Slider, Slider_Config>();
                });
                IMapper mapper = config.CreateMapper();
                Records = mapper.Map<List<Slider>, List<Slider_Config>>(Records_source);

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
        public JsonResult GetSliderById(int id)
        {
            Slider Record = new Slider();
            string msg = string.Empty;

            try
            {
                Record = DogTraining.Slider.SingleOrDefault("Where Id=@0 and Status<2", id);
            }
            catch (Exception ex)
            {
                msg = ex.ToString();
                return Json(new { Result = 0, Message = msg }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { Result = 1, Records = Record, Message = msg }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult UpdateSlider(Slider md)
        {
            Slider Record = DogTraining.Slider.SingleOrDefault("where Id=@0", md.Id);
            string msg = string.Empty;
            try
            {
                Record.Image = md.Image;
                Record.Title = md.Title;
                Record.Description = md.Description;
                Record.ButtonName = md.ButtonName;
                Record.ButtonLink = md.ButtonLink;
                Record.Save();
            }
            catch (Exception ex)
            {
                msg = ex.ToString();
                return Json(new { Result = 0, Message = msg }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { Result = 1, Records = Record, Message = msg }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteSlider(int id, int status)
        {
            Slider Record = DogTraining.Slider.SingleOrDefault("where Id=@0", id);
            string msg = string.Empty;
            try
            {
                Record.Status = status;
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