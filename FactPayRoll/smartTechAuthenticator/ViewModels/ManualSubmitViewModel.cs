using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace smartTechAuthenticator.ViewModels
{
    public class ManualSubmitViewModel
    {
        [Required(ErrorMessage ="Code field is required")]
        [Display(Name ="Code")]
        [MinLength(16, ErrorMessage = " Minimum 16 digits allowed"),MaxLength(17,ErrorMessage ="Maximum 17 digits allowed")]
        public string CodeText { get; set; }
    }
}