using System;

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