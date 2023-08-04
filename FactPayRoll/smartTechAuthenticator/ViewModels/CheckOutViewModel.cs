using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace smartTechAuthenticator.ViewModels
{
    public class CheckOutViewModel
    {
        public ProductNewViewModel ProductDetail { get; set; }
        public CustomerInfoViewModel CustomerDetail { get; set; }

        public Order_ItemViewModel OrderDetail { get; set; }
        public int StateId { get; set; }

        public Guid Id { get; set; }
        public int DistricttId { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string PostCode { get; set; }
        public string Message { get; set; }
        public string Shippingfees { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }


    }
}