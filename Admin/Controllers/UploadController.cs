using Admin.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Admin.Controllers
{
    public class UploadController : Controller
    {
        // GET: Upload
        public JsonResult MultipleUpload()
        {
            var imageStr = string.Empty;
            foreach (string item in Request.Files)
            {
                HttpPostedFileBase file = Request.Files[item];
                Random r = new Random();
                string filename = r.Next().ToString() + "_" + file.FileName;

                //create folder by month
                string now = DateTime.Now.ToString("MMyyyy");
                string forwardFolder = @"" + Strings.ForwardUploadFolderRoot + "";
                string newFolder = @"" + Strings.UploadFolderRoot + "" + now + "";

                string firstPath = @"" + Strings.ForwardUploadFolderRoot + "\\" + filename;
                string destinationPath = @"" + Strings.UploadFolderRoot + "" + now + "\\" + filename;

                file.SaveAs(firstPath);

                try
                {
                    // Determine whether the directory exists.
                    if (Directory.Exists(newFolder))
                    {
                        //save file to exists folder
                        System.IO.File.Move(firstPath, destinationPath);
                        imageStr = string.Join(", ", "/Content/uploads/" + now + "/" + filename);
                    }

                    // Try to create the directory.
                    DirectoryInfo di = Directory.CreateDirectory(newFolder);
                    DirectoryInfo dii = Directory.CreateDirectory(newFolder + "\\thumb");
                    //save file to new folder
                    System.IO.File.Move(firstPath, destinationPath);
                    imageStr = string.Join(", ", "/Content/uploads/" + now + "/" + filename);
                }
                catch (Exception e)
                {
                    Console.WriteLine("The process failed: {0}", e.ToString());
                }
            }
            return Json(imageStr);
        }
    }
}