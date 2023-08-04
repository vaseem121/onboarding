using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace smartTechAuthenticator.Models
{
    public class ShippingTracking
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }
        [Required]
        public Guid UserId { get; set; }
        public Guid OrderId { get; set; }
        public string Status { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime ShippedDate { get; set; }
        public DateTime OutForDeliveryDate { get; set; }
        public DateTime Delivered { get; set; }
        public string TrackingNumber { get; set; }
        public string CourierCompnyName { get; set; } 

    }
}