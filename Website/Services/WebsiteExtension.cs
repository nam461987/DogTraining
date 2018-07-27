using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Routing;

namespace Website.Services
{
    public static class WebsiteExtension
    {
        private static readonly string[] VietnameseSigns =
        {

            "aAeEoOuUiIdDyY",

            "áàạảãâấầậẩẫăắằặẳẵ",

            "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",

            "éèẹẻẽêếềệểễ",

            "ÉÈẸẺẼÊẾỀỆỂỄ",

            "óòọỏõôốồộổỗơớờợởỡ",

            "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",

            "úùụủũưứừựửữ",

            "ÚÙỤỦŨƯỨỪỰỬỮ",

            "íìịỉĩ",

            "ÍÌỊỈĨ",

            "đ",

            "Đ",

            "ýỳỵỷỹ",

            "ÝỲỴỶỸ"

        };

        public static string Slug(string value)
        {
            //First to lower case
            value = _stripVietnameseSigns(value.ToLowerInvariant());
            //Remove all accents
            var bytes = Encoding.GetEncoding("Cyrillic").GetBytes(value);
            value = Encoding.ASCII.GetString(bytes);
            //Replace spaces
            value = Regex.Replace(value, @"\s", "-", RegexOptions.Compiled);
            //Remove invalid chars
            value = Regex.Replace(value, @"[^\w\s\p{Pd}]", "", RegexOptions.Compiled);
            //Trim dashes from end
            value = value.Trim('-', '_');
            //Replace double occurences of - or \_
            value = Regex.Replace(value, @"([-_]){2,}", "$1", RegexOptions.Compiled);
            return value;
        }

        private static string _stripVietnameseSigns(string str)
        {
            for (var i = 1; i < VietnameseSigns.Length; i++)
            {
                for (var j = 0; j < VietnameseSigns[i].Length; j++)
                {
                    str = str.Replace(VietnameseSigns[i][j], VietnameseSigns[0][i - 1]);
                }
            }

            return str;
        }

        public static string GetUrl(string routeName, RouteValueDictionary dict)
        {
            var vpd = RouteTable.Routes.GetVirtualPath(null, routeName, dict);
            return vpd != null ? vpd.VirtualPath : String.Empty;
        }

        public static List<T> ConvertToList<T>(this IEnumerable<object[]> records) where T : class, new()
        {
            try
            {
                var list = new List<T>();

                foreach (var item in records)
                {
                    var obj = Activator.CreateInstance<T>();
                    var properties = obj.GetType().GetProperties();
                    for (var i = 0; i < properties.Length; i++)
                    {
                        try
                        {
                            var propertyInfo = obj.GetType().GetProperty(properties[i].Name);
                            var t = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;
                            var safeValue = (item[i] == null) ? null : Convert.ChangeType(item[i], t);
                            propertyInfo.SetValue(obj, safeValue, null);
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    list.Add(obj);
                }

                return list;
            }
            catch
            {
                return null;
            }
        }
    }
}