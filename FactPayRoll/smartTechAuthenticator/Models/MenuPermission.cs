using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace smartTechAuthenticator.Models
{
    public class MenuPermission
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }
        public Guid AdminId { get; set; }
        public Guid SubAdminId { get; set; }
        public bool MallManage { get; set; }
        public bool CustomerManage { get; set; }
        public bool TicketSupportManage { get; set; }
        public bool Setting { get; set; }
        public bool AuthenticatorManage { get; set; }
        public bool FrontEndManage { get; set; } 
        public bool CertificateManage { get; set; }
        public bool NewsManage { get; set; }
        public bool FormManage { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public Guid UpdatedBy { get; set; }
    }
}