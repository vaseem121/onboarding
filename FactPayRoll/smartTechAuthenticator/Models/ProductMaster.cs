using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Web;

namespace smartTechAuthenticator.Models
{
    [Table("ProductMaster")]
    public class ProductMaster
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key] 
        public Guid Id { get; set; }
        [Required]
        public string ProductName { get; set; }
        public int QrId { get; set; }  
       // public   QrCodeMaster QrCodeMaster { get; set; }
        [ForeignKey("ProductCategory")]
        public int CategoryId { get; set; }
        public ProductCategory ProductCategory { get; set; }
        public bool IsActive { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; } 
        public string Authenticode { get; set; } 
        public string Location { get; set; } 
        public string BachNumber { get; set; } 
        public string INVNo { get; set; } 
        public string Price { get; set; } 
        public string Photo { get; set; } 
        public string Shipping { get; set; } 
        public string Tax { get; set; } 
        public string TotalPrice { get; set; }
        public string Discount { get; set; }
        public string Stock { get; set; }
        public int ProductType { get; set; }
        public string FormIds { get; set; }

    }
}