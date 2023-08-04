using smartTechAuthenticator.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace smartTechAuthenticator.ViewModels
{
    public class TrackingFormsViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage ="Mobile number required")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name="Mobile Number")]
        public string MobileNo { get; set; }
        [Required(ErrorMessage ="Lot number required")]
        public string LotNumber { get; set; }
        [Required(ErrorMessage ="Time required")]
        public string Time { get; set; }
        [Required(ErrorMessage = "Date required")]
        public string Date { get; set; }
        [Required(ErrorMessage ="Place required")]
        public string Place { get; set; }
        public Guid TestkitId { get; set; }
        public string TestResults { get; set; }
        public string AntigenType { get; set; }
        public Guid CustId { get; set; }

        #region Customer information start
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerMobileNo { get; set; }
        public string Nric { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string  StateName { get; set; }
        public string DistricttName { get; set; }
        #endregion Customer informaton end

        #region Product information start 
        public string ProductName { get; set; }
        public string AuthentiCode { get; set; }
        public string BachNumber { get; set; }
        public string INVNo { get; set; }
        public int QrId { get; set; } 
        public int CategoryId { get; set; }  
        public string Description { get; set; }
        public string CategoryName { get; set; }
        #endregion Product Information end
        public string FileUrl { get; set; }
        public string QrCode { get; set; } 
        public int IsVerified { get; set; }
        public string VerifiedBy { get; set; }
     // public string No { get; set; }
        public string ProductId { get; set; }
     // public string date { get; set; }
        public string CertificateImage { get; set; }
        public List<TrackingFormsViewModel> ViewTrackingFormsList { get; set; }
    }
}