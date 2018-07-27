using Admin.Services;
using DogTraining;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace cotoiday_admin.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(Admin_Account md)
        {
            var error = String.Empty;
            if (md.UserName != null || md.UserName != "")
            {
                var convertPass = WebsiteExtension.EncryptPassword(md.PasswordHash);
                try
                {
                    List<Admin_Account> CheckUser = Admin_Account.Query("Where UserName=@0 AND PasswordHash=@1 AND BranchId=@2 AND Status=1",
                                                    md.UserName,convertPass,1).ToList();

                    if (CheckUser.Count > 0 && CheckUser.Count <2)
                    {
                        Session["UserID"] = CheckUser.FirstOrDefault().Id.ToString();
                        Session["TypeId"] = CheckUser.FirstOrDefault().TypeId.ToString();
                        Session["UserName"] = CheckUser.FirstOrDefault().UserName.ToString();
                        Session["BranchId"] = CheckUser.FirstOrDefault().BranchId.ToString();
                        Session.Timeout = 120;
                        return Redirect("/");
                    }
                    else
                    {
                        return Redirect("/Login/Login");
                    }
                }
                catch (Exception ex)
                {
                    return Redirect("/Login/Login");
                }
            }
            else
            {
                return Redirect("/Login/Login");
            }
        }
        public ActionResult Logout()
        {
            Session.Abandon();
            FormsAuthentication.SignOut();
            return Redirect("/Login/Login");
        }
        [HttpGet]
        public ActionResult ChangePass()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ChangePass(Admin_Account md, string OldPassWord)
        {
            var convertPass = WebsiteExtension.EncryptPassword(md.PasswordHash);

            Admin_Account CheckUser = Admin_Account.SingleOrDefault("Where Id=@0 AND UserName=@1 AND PasswordHash=@2 AND Status=1",
                                                    md.Id,md.UserName, WebsiteExtension.EncryptPassword(OldPassWord));

            if (CheckUser != null)
            {
                try
                {
                    CheckUser.PasswordHash = convertPass;
                    CheckUser.UpdatedDate = DateTime.Now;
                    CheckUser.UpdatedStaffId = Convert.ToInt32(Session["UserID"]);
                    CheckUser.Save();

                    return Redirect("/Login/Login");
                }
                catch
                {
                    return View();
                }
            }
            return View();
        }
    }
}