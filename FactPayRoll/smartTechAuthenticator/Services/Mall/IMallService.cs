using smartTechAuthenticator.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace smartTechAuthenticator.Services.Mall
{
   public interface IMallService
    {
        List<ProductCategoryViewModel> GetProductCategoryList();    
        List<ProductNewViewModel> GetProductByCategoryIdList(int CategoryId);
        List<ProductNewViewModel> GetProductList();
        List<ProductNewViewModel> GetSerchProductList(string ProuctName);
        ProductNewViewModel GetProductDetail(Guid? Id);
        List<AddToCartViewModel> GetAddToCartList(Guid? CustId);
        Task<ResponseModel> DeleteCartDetails(Guid Id);
        Task<ResponseModel> AddinCart(AddToCartViewModel cart);
        Task<ResponseModel> managecart(AddToCartViewModel add);
        ProductNewViewModel GetProductDetailCheckOut(Guid? Id,int Qty);
        CustomerInfoViewModel GetCustomerDetail(Guid? Id);
        List<SelectListItem> GetStates();
        List<SelectListItem> GetDistrictts(int StateId);
        Task<ResponseModel> UpdateAddress(CheckOutViewModel customer);
        ProductNewViewModel GetProductDetailCartCheckOut(Guid? Userid);
        ShippingViewModel GetShippingfee(Guid? Id);
        List<SelectListItem> GetDistrictts(int StateId, int districtId);
        Task<ResponseModel> SaveOrderPayment(PaymentData data);
        Order_ItemViewModel GetOrderDetails(Guid? OrderId, Guid? UserId);
        Task<ResponseModel> SaveCartOrderPayment(PaymentData data, Guid? UserId);
        (int TotalCount, int FilteredCount, dynamic Categorys) GetOrders(int skip, int take, string sortColumn, string sortColumnDir, string searchValue, string UserId);
        Task<OrderHistory> OrderHistoryDetails(string Id);
        List<OrderHistory> OrderHistoryList(string UserId);
        List<OrderHistory> OrderHistorySearchList(string UserId, string UserName);
        List<ProductGalleryViewModel> GetGallery(string ProductId);

    }
}
