using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace smartTechAuthenticator.Models
{
    [Table("QrCodeMaster")]
    public class QrCodeMaster
    {
        [Key]
        public int Id { get; set; }    
        [Required]
        [StringLength(50)]
        public string QrCode { get; set; }
        public string QrImageUrl { get; set; }
        public bool IsActive { get; set; }
        public Guid ProductId { get; set; }
        public int CategoryId { get; set; }
        public bool IsExpire { get; set; }
        public string CustomerId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}