using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using System.ComponentModel.DataAnnotations.Schema;

namespace smartTechAuthenticator.Models
{
    [Table("DistricttMaster")]
    public class DistricttMaster
    {
        public int Id { get; set; }
        [Required]
        public string DistricttName { get; set; }
        [Display(Name ="State")]
        public int StateId { get; set; }
        [ForeignKey("StateId")]
        public StateMater StateMater { get; set; }
    }
}