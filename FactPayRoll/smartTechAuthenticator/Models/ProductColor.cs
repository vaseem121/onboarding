using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace smartTechAuthenticator.Models
{
    public class ProductColor
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string Color { get; set; }
        public DateTime DateCreated { get; set; }
        public Guid CreatedBy { get; set; }
    }
}