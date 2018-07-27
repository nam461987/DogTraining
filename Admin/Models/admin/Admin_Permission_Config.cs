using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Admin.Models.admin
{
    public class Admin_Permission_Config
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}