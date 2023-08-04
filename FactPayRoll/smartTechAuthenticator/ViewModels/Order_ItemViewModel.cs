using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace smartTechAuthenticator.ViewModels
{
    public class Order_ItemViewModel
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public decimal TotalAmount { get; set; }
        public string ProductId { get; set; }
        public string PaymentId { get; set; }
        public int Qty { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ShippingTrackingId { get; set; }
        public string Order_Id { get; set; }
    }

    public class OrderHistory
    {
        public string Id { get; set; }
        public string Order_Id { get; set; }
        public string CustId { get; set; }
        public string UserName { get; set; }
        public decimal TotalAmount { get; set; }
        public string Paymentstatus { get; set; }
        public string TrackingId { get; set; }
        public string Shippingstatus { get; set; }
        public DateTime ShippedDate { get; set; }
        public DateTime OutForDeliveryDate { get; set; }
        public DateTime Delivered { get; set; }
        public DateTime OrderDate { get; set; }
        public List<OrderDetails> OrderDetails { get; set; }
        public string date { get; set; }
        public string CourierCompany { get; set; }
        public string TrackingNumber { get; set; }
        public List<OrderHistory> OrderHistoryList { get; set; }
        public CustomerInfoViewModel CustomerDetail { get; set; }

        public List<Tarck> TrackList = new List<Tarck>
        {
            new Tarck{TarckingId="1",TarckStatus="Orderd"},
            new Tarck{TarckingId="2",TarckStatus="Shipped"},
            new Tarck{TarckingId="3",TarckStatus="Out for Delivery"},
            new Tarck{TarckingId="4",TarckStatus="Delivered"},
        };
        public string Qty { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class OrderDetails
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string Qty { get; set; }
        public string Photo { get; set; }
        public string Price { get; set; }
       
    }

    public class Tarck
    {
        public string TarckingId { get; set; }
        public string TarckStatus { get; set; }
    }

    public class PaymentData
    {
        public decimal TotalAmount { get; set; }
        public decimal Qty { get; set; }
        public string ProductId { get; set; }
        public string Order_Id { get; set; }
        public string payment_method_types { get; set; }
        public string payment_status { get; set; }
        public string UserId { get; set; }
        public string Bill_Id { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }

    }


}