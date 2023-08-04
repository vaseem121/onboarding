using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace smartTechAuthenticator.ViewModels
{ 
    public class SignInViewModel
    {
        public string Id { get; set; }
        [Required]
        [Display(Name = "User Name(Email Address)")]
        public string UserName { get; set; }
        [Required]
        [Display(Name ="Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
    public class Login
    {
        [Required]
        [RegularExpression(@"^([0-9a-zA-Z]([\+\-_\.][0-9a-zA-Z]+)*)+@(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]*\.)+[a-zA-Z0-9]{2,3})$", ErrorMessage = "Your email address is not in a valid format")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please enter password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string UserId { get; set; }
    }

    public class PasswordChangeModel
    {
        public string Id { get; set; }        
        [Required]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [Display(Name = "New Password")]
        [DataType(DataType.Password)]
        public string newPassword { get; set; }
        [Required]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("newPassword", ErrorMessage = "Confirm password doesn't match, Type again !")]
        public string Confirmpwd { get; set; }
    }
    public class ForgotPassword
    {
        public string Email { get; set; }
        public string id { get; set; }
        public string code { get; set; }
        public string Password { get; set; }
        public string OTP { get; set; }
        public DateTime OTPExpTime { get; set; }
    }
}