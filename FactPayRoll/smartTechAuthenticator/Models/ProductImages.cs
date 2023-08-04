using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace smartTechAuthenticator.Models
{
    public class ProductImages
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }
        [Required]
        public Guid ProductId { get; set; }
        public string Photo { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedDateBy { get; set; }
    }
}