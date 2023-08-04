using Newtonsoft.Json;
using smartTechAuthenticator.Services.Comman;
using smartTechAuthenticator.Services.Comman.CustomFilters;
using smartTechAuthenticator.Services.Mall;
using smartTechAuthenticator.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using smartTechAuthenticator.Models;
using System.Security.Cryptography;
using Stripe.Checkout;
using System.Web.Http.Results;
using Stripe;
using Stripe.Infrastructure;

namespace smartTechAuthenticator.Controllers
{
    [Authorized]
    public class MallController : Controller
    {
        private readonly IMallService mall;
        private readonly ApplicationDbContext context;
        static string Api_Key = System.Configuration.ConfigurationManager.AppSettings["Api_Key"].ToString();
        public MallController(IMallService _mall, ApplicationDbContext _context)
        {
            mall = _mall;
            context = _context;
        }
        // GET: Mall
        public ActionResult Index(int CategoryId = 0, string ProuctName = null)
        {
            var UserId = Session["UserId"].ToString();
            MallViewModel model = new MallViewModel();
            model.ProductCategoryList = mall.GetProductCategoryList();
            model.AddToCartList = mall.GetAddToCartList(new Guid(UserId));
            if (model.AddToCartList.Count > 0)
            {
                model.TotalCartPrice = model.AddToCartList.Sum(i => Convert.ToDecimal(i.TotalPrice));
                model.TotalQty = model.AddToCartList.Sum(i => Convert.ToInt32(i.Qty));
            }
            if (CategoryId != 0)
            {
                model.ProductList = mall.GetProductByCategoryIdList(CategoryId);
            }
            else if (ProuctName != null)
            {
                model.ProductList = mall.GetSerchProductList(ProuctName);
            }
            else
            {
                model.ProductList = mall.GetProductList();
            }
            return View(model);
        }

        public ActionResult Description(Guid? Id)
        {
            if (Id == null)
            {
                return RedirectToAction("Index", "Mall");
            }
            MallViewModel model = new MallViewModel();
            model.ProductDetail = mall.GetProductDetail(Id);
            model.ProductGallery = mall.GetGallery(Id.ToString());
            model.productColorList = context.ProductColor.Where(x => x.ProductId == Id).ToList();
            model.productSizeList = context.ProductSize.Where(x => x.ProductId == Id).ToList();
            return View(model);
        }
        [HttpPost]
        public async Task<JsonResult> AddToCart(string Qty = null, string ProductId = null, string Price = null, string Size = null, string Color = null)
        {

            Guid UserId = new Guid(Session["UserId"].ToString());
            int price = Convert.ToInt32(Price);
            int qty = Convert.ToInt32(Qty);
            string size = Size;
            string color = Color;
            var Total = price * qty;
            if (UserId != null)
            {
                var data = new AddToCartViewModel
                {
                    CustId = UserId,
                    ProductId = new Guid(ProductId),
                    TotalPrice = Total.ToString(),
                    Qty = qty,
                    Size = size,
                    Color = color,
                };
                ResponseModel response = await mall.AddinCart(data);
                if (response.Status == Status.Success)
                {
                    return Json(Status.Success);
                }
                else
                {
                    return Json(Status.Failure);
                }
            }
            return Json(Status.Success);
        }
        public ActionResult CheckOut(string Id = null, int Qty = 0, string Size = null, string Color = null)
        {
            if (Qty == 0)
            {
                return RedirectToAction("Description", "Mall");
            }
            string size = Size;
            string color = Color;
            Guid UserId = new Guid(Session["UserId"].ToString());
            CheckOutViewModel model = new CheckOutViewModel();
            model.ProductDetail = mall.GetProductDetailCheckOut(new Guid(Id), Qty);
            model.CustomerDetail = mall.GetCustomerDetail(UserId);
            var ship = mall.GetShippingfee(UserId);
            if (ship == null)
            {
                model.Shippingfees = "0.00";
            }
            else
            {
                model.Shippingfees = Convert.ToString(ship.ShippingCharge);
            }
            IEnumerable<SelectListItem> States = mall.GetStates();
            IEnumerable<SelectListItem> District = mall.GetDistrictts(model.CustomerDetail.StateId, model.CustomerDetail.DistricttId);
            ViewData["District"] = District;
            ViewData["States"] = States;
            model.Size = size;
            model.Color = color;
            Decimal totalprice = model.ProductDetail.TotalPrice != null ? Convert.ToDecimal(model.ProductDetail.TotalPrice) : 0;
            Decimal tax = model.ProductDetail.Tax != null ? Convert.ToDecimal(model.ProductDetail.Tax) : 0;
            Decimal discount = model.ProductDetail.Discount != null ? Convert.ToDecimal(model.ProductDetail.Discount) : 0;
            if (tax > 0)
            {
                totalprice += totalprice * tax / 100;
            }
            Decimal totalwithdiscount = totalprice - discount;
            model.ProductDetail.TotalPrice = Convert.ToString(totalwithdiscount);
            return View(model);
        }

