using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Admin.Models.service
{
    public class News_Config
    {
        public int Id { get; set; }
        public int TypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime PostedDate { get; set; }
        public string Tags { get; set; }
        public string Avatar { get; set; }
        public int? Status { get; set; }
    }
}