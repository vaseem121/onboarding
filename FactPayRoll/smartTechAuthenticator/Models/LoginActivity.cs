using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace smartTechAuthenticator.Models
{
    public class LoginActivity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }
        public string UserName { get; set; }

        public Guid CustomerUserId { get; set; }
        public string IP_Address { get; set; }
        public Nullable<int> EventType { get; set; }
        public Nullable<int> EventReason { get; set; }
        public Nullable<System.DateTime> EventDateTime { get; set; }
        public string Location_Latitude { get; set; }
        public string Location_Longitude { get; set; }
    }
}