        [HttpPost]
        public async Task<JsonResult> SaveCheckOutAddress(int stateid = 0, int districtId = 0, string postcode = null, string address1 = null, string address2 = null)
        {
            Guid UserId = new Guid(Session["UserId"].ToString());
            ResponseModel model = new ResponseModel();
            if (postcode != null && address1 != null && address2 != null && stateid != 0 && districtId != 0)
            {
                CheckOutViewModel data = new CheckOutViewModel();
                data.Address1 = address1;
                data.PostCode = postcode;
                data.Address2 = address2;
                data.StateId = stateid;
                data.DistricttId = districtId;
                data.Id = UserId;
                model = await mall.UpdateAddress(data);
                if (model.Status == Status.Success)
                {
                    return Json(Status.Success);
                }
                else
                {
                    return Json(Status.Failure);
                }
            }
            else
            {
                return Json(Status.Failure);
            }
            /// return Json(Status.Success);
        }


        public ActionResult checkOutConfirmation(string id = null, int Qty = 0, string Size = null, string Color = null)
        {
            if (id == null)
            {
                return RedirectToAction("Description", "Mall");
            }
            string size = Size;
            string color = Color;
            Guid UserId = new Guid(Session["UserId"].ToString());
            CheckOutViewModel model = new CheckOutViewModel();
            model.ProductDetail = mall.GetProductDetailCheckOut(new Guid(id), Qty);
            model.CustomerDetail = mall.GetCustomerDetail(UserId);
            var ship = mall.GetShippingfee(UserId);
            model.Size = size;
            model.Color = color;
            if (ship == null)
            {
                model.Shippingfees = "0.00";
            }
            else
            {
                model.Shippingfees = Convert.ToString(ship.ShippingCharge);
            }
            Decimal totalprice = model.ProductDetail.TotalPrice != null ? Convert.ToDecimal(model.ProductDetail.TotalPrice) : 0;
            Decimal tax = model.ProductDetail.Tax != null ? Convert.ToDecimal(model.ProductDetail.Tax) : 0;
            Decimal discount = model.ProductDetail.Discount != null ? Convert.ToDecimal(model.ProductDetail.Discount) : 0;
            if (tax > 0)
            {
                totalprice += totalprice * tax / 100;
            }
            Decimal totalwithdiscount = totalprice - discount;
            model.ProductDetail.TotalPrice = Convert.ToString(totalwithdiscount);
            //model.Message= Msg;
            return View(model);
        }

