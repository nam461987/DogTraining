using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Admin.Models.admin
{
    public class Admin_Account_Config
    {
        public int Id { get; set; }
        public int TypeId { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public int Active { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}