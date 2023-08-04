using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace smartTechAuthenticator.ViewModels
{
    public class ProductGalleryViewModel
    {
        public Guid Id { get; set; }
        public string ProductId { get; set; }
        [Required]
        [Display(Name = "Select Photo")]
        public string Photo { get; set; }
        public string ProductName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public List<ProductGalleryViewModel> ProductGalleryList { get; set; }   
    }
}