        public ActionResult Payment(string id = null, int Qty = 0, decimal Amount = 0, string Size = null, string Color = null)
        {
            ResponseModel res = new ResponseModel();
            CheckOutViewModel model = new CheckOutViewModel();

            Guid UserId = new Guid(Session["UserId"].ToString());
            var UserData = context.CustomerInfo.Where(x => x.Id == UserId).FirstOrDefault();
            string size = Size;
            string color = Color;
            if (UserData != null)
            {
                try
                {
                    var prodect = context.ProductMasters.Where(a => a.IsActive != false && a.Id == new Guid(id)).FirstOrDefault();

                    StripeConfiguration.ApiKey = "sk_test_51LAXO9IXGaxCW1IfrpV8y9w3UjESGM1yIPeIl0IyzPONpQYfDYW1WjwgbuQ7hZrQEKCFr1JWZ75Ybn2D0APAZla100aIjk0POP";
                    var options = new SessionCreateOptions
                    {
                        // Customer = UserData.Name,
                        // CustomerEmail = UserData.Email,
                        LineItems = new List<SessionLineItemOptions>
                     {
                      new SessionLineItemOptions
                      {
                      PriceData = new SessionLineItemPriceDataOptions
                      {
                       UnitAmount = (long?)Amount,
                      Currency = "myr",
                     ProductData = new SessionLineItemPriceDataProductDataOptions
                       {
                       Name = prodect.ProductName,
                        },
                      },
                      Quantity = Qty,
                      },
                     },
                        PaymentMethodTypes = new List<string> { "card" },
                        Mode = "payment",
                        // SuccessUrl = "https://localhost:44343/Mall/Reciept?id=" + id + "&qty=" + Qty + "&Size=" + size + "&Color=" + color + "",
                        SuccessUrl = "http://38.17.52.106:2020/Mall/Reciept?id=" + id + "&qty=" + Qty + "&Size=" + size + "&Color=" + color + "",
                        CancelUrl = "http://38.17.52.106:2020/Mall",
                    };
                    var service = new SessionService();
                    Session session = service.Create(options);
                    Session["Data"] = session;
                    Response.Headers.Add("Location", session.Url);
                    return Redirect(session.Url);
                }
                catch (Exception ex)
                {
                    ViewData["Error"] = "Data Saveing Failed";
                    // throw;
                }
            }
            return View();
        }
        public async Task<ActionResult> Reciept(string id, int Qty, string size = null, string color = null)
        {
            ResponseModel res1 = new ResponseModel();
            CheckOutViewModel model = new CheckOutViewModel();
            PaymentData data = new PaymentData();
            Guid UserId = new Guid(Session["UserId"].ToString());
            dynamic res = Session["Data"];
            if (res != null)
            {
                var sessionService = new SessionService();
                Session session = sessionService.Get(res.Id);
                if (session.Status == "complete")
                {
                    data.Size = size;
                    data.Color = color;
                    data.Qty = Qty;
                    data.Order_Id = session.PaymentIntentId;
                    data.ProductId = id;
                    data.payment_status = session.Status;
                    data.UserId = UserId.ToString();
                    data.Bill_Id = session.Id;
                    data.TotalAmount = Convert.ToDecimal(session.AmountTotal);
                    var customerService = new CustomerService();
                    Customer customer = customerService.Get(session.CustomerId);
                    res1 = await mall.SaveOrderPayment(data);
                    if (res1.Status == Status.Success && res1.OrderId != null)
                    {
                        ViewData["Success"] = "Payment succcess !!!!";
                        Session["Data"] = null;
                        return RedirectToAction("OrderDetail", "Mall", new { Id = res1.OrderId });
                    }
                    else
                    {
                        ViewData["Error"] = "Data  Saveing Failed";
                    }
                }

            }
              return View();
        }
        public ActionResult OrderHistory(string UserName = null, string date = null)
        {
            OrderHistory model = new OrderHistory();
            OrderHistory model2 = new OrderHistory();
            var UserId = Session["UserId"].ToString();
            model.OrderHistoryList = mall.OrderHistoryList(UserId);
            if (UserName != "" && UserName != null)
            {
                model.OrderHistoryList = mall.OrderHistorySearchList(UserId, UserName.ToString());

            }
            if (date != null && date != "")
            {
                DateTime date1 = Convert.ToDateTime(date).Date;
                model.OrderHistoryList = model.OrderHistoryList.Where(x => x.OrderDate.Date == date1).ToList();
                model.date = date;
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult GetOrder()
        {
            try
            {
                var UserId = Session["UserId"].ToString();
                var draw = Request.Form.GetValues("draw").FirstOrDefault();
                var start = Request.Form.GetValues("start").FirstOrDefault();
                var length = Request.Form.GetValues("length").FirstOrDefault();
                var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
                var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();
                int take = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                (int TotalCount, int FilteredCount, dynamic Customers) data = mall.GetOrders(skip, take, sortColumn, sortColumnDir, searchValue, UserId);
                return Json(new { draw = draw, recordsFiltered = data.TotalCount, recordsTotal = data.TotalCount, data = data.Customers });
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<ActionResult> OrderDetail(string Id)
        {
            OrderHistory model = new OrderHistory();
            Guid UserId = new Guid(Session["UserId"].ToString());
            if (Id != null)
            {
                model = await mall.OrderHistoryDetails(Id);
                model.CustomerDetail = mall.GetCustomerDetail(UserId);

            }
            return View(model);
        }
        public async Task<ActionResult> OrderCutomerDetail(string Id)
        {
            OrderHistory model = new OrderHistory();
            if (Id != null)
            {
                model = await mall.OrderHistoryDetails(Id);
                var id = model.CustId;
                model.CustomerDetail = mall.GetCustomerDetail(new Guid(id));

            }
            return View(model);
        }
        public async Task<ActionResult> OrderDetail1(string Id)
        {
            OrderHistory model = new OrderHistory();
            if (Id != null)
            {
                model = await mall.OrderHistoryDetails(Id);
                var id = model.CustId;
                model.CustomerDetail = mall.GetCustomerDetail(new Guid(id));
            }
            return View(model);
        }

        //private Collection CreateCollection(string Name)
        //{
        //    string api_key = Api_Key;
        //    string title = Name;
        //    Collection collection = new Collection();

        //    //call the api to create collection id
        //    WebRequest req = WebRequest.Create(@"https://www.billplz.com/api/v3/collections?title=" + title);
        //    req.Method = "POST";
        //    req.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes(api_key));
        //    HttpWebResponse resp = req.GetResponse() as HttpWebResponse;
        //    if (resp.StatusCode == HttpStatusCode.OK)
        //    {
        //        // Read the response body as string
        //        Stream dataStream = resp.GetResponseStream();
        //        StreamReader reader = new StreamReader(dataStream);
        //        var data = reader.ReadToEnd();
        //        collection = JsonConvert.DeserializeObject<Collection>(data);
        //        // collection.Id;
        //        resp.Close();
        //        return collection;
        //    }
        //    return collection;
        //}

        public ActionResult ClearCart()
        {
            Guid UserId = new Guid(Session["UserId"].ToString());
            if (UserId!=null)
            {
                try
                {
                    var res = context.AddToCart.Where(x => x.CustId == UserId).ToList();
                    if (res != null)
                    {
                        context.AddToCart.RemoveRange(res);
                        context.SaveChanges();
                        this.ShowMessage("success", "Clear Cart successfully !", ToastType.success);
                    }
                }
                catch (Exception ex)
                {
                    this.ShowMessage("failiure ", "Failled please contect Administrator !", ToastType.error);
                }
              
            }
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> AddToCart2(string ProductId)
        {
            try
            {
                Guid UserId = new Guid(Session["UserId"].ToString());
                Guid id = new Guid(ProductId);
                if (ProductId != null)
                {
                    var data = context.ProductMasters.Where(x => x.Id == id).FirstOrDefault();
                    var Total = data.Price;
                    if (UserId != null)
                    {
                        var data1 = new AddToCartViewModel
                        {
                            CustId = UserId,
                            ProductId = new Guid(ProductId),
                            TotalPrice = Total.ToString(),
                            Qty = 1,
                        };
                        ResponseModel response = await mall.AddinCart(data1);
                        if (response.Status == Status.Success)
                        {
                           
                            this.ShowMessage("success", "Add Cart successfully !", ToastType.success);
                            return RedirectToAction("Index", "Mall");
                        }
                        else
                        {
                           
                            this.ShowMessage("error ", "Add Cart Not successfully !", ToastType.error);
                            return RedirectToAction("Index","Mall");
                        }
                    }
                }
            }
            catch (Exception)
            {
               
                this.ShowMessage("failiure ", "Failled please contect Administrator !", ToastType.error);
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index","Home");
        }
        [HttpPost]
        public async Task<JsonResult> AddToCartJson(string ProductId, int qty = 1)
        {
            try
            {
                Guid UserId = new Guid(Session["UserId"].ToString());
                Guid id = new Guid(ProductId);
                if (ProductId != null)
                {
                    var data = context.ProductMasters.Where(x => x.Id == id).FirstOrDefault();
                    var Total = Convert.ToInt32(data.Price) * qty;
                    if (UserId != null)
                    {
                        var data1 = new AddToCartViewModel
                        {
                            CustId = UserId,
                            ProductId = new Guid(ProductId),
                            TotalPrice = Total.ToString(),
                            Qty = qty,
                        };
                        ResponseModel response = await mall.AddinCart(data1);
                        if (response.Status == Status.Success)
                        {
                            return Json(Status.Success);
                        }
                        else
                        {
                            return Json(Status.Failure);
                        }                                
                }
                }
            }
            catch (Exception)
            {

                this.ShowMessage("failiure ", "Failled please contect Administrator !", ToastType.error);
                return Json(Status.Failure);
            }
            return Json(Status.Success);
        }
    }
}