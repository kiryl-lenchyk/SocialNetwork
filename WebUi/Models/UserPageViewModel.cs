using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUi.Models
{
    public class UserPageViewModel
    {
        public int Id { get; set; }

        public String UserName { get; set; }

        public String Name { get; set; }

        public String Surname { get; set; }

        public DateTime? BirthDay { get; set; }

        public Sex? Sex { get; set; }

        public String AboutUser { get; set; }

        public bool CanWriteMessage { get; set; }

        public bool CanAddToFriends { get; set; }

        public IEnumerable<UserPreviewViewModel> Friends { get; set; }

    }
}