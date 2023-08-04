using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace smartTechAuthenticator.Models
{
    public partial class TrackingForms
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string MobileNo { get; set; }
        public string LotNumber { get; set; }
        public string Time { get; set; }
        public string Date { get; set; }
        public string Place { get; set; }
        public Guid TestkitId { get; set; }
        public string TestResults { get; set; }
        public string AntigenType { get; set; }
        public Guid CustId { get; set; }
        public string FileUrl { get; set; }
        public string CertificateImage { get; set; }
        public string QrCode { get; set; }
        public int IsVerified { get; set; }
        public string VerifiedBy { get; set; }
        
    }
}
