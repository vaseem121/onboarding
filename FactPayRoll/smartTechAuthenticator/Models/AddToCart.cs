using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace smartTechAuthenticator.Models
{
    public class AddToCart
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }
        public string Product { get; set; }
        public int Status { get; set; }
        public Guid? CustId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string TotalPrice { get; set; }
        public int Qty { get; set; }
        public Guid ProductId { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }
    }
}