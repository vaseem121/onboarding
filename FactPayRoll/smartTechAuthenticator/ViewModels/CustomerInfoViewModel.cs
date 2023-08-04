using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace smartTechAuthenticator.ViewModels
{
    public class CustomerInfoViewModel
{
        public Guid Id { get; set; }
       
        [Display(Name = "Full name")]
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Nric { get; set; }
        [Required(ErrorMessage = "Mobile number required")]
        [Display(Name="Mobile number")] 
        [DataType(DataType.PhoneNumber)]
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
        [Required]
        [DataType(DataType.Password)] 
        [Display(Name = "Password")]
        public string UserPass { get; set; }
        public int CustomerAddressId { get; set; }
         [Display(Name="State")]
        
        public int StateId { get; set; }
     
        [Display(Name= "Districtt")]
        public int DistricttId { get; set; }
        public string PostCode { get; set; }
        [Display(Name="Address")]
        public string Address { get; set; }
        public string State { get; set; }
        public string District { get; set; }
        public bool IsActive { get; set; }
        public string Photo { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int UserType { get; set; }
        public string AddedBy { get; set; }
        public string Website { get; set; }
        public string Location { get; set; }
        public List<CustomerInfoViewModel> CompanyList { get; set; }
        public List<ProList> ListPro { get; set; }
        public List<formDetailsList> FormResPro { get; set; }
        public List<ProList> ListPro1 { get; set; }
        public string FormIds { get; set; }
        public int TotalEmployees { get; set; }
        public int TotalAccountant { get; set; }
        public  int TotalFormSubmit { get; set; }
    }
   
}