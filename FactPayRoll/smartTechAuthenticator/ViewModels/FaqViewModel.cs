using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace smartTechAuthenticator.ViewModels
{
    public class FaqViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public List<FaqViewModel> FAQList { get; set; }
    }

    public class HelpGuidViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public List<HelpGuidViewModel> HelpList { get; set; }
    }
    public class TermPrivacyViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public List<TermPrivacyViewModel> TermPrivacyList { get; set; }
    }
    public class AboutViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public List<AboutViewModel> aboutList { get; set; }
    }
}