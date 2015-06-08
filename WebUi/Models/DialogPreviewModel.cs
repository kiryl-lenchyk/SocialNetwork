using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUi.Models
{
    public class DialogPreviewModel
    {
        public int UserId { get; set; }

        public String UserName { get; set; }

        public String UserSurname { get; set; }

        public String LastMessage { get; set; }

        public bool IsReaded { get; set; }

    }
}