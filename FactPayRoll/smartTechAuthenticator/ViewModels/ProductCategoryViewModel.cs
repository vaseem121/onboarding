using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace smartTechAuthenticator.ViewModels
{
    public class ProductCategoryViewModel
    {

        [Required(ErrorMessage = "Category Name required")]
        [Display(Name = "Category Name")]
        public string CategoryName { get; set; }
        public string CategoryId { get; set; }
    }
}