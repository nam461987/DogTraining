using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Admin.Common
{
    public class Constants
    {
        public const int Branch = 1;
    }
    public class Strings
    {
        public static string UploadFolderRoot = ConfigurationManager.AppSettings["UploadFolderRoot"];
        public static string ForwardUploadFolderRoot = ConfigurationManager.AppSettings["ForwardUploadFolderRoot"];
    }
}