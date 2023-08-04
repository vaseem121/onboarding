using smartTechAuthenticator.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace smartTechAuthenticator.Models
{
    public class News
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string NewsTitle { get; set; }
        public string Description { get; set; }
        public DateTime Createddate { get; set; }
        public DateTime Updateddate { get; set; }
        public string CreatedBy { get; set; }
        public string Photo { get; set; }

      
    }
}