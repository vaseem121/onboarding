using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace smartTechAuthenticator.ViewModels
{
    public class CompanyDetailsViewModel
    {
        public Guid CompanyId { get; set; }
        public string UserId { get; set; }
        [Required]
        public string CompanyName { get; set; }
        public string Location { get; set; }
        public string Website { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string FormIds { get; set; }
        public bool check { get; set; }
        public List<CompanyDetailsViewModel> CompanyList { get; set; }
        public List<ProList> ListPro { get; set; }
        public List<formDetailsList> FormResPro { get; set; }
        public List<ProList> ListPro1 { get; set; }
    }
    //public class ProList1
    //{
    //    public Guid Id { get; set; }
    //    public string Name { get; set; }
    //    public bool check { get; set; }
    //    public string FormIds { get; set; }
    //    public bool IsActive { get; set; }
    //    public DateTime DateCreated { get; set; }
    //}
}