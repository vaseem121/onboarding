using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace smartTechAuthenticator.ViewModels
{
    public class ProductViewModel
    { 
      
        [Required(ErrorMessage = "Product Name required")]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }
        public string CategoryName { get; set; }
        public int QrId { get; set; }   
        public Guid ProductId { get; set; }
        [Required(ErrorMessage = "Category required")]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        public bool IsActive { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<ProductViewModel> ProductList { get; set; }
        [Display(Name = "Authentic Code")]
        public string Authenticode { get; set; }
        [Display(Name = "Location")]
        public string Location { get; set; }
        [Display(Name = "Batch No")]
        public string BachNumber { get; set; }
        [Display(Name = "INV NO")]
        public string INVNo { get; set; }
        public Guid Id { get; set; }
        public string Price { get; set; }
        public string  Photo { get; set; }
        public string Shipping { get; set; }
        public string Tax { get; set; }
        public string TotalPrice { get; set; }
        public string Stock { get; set; }
        public string FormIds { get; set; }
        public List<ProList> ListPro{ get; set; }
        public List<formDetailsList> FormResPro { get; set; }
        public List<ProList> ListPro1 { get; set; }

    }
    public class ProList
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool check { get; set; }
        public string FormIds { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
    }

    public class ProductNewViewModel
    {

        [Required(ErrorMessage = "ProductName required")]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }
        public int QrId { get; set; }
        public Guid ProductId { get; set; }
        [Required(ErrorMessage = "Category required")]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        public bool IsActive { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<ProductViewModel> ProductList { get; set; }
        [Display(Name = "Authentic Code")]
        public string Authenticode { get; set; }
        [Display(Name = "Location")]
        public string Location { get; set; }
        [Display(Name = "Batch No")]
        public string BachNumber { get; set; }
        [Display(Name = "INV NO")]
        public string INVNo { get; set; }
        public string Id { get; set; }
        public string Price { get; set; }
        public string Photo { get; set; }
        public string Tax { get; set; }
        public string TotalPrice { get; set; }
        public string CategoryName { get; set; }
        public string Stock { get; set; }
        public string Discount { get; set; }
        [Display(Name = "Product Type")]
        public int ProductType { get; set; }
        public int Qty { get; set; }
        [Display(Name = "Product Color")]
        public string[] Color { get; set; }
        [Display(Name = "Product Size")]
        public string[] Size { get; set; }
        [Display(Name = "Product Tag")]
        public string[] ProductTag { get; set; }
        public List<ProType> producttype = new List<ProType>()
        {
            new ProType{Id=1,Name="Single Type"},
            new ProType{Id=2,Name="Variable Type"}
        };
        public List<ProductNewViewModel> productNewViewList { get; set; }
        public List<ProductSizeView> productSizeList { get; set; }
        public List<ProductColorView> productColorList { get; set; }
        public List<ProductTagView> ProductTagViewList { get; set; }
        public ProductSizeView productsize { get; set; }
        public ProductColorView productColor { get; set; }
        public ProductTagView ProductTagView { get; set; }
    }
    public class ProType
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class ProductComonModel
    {
        public Guid Id { get; set; }
    }

    public class ProductSizeView
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string Size { get; set; }
        public DateTime DateCreated { get; set; }
        public Guid CreatedBy { get; set; }
    }
    public class ProductColorView
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string Color { get; set; }
        public DateTime DateCreated { get; set; }
        public Guid CreatedBy { get; set; }
    }
    public class ProductTagView
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string Tag { get; set; }
        public DateTime createdDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public Guid CreatedBy { get; set; }
    }

    public class formDetailsList
    {
        public Guid ResponseId { get; set; }
        public string Name { get; set; }
        public string CustomerName { get; set; }
        public Guid CustomerId { get; set; }
        public Guid FormIds { get; set; }
        public DateTime DateCreated { get; set; }
    }
}