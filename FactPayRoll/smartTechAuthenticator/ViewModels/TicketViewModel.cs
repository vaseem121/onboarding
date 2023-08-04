using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace smartTechAuthenticator.ViewModels
{ 
    public class TicketViewModel
    {
      //  public Guid Id { get; set; }

        public string Photo { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Message { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string CustomerId { get; set; }
        public string UpdatedBy { get; set; }
        public string Id { get; set; }
        public string Answer { get; set; }
        [Display(Name ="Label Status")]
        public int LabelStatus { get; set; }
        public bool NotificationCust { get; set; }
        public bool NotificationAdmin { get; set; }
        public int Notification{ get; set; }
        public string date{ get; set; }
        public int Rating { get; set; }
        public List<TicketMessageSystemViewModels> ViewTicketMessageList { get; set; }
        public List<TicketViewModel> TicketViewmodelList { get; set; }

        public List<Statuss> StatusList = new List<Statuss>()
        {
            new Statuss{Id="UnSolved",Status="UnSolved"},
            new Statuss{Id="Solved",Status="Solved"},
        };
        public string Description { get; set; }
        public Guid TicketId { get; set; }
        public Guid? TicketId1 { get; set; }

        public List<LabelStatus> LabelStatuses = new List<LabelStatus>()
        {
            new LabelStatus{StatusId=1,Status="Common "},
            new LabelStatus{StatusId=2,Status="Normal "},
            new LabelStatus{StatusId=3,Status="Urgent  "},
            new LabelStatus{StatusId=4,Status="Critical "},
            new LabelStatus{StatusId=5,Status="Extend "},
            new LabelStatus{StatusId=6,Status="Complete  "},
        };
    }
   
    public class Statuss
    {
        public string Id { get; set; }
        public string Status { get; set; }
    }
    public class LabelStatus
    {
        public int StatusId { get; set; }
        public string  Status { get; set; }
    }

}