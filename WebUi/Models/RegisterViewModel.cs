using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace WebUi.Models
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "User name*")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password*")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password*")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Birth day")]
        public DateTime? BirthDay { get; set; }

        [Required]
        [Display(Name = "Name*")]
        public String Name { get; set; }

        [Required]
        [Display(Name = "Surname*")]
        public String Surname { get; set; }

        [Display(Name = "Sex")]
        public Sex? Sex { get; set; }
        
        [Display(Name = "About you")]
        public String AboutUser { get; set; }
    }
}