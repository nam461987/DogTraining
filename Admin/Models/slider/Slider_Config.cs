using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Admin.Models.slider
{
    public class Slider_Config
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ButtonName { get; set; }
        public string ButtonLink { get; set; }
        public int? Status { get; set; }
    }
}