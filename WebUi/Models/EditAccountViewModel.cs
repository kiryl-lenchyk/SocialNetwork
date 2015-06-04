using System;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace WebUi.Models
{
    public class EdirAccountViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Birth day")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? BirthDay { get; set; }

        [Required]
        [Display(Name = "Name")]
        public String Name { get; set; }

        [Required]
        [Display(Name = "Surname")]
        public String Surname { get; set; }

        [Display(Name = "Sex")]
        public Sex? Sex { get; set; }
        
        [Display(Name = "About you")]
        public String AboutUser { get; set; }

    }
}