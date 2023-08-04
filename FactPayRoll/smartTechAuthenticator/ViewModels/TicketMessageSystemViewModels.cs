using smartTechAuthenticator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace smartTechAuthenticator.ViewModels
{
    public class TicketMessageSystemViewModels
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public Guid TicketId { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid CustomerId { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string CustomerName { get; set; }
        public bool NotificationCust { get; set; }
        public bool NotificationAd { get; set; }
        public List<TicketMessageSystem> viewTicketMessageList { get; set; }
    }
}