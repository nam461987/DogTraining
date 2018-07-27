using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Website
{
    public class Constants
    {
        //public const int Branch = 1;
    }
    public class Strings
    {
        public static string YelpApiKey = ConfigurationManager.AppSettings["YelpApiKey"];
        public static string BusinessId = ConfigurationManager.AppSettings["BusinessId"];
        public static string EmailStr = ConfigurationManager.AppSettings["Email"];
        public static string EPassStr = ConfigurationManager.AppSettings["EPass"];
    }
}