using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace smartTechAuthenticator.ViewModels
{
    public class ShippingViewModel
    {
        //public Guid Id { get; set; }
        [Required]
        [Display(Name ="Shipping Method")]
        public string ShippingMethod { get; set; }
        [Display(Name = "Shipping Charge")]
        public decimal ShippingCharge { get; set; }
        [Display(Name = "State ")]
        public string StateId { get; set; }
        [Display(Name = "District")]
        public string DistricttId { get; set; }
        public DateTime Createddate { get; set; }
        public DateTime Shippingdate { get; set; }
        public string CreatedBy { get; set; }
        public string Id { get; set; }
        public string State_Id { get; set; }
        public string Districtt_Id { get; set; }
        public string State { get; set; }
        public string District { get; set; }
        public string Final_Dis { get; set; }
        public string Final_State { get; set; }
        public List<ShippingViewModel> ShippingList { get; set; }
        public List<ShippingViewModel> ShippingList1 { get; set; }
    }

    public class ShippingList
    {
        public string Id { get; set; }
        public string State { get; set; }
        public string District { get; set; }
        public string ShippingMethod { get; set; }
        public string ShippingCharge { get; set; }
        public string State_Id { get; set; }
        public string Districtt_Id { get; set; }
        public string Final_Dis { get; set; }
        public string Final_State { get; set; }
    }



}