using smartTechAuthenticator.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace smartTechAuthenticator.Models
{
    public partial class CustomerInfo
    {
        public Guid Id { get; set; }
        [Required]
        [Display(Name="Full Name")] 
        public string Name { get; set; }
        public string Nric { get; set; }
        public string MobileNo { get; set; }
        public string DeviceId { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public DateTime DateCreated { get; set; }
        public int CompanyId { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        [Required]
        [Display(Name="Password")]
        public string UserPass { get; set; }
        public int CustomerAddressId { get; set; } 
        public UserType UserType { get; set; }
        public bool IsActive { get; set; }
        public string ForgotPasswordOTP { get; set; }
        public DateTime FPCreateTime { get; set; }
        public DateTime FPExpTime { get; set; }
        public string Photo { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string AddedBy { get; set; }
        public string Website { get; set; }
        public string Location { get; set; }
        public string FormIds { get; set; }

    }
}
