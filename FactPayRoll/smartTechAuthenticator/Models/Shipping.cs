using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace smartTechAuthenticator.Models
{
    public class Shipping
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string ShippingMethod { get; set; }
        public decimal ShippingCharge { get; set; }
        public string StateId { get; set; }
        public string DistricttId { get; set; }
        public DateTime Createddate { get; set; }
        public DateTime Shippingdate { get; set; }
        public string CreatedBy { get; set; }
    }
}