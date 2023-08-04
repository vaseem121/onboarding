using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace smartTechAuthenticator.ViewModels
{
    public class TrackingTestTypeViewModel
    {
        [Required]
        public int TrackingId { get; set; }
        [Required (ErrorMessage ="Please select test result")]
        [Display(Name ="What is your result")]
        public string TestResults { get; set; }
        [Required(ErrorMessage ="Please select anitigen type")]
        [Display(Name ="What type test was done?")]
        public string AntigenType { get; set; } 
        [Display(Name = "Take Result Photo")]
        public HttpPostedFileBase FileUrl { get; set; }
    }
}