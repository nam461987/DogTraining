using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Admin.Models.admin
{
    public class Admin_Group_Permission_Config
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public string PermissionId { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
    }
}