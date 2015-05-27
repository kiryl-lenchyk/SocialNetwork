using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebUi.Models
{
    public class UserFindModel
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