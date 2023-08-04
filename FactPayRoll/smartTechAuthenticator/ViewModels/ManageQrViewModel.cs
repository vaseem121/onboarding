using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace smartTechAuthenticator.ViewModels
{
    public class ManageQrViewModel
    {
        public int Id { get; set; }
        public string QrCode { get; set; }
        public string QrImageUrl { get; set; }
        public bool IsActive { get; set; }
        public Guid ProductId { get; set; }
        [Required(ErrorMessage = "Category required")]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        public List<ManageQrViewModel> ManageQrList { get; set; }
       public List<ProductViewModel> ProductList { get; set; }    
    }

    public class NewManageQrViewModel
    {
        public int Id { get; set; }
        public string QrCode { get; set; }
        public string QrImageUrl { get; set; }
        public bool IsActive { get; set; }
        [Required(ErrorMessage = "Product required")]
        [Display(Name = "Product")]
        public Guid ProductId { get; set; }
        [Required(ErrorMessage = "Category required")]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        public List<ManageQrViewModel> ManageQrList { get; set; }
        public List<ProductViewModel> ProductList { get; set; }
    }
}