using NLog;
using smartTechAuthenticator.Models;
using smartTechAuthenticator.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace smartTechAuthenticator.Services.Mall
{
    public class MallService : IMallService
    {
        private readonly ApplicationDbContext context;
        public readonly Logger logger;
        public MallService(ApplicationDbContext _context, Logger _logger)
        {
            context = _context;
            logger = _logger;
        }

        public List<ProductCategoryViewModel> GetProductCategoryList()
        {
            List<ProductCategoryViewModel> model = new List<ProductCategoryViewModel>();
            try
            {
                model = (from a in context.ProductCategories
                         select new ProductCategoryViewModel
                         {
                             CategoryId = a.Id.ToString(),
                             CategoryName = a.CategoryName
                         }).ToList();
            }
            catch (Exception ex)
            {
                return model;
            }

            return model;
        }
        public List<ProductNewViewModel> GetProductList()
        {
            List<ProductNewViewModel> model = new List<ProductNewViewModel>();
            try
            {
                model = (from a in context.ProductMasters
                         where a.IsActive == true
                         select new ProductNewViewModel
                         {
                             ProductName = a.ProductName,
                             IsActive = a.IsActive,
                             Authenticode = a.Authenticode,
                             BachNumber = a.BachNumber,
                             CategoryId = a.CategoryId,
                             CreatedDate = a.CreatedDate,
                             Description = a.Description,
                             Id = a.Id.ToString(),
                             INVNo = a.INVNo,
                             Location = a.Location,
                             Photo = a.Photo,
                             Price = a.Price,
                             Tax = a.Tax,
                             TotalPrice = a.TotalPrice
                         }).ToList();
            }
            catch (Exception ex)
            {
                return model;
            }

            return model;
        }
        public List<ProductNewViewModel> GetProductByCategoryIdList(int CategoryId)
        {
            List<ProductNewViewModel> model = new List<ProductNewViewModel>();
            try
            {
                model = (from a in context.ProductMasters
                         where a.IsActive != false && a.CategoryId == CategoryId
                         select new ProductNewViewModel
                         {
                             ProductName = a.ProductName,
                             IsActive = a.IsActive,
                             Authenticode = a.Authenticode,
                             BachNumber = a.BachNumber,
                             CategoryId = a.CategoryId,
                             CreatedDate = a.CreatedDate,
                             Description = a.Description,
                             Id = a.Id.ToString(),
                             INVNo = a.INVNo,
                             Location = a.Location,
                             Photo = a.Photo,
                             Price = a.Price,
                             Tax = a.Tax,
                             TotalPrice = a.TotalPrice
                         }).ToList();
            }
            catch (Exception ex)
            {
                return model;
            }

            return model;
        }
        public ProductNewViewModel GetProductDetail(Guid? Id)
        {
            ProductNewViewModel model = new ProductNewViewModel();
            try
            {
                model = (from a in context.ProductMasters.Where(a => a.IsActive != false && a.Id == Id)
                         join Cat in context.ProductCategories on a.CategoryId equals Cat.Id
                         select new ProductNewViewModel
                         {
                             ProductName = a.ProductName,
                             IsActive = a.IsActive,
                             Authenticode = a.Authenticode,
                             BachNumber = a.BachNumber,
                             CategoryId = a.CategoryId,
                             CreatedDate = a.CreatedDate,
                             Description = a.Description,
                             Id = a.Id.ToString(),
                             INVNo = a.INVNo,
                             Location = a.Location,
                             Photo = a.Photo,
                             Price = a.Price,
                             Tax = a.Tax,
                             TotalPrice = a.TotalPrice,
                             CategoryName = Cat.CategoryName,
                             ProductType = a.ProductType
                         }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                return model;
            }

            return model;
        }
        public List<ProductGalleryViewModel> GetGallery(string ProductId)
        {
            ProductGalleryViewModel model = new ProductGalleryViewModel();
            try
            {
                if (ProductId != null)
                {
                    model.ProductGalleryList = (from a in context.ProductGallery.Where(x => x.ProductId == ProductId)
                                                select new ProductGalleryViewModel
                                                {
                                                    Id = a.Id,
                                                    ProductId = a.ProductId,
                                                    Photo = a.Photo
                                                }).ToList();
                }
            }
            catch (Exception ex)
            {

            }
            return model.ProductGalleryList;
        }
        public List<ProductNewViewModel> GetSerchProductList(string ProuctName)
        {
            List<ProductNewViewModel> model = new List<ProductNewViewModel>();
            try
            {
                var data = context.ProductMasters.Where(m => m.ProductName.Contains(ProuctName)).ToList();
                model = (from a in data
                         where a.IsActive != false
                         select new ProductNewViewModel
                         {
                             ProductName = a.ProductName,
                             IsActive = a.IsActive,
                             Authenticode = a.Authenticode,
                             BachNumber = a.BachNumber,
                             CategoryId = a.CategoryId,
                             CreatedDate = a.CreatedDate,
                             Description = a.Description,
                             Id = a.Id.ToString(),
                             INVNo = a.INVNo,
                             Location = a.Location,
                             Photo = a.Photo,
                             Price = a.Price,
                             Tax = a.Tax,
                             TotalPrice = a.TotalPrice
                         }).ToList();
            }
            catch (Exception ex)
            {
                return model;
            }

            return model;
        }
        public List<AddToCartViewModel> GetAddToCartList(Guid? CustId)
        {
            List<AddToCartViewModel> model = new List<AddToCartViewModel>();
            try
            {
                model = (from a in context.AddToCart
                         where a.CustId == CustId
                         join b in context.ProductMasters on a.ProductId equals b.Id
                         select new AddToCartViewModel
                         {
                             ProductName = b.ProductName,
                             Description = b.Description,
                             Id = a.Id,
                             Qty = a.Qty,
                             TotalPrice = a.TotalPrice,
                             Photo = b.Photo,
                             ProductId = b.Id,
                             Price = b.Price
                         }).ToList();
            }
            catch (Exception ex)
            {
                return model;
            }

            return model;
        }

        public async Task<ResponseModel> DeleteCartDetails(Guid Id)
        {
            ResponseModel model = new ResponseModel();
            try
            {
                var Info = context.AddToCart.FirstOrDefault(x => x.Id == Id);
                if (Info != null)
                {
                    var data = context.AddToCart.Find(Info.Id);
                    if (data != null)
                    {
                        context.AddToCart.Remove(data);
                    }
                    await context.SaveChangesAsync();
                    model.Status = Status.Success;
                }
            }
            catch (Exception ex)
            {
                //logger.Error(ex);
                model.Status = Status.Failure;
            }
            return model;
        }

        public async Task<ResponseModel> AddinCart(AddToCartViewModel cart)
        {
            ResponseModel model = new ResponseModel();
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var res = context.AddToCart.Where(x => x.CustId == cart.CustId && x.ProductId == cart.ProductId).FirstOrDefault();
                    if (res == null)
                    {

                        AddToCart data = new AddToCart();
                        data.Id = Guid.NewGuid();
                        data.Qty = cart.Qty;
                        data.CreatedDate = DateTime.Now;
                        data.TotalPrice = cart.TotalPrice;
                        data.ProductId = cart.ProductId;
                        data.Status = 1;
                        data.CustId = cart.CustId;
                        data.CreatedBy = Convert.ToString(cart.CustId);
                        data.Size = cart.Size;
                        data.Color = cart.Color;
                        context.AddToCart.Add(data);
                        await context.SaveChangesAsync();
                        await transaction.CommitAsync();
                        model.Status = Status.Success;
                    }
                    else
                    {
                        res.Size = cart.Size;
                        res.Color = cart.Color;
                        int q = Convert.ToInt32(cart.Qty);
                        int q1 = Convert.ToInt32(res.Qty);
                        int price = Convert.ToInt32(cart.TotalPrice);
                        int price1 = Convert.ToInt32(res.TotalPrice);
                        var tp = price + price1;
                        res.Qty = q + q1;
                        res.TotalPrice = Convert.ToString(tp);
                        context.AddToCart.Update(res);
                        await context.SaveChangesAsync();
                        await transaction.CommitAsync();
                        model.Status = Status.Success;
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex);
                    model.Status = Status.Failure;
                    await transaction.RollbackAsync();
                }
                return model;
            }
        }
        public async Task<ResponseModel> managecart(AddToCartViewModel add)
        {
            ResponseModel model = new ResponseModel();
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {

                    var res = context.AddToCart.Where(x => x.CustId == add.CustId && x.Id == add.ProductId).FirstOrDefault();
                    var pd = context.ProductMasters.Where(x => x.Id == res.ProductId).FirstOrDefault();
                    var price = add.Qty * Convert.ToInt32(pd.Price);
                    if (res != null)
                    {
                        AddToCart data = new AddToCart();
                        res.Qty = add.Qty;
                        res.TotalPrice = Convert.ToString(price);
                        context.AddToCart.Update(res);
                        await context.SaveChangesAsync();
                        await transaction.CommitAsync();
                        model.Status = Status.Success;
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex);
                    model.Status = Status.Failure;
                    await transaction.RollbackAsync();
                }
            }
            return model;

        }

        public ProductNewViewModel GetProductDetailCheckOut(Guid? Id, int Qty)
        {
            ProductNewViewModel model = new ProductNewViewModel();
            try
            {
                model = (from a in context.ProductMasters.Where(a => a.IsActive != false && a.Id == Id)
                         select new ProductNewViewModel
                         {
                             ProductName = a.ProductName,
                             IsActive = a.IsActive,
                             Authenticode = a.Authenticode,
                             BachNumber = a.BachNumber,
                             CategoryId = a.CategoryId,
                             CreatedDate = a.CreatedDate,
                             Description = a.Description,
                             Id = a.Id.ToString(),
                             INVNo = a.INVNo,
                             Location = a.Location,
                             Photo = a.Photo,
                             Price = a.Price,
                             Tax = a.Tax,
                             Discount = a.Discount,
                             TotalPrice = (Convert.ToDecimal(a.Price) * Qty).ToString(),
                             Qty = Qty
                         }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                return model;
            }

            return model;
        }



        public CustomerInfoViewModel GetCustomerDetail(Guid? Id)
        {
            CustomerInfoViewModel model = new CustomerInfoViewModel();
            try
            {
                model = (from a in context.CustomerInfo.Where(a => a.Id == Id)
                         join Cu in context.CustomerAddressMasters on a.CustomerAddressId equals Cu.Id
                         join s in context.StateMaters on Cu.StateId equals s.Id
                         join d in context.DistricttMasters on Cu.DistricttId equals d.Id
                         select new CustomerInfoViewModel
                         {
                             Address = Cu.Address,
                             Address1 = a.Address1,
                             Address2 = a.Address2,
                             Address3 = a.Address3,
                             Email = a.Email,
                             MobileNo = a.MobileNo,
                             Name = a.Name,
                             PostCode = Cu.PostCode,
                             DistricttId = Cu.DistricttId,
                             StateId = Cu.StateId,
                             State = s.StateName,
                             District = d.DistricttName
                         }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                return model;
            }

            return model;
        }
        //public CustomerInfoViewModel GetCustomerDetail1(string Id)
        //{
        //    CustomerInfoViewModel model = new CustomerInfoViewModel();
        //    try
        //    {
        //        model = (from a in context.CustomerInfo.Where(a => a.Id == Id.ToString)
        //                 join Cu in context.CustomerAddressMasters on a.CustomerAddressId equals Cu.Id
        //                 join s in context.StateMaters on Cu.StateId equals s.Id
        //                 join d in context.DistricttMasters on Cu.DistricttId equals d.Id
        //                 select new CustomerInfoViewModel
        //                 {
        //                     Address = Cu.Address,
        //                     Address1 = a.Address1,
        //                     Address2 = a.Address2,
        //                     Address3 = a.Address3,
        //                     Email = a.Email,
        //                     MobileNo = a.MobileNo,
        //                     UserName = a.Name,
        //                     PostCode = Cu.PostCode,
        //                     DistricttId = Cu.DistricttId,
        //                     StateId = Cu.StateId,
        //                     State = s.StateName,
        //                     District = d.DistricttName
        //                 }).FirstOrDefault();
        //    }
        //    catch (Exception ex)
        //    {
        //        return model;
        //    }

        //    return model;
        //}

        public List<SelectListItem> GetDistrictts(int StateId, int districtId)
        {
            return context.DistricttMasters.Where(x => x.StateId == StateId && x.Id == districtId).Select(x => new SelectListItem() { Text = x.DistricttName, Value = x.Id.ToString() }).ToList();
        }
        public ShippingViewModel GetShippingfee(Guid? Id)
        {
            ShippingViewModel res = new ShippingViewModel();
            try
            {
                //res = (from a in context.CustomerInfo.Where(a => a.Id == Id)
                //       join Cu in context.CustomerAddressMasters on a.CustomerAddressId equals Cu.Id
                //      join s in context.Shipping on new { Cu.StateId, Cu.DistricttId } equals new { s.StateId, s.DistricttId } select
                //           new ShippingViewModel
                //           {
                //               ShippingCharge = s.ShippingCharge,
                //               Id = s.Id.ToString()
                //           }).FirstOrDefault();

                var res1 = (from a in context.CustomerInfo.Where(a => a.Id == Id)
                            join Cu in context.CustomerAddressMasters on a.CustomerAddressId equals Cu.Id
                            select Cu).FirstOrDefault();
                var StateId = res1.StateId;
                var DisId = res1.DistricttId;
                var data = context.Shipping.ToList();
                foreach (var item in data)
                {
                    if (item.StateId != null && item.DistricttId != null)
                    {
                        var a = item.StateId;
                        var b = a.Split(',');
                        var d = item.DistricttId.Remove(item.DistricttId.Length - 1, 1);
                        var d1 = d.Split(',');
                        foreach (var item1 in b)
                        {
                            if (StateId.ToString() == item1)
                            {
                                foreach (var item2 in d1)
                                {
                                    if (DisId.ToString() == item2)
                                    {
                                        if (item.ShippingCharge != 0)
                                        {
                                            res.ShippingCharge = item.ShippingCharge;
                                            res.Id = item.Id.ToString();
                                            return res;
                                        }

                                    }
                                }
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {

            }

            return res;
        }

        public List<SelectListItem> GetStates()
        {
            return context.StateMaters.Select(x => new SelectListItem() { Text = x.StateName, Value = x.Id.ToString() }).ToList();
        }
        public List<SelectListItem> GetDistrictts(int StateId)
        {
            return context.DistricttMasters.Where(x => x.StateId == StateId).Select(x => new SelectListItem() { Text = x.DistricttName, Value = x.Id.ToString() }).ToList();
        }
        public async Task<ResponseModel> UpdateAddress(CheckOutViewModel customer)
        {
            ResponseModel model = new ResponseModel();
            try
            {
                var res = context.CustomerInfo.Where(x => x.Id == customer.Id).FirstOrDefault();
                var addd = context.CustomerAddressMasters.Where(x => x.Id == res.CustomerAddressId).FirstOrDefault();
                if (res != null)
                {
                    res.Address1 = customer.Address1;
                    res.Address2 = customer.Address2;
                    context.CustomerInfo.Update(res);
                    await context.SaveChangesAsync();
                    //      await transection.CommitAsync();

                    model.Status = Status.Success;
                }
                if (addd != null)
                {
                    addd.PostCode = customer.PostCode;
                    addd.StateId = customer.StateId;
                    addd.DistricttId = customer.DistricttId;
                    addd.Address = customer.Address1 + "," + customer.Address2;
                    context.CustomerAddressMasters.Update(addd);
                    await context.SaveChangesAsync();
                    //    await transection.CommitAsync();
                    model.Status = Status.Success;
                }

            }
            catch (Exception ex)
            {
                //  await transection.RollbackAsync();
                model.Status = Status.Failure;
                model.Message = ex.Message;
                //  Logger.Error(ex);
            }
            //  }
            return model;
        }

        public ProductNewViewModel GetProductDetailCartCheckOut(Guid? Userid)
        {
            ProductNewViewModel model = new ProductNewViewModel();
            try
            {
                model.productNewViewList = (from a in context.AddToCart.Where(a => a.CustId == Userid)
                                            join cu in context.ProductMasters.Where(cu => cu.IsActive != false) on a.ProductId equals cu.Id
                                            select new ProductNewViewModel
                                            {
                                                ProductName = cu.ProductName,
                                                IsActive = cu.IsActive,
                                                Authenticode = cu.Authenticode,
                                                BachNumber = cu.BachNumber,
                                                CategoryId = cu.CategoryId,
                                                CreatedDate = a.CreatedDate,
                                                Description = cu.Description,
                                                Id = a.Id.ToString(),
                                                INVNo = cu.INVNo,
                                                Location = cu.Location,
                                                Photo = cu.Photo,
                                                Price = cu.Price,
                                                Tax = cu.Tax,
                                                TotalPrice = (Convert.ToDecimal(cu.Price) * a.Qty).ToString(),
                                                Qty = a.Qty,
                                            }).ToList();
            }
            catch (Exception ex)
            {
                return model;
            }

            return model;
        }

     
        public async Task<ResponseModel> SaveOrderPayment(PaymentData data)
        {
            var shippingfees = GetShippingfee(new Guid(data.UserId));
            decimal ShipC = 0;
            var ShipId = "";
            if (shippingfees == null)
            {
                ShipC = 0;
                ShipId = "";
            }
            else
            {
                ShipC = shippingfees.ShippingCharge;
                ShipId = shippingfees.Id;
            }
            ResponseModel res = new ResponseModel();
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var pro = context.ProductMasters.Where(a => a.IsActive != false && a.Id == new Guid(data.ProductId)).FirstOrDefault();
                    var ammount = Convert.ToDecimal(pro.Price) * data.Qty + ShipC;
                    if (pro != null)
                    {
                        Payment pay = new Payment();
                        pay.Id = Guid.NewGuid();
                        pay.UserId = new Guid(data.UserId);
                        pay.OrderId = data.Order_Id;
                        pay.PaymentType = data.payment_status;
                        pay.Amount = data.TotalAmount;
                        pay.CreatedDate = DateTime.Now;
                        pay.CreatedBy = data.UserId;
                        pay.Status = data.payment_status;
                        pay.Bill_Id = data.Bill_Id;

                        context.Payment.Add(pay);
                        await context.SaveChangesAsync();
                        //await transaction.CommitAsync();

                        Order order = new Order();
                        order.Id = Guid.NewGuid();
                        order.Order_Id = Guid.NewGuid().ToString().Substring(0, 12).Replace("-", "").ToUpper();
                        order.UserId = new Guid(data.UserId);
                        order.PaymentId = pay.Id;
                        order.CreatedBy = data.UserId;
                        order.TotalAmount = ammount;
                        order.CreatedDate = DateTime.Now;
                        order.ShippingId = new Guid(ShipId);
                        context.Order.Add(order);
                        await context.SaveChangesAsync();
                        //await transaction.CommitAsync();

                        Order_Items order_i = new Order_Items();
                        order_i.Id = Guid.NewGuid();
                        order_i.OrderId = order.Id;
                        order_i.ProductId = new Guid(data.ProductId);
                        order_i.Amount = ammount;
                        order_i.Qty = Convert.ToInt32(data.Qty);
                        order_i.CreatedDate = DateTime.Now;
                        order_i.CreatedBy = data.UserId;
                        order_i.UserId = new Guid(data.UserId);
                        order_i.Size = data.Size;
                        order_i.Color = data.Color;
                        context.Order_Items.Add(order_i);
                        await context.SaveChangesAsync();

                        ShippingTracking ship = new ShippingTracking();
                        ship.Id = Guid.NewGuid();
                        ship.UserId = new Guid(data.UserId);
                        ship.OrderId = order.Id;
                        ship.Status = "1";
                        ship.OrderDate = DateTime.Now;
                        ship.CreateDate = DateTime.Now;

                        context.ShippingTracking.Add(ship);
                        await context.SaveChangesAsync();
                        await transaction.CommitAsync();
                        res.Status = Status.Success;
                        res.OrderId = order.Id;
                    }
                    else
                    {
                        res.Status = Status.Failure;
                        await transaction.RollbackAsync();
                    }
                }
                catch (Exception)
                {
                    res.Status = Status.Failure;
                    await transaction.RollbackAsync();
                }
            }
            return res;
        }

        public Order_ItemViewModel GetOrderDetails(Guid? OrderId, Guid? UserId)
        {
            Order_ItemViewModel model = new Order_ItemViewModel();
            try
            {
                model = (from a in context.Order.Where(x => x.UserId == UserId && x.Id == OrderId)
                         select new Order_ItemViewModel
                         {
                             Id = a.Id.ToString(),
                             PaymentId = a.PaymentId.ToString(),
                             CreatedDate = a.CreatedDate,
                             TotalAmount = a.TotalAmount,
                             Order_Id = a.Order_Id
                         }).FirstOrDefault();
                return model;
            }
            catch (Exception)
            {
                return model;
            }
        }
        public async Task<ResponseModel> SaveCartOrderPayment(PaymentData data, Guid? UserId)
        {
            ResponseModel res = new ResponseModel();
            List<AddToCartViewModel> model = new List<AddToCartViewModel>();
            var shippingfees = GetShippingfee(UserId);
            decimal ShipC = 0;
            var ShipId = "";
            if (shippingfees == null)
            {
                ShipC = 0;
                ShipId = "";
            }
            else
            {
                ShipC = shippingfees.ShippingCharge;
                ShipId = shippingfees.Id;
            }
            model = (from a in context.AddToCart
                     where a.CustId == UserId
                     join b in context.ProductMasters on a.ProductId equals b.Id
                     select new AddToCartViewModel
                     {
                         ProductName = b.ProductName,
                         Description = b.Description,
                         Id = a.Id,
                         Qty = a.Qty,
                         TotalPrice = a.TotalPrice,
                         Photo = b.Photo,
                         ProductId = b.Id,
                         Price = b.Price
                     }).ToList();
            if (model != null)
            {
                var ammount = model.Sum(i => Convert.ToDecimal(i.TotalPrice)) + ShipC;
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        Payment pay = new Payment();
                        pay.Id = Guid.NewGuid();
                        pay.UserId = new Guid(data.UserId);
                        pay.OrderId = data.Order_Id;
                        pay.PaymentType = data.payment_status;
                        pay.Amount = data.TotalAmount;
                        pay.CreatedDate = DateTime.Now;
                        pay.CreatedBy = data.UserId;
                        pay.Status = data.payment_status;
                        pay.Bill_Id = data.Bill_Id;

                        context.Payment.Add(pay);
                        await context.SaveChangesAsync();
                        //await transaction.CommitAsync();

                        Order order = new Order();
                        order.Id = Guid.NewGuid();
                        order.Order_Id = Guid.NewGuid().ToString().Substring(0, 12).Replace("-", "").ToUpper();
                        order.UserId = new Guid(data.UserId);
                        order.PaymentId = pay.Id;
                        order.CreatedBy = data.UserId;
                        order.TotalAmount = ammount;
                        order.CreatedDate = DateTime.Now;
                        order.ShippingId = new Guid(ShipId);
                        context.Order.Add(order);
                        await context.SaveChangesAsync();
                        //await transaction.CommitAsync();

                        foreach (var item in model)
                        {
                            Order_Items order_i = new Order_Items();
                            order_i.Id = Guid.NewGuid();
                            order_i.OrderId = order.Id;
                            order_i.ProductId = item.ProductId;
                            order_i.Amount = Convert.ToInt32(item.TotalPrice);
                            order_i.Qty = Convert.ToInt32(item.Qty);
                            order_i.CreatedDate = DateTime.Now;
                            order_i.CreatedBy = data.UserId;
                            order_i.UserId = new Guid(data.UserId);
                            order_i.Size = data.Size;
                            order_i.Color = data.Color;
                            context.Order_Items.Add(order_i);
                            await context.SaveChangesAsync();
                        }
                        ShippingTracking ship = new ShippingTracking();
                        ship.Id = Guid.NewGuid();
                        ship.UserId = new Guid(data.UserId);
                        ship.OrderId = order.Id;
                        ship.Status = "1";
                        ship.OrderDate = DateTime.Now;
                        ship.CreateDate = DateTime.Now;

                        context.ShippingTracking.Add(ship);
                        await context.SaveChangesAsync();
                        await transaction.CommitAsync();
                        res.Status = Status.Success;
                        res.OrderId = order.Id;

                        IEnumerable<AddToCart> reco = context.AddToCart.Where(x => x.CustId == UserId);
                        context.AddToCart.RemoveRange(reco);
                        context.SaveChanges();
                        //Payment pay = new Payment();
                        //pay.Id = Guid.NewGuid();
                        //pay.UserId = (Guid)UserId;
                        //pay.PaymentType = "Cash";
                        //pay.Amount = ammount;
                        //pay.CreatedDate = DateTime.Now;
                        //pay.CreatedBy = UserId.ToString();
                        //pay.Status = "Failler";
                        //context.Payment.Add(pay);
                        //await context.SaveChangesAsync();

                        //Order order = new Order();
                        //order.Id = Guid.NewGuid();
                        //order.Order_Id = Guid.NewGuid().ToString().Substring(0, 12).Replace("-", "").ToUpper();
                        //order.UserId = (Guid)UserId;
                        //order.PaymentId = pay.Id;
                        //order.CreatedBy = UserId.ToString();
                        //order.TotalAmount = ammount;
                        //order.CreatedDate = DateTime.Now;
                        //order.ShippingId = new Guid(ShipId);
                        //context.Order.Add(order);
                        //await context.SaveChangesAsync();

                        //foreach (var item in model)
                        //{
                        //    Order_Items order_i = new Order_Items();
                        //    order_i.Id = Guid.NewGuid();
                        //    order_i.OrderId = order.Id;
                        //    order_i.ProductId = item.ProductId;
                        //    order_i.Amount = Convert.ToDecimal(item.Price) * item.Qty;
                        //    order_i.Qty = item.Qty;
                        //    order_i.CreatedDate = DateTime.Now;
                        //    order_i.CreatedBy = UserId.ToString();
                        //    order_i.UserId = (Guid)UserId;
                        //    context.Order_Items.Add(order_i);
                        //    await context.SaveChangesAsync();
                        //}

                        //ShippingTracking ship = new ShippingTracking();
                        //ship.Id = Guid.NewGuid();
                        //ship.UserId = (Guid)UserId;
                        //ship.OrderId = order.Id;
                        //ship.Status = "1";
                        //ship.OrderDate = DateTime.Now;
                        //ship.CreateDate = DateTime.Now;

                        //context.ShippingTracking.Add(ship);
                        //await context.SaveChangesAsync();
                        //await transaction.CommitAsync();
                        //res.Status = Status.Success;
                        //res.OrderId = order.Id;
                    }
                    catch (Exception ex)
                    {
                        res.Status = Status.Failure;
                        await transaction.RollbackAsync();
                    }
                   
                }
            }
            else
            {
                res.Status = Status.Failure;
            }
            return res;
        }


        public async Task<ResponseModel> SaveCartOrderPayment1(Guid? UserId)
        {
            ResponseModel res = new ResponseModel();
            List<AddToCartViewModel> model = new List<AddToCartViewModel>();
            var shippingfees = GetShippingfee(UserId);
            decimal ShipC = 0;
            var ShipId = "";
            if (shippingfees == null)
            {
                ShipC = 0;
                ShipId = "";
            }
            else
            {
                ShipC = shippingfees.ShippingCharge;
                ShipId = shippingfees.Id;
            }
            model = (from a in context.AddToCart
                     where a.CustId == UserId
                     join b in context.ProductMasters on a.ProductId equals b.Id
                     select new AddToCartViewModel
                     {
                         ProductName = b.ProductName,
                         Description = b.Description,
                         Id = a.Id,
                         Qty = a.Qty,
                         TotalPrice = a.TotalPrice,
                         Photo = b.Photo,
                         ProductId = b.Id,
                         Price = b.Price
                     }).ToList();
            if (model != null)
            {
                var ammount = model.Sum(i => Convert.ToDecimal(i.TotalPrice)) + ShipC;
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        Payment pay = new Payment();
                        pay.Id = Guid.NewGuid();
                        pay.UserId = (Guid)UserId;
                        pay.PaymentType = "Cash";
                        pay.Amount = ammount;
                        pay.CreatedDate = DateTime.Now;
                        pay.CreatedBy = UserId.ToString();
                        pay.Status = "Failler";
                        context.Payment.Add(pay);
                        await context.SaveChangesAsync();

                        Order order = new Order();
                        order.Id = Guid.NewGuid();
                        order.Order_Id = Guid.NewGuid().ToString().Substring(0, 12).Replace("-", "").ToUpper();
                        order.UserId = (Guid)UserId;
                        order.PaymentId = pay.Id;
                        order.CreatedBy = UserId.ToString();
                        order.TotalAmount = ammount;
                        order.CreatedDate = DateTime.Now;
                        order.ShippingId = new Guid(ShipId);
                        context.Order.Add(order);
                        await context.SaveChangesAsync();

                        foreach (var item in model)
                        {
                            Order_Items order_i = new Order_Items();
                            order_i.Id = Guid.NewGuid();
                            order_i.OrderId = order.Id;
                            order_i.ProductId = item.ProductId;
                            order_i.Amount = Convert.ToDecimal(item.Price) * item.Qty;
                            order_i.Qty = item.Qty;
                            order_i.CreatedDate = DateTime.Now;
                            order_i.CreatedBy = UserId.ToString();
                            order_i.UserId = (Guid)UserId;
                            context.Order_Items.Add(order_i);
                            await context.SaveChangesAsync();
                        }

                        ShippingTracking ship = new ShippingTracking();
                        ship.Id = Guid.NewGuid();
                        ship.UserId = (Guid)UserId;
                        ship.OrderId = order.Id;
                        ship.Status = "1";
                        ship.OrderDate = DateTime.Now;
                        ship.CreateDate = DateTime.Now;

                        context.ShippingTracking.Add(ship);
                        await context.SaveChangesAsync();
                        await transaction.CommitAsync();
                        res.Status = Status.Success;
                        res.OrderId = order.Id;
                    }
                    catch (Exception ex)
                    {
                        res.Status = Status.Failure;
                        await transaction.RollbackAsync();
                    }
                    IEnumerable<AddToCart> reco = context.AddToCart.Where(x => x.CustId == UserId);
                    context.AddToCart.RemoveRange(reco);
                    context.SaveChanges();
                }
            }
            else
            {
                res.Status = Status.Failure;
            }
            return res;
        }


        public (int TotalCount, int FilteredCount, dynamic Categorys) GetOrders(int skip, int take, string sortColumn, string sortColumnDir, string searchValue, string UserId)
        {
            List<OrderHistory> Orders = new List<OrderHistory>();
            Orders = (from a in context.Order.Where(x => x.UserId == new Guid(UserId))
                      join b in context.CustomerInfo on a.UserId equals b.Id
                      join c in context.Payment on a.PaymentId equals c.Id
                      select new OrderHistory
                      {
                          Id = a.Id.ToString(),
                          UserName = b.Name,
                          Paymentstatus = c.Status,
                          OrderDate = a.CreatedDate,
                          TotalAmount = a.TotalAmount
                      }).ToList();

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            {
                switch (sortColumn)
                {
                    case "UserName":
                        Orders = sortColumnDir == "asc" ? Orders.OrderBy(x => x.UserName).ToList() : Orders.OrderByDescending(x => x.UserName).ToList();
                        break;

                }
                switch (sortColumn)
                {
                    case "OrderDate":
                        Orders = sortColumnDir == "asc" ? Orders.OrderBy(x => x.OrderDate).ToList() : Orders.OrderByDescending(x => x.OrderDate).ToList();
                        break;

                }

            }
            if (!string.IsNullOrEmpty(searchValue))
            {
                Orders = Orders.Where(m => m.UserName.Contains(searchValue)).ToList();
            }
            if (!string.IsNullOrEmpty(searchValue))
            {
                Orders = Orders.Where(m => m.Id.Contains(searchValue)).ToList();
            }
            int recordsTotal = Orders.Count();
            var data = Orders.Select(x => new
            {
                Id = x.Id,
                UserName = x.UserName,
                Paymentstatus = x.Paymentstatus,
                OrderDate = x.OrderDate.ToString("dd-MM-yyyy"),
                TotalAmount = x.TotalAmount,
            }).Skip(skip).Take(take).ToList();
            return (recordsTotal, recordsTotal, data);
        }

        public async Task<OrderHistory> OrderHistoryDetails(string Id)
        {
            OrderHistory details = new OrderHistory();
            try
            {
                details = (from a in context.Order.Where(x => x.Id == new Guid(Id))
                           join b in context.CustomerInfo on a.UserId equals b.Id
                           join c in context.Payment on a.PaymentId equals c.Id
                           join d in context.ShippingTracking on a.Id equals d.OrderId
                           select new OrderHistory
                           {
                               Id = a.Id.ToString(),
                               UserName = b.Name,
                               CustId = b.Id.ToString(),
                               Paymentstatus = c.Status,
                               OrderDate = a.CreatedDate,
                               TotalAmount = a.TotalAmount,
                               Shippingstatus = d.Status,
                               ShippedDate = d.ShippedDate,
                               OutForDeliveryDate = d.OutForDeliveryDate,
                               Delivered = d.Delivered,
                               CourierCompany=d.CourierCompnyName,
                               TrackingNumber=d.TrackingNumber
                           }).FirstOrDefault();
                details.OrderDetails = (from or in context.Order_Items.Where(x => x.OrderId == new Guid(Id))
                                        join p in context.ProductMasters on or.ProductId equals p.Id
                                        select new OrderDetails
                                        {
                                            ProductId = or.ProductId.ToString(),
                                            ProductName = p.ProductName,
                                            Qty = or.Qty.ToString(),
                                            Price = or.Amount.ToString(),
                                            Photo = p.Photo.ToString()
                                        }).ToList();
                return details;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<OrderHistory> OrderHistoryList(string UserId)
        {
            List<OrderHistory> details = new List<OrderHistory>();
            try
            {

                details = (from a in context.Order.Where(x => x.UserId == new Guid(UserId))
                           join b in context.CustomerInfo on a.UserId equals b.Id
                           join c in context.Payment on a.PaymentId equals c.Id
                           join d in context.Order_Items on a.Id equals d.OrderId
                           join e in context.ShippingTracking on a.Id equals e.OrderId
                           select new OrderHistory
                           {
                               Id = a.Id.ToString(),
                               Order_Id = a.Id.ToString(),
                               UserName = b.Name,
                               Paymentstatus = c.Status,
                               OrderDate = a.CreatedDate,
                               TotalAmount = a.TotalAmount,
                               Shippingstatus = e.Status,
                               Qty = context.Order_Items.Where(y => y.OrderId == a.Id).Sum(y => y.Qty).ToString()
                           }).OrderByDescending(x => x.OrderDate).ToList();


                return details;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<OrderHistory> OrderHistorySearchList(string UserId, string UserName)
        {
            List<OrderHistory> details = new List<OrderHistory>();
            try
            {
                var details1 = (from a in context.Order.Where(x => x.UserId == new Guid(UserId))
                                join b in context.CustomerInfo on a.UserId equals b.Id
                                join c in context.Payment on a.PaymentId equals c.Id
                                join d in context.Order_Items on a.Id equals d.OrderId
                                join e in context.ShippingTracking on a.Id equals e.OrderId
                                select new OrderHistory
                                {
                                    Id = a.Id.ToString(),
                                    Order_Id = a.Id.ToString(),
                                    UserName = b.Name,
                                    Paymentstatus = c.Status,
                                    OrderDate = a.CreatedDate,
                                    TotalAmount = a.TotalAmount,
                                    Shippingstatus = e.Status,
                                    Qty = context.Order_Items.Where(y => y.OrderId == a.Id).Sum(y => y.Qty).ToString()
                                }).ToList();
                details = details1.Where(m => m.UserName.Contains(UserName)).ToList();
                return details;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}