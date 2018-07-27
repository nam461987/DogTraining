using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Admin.Models.admin
{
    public class Option
    {
        public string DisplayText { get; set; }
        public int Value { get; set; }
    }
    public class Option2
    {
        public string DisplayText { get; set; }
        public string Value { get; set; }
    }
    public class Group_Config
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}