using Admin.Common;
using Admin.Models.dog;
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
    public class DogController : Controller
    {
        // GET: Dog
        public ActionResult Dog()
        {
            return View();
        }
        public JsonResult GetAllDog(DataModel obj)
        {
            string order = string.Empty;
            int totalRecords = int.MinValue;

            if (obj._od != null)
            {
                order = " Order By " + obj._od.FirstOrDefault().Key + " " + obj._od.FirstOrDefault().Value;
            }

            List<Dog> Records_source = new List<Dog>();
            List<Dog_Config> Records = new List<Dog_Config>();
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
                    Records_source = DogTraining.Dog.Query("Where Status<2 AND " + querystring + order + "").ToList();
                }
                else
                {
                    Records_source = DogTraining.Dog.Query("Where Status<2 " + order + "").ToList();
                }

                // Map du lieu sang Model khac
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Dog, Dog_Config>();
                });
                IMapper mapper = config.CreateMapper();
                Records = mapper.Map<List<Dog>, List<Dog_Config>>(Records_source);

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
        public JsonResult InsertDog(Dog md)
        {
            var msg = string.Empty;
            Dog NewRecord = new Dog();

            try
            {
                NewRecord.Name = md.Name;
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
        public JsonResult GetDogById(int id)
        {
            Dog Record = new Dog();
            string msg = string.Empty;

            try
            {
                Record = DogTraining.Dog.SingleOrDefault("Where Id=@0 and Status<2", id);
            }
            catch (Exception ex)
            {
                msg = ex.ToString();
                return Json(new { Result = 0, Message = msg }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { Result = 1, Records = Record, Message = msg }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult UpdateDog(Dog md)
        {
            Dog Record = DogTraining.Dog.SingleOrDefault("where Id=@0", md.Id);
            string msg = string.Empty;
            try
            {
                Record.Name = md.Name;
                Record.Avatar = md.Avatar;
                Record.Save();
            }
            catch (Exception ex)
            {
                msg = ex.ToString();
                return Json(new { Result = 0, Message = msg }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { Result = 1, Records = Record, Message = msg }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteDog(int id, int status)
        {
            Dog Record = DogTraining.Dog.SingleOrDefault("where Id=@0", id);
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