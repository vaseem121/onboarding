using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace smartTechAuthenticator.ViewModels
{
    public class BannerCarouselViewModel
    {
       // public Guid Id { get; set; }
        public string Photo { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Id { get; set; }
        public List<BannerCarouselViewModel> BannerCarouselList { get; set; }
        public List<NewsViewModel> NewsList { get; set; }
        public List<ProductViewModel> ProductList { get; set; }
        public List<CustomerInfoViewModel> CompanyList { get; set; }
    }
  
}