using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace smartTechAuthenticator.Models
{
    public class Dropdown
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }
        public Guid FormPropertyId { get; set; }
        public string Name { get; set; }
        public int RowNumber { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}