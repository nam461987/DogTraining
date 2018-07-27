using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Admin.Models
{
    public class AjaxRequestData
    {
        public class DataModel
        {
            public Dictionary<string, object> _c { get; set; }
            public Dictionary<string, object> _d { get; set; }
            public string _f { get; set; }
            public Dictionary<string, object> _od { get; set; }
            public int _os { get; set; }
            public int _lm { get; set; }
        }
    }
}