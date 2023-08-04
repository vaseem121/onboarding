using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using System.ComponentModel.DataAnnotations.Schema;

namespace smartTechAuthenticator.Models
{
    [Table("StateMater")]
    public class StateMater
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name ="State Name")]
        public string StateName { get; set; }
    }
}