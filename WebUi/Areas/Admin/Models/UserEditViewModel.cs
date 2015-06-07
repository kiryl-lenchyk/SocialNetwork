using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebUi.Models;

namespace WebUi.Areas.Admin.Models
{
    public class UserEditViewModel
    {
        public int Id { get; set; }

         [Display(Name = "User Name")]
        public String UserName { get; set; }

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

        public List<Role> AllRoles { get; set; }
        public List<int> UserRolesIds { get; set; }
        public int[] SelectedRoles { get; set; } 
    }
}