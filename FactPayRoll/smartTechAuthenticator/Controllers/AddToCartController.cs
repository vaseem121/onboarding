using Nest;
using Newtonsoft.Json;
using smartTechAuthenticator.Models;
using smartTechAuthenticator.Services.Comman.CustomFilters;
using smartTechAuthenticator.Services.Mall;
using smartTechAuthenticator.ViewModels;
using Stripe.Checkout;
using Stripe;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Status = smartTechAuthenticator.ViewModels.Status;

namespace smartTechAuthenticator.Controllers
{
    [Authorized]
    public class AddToCartController : Controller
    {
        private readonly IMallService mall;
        private readonly ApplicationDbContext context;
        static string Api_Key = System.Configuration.ConfigurationManager.AppSettings["Api_Key"].ToString();
        public AddToCartController(IMallService _mall, ApplicationDbContext _context)
        {
            mall = _mall;
            context= _context;
        }
        // GET: AddToCart
        public ActionResult Index()
        {
            var UserId = Session["UserId"].ToString();
            MallViewModel model = new MallViewModel();
            model.AddToCartList = mall.GetAddToCartList(new Guid(UserId));
            if (model.AddToCartList.Count > 0)
            {
                model.TotalCartPrice = model.AddToCartList.Sum(i => Convert.ToDecimal(i.TotalPrice));
                model.TotalQty = model.AddToCartList.Sum(i => Convert.ToInt32(i.Qty));
            }

            return View(model);
        }

        public async Task<ActionResult> DeleteCart(Guid Id)
        {
            var msg = await mall.DeleteCartDetails(Id);
            return RedirectToAction("Index", "Mall");
        }

        public async Task<ActionResult> DeleteInCart(Guid Id)
        {
            var msg = await mall.DeleteCartDetails(Id);
            return RedirectToAction("Index", "Mall");
        }
        public async Task<ActionResult> ManageCart(string Qty=null,string Id=null,string Qt=null)
        {
            Guid UserId = new Guid(Session["UserId"].ToString());
            int qty;
          
            if (Qt == "Plus")
            {
                qty = Convert.ToInt32(Qty) + 1;
            }
            else
            {
                if (Qty == "1")
                {
                    return RedirectToAction("Index", "AddToCart");
                }
                qty = Convert.ToInt32(Qty) - 1;

            }
            if (UserId != null)
            {
                var data = new AddToCartViewModel
                {
                    CustId = UserId,
                    ProductId = new Guid(Id),
                    Qty = qty,
                };
                ResponseModel response = await mall.managecart(data);
                if (response.Status == Status.Success)
                {
                    return RedirectToAction("Index", "AddToCart");
                }
                else
                {
                    return RedirectToAction("Index", "AddToCart");
                }
            }
            return View();
        }

        public ActionResult CartToCheckOut()
        {
           
            Guid UserId = new Guid(Session["UserId"].ToString());
            CheckOutViewModel model = new CheckOutViewModel();
            model.ProductDetail = mall.GetProductDetailCartCheckOut(UserId);
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
            return View(model);
        }

        public ActionResult checkOutConfirmations()
        {
            Guid UserId = new Guid(Session["UserId"].ToString());
            if (UserId == null)
            {
                return RedirectToAction("Description", "Mall");
            }
            CheckOutViewModel model = new CheckOutViewModel();
            model.ProductDetail = mall.GetProductDetailCartCheckOut(UserId);
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

            //model.Message= Msg;
            return View(model);
        }

        //public async Task<ActionResult> Payment(string ok = null,decimal Amount=0)
        //{
        //    ResponseModel res = new ResponseModel();
        //    CheckOutViewModel model = new CheckOutViewModel();
        //    Guid UserId = new Guid(Session["UserId"].ToString());
        //    if (UserId == null && ok != null)
        //    {
        //        return RedirectToAction("Description", "Mall");
        //    }
        //    else
        //    {
        //        var UserData = context.CustomerInfo.Where(x => x.Id == UserId).FirstOrDefault();
        //        if (UserData != null)
        //        {
        //            var data1 = CreateCollection(UserData.Name);
        //            // CreateBill(data.Id);
        //            string api_key = Api_Key;
        //            string collection_id = data1.Id;
        //            string email = UserData.Email;
        //            string name = UserData.Name;
        //            string amount = Amount.ToString();
        //            string callback_url = "https://localhost:44343/Mall/GetResponce1/";
        //            string description = "Test";
        //            string redirect_url = "https://localhost:44343/Mall/GetResponce/";
        //            BillViewModel bill = new BillViewModel();

