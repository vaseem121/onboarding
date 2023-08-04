using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace smartTechAuthenticator.ViewModels
{
    public class ProductImagesViewModel
    {
        public Guid Id { get; set; }
        [Required]
        public Guid ProductId { get; set; }
        public string Photo { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedDateBy { get; set; }
    }
}