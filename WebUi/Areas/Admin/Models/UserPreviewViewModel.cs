using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUi.Areas.Admin.Models
{
    public class UserPreviewViewModel
    {
        public int Id { get; set; }

        public String UserName { get; set; }

        public String Name { get; set; }

        public String Surname { get; set; }
    }
}