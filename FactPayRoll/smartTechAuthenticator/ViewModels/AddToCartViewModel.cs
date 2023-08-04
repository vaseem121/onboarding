using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace smartTechAuthenticator.ViewModels
{
    public class AddToCartViewModel
    {
        public Guid Id { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public Guid? CustId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string TotalPrice { get; set; }
        public string Price { get; set; }
        public int Qty { get; set; }
        public string GrandTotalPrice { get; set; }
        public string Photo { get; set; }
        public Guid ProductId { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }
    }
}