        //            WebRequest req = WebRequest.Create(@"https://www.billplz.com/api/v3/bills?collection_id=" + collection_id + "&description=" + description + "&email=" + email + "&name=" + name + "&amount="
        //              + amount + "&callback_url=" + callback_url);
        //            req.Method = "POST";
        //            req.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes(api_key));
        //            try
        //            {
        //                HttpWebResponse resp = req.GetResponse() as HttpWebResponse;

        //                if (resp.StatusCode == HttpStatusCode.OK)
        //                {
        //                    // Read the response body as string
        //                    Stream dataStream = resp.GetResponseStream();
        //                    StreamReader reader = new StreamReader(dataStream);
        //                    var data = reader.ReadToEnd();
        //                    bill = JsonConvert.DeserializeObject<BillViewModel>(data);
        //                    resp.Close();
        //                    //redirect user to billplz website for payment
        //                    return Redirect(bill.Url);
        //                }

        //            }
        //            catch (Exception ex)
        //            {
        //                throw new Exception(ex.Message);
        //            }

        //            //res = await mall.SaveCartOrderPayment(UserId);
        //            //if (res.Status == Status.Success && res.OrderId != null)
        //            //{
        //            //    //model.ProductDetail = mall.GetProductDetailCheckOut(new Guid(id), Qty);
        //            //    model.CustomerDetail = mall.GetCustomerDetail(UserId);
        //            //    model.OrderDetail = mall.GetOrderDetails(res.OrderId, UserId);
        //            //    return View(model);
        //            //}

        //        }
        //        return View(model);
        //    }
        //}

        private Collection CreateCollection(string Name)
        {
            string api_key = Api_Key;
            string title = Name;
            Collection collection = new Collection();

            //call the api to create collection id
            WebRequest req = WebRequest.Create(@"https://www.billplz.com/api/v3/collections?title=" + title);
            req.Method = "POST";
            req.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes(api_key));
            HttpWebResponse resp = req.GetResponse() as HttpWebResponse;
            if (resp.StatusCode == HttpStatusCode.OK)
            {
                // Read the response body as string
                Stream dataStream = resp.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                var data = reader.ReadToEnd();
                collection = JsonConvert.DeserializeObject<Collection>(data);
                // collection.Id;
                resp.Close();
                return collection;
            }
            return collection;
        }

        public ActionResult Payment1(string ok = null,decimal Amount = 0, int Qty = 0)
        {
            ResponseModel res = new ResponseModel();
            CheckOutViewModel model = new CheckOutViewModel();
            Guid UserId = new Guid(Session["UserId"].ToString());
           
            if (UserId == null && ok != null)
            {
                return RedirectToAction("Description", "Mall");
            }
            else
            {
              var  UserData = context.CustomerInfo.Where(x => x.Id == UserId).FirstOrDefault();
                if (UserData != null)
                {
                  try
                    {
                           // var prodect = context.ProductMasters.Where(a => a.IsActive != false && a.Id == new Guid(id)).FirstOrDefault();
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
                       Name = "Cart Product",
                        },
                      },
                      Quantity = Qty,
                      },
                     },
                            PaymentMethodTypes = new List<string> { "card" },
                            Mode = "payment",
                          // SuccessUrl = "https://localhost:44343/AddTocart/Reciept",
                           SuccessUrl = "http://38.17.52.106:2020/Mall/Reciept",
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
                        //throw new Exception(ex.Message);
                    }
                }

            }
            return View(model);
        }
        public async Task<ActionResult> Reciept()
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
                    data.Order_Id = session.PaymentIntentId;
                    data.payment_status = session.Status;
                    data.UserId = UserId.ToString();
                    data.Bill_Id = session.Id;
                    data.TotalAmount = Convert.ToDecimal(session.AmountTotal);
                    var customerService = new CustomerService();
                    Customer customer = customerService.Get(session.CustomerId);
                 //   res1 = await mall.SaveOrderPayment(data);
                    res1 = await mall.SaveCartOrderPayment(data, UserId);
                    if (res1.Status == Status.Success && res1.OrderId != null)
                    {

                        ViewData["Success"] = "Payment succcess !!!!";
                        Session["Data"] = null;
                        return RedirectToAction("OrderDetail", "Mall", new { Id = res1.OrderId });
                    }
                    else
                    {
                        ViewData["Error"] = "Data Saveing Failed";
                    }
                }

            }
            return View();
        }
    }
}