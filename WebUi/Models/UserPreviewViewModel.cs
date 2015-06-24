using System;

namespace WebUi.Models
{
    public class UserPreviewViewModel
    {
        public int Id { get; set; }

        public String Name { get; set; }

        public String Surname { get; set; }

        public DateTime? BirthDay { get; set; }

        public Sex? Sex { get; set; }

    }
}