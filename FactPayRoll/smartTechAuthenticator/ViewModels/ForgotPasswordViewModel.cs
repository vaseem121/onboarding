using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace smartTechAuthenticator.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [DataType(DataType.Text)]
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email is required")]

        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$",
        ErrorMessage = "Email is not valid")]

        public string Email { get; set; }
        public string First { get; set; }
        public string Second { get; set; }
        public string Third { get; set; }
        public string Fourth { get; set; }
        public string Fifth { get; set; }
        public string Sixth { get; set; }
        public string Code { get; set; }
    }
    public class PasswordCreateModel
    {
        public string Email { get; set; }
        [Required]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        [Required]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Confirm password doesn't match, Type again !")]
        public string Confirmpwd { get; set; }
    }
}