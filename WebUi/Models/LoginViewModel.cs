using System.ComponentModel.DataAnnotations;

namespace WebUi.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "User name is required")]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}