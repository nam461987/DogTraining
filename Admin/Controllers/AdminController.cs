using Admin.Common;
using Admin.Models.admin;
using Admin.Services;
using AutoMapper;
using DogTraining;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using static Admin.Models.AjaxRequestData;

namespace Admin.Controllers
{
    [SessionAuthorize]
    public class AdminController : Controller
    {
        // GET: Admin
        [HasCredential(Role = "admin_user_list")]
        public ActionResult Account()
        {
            return View();
        }
        [HasCredential(Role = "admin_group_list")]
        public ActionResult Group()
        {
            return View();
        }
        [HasCredential(Role = "admin_permission_list")]
        public ActionResult Permission()
        {
            return View();
        }
        [HasCredential(Role = "admin_group_permission_list")]
        public ActionResult GroupPermission()
        {
            return View();
        }

        // ---------Phan thao tac du lieu cho Account---------------------
        [HasCredential(Role = "admin_user_list")]
        public JsonResult Get_Account_List(DataModel obj)
        {
            string order = string.Empty;
            int totalRecords = int.MinValue;

            if (obj._od != null)
            {
                order = " Order By " + obj._od.FirstOrDefault().Key + " " + obj._od.FirstOrDefault().Value;
            }

            List<Admin_Account> AccList_source = new List<Admin_Account>();
            List<Admin_Account_Config> AccList = new List<Admin_Account_Config>();
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

                            case "DisplayName":
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
                    AccList_source = Admin_Account.Query("Where Status=1 AND " + querystring + order + "").ToList();
                }
                else
                {
                    AccList_source = Admin_Account.Query("Where Status=1 " + order + "").ToList();
                }

