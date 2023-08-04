using smartTechAuthenticator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace smartTechAuthenticator.ViewModels
{
    public class MallViewModel
    {
        public List<ProductNewViewModel> ProductList { get; set;}
        public ProductNewViewModel ProductDetail { get; set;}
        public List<ProductCategoryViewModel> ProductCategoryList { get; set;}
        public List<AddToCartViewModel> AddToCartList { get; set;}
        public decimal TotalCartPrice { get; set; }
        public int TotalQty { get; set; }
        public List<ProductGalleryViewModel> ProductGallery { get; set; }
        public List<ProductSize> productSizeList { get; set; }
        public List<ProductColor> productColorList { get; set; }
        public ProductSizeView Size { get; set; }
        public ProductColorView Color { get; set; }
    }
}