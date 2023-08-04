using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace smartTechAuthenticator.ViewModels
{
    public class ViewTestHistoryModel
    {
        public Guid TestkitId { get; set; }
        public string TestResults { get; set; }
        public string AntigenType { get; set; }
        public Guid CustId { get; set; }
        public string FileUrl { get; set; }
        public string Time { get; set; }
        [Required(ErrorMessage = "Date required")]
        public string Date { get; set; }

        public List<ViewTestHistoryModel> ViewDataList { get; set; }
    }
}