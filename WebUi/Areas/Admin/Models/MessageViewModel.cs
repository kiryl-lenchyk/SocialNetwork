using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUi.Areas.Admin.Models
{
    public class MessageViewModel
    {
        public int Id { get; set; }

        public UserPreviewViewModel Sender { get; set; }

        public UserPreviewViewModel Target { get; set; }

        public DateTime CreatingTime { get; set; }

        public String Text { get; set; }

    }
}