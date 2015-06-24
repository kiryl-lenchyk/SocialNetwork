using System;
using System.ComponentModel.DataAnnotations;


namespace WebUi.Models
{
    public class UserFinViewModel
    {
        public String Name { get; set; }

        public String Surname { get; set; }

        [Display(Name = "Birth Day Min")]
        public DateTime? BirthDayMin { get; set; }

        [Display(Name = "Birth Day Max")]
        public DateTime? BirthDayMax { get; set; }

        public Sex? Sex { get; set; }

    }
}