using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace smartTechAuthenticator.ViewModels
{
    public class MenuPermissionViewModel
    {
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
        public List<Menu> MenuList1 { get; set; }
        public List<Menu> MenuList = new List<Menu>()
        {
            new Menu{Id=1, Menuname="Mall Manage"},
            new Menu{Id=2, Menuname="Customer Manage"},
            new Menu{Id=3, Menuname="Ticket Support Manage"},
            new Menu{Id=4, Menuname="Setting"},
            new Menu{Id=5, Menuname="Authenticator Manage"},
            new Menu{Id=6, Menuname="Front End Manage"},
            new Menu{Id=7, Menuname="Certificate Manage"},
            new Menu{Id=8, Menuname="News Manage"},
            new Menu{Id=9, Menuname="Form Manage"},
        };
    }
    public class Menu
    {
        public int Id { get; set; }
        public string Menuname { get; set; }
        public bool check { get; set; }
    }

}