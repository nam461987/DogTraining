using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Admin.Models.dog
{
    public class Dog_Config
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
        public int? Status { get; set; }
    }
}