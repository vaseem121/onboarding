using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace smartTechAuthenticator.Models
{
    public class Payment
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }
        [Required]
        public Guid UserId { get; set; }
        public string OrderId { get; set; }
        public string Status { get; set; }
        public decimal Amount { get; set; }
        public string PaymentType { get; set; }
        public string Bill_Id { get; set; }
       // public string TransactionDate { get; set; }
        //public string PaymentDetails { get; set; }
        //public string payment_intent { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
    }
}