using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace smartTechAuthenticator.Models
{
    public class Order
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }
        [Required]
        public Guid UserId { get; set; }
        public string Order_Id { get; set; }
        public decimal TotalAmount { get; set; }
        //public Guid ProductId { get; set; }
        public Guid PaymentId { get; set; }
        //public int Qty { get; set; }
        public DateTime CreatedDate { get; set; }   
        public string CreatedBy { get; set; }
        public Guid ShippingTrackingId { get; set; }
        public Guid ShippingId { get; set; }
    }
}