                // Map du lieu sang Model khac
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Admin_Account, Admin_Account_Config>();
                });
                IMapper mapper = config.CreateMapper();
                AccList = mapper.Map<List<Admin_Account>, List<Admin_Account_Config>>(AccList_source);

                int pSize = obj._lm;
                totalRecords = AccList.Count();
                if (totalRecords > 1)
                {
                    AccList = AccList.Skip(obj._os).Take(pSize).ToList();
                }
            }
            catch (Exception ex)
            {
                msg = ex.ToString();
            }

            return Json(new { Result = 1, TotalRecordCount = totalRecords, Records = AccList, Message = msg }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetGroupOption()
        {
            var options = new List<Option>
                {
                    new Option
                    {
                        DisplayText = "Chọn nhóm",
                        Value = 0
                    }
                };

            List<Admin_Group> groups = Admin_Group.Query("Where Id!=1 And Status=1").ToList();

            if (groups.Any())
            {
                options.AddRange(groups.Select(c => new Option
                {
                    DisplayText = Convert.ToString(c.Name),
                    Value = Convert.ToInt32(c.Id)
                }).ToList());
            }

            return Json(new
            {
                Result = 1,
                Records = options,
                options.Count
            }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetModuleOption()
        {
            var options = new List<Option2>
                {
                    new Option2
                    {
                        DisplayText = "Chọn module",
                        Value = ""
                    }
                };

            List<Admin_Permission> groups = Admin_Permission.Query("Where Status=1").ToList();

            if (groups.Any())
            {
                options.AddRange(groups.Select(c => c.Code).Distinct().Select(c => new Option2
                {
                    DisplayText = Convert.ToString(c),
                    Value = Convert.ToString(c)
                }).ToList());
            }

            return Json(new
            {
                Result = 1,
                Records = options,
                Total = options.Count
            }, JsonRequestBehavior.AllowGet);
        }
        [HasCredential(Role = "admin_user_delete")]
        public JsonResult Change_Account_Active(Admin_Account obj)
        {
            List<Admin_Account> Acc = Admin_Account.Query("Where Id=@0", obj.Id).ToList();
            try
            {
                if (Acc.Count > 0)
                {
                    Acc.FirstOrDefault().Active = obj.Active;
                    Acc.FirstOrDefault().Save();
                }
            }
            catch
            {
                return Json(0);
            }
            return Json(new { Result = 1, Records = Acc });
        }
        [HasCredential(Role = "admin_user_update")]
        public JsonResult Update_Account(Admin_Account obj)
        {
            List<Admin_Account> Acc = Admin_Account.Query("Where Id=@0", obj.Id).ToList();
            try
            {
                if (Acc.Any())
                {
                    if (!string.IsNullOrEmpty(obj.PasswordHash))
                    {
                        Acc.FirstOrDefault().PasswordHash = WebsiteExtension.EncryptPassword(obj.PasswordHash);
                    }
                    Acc.FirstOrDefault().UserName = obj.UserName;
                    Acc.FirstOrDefault().FullName = obj.FullName;
                    Acc.FirstOrDefault().TypeId = obj.TypeId;
                    Acc.FirstOrDefault().Mobile = obj.Mobile;
                    Acc.FirstOrDefault().Email = obj.Email;
                    Acc.FirstOrDefault().Address = obj.Address;
                    Acc.SingleOrDefault().UpdatedDate = DateTime.Now;
                    Acc.FirstOrDefault().Save();
                }
            }
            catch (Exception ex)
            {
                return Json(0);
            }
            return Json(new { Result = 1, Records = Acc });
        }
        [HasCredential(Role = "admin_user_create")]
        public JsonResult Create_Account(Admin_Account obj)
        {
            Admin_Account Acc = new Admin_Account();
            try
            {
                if (!string.IsNullOrEmpty(obj.PasswordHash))
                {
                    Acc.PasswordHash = WebsiteExtension.EncryptPassword(obj.PasswordHash);
                }
                Acc.BranchId = Constants.Branch;
                Acc.UserName = obj.UserName;
                Acc.FullName = obj.FullName;
                Acc.TypeId = obj.TypeId;
                Acc.Mobile = obj.Mobile;
                Acc.Email = obj.Email;
                Acc.Address = obj.Address;
                Acc.CreatedDate = DateTime.Now;
                Acc.Status = 1;
                Acc.Active = 1;
                Acc.Save();
            }
            catch (Exception ex)
            {
                return Json(0);
            }
            return Json(new { Result = 1, Records = Acc });
        }
        // END ---------Phan thao tac du lieu cho Account---------------------

        // ---------Phan thao tac du lieu cho Group---------------------
        [HasCredential(Role = "admin_group_list")]
        public JsonResult Get_Group_List(DataModel obj)
        {
            string order = string.Empty;
            int totalRecords = int.MinValue;
            if (obj._od != null)
            {
                order = " Order By " + obj._od.FirstOrDefault().Key + " " + obj._od.FirstOrDefault().Value;
            }

            List<Admin_Group> AccList_source = new List<Admin_Group>();
            List<Group_Config> AccList = new List<Group_Config>();
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
                    AccList_source = Admin_Group.Query("Where " + querystring + order + "").ToList();
                }
                else
                {
                    AccList_source = Admin_Group.Query("" + order + "").ToList();
                }

                // Map du lieu sang Model khac
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Admin_Group, Group_Config>();
                });
                IMapper mapper = config.CreateMapper();
                AccList = mapper.Map<List<Admin_Group>, List<Group_Config>>(AccList_source);

                int pSize = obj._lm;
                totalRecords = AccList.Count();
                if (totalRecords > 1)
                {
                    AccList = AccList.Skip(obj._os).Take(pSize).ToList();
                }

            }
            catch (Exception ex)
            {
                msg = ex.ToString();
            }

            return Json(new { Result = 1, TotalRecordCount = totalRecords, Records = AccList, Message = msg }, JsonRequestBehavior.AllowGet);
        }
        [HasCredential(Role = "admin_group_delete")]
        public JsonResult Delete_Group(Admin_Group obj)
        {
            List<Admin_Group> Acc = Admin_Group.Query("Where Id=@0", obj.Id).ToList();
            try
            {
                if (Acc.Count > 0)
                {
                    Acc.FirstOrDefault().Status = obj.Status;
                    Acc.FirstOrDefault().Save();
                }
            }
            catch
            {
                return Json(0);
            }
            return Json(new { Result = 1, Records = Acc });
        }

        [HasCredential(Role = "admin_group_update")]
        public JsonResult Update_Group(Admin_Group obj)
        {
            List<Admin_Group> Acc = Admin_Group.Query("Where Id=@0", obj.Id).ToList();
            try
            {
                if (Acc.Any())
                {
                    Acc.FirstOrDefault().Name = obj.Name;
                    Acc.FirstOrDefault().Description = obj.Description;
                    Acc.SingleOrDefault().UpdatedDate = DateTime.Now;
                    Acc.FirstOrDefault().Save();
                }
            }
            catch (Exception ex)
            {
                return Json(0);
            }
            return Json(new { Result = 1, Records = Acc });
        }
        [HasCredential(Role = "admin_group_create")]
        public JsonResult Create_Group(Admin_Group obj)
        {
            Admin_Group Acc = new Admin_Group();
            try
            {
                Acc.Name = obj.Name;
                Acc.Description = obj.Description;
                Acc.CreatedDate = DateTime.Now;
                Acc.Status = 1;
                Acc.Save();
            }
            catch (Exception ex)
            {
                return Json(0);
            }
            return Json(new { Result = 1, Records = Acc });
        }
        //END ---------Phan thao tac du lieu cho Group---------------------

        // ---------Phan thao tac du lieu cho Permission---------------------
        [HasCredential(Role = "admin_permission_list")]
        public JsonResult Get_Permission_List(DataModel obj)
        {
            string order = string.Empty;
            int totalRecords = int.MinValue;
            if (obj._od != null)
            {
                order = " Order By " + obj._od.FirstOrDefault().Key + " " + obj._od.FirstOrDefault().Value;
            }

            List<Admin_Permission> AccList_source = new List<Admin_Permission>();
            List<Admin_Permission_Config> AccList = new List<Admin_Permission_Config>();
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
                    AccList_source = Admin_Permission.Query("Where Status=1 AND " + querystring + order + "").ToList();
                }
                else
                {
                    AccList_source = Admin_Permission.Query("Where  Status=1 " + order + "").ToList();
                }

                // Map du lieu sang Model khac
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Admin_Permission, Admin_Permission_Config>();
                });
                IMapper mapper = config.CreateMapper();
                AccList = mapper.Map<List<Admin_Permission>, List<Admin_Permission_Config>>(AccList_source);

                int pSize = obj._lm;
                totalRecords = AccList.Count();
                if (totalRecords > 1)
                {
                    AccList = AccList.Skip(obj._os).Take(pSize).ToList();
                }

            }
            catch (Exception ex)
            {
                msg = ex.ToString();
            }

            return Json(new { Result = 1, TotalRecordCount = totalRecords, Records = AccList, Message = msg }, JsonRequestBehavior.AllowGet);
        }
        [HasCredential(Role = "admin_permission_delete")]
        public JsonResult Delete_Permission(Admin_Permission obj)
        {
            List<Admin_Permission> Acc = Admin_Permission.Query("Where Id=@0", obj.Id).ToList();
            try
            {
                if (Acc.Count > 0)
                {
                    Acc.FirstOrDefault().Status = obj.Status;
                    Acc.FirstOrDefault().Save();
                }
            }
            catch
            {
                return Json(0);
            }
            return Json(new { Result = 1, Records = Acc });
        }
        [HasCredential(Role = "admin_permission_update")]
        public JsonResult Update_Permission(Admin_Permission obj)
        {
            List<Admin_Permission> Acc = Admin_Permission.Query("Where Id=@0", obj.Id).ToList();
            try
            {
                if (Acc.Any())
                {
                    Acc.FirstOrDefault().Name = obj.Name;
                    Acc.FirstOrDefault().Code = obj.Code;
                    Acc.FirstOrDefault().Description = obj.Description;
                    Acc.SingleOrDefault().UpdatedDate = DateTime.Now;
                    Acc.FirstOrDefault().Save();
                }
            }
            catch (Exception ex)
            {
                return Json(0);
            }
            return Json(new { Result = 1, Records = Acc });
        }
        [HasCredential(Role = "admin_permission_create")]
        public JsonResult Create_Permission(Admin_Permission obj)
        {
            Admin_Permission Acc = new Admin_Permission();
            try
            {
                Acc.Name = obj.Name;
                Acc.Code = obj.Code;
                Acc.Description = obj.Description;
                Acc.CreatedDate = DateTime.Now;
                Acc.Status = 1;
                Acc.Save();

                //set permission for admin group
                if (Acc.Id > 0)
                {
                    Admin_Group_Permission newGrPer = new Admin_Group_Permission();
                    {
                        newGrPer.GroupId = 1;
                        newGrPer.PermissionId = Acc.Id;
                        newGrPer.Status = 1;
                        newGrPer.CreatedDate = DateTime.Now;
                        newGrPer.Save();
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(0);
            }
            return Json(new { Result = 1, Records = Acc });
        }
        //END ---------Phan thao tac du lieu cho Permission---------------------

        [HasCredential(Role = "admin_group_permission_list")]
        public JsonResult GetPermissionByGroupAndModule(int GroupId, string Module)
        {
            var db = new PetaPoco.Database("DogTrainingCon");
            var result = db.Query<Admin_Group_Permission_Config>(";EXEC GetPermissionByGroupAndModule @@GroupId = @0, @@Module=@1",
                GroupId, Module).ToList();
            return Json(new { Result = 1, Records = result });
        }
        [HasCredential(Role = "admin_group_permission_create,admin_group_permission_update")]
        public JsonResult InsertOrUpdatePermission(int GroupId, int PermissionId, int Status)
        {
            try
            {
                var db = new PetaPoco.Database("DogTrainingCon");
                var result = db.Fetch<dynamic>(";EXEC InsertOrUpdatePermission @@GroupId = @0, @@PermissionId=@1,@@Status=@2,@@CurDate=@3",
                    GroupId, PermissionId, Status,DateTime.Now);
            }
            catch
            {
                return Json(new { Result = 0 });
            }
            return Json(new { Result = 1 });
        }
    }
}