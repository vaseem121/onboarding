using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace smartTechAuthenticator.Models
{
    public partial class CompanyInfo
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public DateTime CreatedTs { get; set; }
        public string Status { get; set; }
        public string LoginId { get; set; }
        public string LoginPass { get; set; }
    }
}
