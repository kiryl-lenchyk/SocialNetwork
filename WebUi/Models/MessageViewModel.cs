using System;

namespace WebUi.Models
{
    public class MessageViewModel
    {
        public int UserId { get; set; }

        public String UserName { get; set; }

        public String UserSurname { get; set; }

        public String Text { get; set; }

        public DateTime CreaingTime { get; set; }

        public bool IsSended { get; set; }

        public bool IsReaded { get; set; }
    }
}