
using Microsoft.EntityFrameworkCore;
using NLog;
using smartTechAuthenticator.Migrations;
using smartTechAuthenticator.Models;
using smartTechAuthenticator.ViewModels;
using Stripe;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace smartTechAuthenticator.Services.Customers
{
    public class CustomersService : ICustomersService
    {

        private readonly ApplicationDbContext context;
        public readonly Logger logger;

        public CustomersService(ApplicationDbContext _context, Logger _logger)
        {
            context = _context;
            logger = _logger;

        }
        public (int TotalCount, int FilteredCount, dynamic Customers) GetCustomers(int skip, int take, string sortColumn, string sortColumnDir, string searchValue)
        {
            var customers = context.CustomerInfo.Where(x => x.IsActive && x.UserType != UserType.Admin).Select(x => new
            {
                Id = x.Id,
                Name = x.Name,
                Email = x.Email,
                DateCreated = x.DateCreated,
                CustomerAddressId = x.CustomerAddressId,
                MobileNo = x.MobileNo,
                Nric = x.Nric,
                IsActive = x.IsActive
            });
            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            {
                switch (sortColumn)
                {
                    case "Name":
                        customers = sortColumnDir == "asc" ? customers.OrderBy(x => x.Email) : customers.OrderByDescending(x => x.Name);
                        break;
                    case "Email":
                        customers = sortColumnDir == "asc" ? customers.OrderBy(x => x.Email) : customers.OrderByDescending(x => x.Email);
                        break;
                    case "MobileNo":
                        customers = sortColumnDir == "asc" ? customers.OrderBy(x => x.MobileNo) : customers.OrderByDescending(x => x.MobileNo);
                        break;
                    case "DateCreated":
                        customers = sortColumnDir == "asc" ? customers.OrderBy(x => x.DateCreated) : customers.OrderByDescending(x => x.DateCreated);
                        break;
                }

            }
            if (!string.IsNullOrEmpty(searchValue))
            {
                customers = customers.Where(m => m.Name.Contains(searchValue) || m.Email.Contains(searchValue) || m.MobileNo.Contains(searchValue) || m.Nric.Contains(searchValue));
            }
            int recordsTotal = customers.Count();
            var data = customers.Select(x => new
            {
                Id = x.Id,
                Name = x.Name,
                Email = x.Email,
                DateCreated = x.DateCreated.ToString("dd-MM-yyyy"),
                CustomerAddressId = x.CustomerAddressId,
                MobileNo = x.MobileNo,
                Nric = x.Nric,
                IsActive = x.IsActive
            }).Skip(skip).Take(take).ToList();
            return (recordsTotal, recordsTotal, data);
        }

        public (int TotalCount, int FilteredCount, dynamic Customers) GetCustomersTestHistory(int skip, int take, string sortColumn, string sortColumnDir, string searchValue, string CustomerId)
        {
           var customers = (from trackForm in context.TrackingForms.Where(x => x.CustId == new Guid(CustomerId))
                            join product in context.ProductMasters on trackForm.TestkitId equals product.Id
                     select new TrackingFormsViewModel
                     {
                         ProductName = product.ProductName,
                         Id = trackForm.Id,
                         AuthentiCode = trackForm.QrCode,
                         CustId = trackForm.CustId,
                         Date = trackForm.Date,
                         Time = trackForm.Time,
                         Place = trackForm.Place,
                         TestResults = trackForm.TestResults,
                         AntigenType = trackForm.AntigenType,
                         FileUrl = trackForm.FileUrl,
                         CertificateImage = trackForm.CertificateImage,
                         IsVerified = trackForm.IsVerified
                     });
            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            {
                switch (sortColumn)
                {
                    case "ProductName":
                        customers = sortColumnDir == "asc" ? customers.OrderBy(x => x.ProductName) : customers.OrderByDescending(x => x.ProductName);
                        break;
                    case "TestResults":
                        customers = sortColumnDir == "asc" ? customers.OrderBy(x => x.TestResults) : customers.OrderByDescending(x => x.TestResults);
                        break;
                    case "AuthentiCode":
                        customers = sortColumnDir == "asc" ? customers.OrderBy(x => x.AuthentiCode) : customers.OrderByDescending(x => x.AuthentiCode);
                        break;
                    case "Date":
                        customers = sortColumnDir == "asc" ? customers.OrderBy(x => x.Date) : customers.OrderByDescending(x => x.Date);
                        break;
                }

            }
            if (!string.IsNullOrEmpty(searchValue))
            {
                customers = customers.Where(m => m.Name.Contains(searchValue) || m.Email.Contains(searchValue) || m.MobileNo.Contains(searchValue) || m.Nric.Contains(searchValue));
            }
            int recordsTotal = customers.Count();
            var data = customers.Select(x => new
            {
                Id = x.Id,
                ProductName = x.ProductName,
                AntigenType = x.AntigenType,
                Date = x.Date.ToString(),
                AuthentiCode = x.AuthentiCode,
                TestResults = x.TestResults,
                
            }).Skip(skip).Take(take).ToList();
            return (recordsTotal, recordsTotal, data);
        }
        public (int TotalCount, int FilteredCount, dynamic Customers) GetCustomersIPHistory(int skip, int take, string sortColumn, string sortColumnDir, string searchValue, string CustomerId)
        {
            var IP_data = (from Activity in context.LoginActivity.Where(x => x.CustomerUserId == new Guid(CustomerId))
                             select new LoginActivity
                             {
                                 UserName= Activity.UserName,
                                 IP_Address= Activity.IP_Address,
                                 CustomerUserId= Activity.CustomerUserId,
                                 EventDateTime = Activity.EventDateTime,
                             });
            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            {
                switch (sortColumn)
                {
                    case "UserName":
                        IP_data = sortColumnDir == "asc" ? IP_data.OrderBy(x => x.UserName) : IP_data.OrderByDescending(x => x.UserName);
                        break;
                    case "IP_Address":
                        IP_data = sortColumnDir == "asc" ? IP_data.OrderBy(x => x.IP_Address) : IP_data.OrderByDescending(x => x.IP_Address);
                        break;
                    case "CustomerUserId":
                        IP_data = sortColumnDir == "asc" ? IP_data.OrderBy(x => x.CustomerUserId) : IP_data.OrderByDescending(x => x.CustomerUserId);
                        break;
                    case "EventDateTime":
                        IP_data = sortColumnDir == "asc" ? IP_data.OrderBy(x => x.EventDateTime) : IP_data.OrderByDescending(x => x.EventDateTime);
                        break;
                }

            }
            if (!string.IsNullOrEmpty(searchValue))
            {
                IP_data = IP_data.Where(m => m.UserName.Contains(searchValue) || m.IP_Address.Contains(searchValue) || m.CustomerUserId.ToString().Contains(searchValue) || m.EventDateTime.ToString().Contains(searchValue));
            }
            int recordsTotal = IP_data.Count();
            var data = IP_data.Select(x => new
            {
                Id = x.Id,
                CustomerUserId=x.CustomerUserId,
                UserName = x.UserName,
                IP_Address = x.IP_Address,
                EventDateTime = x.EventDateTime.ToString(),
                
            }).Skip(skip).Take(take).ToList();
            return (recordsTotal, recordsTotal, data);
        }


        public (int TotalCount, int FilteredCount, dynamic Customers) GetCustomersOrderHistory(int skip, int take, string sortColumn, string sortColumnDir, string searchValue, string CustomerId)
            {
            var OrderHistory = (from a in context.Order.Where(x=>x.UserId== new Guid(CustomerId))
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
                           });
            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            {
                switch (sortColumn)
                {
                    case "UserName":
                        OrderHistory = sortColumnDir == "asc" ? OrderHistory.OrderBy(x => x.UserName) : OrderHistory.OrderByDescending(x => x.UserName);
                        break;
                    case "OrderDate":
                        OrderHistory = sortColumnDir == "asc" ? OrderHistory.OrderBy(x => x.OrderDate) : OrderHistory.OrderByDescending(x => x.OrderDate);
                        break;
                    case "Order_Id":
                        OrderHistory = sortColumnDir == "asc" ? OrderHistory.OrderBy(x => x.Order_Id) : OrderHistory.OrderByDescending(x => x.Order_Id);
                        break;
                    case "TotalAmount":
                        OrderHistory = sortColumnDir == "asc" ? OrderHistory.OrderBy(x => x.TotalAmount) : OrderHistory.OrderByDescending(x => x.TotalAmount);
                        break;
                }

            }
            if (!string.IsNullOrEmpty(searchValue))
            {
                OrderHistory = OrderHistory.Where(m => m.UserName.Contains(searchValue) || m.OrderDate.ToString().Contains(searchValue) || m.Order_Id.Contains(searchValue) || m.TotalAmount.ToString().Contains(searchValue));
            }
            int recordsTotal = OrderHistory.Count();
            var data = OrderHistory.Select(x => new
            {
                Id = x.Id,
                UserName = x.UserName,
                Order_Id = x.Order_Id,
                OrderDate = x.OrderDate.ToString(),
                Shippingstatus = x.Shippingstatus,
                Qty = x.Qty,
                TotalAmount=x.TotalAmount,
            }).Skip(skip).Take(take).ToList();
            return (recordsTotal, recordsTotal, data);
        }



        public async Task<ResponseModel> UpdateCustomerDetails(CustomerInfoViewModel customer)
        {
            ResponseModel model = new ResponseModel();
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var customerInfo = context.CustomerInfo.FirstOrDefault(x => x.Id == customer.Id);
                    if (customerInfo != null)
                    {
                        customerInfo.Name = customer.Name;
                        customerInfo.MobileNo = customer.MobileNo;
                        customerInfo.Nric = customer.Nric;
                        context.CustomerInfo.Update(customerInfo);
                        var customerAddressInfo = context.CustomerAddressMasters.FirstOrDefault(x => x.Id == customerInfo.CustomerAddressId);
                        if (customerAddressInfo != null)
                        {
                            customerAddressInfo.Address = customer.Address;
                            customerAddressInfo.StateId = customer.StateId;
                            customerAddressInfo.DistricttId = customer.DistricttId;
                            customerAddressInfo.PostCode = customer.PostCode;
                            context.CustomerAddressMasters.Update(customerAddressInfo);
                        }
                    }
                    await context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    model.Status = Status.Success;
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

        public async Task<CustomerInfoViewModel> GetCustomers(Guid CustomerId)
        {
            var customerInfo = context.CustomerInfo.Join(context.CustomerAddressMasters,
                cust => cust.CustomerAddressId,
                adddress => adddress.Id,
                (cust, adddress) => new CustomerInfoViewModel
                {
                    Id = cust.Id,
                    Name = cust.Name,
                    Email = cust.Email,
                    MobileNo = cust.MobileNo,
                    Nric = cust.Nric,
                    StateId = adddress.StateId,
                    DistricttId = adddress.DistricttId,
                    PostCode = adddress.PostCode,
                    Address = adddress.Address,
                    UserPass = cust.UserPass
                }).FirstOrDefault(x => x.Id == CustomerId);
            return customerInfo;
        }

        public async Task<ResponseModel> DeleteCustomerDetails(CustomerInfoViewModel customer)
        {
            ResponseModel model = new ResponseModel();
            var customerInfo = context.CustomerInfo.FirstOrDefault(x => x.Id == customer.Id);
            if (customerInfo != null)
            {
                customerInfo.IsActive = false;
                await context.SaveChangesAsync();
                model.Status = Status.Success;
            }
            return model;
        }

        public List<ManageQrViewModel> GetQrData()
        {
            ManageQrViewModel model = new ManageQrViewModel();
            model.ManageQrList = (from a in context.QrCodeMasters
                                  where a.IsActive == true
                                  select new ManageQrViewModel
                                  {
                                      Id = a.Id,
                                      QrCode = a.QrCode,
                                      QrImageUrl = a.QrImageUrl,
                                      ProductId = context.ProductMasters.Where(m => m.QrId == a.Id).FirstOrDefault().Id
                                  }).Take(100).ToList();

            return model.ManageQrList;
        }
        public ResponseModel GenrateQrCode(ManageQrViewModel data)
        {
            ResponseModel model = new ResponseModel();
            try
            {
                var qrdata = context.QrCodeMasters.FirstOrDefault(x => x.QrCode == data.QrCode && x.IsActive == true);
                if (qrdata == null)
                {
                    //QrCodeMaster qr = new QrCodeMaster();
                    //qr.QrImageUrl = data.QrImageUrl;
                    //qr.CategoryId = data.CategoryId;
                    //qr.ProductId = data.ProductId;
                    //qr.CreatedDate = DateTime.Now;
                    //qr.IsActive = true;
                    //qr.QrCode = data.QrCode;
                    //context.Add(qr);
                    //var A = context.SaveChanges();
                }
                model.Status = Status.Success;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                model.Status = Status.Failure;
            }

            return model;
        }

        public ResponseModel CheckGenrateQrCode(string Qrcode)
        {
            ResponseModel model = new ResponseModel();
            try
            {
                var qrdata = context.QrCodeMasters.FirstOrDefault(x => x.QrCode == Qrcode && x.IsActive == true);
                if (qrdata == null)
                {
                    model.Status = Status.Success;
                }
                else
                {
                    model.Status = Status.Failure;
                }

            }
            catch (Exception ex)
            {
                logger.Error(ex);
                model.Status = Status.Failure;
            }

            return model;
        }

        public async Task<ResponseModel> DeleteQrData(ManageQrViewModel mo)
        {
            ResponseModel model = new ResponseModel();
            var customerInfo = context.QrCodeMasters.FirstOrDefault(x => x.Id == mo.Id);
            if (customerInfo != null)
            {
                customerInfo.IsActive = false;
                await context.SaveChangesAsync();
                model.Status = Status.Success;
            }
            return model;
        }


        public List<ProductViewModel> GetAllProductData()
        {
            List<ProductViewModel> model = new List<ProductViewModel>();
            model = (from a in context.ProductMasters
                     where a.IsActive == true
                     select new ProductViewModel
                     {
                         Id = a.Id,
                         ProductName = a.ProductName
                     }).ToList();

            return model;
        }

        public List<SelectListItem> GetAllProductData1()
        {
            return context.ProductMasters.Select(x => new SelectListItem() { Text = x.ProductName, Value = x.Id.ToString() }).ToList();
        }

        public async Task<ResponseModel> UpdateProductDetails(ProductViewModel prod)
        {
            ResponseModel model = new ResponseModel();
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var Info = context.ProductMasters.FirstOrDefault(x => x.Id == prod.ProductId);
                    if (Info != null)
                    {

                        Info.QrId = prod.QrId;
                        context.ProductMasters.Update(Info);

                    }
                    await context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    model.Status = Status.Success;
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

        public List<CustomerInfoViewModel> GetProductList()
        {
            List<CustomerInfoViewModel> model = new List<CustomerInfoViewModel>();
            model = (from a in context.CustomerInfo
                     select new CustomerInfoViewModel
                     {
                         Id = a.Id,
                         Name = a.Name,
                         Location = a.Location,
                         Website = a.Website,
                         Email = a.Email,
                         UserPass = a.UserPass,
                         MobileNo = a.MobileNo
                     }).ToList();

            return model;
        }

        public (int TotalCount, int FilteredCount, dynamic Customers) GetProductAllList(int skip, int take, string sortColumn, string sortColumnDir, string searchValue)
        {
            var customers = context.CustomerInfo.Where(a=> a.UserType == (UserType)2).Select(a => new 
            {
                Id = a.Id,
               Name = a.Name,
                Location = a.Location,
                Website = a.Website,
                Email = a.Email,
                UserPass = a.UserPass,
                MobileNo = a.MobileNo
                
                
            });
            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            {
                switch (sortColumn)
                {
                    case "Name":
                        customers = sortColumnDir == "asc" ? customers.OrderBy(x => x.Name) : customers.OrderByDescending(x => x.Name);
                        break;
                    case "Location":
                        customers = sortColumnDir == "asc" ? customers.OrderBy(x => x.Location) : customers.OrderByDescending(x => x.Location);
                        break;
                   
                }

            }
            if (!string.IsNullOrEmpty(searchValue))
            {
                customers = customers.Where(m => m.Name.Contains(searchValue) || m.Location.Contains(searchValue));
            }
            int recordsTotal = customers.Count();
            var data = customers.Select(a => new
            {
               Id = a.Id,
           Name = a.Name,
                Location = a.Location,
                Website = a.Website,
                Email = a.Email,
                UserPass = a.UserPass,
                MobileNo = a.MobileNo,
            }).Skip(skip).Take(take).ToList();
            return (recordsTotal, recordsTotal, data);
        }



        public async Task<ResponseModel> CreateProduct(CustomerInfoViewModel prod)
        {
            ResponseModel model = new ResponseModel();


            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    CustomerInfo pm = new CustomerInfo();
                    pm.Id = Guid.NewGuid();
                    pm.Name = prod.Name;
                    pm.Location = prod.Location;
                    pm.Website = prod.Website;
                    pm.Email = prod.Email;
                    pm.MobileNo = prod.MobileNo;
                    pm.UserPass = prod.UserPass;
                    pm.UserType = (UserType)2;
                    pm.DateCreated = DateTime.Now;
                    pm.UpdatedDate = DateTime.Now;
                    pm.Nric = "";
                    pm.DeviceId = "";
                    pm.Address1 = "";
                    pm.Address2 = "";
                    pm.Address3 = "";
                    pm.CompanyId = 0;
                    pm.CustomerAddressId = 0;
                    pm.IsActive = true;
                    pm.FName = "";
                    pm.LName = "";
                    pm.AddedBy = "CD127FAB-7B41-4D54-8D90-9D8CF22FCDE0";


                    context.Add(pm);
                   
                    await context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    model.Status = Status.Success;
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
        public (int TotalCount, int FilteredCount, dynamic Customers) GetQrData(int skip, int take, string sortColumn, string sortColumnDir, string searchValue)
        {
            var customers = context.QrCodeMasters.Where(x => x.IsActive && x.IsExpire == false).Select(x => new
            {
                Id = x.Id,
                QrCode = x.QrCode,
                ProductName = context.ProductMasters.Where(m => m.Id == x.ProductId).FirstOrDefault().ProductName,
                IsActive = x.IsActive,
            });
            //if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            //{
            //    switch (sortColumn)
            //    {
            //        case "QrCode":
            //            customers = sortColumnDir == "asc" ? customers.OrderBy(x => x.QrCode) : customers.OrderByDescending(x => x.QrCode);
            //            break;
            //        case "QrImageUrl":
            //            customers = sortColumnDir == "asc" ? customers.OrderBy(x => x.QrImageUrl) : customers.OrderByDescending(x => x.QrImageUrl);
            //            break;           
            //    }

            //}
            if (!string.IsNullOrEmpty(searchValue))
            {
                customers = customers.Where(m => m.QrCode.Contains(searchValue));
            }
            int recordsTotal = customers.Count();
            var data = customers.Select(x => new
            {
                Id = x.Id,
                QrCode = x.QrCode,
                ProductName = x.ProductName,
                IsActive = x.IsActive,
            }).Skip(skip).Take(take).ToList();
            return (recordsTotal, recordsTotal, data);
        }
        public List<SelectListItem> GetProductCategory()
        {
            return context.ProductCategories.Select(x => new SelectListItem() { Text = x.CategoryName, Value = x.Id.ToString() }).ToList();
        }

        public async Task<ResponseModel> DeleteProductDetails(CustomerInfoViewModel model1)
        {
            ResponseModel model = new ResponseModel();
            var Info = context.CustomerInfo.FirstOrDefault(x => x.Id == model1.Id);
            if (Info != null)
            {
              
              context.CustomerInfo.Remove(Info);
                await context.SaveChangesAsync();
                model.Status = Status.Success;
            }
            return model;
        }

        public async Task<CustomerInfoViewModel> GetProduct(Guid Id)
        {
            CustomerInfoViewModel data = new CustomerInfoViewModel();
                data = (from a in context.CustomerInfo
                        where a.Id == Id
                               
                                select new CustomerInfoViewModel
                                {
                                    Id = a.Id,
                                  Name = a.Name,
                                    Location = a.Location,
                                    Website = a.Website,
                                    Email = a.Email,
                                    UserPass = a.UserPass,
                                    MobileNo = a.MobileNo
                                }).FirstOrDefault();
         
            
            
            return data;
        }

        public async Task<CustomerInfoViewModel> GetAccountant(Guid Id)
        {
            CustomerInfoViewModel data = new CustomerInfoViewModel();
            data = (from a in context.CustomerInfo
                    where a.Id == Id

                    select new CustomerInfoViewModel
                    {
                        Id = a.Id,
                        Name = a.Name,
                        Location = a.Location,
                        Website = a.Website,
                        Email = a.Email,
                        UserPass = a.UserPass,
                        MobileNo = a.MobileNo
                    }).FirstOrDefault();



            return data;
        }

        public async Task<ResponseModel> UpdateProductData(CustomerInfoViewModel prod)
        {
            ResponseModel model = new ResponseModel();
            //using (var transaction = context.Database.BeginTransaction())
            //{
                try
                {
                    var Info = context.CustomerInfo.FirstOrDefault(x => x.Id == prod.Id);
                if (Info != null)
                {

                    Info.Name = prod.Name;
                    Info.Location = prod.Location;
                    Info.Website = prod.Website;
                    Info.Email = prod.Email;
                    Info.MobileNo = prod.MobileNo;
                    Info.UserPass = prod.UserPass;

                    context.CustomerInfo.Update(Info);
                    await context.SaveChangesAsync();

                   
                }
                 //   await transaction.CommitAsync();
                    model.Status = Status.Success;
                }
                catch (Exception ex)
                {
                    logger.Error(ex);
                    model.Status = Status.Failure;
                //    await transaction.RollbackAsync();
                }
           // }
            return model;
        }
        public (int TotalCount, int FilteredCount, dynamic Categorys) GetCategorys(int skip, int take, string sortColumn, string sortColumnDir, string searchValue)
        {
            var category = context.CustomerInfo.Where(a => a.UserType == (UserType)3).Select(x => new
            {
                Id=x.Id,
                Name=x.Name,
                Location=x.Location,
                Website=x.Website,
                Email=x.Email,
                MobileNo=x.MobileNo,
                UserPass=x.UserPass,

            });
            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            {
                switch (sortColumn)
                {
                    case "Name":
                        category = sortColumnDir == "asc" ? category.OrderBy(x => x.Name) : category.OrderByDescending(x => x.Name);
                        break;

                }

            }
            if (!string.IsNullOrEmpty(searchValue))
            {
                category = category.Where(m => m.Name.Contains(searchValue));
            }
            int recordsTotal = category.Count();
            var data = category.Select(x => new
            {
                Id = x.Id,
                Name = x.Name,
                Location = x.Location,
                Website = x.Website,
                Email = x.Email,
                MobileNo = x.MobileNo,
                UserPass = x.UserPass,
            }).Skip(skip).Take(take).ToList();
            return (recordsTotal, recordsTotal, data);
        }

        public (int TotalCount, int FilteredCount, dynamic Categorys) GetAccountants(int skip, int take, string sortColumn, string sortColumnDir, string searchValue)
        {
            var category = context.CustomerInfo.Where(a => a.UserType == (UserType)1).Select(x => new
            {
                Id = x.Id,
                Name = x.Name,
                Location = x.Location,
                Website = x.Website,
                Email = x.Email,
                MobileNo = x.MobileNo,
                UserPass = x.UserPass,

            });
            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            {
                switch (sortColumn)
                {
                    case "Name":
                        category = sortColumnDir == "asc" ? category.OrderBy(x => x.Name) : category.OrderByDescending(x => x.Name);
                        break;

                }

            }
            if (!string.IsNullOrEmpty(searchValue))
            {
                category = category.Where(m => m.Name.Contains(searchValue));
            }
            int recordsTotal = category.Count();
            var data = category.Select(x => new
            {
                Id = x.Id,
                Name = x.Name,
                Location = x.Location,
                Website = x.Website,
                Email = x.Email,
                MobileNo = x.MobileNo,
                UserPass = x.UserPass,
            }).Skip(skip).Take(take).ToList();
            return (recordsTotal, recordsTotal, data);
        }

        public async Task<ResponseModel> CreateCategory(CustomerInfoViewModel prod)
        {
            ResponseModel model = new ResponseModel();
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    CustomerInfo pm = new CustomerInfo();
                    pm.Id = Guid.NewGuid();
                    pm.Name = prod.Name;
                    pm.Location = prod.Location;
                    pm.Website = prod.Website;
                    pm.MobileNo = prod.MobileNo;
                    pm.Email = prod.Email;
                    pm.UserPass = prod.UserPass;

                    pm.UserType = (UserType)3;
                    pm.DateCreated = DateTime.Now;
                    pm.UpdatedDate = DateTime.Now;
                    pm.Nric = "";
                    pm.DeviceId = "";
                    pm.Address1 = "";
                    pm.Address2 = "";
                    pm.Address3 = "";
                    pm.CompanyId = 0;
                    pm.CustomerAddressId = 0;
                    pm.IsActive = true;
                    pm.FName = "";
                    pm.LName = "";
                    pm.AddedBy = "CD127FAB-7B41-4D54-8D90-9D8CF22FCDE0";
                    context.Add(pm);
                    await context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    model.Status = Status.Success;
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

        public async Task<ResponseModel> CreateAccountant(CustomerInfoViewModel prod)
        {
            ResponseModel model = new ResponseModel();
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    CustomerInfo pm = new CustomerInfo();
                    pm.Id = Guid.NewGuid();
                    pm.Name = prod.Name;
                    pm.Location = prod.Location;
                    pm.Website = prod.Website;
                    pm.MobileNo = prod.MobileNo;
                    pm.Email = prod.Email;
                    pm.UserPass = prod.UserPass;

                    pm.UserType = (UserType)1;
                    pm.DateCreated = DateTime.Now;
                    pm.UpdatedDate = DateTime.Now;
                    pm.Nric = "";
                    pm.DeviceId = "";
                    pm.Address1 = "";
                    pm.Address2 = "";
                    pm.Address3 = "";
                    pm.CompanyId = 0;
                    pm.CustomerAddressId = 0;
                    pm.IsActive = true;
                    pm.FName = "";
                    pm.LName = "";
                    pm.AddedBy = "CD127FAB-7B41-4D54-8D90-9D8CF22FCDE0";
                    context.Add(pm);
                    await context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    model.Status = Status.Success;
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


        public async Task<CustomerInfoViewModel> GetCategoryDetail(string Id)
        {
            var CategoryInfo = (from a in context.CustomerInfo
                                where a.Id == new Guid(Id)
                                select new CustomerInfoViewModel
                                {
                                  Name = a.Name,
                                    Location = a.Location,
                                    Website = a.Website,
                                    MobileNo = a.MobileNo,
                                Email = a.Email,
           UserPass = a.UserPass,
        }).FirstOrDefault();
            return CategoryInfo;
        }

        public async Task<CustomerInfoViewModel> GetAccountantDetail(string Id)
        {
            var CategoryInfo = (from a in context.CustomerInfo
                                where a.Id == new Guid(Id)
                                select new CustomerInfoViewModel
                                {
                                    Name = a.Name,
                                    Location = a.Location,
                                    Website = a.Website,
                                    MobileNo = a.MobileNo,
                                    Email = a.Email,
                                    UserPass = a.UserPass,
                                }).FirstOrDefault();
            return CategoryInfo;
        }

        public async Task<ResponseModel> UpdateCategoryData(CustomerInfoViewModel prod)
        {
            ResponseModel model = new ResponseModel();
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var Info = context.CustomerInfo.FirstOrDefault(x => x.Id == prod.Id);
                    if (Info != null)
                    {

                        Info.Name = prod.Name;
                        Info.Location = prod.Location;
                        Info.Website = prod.Website;
                        Info.Email = prod.Email;
                        Info.MobileNo = prod.MobileNo;
                        Info.UserPass = prod.UserPass;

                        context.CustomerInfo.Update(Info);

                    }
                    await context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    model.Status = Status.Success;
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

        public async Task<ResponseModel> UpdateAccountantData(CustomerInfoViewModel prod)
        {
            ResponseModel model = new ResponseModel();
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var Info = context.CustomerInfo.FirstOrDefault(x => x.Id == prod.Id);
                    if (Info != null)
                    {

                        Info.Name = prod.Name;
                        Info.Location = prod.Location;
                        Info.Website = prod.Website;
                        Info.Email = prod.Email;
                        Info.MobileNo = prod.MobileNo;
                        Info.UserPass = prod.UserPass;

                        context.CustomerInfo.Update(Info);

                    }
                    await context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    model.Status = Status.Success;
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


        public async Task<ResponseModel> DeleteCategoryDetails(CustomerInfoViewModel model1)
        {
            ResponseModel model = new ResponseModel();
            try
            {
                var Info = context.CustomerInfo.FirstOrDefault(x => x.Id == model1.Id);
                if (Info != null)
                {
                    var data = context.CustomerInfo.Find(Info.Id);
                    if (data != null)
                    {
                        context.CustomerInfo.Remove(data);
                    }
                    await context.SaveChangesAsync();
                    model.Status = Status.Success;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                model.Status = Status.Failure;
            }
            return model;
        }

        public async Task<ResponseModel> DeleteAccountantDetails(CustomerInfoViewModel model1)
        {
            ResponseModel model = new ResponseModel();
            try
            {
                var Info = context.CustomerInfo.FirstOrDefault(x => x.Id == model1.Id);
                if (Info != null)
                {
                    var data = context.CustomerInfo.Find(Info.Id);
                    if (data != null)
                    {
                        context.CustomerInfo.Remove(data);
                    }
                    await context.SaveChangesAsync();
                    model.Status = Status.Success;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                model.Status = Status.Failure;
            }
            return model;
        }

        public List<SelectListItem> GetProductByCategory(ProductCategoryViewModel model)
        {
            return context.ProductMasters.Where(m => m.CategoryId == Convert.ToInt32(model.CategoryId)).Select(x => new SelectListItem() { Text = x.ProductName, Value = x.Id.ToString() }).ToList();
        }

        public List<ViewTestHistoryModel> ViewCustomerTestList(Guid CustomerId)
        {
            List<ViewTestHistoryModel> model = new List<ViewTestHistoryModel>();
            model = (from a in context.TrackingForms
                     where a.CustId == CustomerId
                     select new ViewTestHistoryModel
                     {
                         CustId = a.CustId,
                         Date = a.Date,
                         TestResults = a.TestResults,
                         AntigenType = a.AntigenType,
                         FileUrl = a.FileUrl
                     }).ToList();
            return model;
        }

        public int NoofCustomer()
        {
            var Customer = context.CustomerInfo.Where(x=>x.IsActive && x.UserType != UserType.Admin && x.UserType != UserType.SubAdmin).Count();
            return Customer;
        }
        public int NoofCodes()
        {
            var Codes = 0;//context.QrCodeMasters.Count();
            return Codes;
        }
        public int NoofTest()
        {
            var Test = context.TrackingForms.Count();
            return Test;
        }
        public int NoOfPendding()
        {
            var Pendding = context.TrackingForms.Where(x => x.IsVerified == 0).Count();
            return Pendding;
        }
        public int NoOfPostingNews()
        {
            var News = context.News.Count();
            return News;
        }
        public int NoOfUnSolvedTicket()
        {
            var unsolveticket = context.Ticket.Where(x => x.Status.ToLower() == "Unsolved").Count();
            return unsolveticket;
        }
       public int NoOfTotalTicket()
       {
            var Totalticket = context.Ticket.Where(x => x.Status.ToLower() == "Unsolved" || x.Status.ToLower()=="Solved").Count();
            return Totalticket;
        }

        public int NoOfOrderList()
        {
            var OrderList = context.Order.Count();
            return OrderList;
        }
        public int NoOfDailyOrderList()
        {
            var OrderList = context.Order.Where(x=>x.CreatedDate.Date==DateTime.Today).Count();
            return OrderList;
        }

        public ResponseModel UpdateQrCode(int Id, string QrImageName)
        {
            ResponseModel model = new ResponseModel();
            try
            {
                if (Id > 0)
                {
                    var qrdata = context.QrCodeMasters.FirstOrDefault(x => x.Id == Id);
                    if (qrdata != null)
                    {
                        qrdata.QrImageUrl = QrImageName;
                        context.SaveChanges();
                        model.Status = Status.Success;
                    }
                    else
                    {
                        model.Status = Status.Failure;
                    }
                }

            }
            catch (Exception ex)
            {
                logger.Error(ex);
                model.Status = Status.Failure;
            }

            return model;
        }
        public async Task<ResponseModel> ResetQRData(ManageQrViewModel Res)
        {
            ResponseModel model = new ResponseModel();
            var customerInfo = context.QrCodeMasters.FirstOrDefault(x => x.Id == Res.Id);
            if (customerInfo != null)
            {
                customerInfo.IsExpire = false;
                customerInfo.CustomerId = null;
                await context.SaveChangesAsync();
                model.Status = Status.Success;
            }
            return model;
        }
        public (int TotalCount, int FilteredCount, dynamic Customers) GetCheckQrData(int skip, int take, string sortColumn, string sortColumnDir, string searchValue)
        {
            var customers = context.QrCodeMasters.Where(x => x.IsActive && x.CustomerId != null && x.IsExpire == true).Select(x => new
            {
                Id = x.Id,
                QrCode = x.QrCode,
                // CustomerId = x.CustomerId,
                CreatedDate = x.CreatedDate,
                IsExpire = x.IsExpire,
                CustomerName = context.CustomerInfo.Where(y => y.Id.ToString() == x.CustomerId).FirstOrDefault().Name
            });
            if (!string.IsNullOrEmpty(searchValue))
            {
                customers = customers.Where(m => m.QrCode.Contains(searchValue));
            }
            int recordsTotal = customers.Count();

            var data = customers.Where(x => x.IsExpire == true).Select(x => new
            {
                Id = x.Id,
                QrCode = x.QrCode,
                // CustomerId = x.CustomerId,
                CreatedDate = x.CreatedDate.ToString("dd-MM-yyyy-hh:mm tt"),
                IsExpire = Convert.ToInt32(x.IsExpire),
                CustomerName = x.CustomerName
            }).Skip(skip).Take(take).ToList();
            return (recordsTotal, recordsTotal, data);
        }


        public (int TotalCount, int FilteredCount, dynamic Categorys) GetBannerCarousal(int skip, int take, string sortColumn, string sortColumnDir, string searchValue)
        {
            var BannerCarousels = context.BannerCarousel.Select(x => new
            {
                Id = x.Id,
                Title = x.Title,
                Photos = x.Photo,
                Description = x.Description
            });
            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            {
                switch (sortColumn)
                {
                    case "Title":
                        BannerCarousels = sortColumnDir == "asc" ? BannerCarousels.OrderBy(x => x.Title) : BannerCarousels.OrderByDescending(x => x.Title);
                        break;

                }

            }
            if (!string.IsNullOrEmpty(searchValue))
            {
                BannerCarousels = BannerCarousels.Where(m => m.Title.Contains(searchValue));
            }
            int recordsTotal = BannerCarousels.Count();
            var data = BannerCarousels.Select(x => new
            {
                Id = x.Id,
                Title = x.Title,
                Photos = x.Photos,
                Description = x.Description
            }).Skip(skip).Take(take).ToList();
            return (recordsTotal, recordsTotal, data);
        }

        public async Task<ResponseModel> CreateBannerCarousal(BannerCarouselViewModel bcvm)
        {
            ResponseModel model = new ResponseModel();
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    BannerCarousel pm = new BannerCarousel();
                    // pm.Id = Guid.NewGuid();
                    pm.Title = bcvm.Title;
                    pm.Description = bcvm.Description;
                    pm.Photo = bcvm.Photo;
                    context.Add(pm);
                    await context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    model.Status = Status.Success;
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

        public async Task<ResponseModel> DeleteBannerCarousalDetails(BannerCarouselViewModel model1)
        {
            ResponseModel model = new ResponseModel();
            try
            {
                var Info = context.BannerCarousel.FirstOrDefault(x => x.Id == new Guid(model1.Id));
                if (Info != null)
                {
                    var data = context.BannerCarousel.Find(Info.Id);
                    if (data != null)
                    {
                        context.BannerCarousel.Remove(data);
                    }
                    await context.SaveChangesAsync();
                    model.Status = Status.Success;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                model.Status = Status.Failure;
            }
            return model;
        }
        public async Task<BannerCarouselViewModel> GetBannerCarousalDetail(string Id)
        {
            var BannerInfo = (from a in context.BannerCarousel
                              where a.Id == new Guid(Id)
                              select new BannerCarouselViewModel
                              {
                                  Id = a.Id.ToString(),
                                  Description = a.Description,
                                  Photo = a.Photo,
                                  Title = a.Title
                              }).FirstOrDefault();
            return BannerInfo;
        }

        public async Task<ResponseModel> UpdateBannerCarousalData(BannerCarouselViewModel prod)
        {
            ResponseModel model = new ResponseModel();
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var Info = context.BannerCarousel.FirstOrDefault(x => x.Id == new Guid(prod.Id));
                    if (Info != null)
                    {

                        Info.Title = prod.Title;
                        Info.Description = prod.Description;
                        if (prod.Photo != null)
                        {
                            Info.Photo = prod.Photo;
                        }
                        context.BannerCarousel.Update(Info);

                    }
                    await context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    model.Status = Status.Success;
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

        public List<BannerCarouselViewModel> GrtBannerCarouselList()
        {
            BannerCarouselViewModel model = new BannerCarouselViewModel();
            model.BannerCarouselList = (from a in context.BannerCarousel
                                        select new BannerCarouselViewModel
                                        {
                                            Id = a.Id.ToString(),
                                            Description = a.Description,
                                            Photo = a.Photo,
                                            Title = a.Title
                                        }).ToList();
            return model.BannerCarouselList;
        }
        public async Task<CustomerInfoViewModel>GetProfileDetails(Guid UserId)
        {
            try
            {
                CustomerInfoViewModel model = new CustomerInfoViewModel();

                model = (from a in context.CustomerInfo.Where(x => x.Id == UserId)
                         join Adds in context.CustomerAddressMasters on a.CustomerAddressId equals Adds.Id
                         select new CustomerInfoViewModel
                         {
                             Id = a.Id,
                             CustomerAddressId = a.CustomerAddressId,
                             Name = a.Name,
                             MobileNo = a.MobileNo,
                             Address1 = a.Address1,
                             Address2 = a.Address2,
                             StateId = Adds.StateId,
                             DistricttId = Adds.DistricttId,
                             Address3 = a.Address3,
                             Photo = a.Photo,

                         }).FirstOrDefault();
                return model;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<ResponseModel> UpdateProfileDetails(CustomerInfoViewModel model)
        {
            ResponseModel obj = new ResponseModel();
            try
            {
                var data = context.CustomerInfo.FirstOrDefault(x => x.Id == (model.Id));
                if (data != null)
                {

                    data.Name = model.Name;
                    data.MobileNo = model.MobileNo;
                    data.Address1 = model.Address1;
                    data.Address2 = model.Address2;
                    data.Address3 = model.Address3;
                    data.Photo = model.Photo;
                    context.Update(data);

                    var customerAddressInfo = context.CustomerAddressMasters.FirstOrDefault(x => x.Id == model.CustomerAddressId);
                    if (customerAddressInfo != null)
                    {
                        customerAddressInfo.StateId = model.StateId;
                        customerAddressInfo.DistricttId = model.DistricttId;
                        context.CustomerAddressMasters.Update(customerAddressInfo);
                    }
                }
                await context.SaveChangesAsync();
                return new ResponseModel() { Status = Status.Success };
            }
            catch (Exception ex)
            {
                obj.Status = Status.Success;
                obj.Message = ex.Message;
                throw;
            }

        }

        public async Task<ProductNewViewModel> GetProductById(Guid ProductId)
        {
            ProductNewViewModel model = new ProductNewViewModel();
            try
            {
                var a = await context.ProductMasters.FirstOrDefaultAsync(x => x.IsActive == true && x.Id == ProductId);
                model.ProductName = a.ProductName;
                model.Description = a.Description;
                model.CreatedDate = a.CreatedDate;
                model.INVNo = a.INVNo;
                model.Location = a.Location;
                model.Authenticode = a.Authenticode;
                model.BachNumber = a.BachNumber;
                model.CategoryId = a.CategoryId;
                model.Price = a.Price;
                model.Tax = a.Tax;
                model.Photo = a.Photo;
            }
            catch (Exception)
            {
                throw;
            }
            return model;
        }
        public (int TotalCount, int FilteredCount, dynamic Categorys) Tickets(int skip, int take, string sortColumn, string sortColumnDir, string searchValue)
        {

            var BannerCarousels = (from trackForm in context.Ticket.Where(x => x.CreatedBy != null)
                             join product in context.CustomerInfo on trackForm.CreatedBy equals product.Id.ToString()
                             select new
                             {
                                 Id = trackForm.Id,
                                 Title = trackForm.Title,
                                 Photos = trackForm.Photo,
                                 Description = trackForm.Message,
                                 Status = trackForm.Status,
                                 LabelStatus = trackForm.LebelStatus,
                                 CreatedDate = trackForm.CreatedDate,
                                 Name = product.Name
                             });
 //           BannerCarousels = BannerCarousels.OrderByDescending(x => x.CreatedDate);
            if (!(string.IsNullOrEmpty(sortColumn)&&sortColumn!="" && string.IsNullOrEmpty(sortColumnDir)))
            {
                switch (sortColumn)
                {
                    case "Title":
                        BannerCarousels = sortColumnDir == "asc" ? BannerCarousels.OrderBy(x => x.Title) : BannerCarousels.OrderByDescending(x => x.Title);
                        break;
                    case "Description":
                        BannerCarousels = sortColumnDir == "asc" ? BannerCarousels.OrderBy(x => x.Description) : BannerCarousels.OrderByDescending(x => x.Description);
                        break;
                    case "Status":
                        BannerCarousels = sortColumnDir == "asc" ? BannerCarousels.OrderBy(x => x.Status) : BannerCarousels.OrderByDescending(x => x.Status);
                        break;
                    case "CreatedDate":
                        BannerCarousels = sortColumnDir == "asc" ? BannerCarousels.OrderBy(x => x.CreatedDate) : BannerCarousels.OrderByDescending(x => x.CreatedDate);
                        break;
                    case "LabelStatus":
                        BannerCarousels = sortColumnDir == "asc" ? BannerCarousels.OrderBy(x => x.LabelStatus) : BannerCarousels.OrderByDescending(x => x.LabelStatus);
                        break;
                    default:
                        BannerCarousels = BannerCarousels.OrderByDescending(x => x.CreatedDate);
                        break;
                }

            }
            if (!string.IsNullOrEmpty(searchValue))
            {
                BannerCarousels = BannerCarousels.Where(m => m.Title.Contains(searchValue));
            }
            int recordsTotal = BannerCarousels.Count();
 //           BannerCarousels = BannerCarousels.OrderByDescending(x=>x.CreatedDate);
            var data = BannerCarousels.Select(x => new
            {
                Id = x.Id,
                Title = x.Title,
                Photos = x.Photos,
                Description = x.Description,
                Status = x.Status,
                LabelStatus = x.LabelStatus,
                Name = x.Name,
                CreatedDate=x.CreatedDate.ToString("dd-MM-yyyy")
            }).Skip(skip).Take(take).ToList();
            return (recordsTotal, recordsTotal, data);
        }

        public (int TotalCount, int FilteredCount, dynamic Categorys) Tickets(int skip, int take, string sortColumn, string sortColumnDir, string searchValue, string startDate,string endDate, int status)
        {
            if (startDate != null && startDate != "" && endDate != null && endDate != "")
            {
                DateTime startDate1 = Convert.ToDateTime(startDate).Date;
                var aa = endDate.Trim();
                DateTime endDate1 = Convert.ToDateTime(aa).Date;
                var BannerCarousels=(from x in context.Ticket.Where(x => x.CreatedBy != null && x.CreatedDate.Date >= startDate1 && x.CreatedDate.Date <= endDate1)
                 join product in context.CustomerInfo on x.CreatedBy equals product.Id.ToString()
                 select new
                {
                    Id = x.Id,
                    Title = x.Title,
                    Photos = x.Photo,
                    Description = x.Message,
                    Status = x.Status,
                    LabelStatus = x.LebelStatus,
                    CreatedDate = x.CreatedDate,
                    Name=product.Name
                });
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    switch (sortColumn)
                    {
                        case "Title":
                            BannerCarousels = sortColumnDir == "asc" ? BannerCarousels.OrderBy(x => x.Title) : BannerCarousels.OrderByDescending(x => x.Title);
                            break;
                        case "Description":
                            BannerCarousels = sortColumnDir == "asc" ? BannerCarousels.OrderBy(x => x.Description) : BannerCarousels.OrderByDescending(x => x.Description);
                            break;
                        case "Status":
                            BannerCarousels = sortColumnDir == "asc" ? BannerCarousels.OrderBy(x => x.Status) : BannerCarousels.OrderByDescending(x => x.Status);
                            break;
                        case "CreatedDate":
                            BannerCarousels = sortColumnDir == "asc" ? BannerCarousels.OrderBy(x => x.CreatedDate) : BannerCarousels.OrderByDescending(x => x.CreatedDate);
                            break;
                        case "LabelStatus":
                            BannerCarousels = sortColumnDir == "asc" ? BannerCarousels.OrderBy(x => x.LabelStatus) : BannerCarousels.OrderByDescending(x => x.LabelStatus);
                            break;
                        default:
                            BannerCarousels = BannerCarousels.OrderByDescending(x => x.CreatedDate);
                            break;

                    }

                }
                if (!string.IsNullOrEmpty(searchValue))
                {
                    BannerCarousels = BannerCarousels.Where(m => m.Title.Contains(searchValue));
                }
                int recordsTotal = BannerCarousels.Count();
          //      BannerCarousels = BannerCarousels.OrderByDescending(x => x.CreatedDate);
                var data = BannerCarousels.Select(x => new
                {
                    Id = x.Id,
                    Title = x.Title,
                    Photos = x.Photos,
                    Description = x.Description,
                    Status = x.Status,
                    LabelStatus = x.LabelStatus,
                    Name = x.Name,
                    CreatedDate = x.CreatedDate.ToString("dd-MM-yyyy")
                }).Skip(skip).Take(take).ToList();
                return (recordsTotal, recordsTotal, data);
            }
            else
            {
            var BannerCarousels=(from x in context.Ticket.Where(x => x.CreatedBy != null && x.LebelStatus == status)
                 join product in context.CustomerInfo on x.CreatedBy equals product.Id.ToString()
             select new
                {
                    Id = x.Id,
                    Title = x.Title,
                    Photos = x.Photo,
                    Description = x.Message,
                    Status = x.Status,
                    LabelStatus = x.LebelStatus,
                    CreatedDate = x.CreatedDate,
                    Name=product.Name
                });
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    switch (sortColumn)
                    {
                        case "Title":
                            BannerCarousels = sortColumnDir == "asc" ? BannerCarousels.OrderBy(x => x.Title) : BannerCarousels.OrderByDescending(x => x.Title);
                            break;
                        case "Description":
                            BannerCarousels = sortColumnDir == "asc" ? BannerCarousels.OrderBy(x => x.Description) : BannerCarousels.OrderByDescending(x => x.Description);
                            break;
                        case "Status":
                            BannerCarousels = sortColumnDir == "asc" ? BannerCarousels.OrderBy(x => x.Status) : BannerCarousels.OrderByDescending(x => x.Status);
                            break;
                        case "CreatedDate":
                            BannerCarousels = sortColumnDir == "asc" ? BannerCarousels.OrderBy(x => x.CreatedDate) : BannerCarousels.OrderByDescending(x => x.CreatedDate);
                            break;
                        case "LabelStatus":
                            BannerCarousels = sortColumnDir == "asc" ? BannerCarousels.OrderBy(x => x.LabelStatus) : BannerCarousels.OrderByDescending(x => x.LabelStatus);
                            break;
                        default:
                            BannerCarousels = BannerCarousels.OrderByDescending(x => x.CreatedDate);
                            break;
                    }

                }
                if (!string.IsNullOrEmpty(searchValue))
                {
                    BannerCarousels = BannerCarousels.Where(m => m.Title.Contains(searchValue));
                }
                int recordsTotal = BannerCarousels.Count();
         //       BannerCarousels = BannerCarousels.OrderByDescending(x => x.CreatedDate);
                var data = BannerCarousels.Select(x => new
                {
                    Id = x.Id,
                    Title = x.Title,
                    Photos = x.Photos,
                    Description = x.Description,
                    Status = x.Status,
                    LabelStatus = x.LabelStatus,
                    Name = x.Name,
                    CreatedDate = x.CreatedDate.ToString("dd-MM-yyyy")
                }).Skip(skip).Take(take).ToList();
                return (recordsTotal, recordsTotal, data);
            }
        }


        public async Task<TicketViewModel> TicketDetails(Guid? id)
        {
            TicketViewModel model = new TicketViewModel();
            try
            {
                var a = await context.Ticket.Where(x => x.Id == id).FirstOrDefaultAsync();
                if (a!=null)
                {
                    model.Title = a.Title;
                    model.Message = a.Message;
                    model.Id = a.Id.ToString();
                    model.Photo = a.Photo;
                    model.CustomerId = a.CustomerId;
                    model.Status = a.Status;
                    model.LabelStatus = a.LebelStatus;
                }

            }
            catch (Exception)
            {

                throw;
            }
            return model;
        }
        public async Task<ResponseModel> InsertNewsDetails(NewsViewModel news)
        {
            ResponseModel model = new ResponseModel();
            try
            {
                News Ns = new News();
                Ns.Id = Guid.NewGuid();
                Ns.NewsTitle = news.NewsTitle;
                Ns.Description = news.Description;
                Ns.Createddate = DateTime.Now;
                Ns.Updateddate = DateTime.Now;
                Ns.CreatedBy = news.CreatedBy;
                Ns.Photo = news.Photo;
                context.Add(Ns);
                await context.SaveChangesAsync();
                model.Status = Status.Success;
            }
            catch (Exception ex)
            {
                model.Status = Status.Failure;
            }
            return model;
        }

        public async Task<ResponseModel> UpdateTicket(TicketViewModel res)
        {
            ResponseModel model = new ResponseModel();
            using (var transaction = context.Database.BeginTransaction())
            {
                try

                {
                    var info = context.Ticket.Where(x => x.Id == new Guid(res.Id)).FirstOrDefault();
                    if (info != null)
                    {
                        info.Answer = res.Answer;
                        info.Status = res.Status;
                        info.UpdatedBy = res.UpdatedBy;
                        info.LebelStatus = res.LabelStatus;
                        info.UpdatedDate = DateTime.Now;
                        info.NotificationAdmin = false;
                        info.NotificationCust = true;
                        context.Ticket.Update(info);
                    }
                    await context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    model.Status = Status.Success;
                }
                catch (Exception ex)
                {
                    model.Status = Status.Failure;
                    await transaction.RollbackAsync();
                }

            }
            return model;
        }
        public async Task<ResponseModel> DeleteNewsDetails(Guid Id)
        {
            ResponseModel model = new ResponseModel();
            var Info = context.News.FirstOrDefault(a => a.Id == Id);
            if (Info != null)
            {
                var list = context.News.Find(Info.Id);
                if (Info != null)
                {
                    context.Remove(list);
                    await context.SaveChangesAsync();
                    model.Status = Status.Success;
                }
            }

            return model;
        }
        public async Task<ResponseModel> UpdateNewsDetails(NewsViewModel news)
        {
            ResponseModel response = new ResponseModel();

            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var Info = context.News.FirstOrDefault(x => x.Id == news.Id);
                    if (Info != null)
                    {

                        Info.NewsTitle = news.NewsTitle;
                        Info.Description = news.Description;
                        Info.Updateddate = DateTime.Now;
                        if (news.Photo!=null)
                        {
                            Info.Photo = news.Photo;
                        }
                        context.News.Update(Info);
                    }
                    await context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    response.Status = Status.Success;
                }
                catch (Exception ex)
                {
                    logger.Error(ex);
                    response.Status = Status.Failure;
                    await transaction.RollbackAsync();
                }
            }
            return response;
        }
        public NewsViewModel GetNewsData(Guid Id)
        {
            NewsViewModel model = new NewsViewModel();
            var item = context.News.FirstOrDefault(a => a.Id == Id);
            model = (from a in context.News
                     where a.Id == Id
                     select new NewsViewModel
                     {
                         NewsTitle = a.NewsTitle,
                         Id = a.Id,
                         Photo=a.Photo,
                         CreatedBy = a.CreatedBy,
                         Createddate = a.Createddate,
                         Description = a.Description,
                         Updateddate = a.Updateddate

                     }).FirstOrDefault();
            return model;
        }
        public List<NewsViewModel> GetNewList()
        {
            NewsViewModel model = new NewsViewModel();
            model.NewsList = (from a in context.News
                              select new NewsViewModel
                              {
                                  Id = a.Id,
                                  Description = a.Description,
                                  NewsTitle = a.NewsTitle,
                                  Photo = a.Photo
                              }).ToList();
            return model.NewsList;
        }
        public List<SelectListItem> GetNewsCategory()
        {
            return context.ProductCategories.Select(x => new SelectListItem() { Text = x.CategoryName, Value = x.Id.ToString() }).ToList();
        }

        public (int TotalCount, int FilteredCount, dynamic News) GetNewsDetails(int skip, int take, string sortColumn, string sortColumnDir, string searchValue)
        {
            var Newsdata = context.News.Select(x => new
            {
                Id = x.Id,
                Title = x.NewsTitle,
                Description = x.Description,
                Photos = x.Photo,
                Createddate = x.Createddate
            });
            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            {
                switch (sortColumn)
                {
                    case "Title":
                        Newsdata = sortColumnDir == "asc" ? Newsdata.OrderBy(x => x.Title) : Newsdata.OrderByDescending(x => x.Title);
                        break;
                    case "Createddate":
                        Newsdata = sortColumnDir == "asc" ? Newsdata.OrderBy(x => x.Createddate) : Newsdata.OrderByDescending(x => x.Createddate);
                        break;

                }

            }
            if (!string.IsNullOrEmpty(searchValue))
            {
                Newsdata = Newsdata.Where(m => m.Title.Contains(searchValue));
            }
            int recordsTotal = Newsdata.Count();
            var data = Newsdata.OrderByDescending(x=>x.Createddate).Select(x => new
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                Photos = x.Photos,
                Createddate = x.Createddate.ToString("dd-MM-yyyy")

            }).Skip(skip).Take(take).ToList();
            return (recordsTotal, recordsTotal, data);
        }

        public List<NewsViewModel> GrtNewsAllList()
        {
            List<NewsViewModel> model = new List<NewsViewModel>();
            model = (from a in context.News.OrderByDescending(x=>x.Createddate)
                     select new NewsViewModel
                     {
                         Id = a.Id,
                         Description = a.Description,
                         Photo = a.Photo,
                         NewsTitle = a.NewsTitle,
                         Createddate = a.Createddate
                     }).ToList();
            return model;
        }
        public NewsViewModel PostNewsData(Guid Id)
        {
            NewsViewModel model = new NewsViewModel();
            var item = context.News.FirstOrDefault(a => a.Id == Id);
            model = (from a in context.News
                     where a.Id == Id
                     select new NewsViewModel
                     {
                         NewsTitle = a.NewsTitle,
                         Id = a.Id,
                         CreatedBy = a.CreatedBy,
                         Createddate = a.Createddate,
                         Description = a.Description,
                         Photo = a.Photo,
                         Updateddate = a.Updateddate

                     }).FirstOrDefault();
            return model;
        }

        public List<SelectListItem> GetStates()
        {
            return context.StateMaters.Select(x => new SelectListItem() { Text = x.StateName, Value = x.Id.ToString() }).ToList();
        }
        public (int TotalCount, int FilteredCount, dynamic Categorys) Shipping(int skip, int take, string sortColumn, string sortColumnDir, string searchValue)
        {
            List<ShippingList> res1 = new List<ShippingList>();
            var BannerCarousels = context.Shipping.ToList();

            int recordsTotal = BannerCarousels.Count();

            foreach (var item in BannerCarousels)
            {
                ShippingList res = new ShippingList();
                var FinalState = "";
                var finalDis = "";

                var a = item.StateId;
                var d2 = item.DistricttId;
                if (a != null)
                {
                    if (d2 != null)
                    {

                        var b = a.Split(',');
                        //var d = item.DistricttId;
                        var d1 = d2.Split(',');
                        foreach (var item1 in b)
                        {
                            if (item1 != "" && item1 != null)
                            {
                                var state = context.StateMaters.Where(c => (c.Id).ToString() == item1).FirstOrDefault().StateName;
                                FinalState += state + ",";
                            }
                        }
                        foreach (var item2 in d1)
                        {
                            if (item2 != "" && item2 != null)
                            {
                                var dis = context.DistricttMasters.Where(c => (c.Id).ToString() == item2).FirstOrDefault().DistricttName;
                                finalDis += dis + ",";
                            }
                        }


                        res.Id = item.Id.ToString();
                        res.ShippingCharge = item.ShippingCharge.ToString();
                        res.ShippingMethod = item.ShippingMethod.ToString();
                        res.State = FinalState.Remove(FinalState.Length - 1, 1);
                        res.District = finalDis.Remove(finalDis.Length - 1, 1);
                        res1.Add(res);
                    }
                }
            }
            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            {
                switch (sortColumn)
                {
                    case "ShippingMethod":
                        res1 = sortColumnDir == "asc" ? res1.OrderBy(x => x.ShippingMethod).ToList() : res1.OrderByDescending(x => x.ShippingMethod).ToList();
                        break;

                }

            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                res1 = res1.Where(m => m.ShippingMethod.Contains(searchValue)).ToList();
            }
            var data = res1.Select(x => new
            {
                Id = x.Id,
                StateName = x.State,
                DistricttName = x.District,
                ShippingMethod = x.ShippingMethod,
                ShippingCharge = x.ShippingCharge
            }).Skip(skip).Take(take).ToList();
            return (recordsTotal, recordsTotal, data);
        }

        public async Task<ResponseModel> CreateShiping(ShippingViewModel bcvm)
        {
            ResponseModel model = new ResponseModel();
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    //Shipping pm = new Shipping();
                    //pm.Id = Guid.NewGuid();
                    //pm.ShippingMethod = bcvm.ShippingMethod;
                    //pm.ShippingCharge = bcvm.ShippingCharge;
                    //pm.StateId = bcvm.State_Id;
                    //pm.DistricttId = bcvm.Districtt_Id;
                    //pm.Createddate = DateTime.Now;
                    //pm.CreatedBy = bcvm.CreatedBy;
                    //pm.Shippingdate = bcvm.Shippingdate;
                    //context.Add(pm);
                    //await context.SaveChangesAsync();
                    //await transaction.CommitAsync();
                    //model.Status = Status.Success;
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

        public async Task<ShippingViewModel> EditShip(string id)
        {
            ShippingViewModel model = new ShippingViewModel();
            List<ShippingViewModel> model1 = new List<ShippingViewModel>();
            try
            {
                var a = await context.Shipping.Where(x => x.Id == new Guid(id)).FirstOrDefaultAsync();
                if (a != null)
                {
                    model.ShippingMethod = a.ShippingMethod;
                    model.ShippingCharge = a.ShippingCharge;
                    model.Districtt_Id = a.DistricttId.ToString();
                    model.State_Id = a.StateId.ToString();
                    model.Id = a.Id.ToString();

                    var d = a.DistricttId;
                    var d1 = d.Split(',');
                    var b = a.StateId;
                    var b1 = b.Split(',');
                    foreach (var item1 in b1)
                    {
                        if (item1 != "")
                        {

                            ShippingViewModel res = new ShippingViewModel();
                            //var state = context.StateMaters.Where(c => (c.Id).ToString() == item1).FirstOrDefault().StateName;
                            var state = context.StateMaters.Where(c => (c.Id).ToString() == item1).Select(x => new SelectListItem() { Text = x.StateName, Value = x.Id.ToString() }).FirstOrDefault();
                            //FinalState += state + ",";
                            res.Id = state.Value;
                            res.State = state.Text;
                            model1.Add(res);
                        }
                    }
                    List<ShippingViewModel> model2 = new List<ShippingViewModel>();
                    foreach (var item2 in d1)
                    {

                        ShippingViewModel res1 = new ShippingViewModel();
                        if (item2 != "")
                        {
                            //var dis = context.DistricttMasters.Where(c => (c.Id).ToString() == item2).FirstOrDefault().DistricttName;
                            var District = context.DistricttMasters.Where(c => (c.Id).ToString() == item2).Select(x => new SelectListItem() { Text = x.DistricttName, Value = x.Id.ToString() }).FirstOrDefault();
                            //finalDis += dis + ",";
                            res1.Id = District.Value;
                            res1.District = District.Text;
                            model2.Add(res1);
                        }
                    }
                    model.ShippingList = model1;
                    model.ShippingList1 = model2;
                    return model;
                }
            }
            catch (Exception)
            {

                throw;
            }
            return model;
        }
        public List<SelectListItem> GetDistrictts(int StateId, int districtId)
        {
            return context.DistricttMasters.Where(x => x.StateId == StateId && x.Id == districtId).Select(x => new SelectListItem() { Text = x.DistricttName, Value = x.Id.ToString() }).ToList();
        }

        public async Task<ResponseModel> UpdateShip(ShippingViewModel model)
        {
            ResponseModel res = new ResponseModel();
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var a = context.Shipping.Where(s => s.Id == new Guid(model.Id)).FirstOrDefault();
                    if (a != null)
                    {
                        a.ShippingMethod = model.ShippingMethod;
                        a.ShippingCharge = model.ShippingCharge;
                        a.StateId = model.Final_State;
                        a.DistricttId = model.Final_Dis;
                        context.Shipping.Update(a);
                        await context.SaveChangesAsync();
                        await transaction.CommitAsync();
                        res.Status = Status.Success;
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
        public async Task<ResponseModel> DeleteShipingDetails(string Id)
        {
            ResponseModel model = new ResponseModel();
            try
            {
                var Info = context.Shipping.FirstOrDefault(x => x.Id == new Guid(Id));
                if (Info != null)
                {
                    var data = context.Shipping.Find(Info.Id);
                    if (data != null)
                    {
                        context.Shipping.Remove(data);
                    }
                    await context.SaveChangesAsync();
                    model.Status = Status.Success;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                model.Status = Status.Failure;
            }
            return model;
        }
        public (int TotalCount, int FilteredCount, dynamic Categorys) GetOrders(int skip, int take, string sortColumn, string sortColumnDir, string searchValue)
        {
            List<OrderHistory> Orders = new List<OrderHistory>();
            Orders = (from a in context.Order
                      join b in context.CustomerInfo on a.UserId equals b.Id
                      join c in context.Payment on a.PaymentId equals c.Id
                      join d in context.ShippingTracking on a.Id equals d.OrderId
                      select new OrderHistory
                      {
                          Id = a.Id.ToString(),
                          Order_Id = a.Order_Id.ToString(),
                          UserName = b.Name,
                          Paymentstatus = c.Status,
                          OrderDate = a.CreatedDate,
                          TotalAmount = a.TotalAmount,
                          TrackingId = d.Status
                      }).OrderByDescending(x => x.OrderDate).ToList();

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
            int recordsTotal = Orders.Count();
            var data = Orders.Select(x => new
            {
                Id = x.Id.ToString(),
                Order_Id = x.Order_Id.ToString(),
                UserName = x.UserName,
                Paymentstatus = x.Paymentstatus,
                OrderDate = x.OrderDate.ToString("dd-MM-yyyy"),
                TotalAmount = x.TotalAmount,
                TrackingId = x.TrackingId
            }).Skip(skip).Take(take).ToList();
            return (recordsTotal, recordsTotal, data);
        }

        public (int TotalCount, int FilteredCount, dynamic Categorys) GetOrders(int skip, int take, string sortColumn, string sortColumnDir, string searchValue,string date,string status)
        {
            List<OrderHistory> Orders = new List<OrderHistory>();
            if (date!=null && date!="")
            {
                DateTime dd = Convert.ToDateTime(date).Date;
                Orders = (from a in context.Order
                          join b in context.CustomerInfo on a.UserId equals b.Id
                          join c in context.Payment on a.PaymentId equals c.Id
                          join d in context.ShippingTracking on a.Id equals d.OrderId
                          select new OrderHistory
                          {
                              Id = a.Id.ToString(),
                              Order_Id = a.Order_Id.ToString(),
                              UserName = b.Name,
                              Paymentstatus = c.Status,
                              OrderDate = a.CreatedDate,
                              TotalAmount = a.TotalAmount,
                              TrackingId = d.Status
                          }).Where(x => x.OrderDate.Date== dd).ToList();
            }
            else
            {
                Orders = (from a in context.Order
                          join b in context.CustomerInfo on a.UserId equals b.Id
                          join c in context.Payment on a.PaymentId equals c.Id
                          join d in context.ShippingTracking on a.Id equals d.OrderId
                          select new OrderHistory
                          {
                              Id = a.Id.ToString(),
                              Order_Id = a.Order_Id.ToString(),
                              UserName = b.Name,
                              Paymentstatus = c.Status,
                              OrderDate = a.CreatedDate,
                              TotalAmount = a.TotalAmount,
                              TrackingId = d.Status
                          }).Where(x => x.TrackingId==status).ToList();
            }
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
            int recordsTotal = Orders.Count();
            var data = Orders.Select(x => new
            {
                Id = x.Id.ToString(),
                Order_Id = x.Order_Id.ToString(),
                UserName = x.UserName,
                Paymentstatus = x.Paymentstatus,
                OrderDate = x.OrderDate.ToString("dd-MM-yyyy"),
                TotalAmount = x.TotalAmount,
                TrackingId = x.TrackingId
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
                           select new OrderHistory
                           {
                               Id = a.Id.ToString(),
                               UserName = b.Name,
                               Paymentstatus = c.Status,
                               OrderDate = a.CreatedDate,
                               TotalAmount = a.TotalAmount,
                               CustId = b.Id.ToString()
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
                details.Shippingstatus = context.ShippingTracking.Where(x => x.OrderId == new Guid(Id)).Select(x => x.Status).FirstOrDefault();
                details.TrackingNumber = context.ShippingTracking.Where(x => x.OrderId == new Guid(Id)).Select(x => x.TrackingNumber).FirstOrDefault();
                details.CourierCompany = context.ShippingTracking.Where(x => x.OrderId == new Guid(Id)).Select(x => x.CourierCompnyName).FirstOrDefault();
                return details;
            }
            catch (Exception)
            {

                throw;
            }
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

        public async Task<ResponseModel> Updatetarcking(string shipstatus, string OrderId,string TrackingNu, string TrackingCN)
        {
            ResponseModel res = new ResponseModel();
            using (var transaction = context.Database.BeginTransaction())
            {
                //ShippingTracking st = new ShippingTracking();
                var st = context.ShippingTracking.Where(s => s.OrderId == new Guid(OrderId)).FirstOrDefault();
                if (st != null)
                {
                    switch (shipstatus)
                    {
                        case "1":
                            {
                                st.Status = shipstatus;
                                st.TrackingNumber = TrackingNu;
                                st.CourierCompnyName = TrackingCN;
                                st.OrderDate = DateTime.Now;
                            }
                            break;
                        case "2":
                            {
                                st.Status = shipstatus;
                                st.TrackingNumber = TrackingNu;
                                st.CourierCompnyName = TrackingCN;
                                st.ShippedDate = DateTime.Now;
                            }
                            break;
                        case "3":
                            {
                                st.Status = shipstatus;
                                st.TrackingNumber = TrackingNu;
                                st.CourierCompnyName = TrackingCN;
                                st.OutForDeliveryDate = DateTime.Now;
                            }
                            break;
                        case "4":
                            {
                                st.Status = shipstatus;
                                st.TrackingNumber = TrackingNu;
                                st.CourierCompnyName = TrackingCN;
                                st.Delivered = DateTime.Now;
                            }
                            break;
                    }
                    context.ShippingTracking.Update(st);
                    await context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    res.Status = Status.Success;
                }
                else
                {
                    res.Status = Status.Failure;
                    transaction.Rollback();
                }
            }
            return res;
        }

        public async Task<ResponseModel> Create(CustomerInfoViewModel customer)
        {
            ResponseModel model = new ResponseModel();
            using (var transection = await context.Database.BeginTransactionAsync())
            {
                try
                {
                //    CustomerAddresses customerAddress = new CustomerAddresses()
                //    {
                //        StateId = customer.StateId,
                //        DistricttId = customer.DistricttId,
                //        PostCode = customer.PostCode,
                //        Address = customer.Address
                //    };
                //    context.CustomerAddressMasters.Add(customerAddress);
                //    context.SaveChanges();
                //    CustomerInfo customerInfo = new CustomerInfo()
                //    {
                //        Name = customer.Name,
                //        Email = customer.Email,
                //        UserPass = customer.UserPass,
                //        MobileNo = customer.MobileNo,
                //        DateCreated = DateTime.UtcNow,
                //        CustomerAddressId = customerAddress.Id,
                //        Nric = "",
                //        Address1 = customer.Address1,
                //        Address2 = customer.Address2,
                //        Address3 = customer.Address3,
                //        DeviceId = Guid.NewGuid().ToString(),
                //        UserType = UserType.SubAdmin,
                //        IsActive = true
                //    };
                //    customerInfo.DateCreated = DateTime.UtcNow;
                //    await context.CustomerInfo.AddAsync(customerInfo);
                //    await context.SaveChangesAsync();
                //    await transection.CommitAsync();
                //    model.Status = Status.Success;
                }
                catch (Exception ex)
                {
                    await transection.RollbackAsync();
                   model.Status = Status.Failure;
                    model.Message = ex.Message;
                    //Logger.Error(ex);
                }
            }
            return model;
        }

        public async Task<CustomerInfoViewModel> Subadmindetails(Guid CustomerId)
        {
            var customerInfo = context.CustomerInfo.Join(context.CustomerAddressMasters,
                cust => cust.CustomerAddressId,
                adddress => adddress.Id,
                (cust, adddress) => new CustomerInfoViewModel
                {
                    Id = cust.Id,
                    Name = cust.Name,
                    MobileNo = cust.MobileNo,
                    Nric = cust.Nric,
                    StateId = adddress.StateId,
                    DistricttId = adddress.DistricttId,
                    PostCode = adddress.PostCode,
                    Address = adddress.Address
                    //UserPass = cust.UserPass
                }).FirstOrDefault(x => x.Id == CustomerId);
            return customerInfo;
        }

        public async Task<ResponseModel> UpdateSubadmindetails(CustomerInfoViewModel customer)
        {
            ResponseModel model = new ResponseModel();
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var customerInfo = context.CustomerInfo.FirstOrDefault(x => x.Id == customer.Id);
                    if (customerInfo != null)
                    {
                        customerInfo.Name = customer.Name;
                        customerInfo.MobileNo = customer.MobileNo;
                        customerInfo.Nric = customer.Nric;
                        context.CustomerInfo.Update(customerInfo);
                        var customerAddressInfo = context.CustomerAddressMasters.FirstOrDefault(x => x.Id == customerInfo.CustomerAddressId);
                        if (customerAddressInfo != null)
                        {
                            customerAddressInfo.Address = customer.Address;
                            customerAddressInfo.StateId = customer.StateId;
                            customerAddressInfo.DistricttId = customer.DistricttId;
                            customerAddressInfo.PostCode = customer.PostCode;
                            context.CustomerAddressMasters.Update(customerAddressInfo);
                        }
                    }
                    await context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    model.Status = Status.Success;
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

        public async Task<ResponseModel> DeleteSubadminDetails(CustomerInfoViewModel customer)
        {
            ResponseModel model = new ResponseModel();
            var customerInfo = context.CustomerInfo.FirstOrDefault(x => x.Id == customer.Id);
            if (customerInfo != null)
            {
                customerInfo.IsActive = false;
                await context.SaveChangesAsync();
                model.Status = Status.Success;
            }
            return model;
        }
        
       

        public (int TotalCount, int FilteredCount, dynamic Customers) GetAdmindata(int skip, int take, string sortColumn, string sortColumnDir, string searchValue)
        {
            var customers = context.CustomerInfo.Where(x => x.IsActive && x.UserType == UserType.SubAdmin).Select(x => new
            {
                Id = x.Id,
                Name = x.Name,
                Email = x.Email,
                DateCreated = x.DateCreated,
                CustomerAddressId = x.CustomerAddressId,
                MobileNo = x.MobileNo,
                Nric = x.Nric,
                IsActive = x.IsActive
            });
            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            {
                switch (sortColumn)
                {
                    case "Name":
                        customers = sortColumnDir == "asc" ? customers.OrderBy(x => x.Email) : customers.OrderByDescending(x => x.Name);
                        break;
                    case "Email":
                        customers = sortColumnDir == "asc" ? customers.OrderBy(x => x.Email) : customers.OrderByDescending(x => x.Email);
                        break;
                    case "MobileNo":
                        customers = sortColumnDir == "asc" ? customers.OrderBy(x => x.MobileNo) : customers.OrderByDescending(x => x.MobileNo);
                        break;
                    case "DateCreated":
                        customers = sortColumnDir == "asc" ? customers.OrderBy(x => x.DateCreated) : customers.OrderByDescending(x => x.DateCreated);
                        break;
                }

            }
            if (!string.IsNullOrEmpty(searchValue))
            {
                customers = customers.Where(m => m.Name.Contains(searchValue) || m.Email.Contains(searchValue) || m.MobileNo.Contains(searchValue) || m.Nric.Contains(searchValue));
            }
            int recordsTotal = customers.Count();
            var data = customers.Select(x => new
            {
                Id = x.Id,
                Name = x.Name,
                Email = x.Email,
                DateCreated = x.DateCreated.ToString("dd-MM-yyyy"),
                CustomerAddressId = x.CustomerAddressId,
                MobileNo = x.MobileNo,
                Nric = x.Nric,
                IsActive = x.IsActive
            }).Skip(skip).Take(take).ToList();
            return (recordsTotal, recordsTotal, data);
        }


        public async Task<TrackingForms> EditVerify(int Id)
        {
            TrackingForms model = new TrackingForms();
            var a = await context.TrackingForms.Where(x => x.Id == Id).FirstOrDefaultAsync();
            if (a != null)
            {
                model.Id = a.Id;
                model.Name = a.Name;
                model.CertificateImage = a.CertificateImage;
                model.IsVerified = a.IsVerified;
            }
            return model;
        }
        public async Task<ResponseModel> UpdateVerify(TrackingForms Track)
        {
            ResponseModel model = new ResponseModel();
            using (var transaction = context.Database.BeginTransaction())
            {
                var data = context.TrackingForms.FirstOrDefault(x => x.Id == Track.Id);
                if (data != null)
                {
                    data.Name = Track.Name;
                    data.CertificateImage = Track.CertificateImage;
                    data.IsVerified = Track.IsVerified;
                    data.VerifiedBy = Track.VerifiedBy;
                    context.TrackingForms.Update(data);
                }
                await context.SaveChangesAsync();
                await transaction.CommitAsync();
                model.Status = Status.Success;
            }

            return model;
        }
        public async Task<ResponseModel> AddGallery(List<ProductGalleryViewModel> gallery)
        {
            ResponseModel model = new ResponseModel();
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    foreach (var item in gallery)
                    {
                        ProductGallery pm = new ProductGallery();
                        pm.Id = Guid.NewGuid();
                        pm.ProductId = item.ProductId.ToString();
                        pm.CreatedDate = DateTime.Now;
                        pm.CreatedBy = item.CreatedBy;
                        pm.Photo = item.Photo;
                        context.ProductGallery.Add(pm);
                        await context.SaveChangesAsync();

                        model.Status = Status.Success;
                    }
                    await transaction.CommitAsync();
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


        public async Task<ResponseModel> Savedata(AboutViewModel data)
        {
            ResponseModel model = new ResponseModel();
            using (var transection = await context.Database.BeginTransactionAsync())
            {

                try
                {

                    About customerabout = new About()
                    {
                        Id = Guid.NewGuid(),
                        Title = data.Title,
                        Description = data.Description,
                        CreatedBy = data.CreatedBy,
                        CreatedDate = DateTime.Now,
                        UpdatedDate = DateTime.Now,

                    };
                    context.About.Add(customerabout);
                    context.SaveChanges();

                    await context.SaveChangesAsync();
                    await transection.CommitAsync();
                    model.Status = Status.Success;
                }
                catch (Exception ex)
                {
                    await transection.RollbackAsync();
                    model.Status = Status.Failure;
                    model.Message = ex.Message;
                    //Logger.Error(ex);
                }
            }
            return model;
        }
        public List<AboutViewModel> GetAboutList()
        {
            AboutViewModel model = new AboutViewModel();
            model.aboutList = (from a in context.About
                               select new AboutViewModel
                               {
                                   Id = a.Id,
                                   Description = a.Description,
                                   Title = a.Title

                               }).ToList();
            return model.aboutList;
        }
        public AboutViewModel GetAboutData(Guid Id)
        {
            AboutViewModel model = new AboutViewModel();
            var item = context.About.FirstOrDefault(a => a.Id == Id);
            model = (from a in context.About
                     where a.Id == Id
                     select new AboutViewModel
                     {
                         Title = a.Title,
                         Id = a.Id,
                         CreatedBy = a.CreatedBy,
                         CreatedDate = a.CreatedDate,
                         Description = a.Description,
                         UpdatedDate = a.UpdatedDate

                     }).FirstOrDefault();
            return model;
        }
        public async Task<ResponseModel> UpdateAboutDetails(AboutViewModel model)
        {
            ResponseModel response = new ResponseModel();

            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var Info = context.About.FirstOrDefault(x => x.Id == model.Id);
                    if (Info != null)
                    {

                        Info.Title = model.Title;
                        Info.Description = model.Description;
                        Info.UpdatedDate = DateTime.Now;
                        context.About.Update(Info);
                    }
                    await context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    response.Status = Status.Success;
                }
                catch (Exception ex)
                {
                    logger.Error(ex);
                    response.Status = Status.Failure;
                    await transaction.RollbackAsync();
                }
            }
            return response;
        }
        public async Task<ResponseModel> DeleteAboutDetails(Guid Id)
        {
            ResponseModel model = new ResponseModel();
            var Info = context.About.FirstOrDefault(a => a.Id == Id);
            if (Info != null)
            {
                var list = context.About.Find(Info.Id);
                if (Info != null)
                {
                    context.Remove(list);
                    await context.SaveChangesAsync();
                    model.Status = Status.Success;
                }
            }

            return model;
        }
        public (int TotalCount, int FilteredCount, dynamic News) GetAboutDetails(int skip, int take, string sortColumn, string sortColumnDir, string searchValue)
        {
            var Aboutdata = context.About.Select(x => new
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description
            });
            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            {
                switch (sortColumn)
                {
                    case "Title":
                        Aboutdata = sortColumnDir == "asc" ? Aboutdata.OrderBy(x => x.Title) : Aboutdata.OrderByDescending(x => x.Title);
                        break;

                }

            }
            if (!string.IsNullOrEmpty(searchValue))
            {
                Aboutdata = Aboutdata.Where(m => m.Title.Contains(searchValue));
            }
            int recordsTotal = Aboutdata.Count();
            var data = Aboutdata.Select(x => new
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description
            }).Skip(skip).Take(take).ToList();
            return (recordsTotal, recordsTotal, data);
        }


        public async Task<ResponseModel> SaveFAQdata(FaqViewModel data)
        {
            ResponseModel model = new ResponseModel();
            using (var transection = await context.Database.BeginTransactionAsync())
            {

                try
                {

                    FAQ customerabout = new FAQ()
                    {
                        Id = Guid.NewGuid(),
                        Title = data.Title,
                        Description = data.Description,
                        CreatedBy = data.CreatedBy,
                        CreatedDate = DateTime.Now,
                        UpdatedDate = DateTime.Now,

                    };
                    context.FAQ.Add(customerabout);
                    context.SaveChanges();

                    await context.SaveChangesAsync();
                    await transection.CommitAsync();
                    model.Status = Status.Success;
                }
                catch (Exception ex)
                {
                    await transection.RollbackAsync();
                    model.Status = Status.Failure;
                    model.Message = ex.Message;
                    //Logger.Error(ex);
                }
            }
            return model;
        }
        public List<FaqViewModel> GetFAQList()
        {
            FaqViewModel model = new FaqViewModel();
            model.FAQList = (from a in context.FAQ
                             select new FaqViewModel
                             {
                                 Id = a.Id,
                                 Description = a.Description,
                                 Title = a.Title

                             }).ToList();
            return model.FAQList;
        }
        public FaqViewModel GetFAQData(Guid Id)
        {
            FaqViewModel model = new FaqViewModel();
            var item = context.FAQ.FirstOrDefault(a => a.Id == Id);
            model = (from a in context.FAQ
                     where a.Id == Id
                     select new FaqViewModel
                     {
                         Title = a.Title,
                         Id = a.Id,
                         CreatedBy = a.CreatedBy,
                         CreatedDate = a.CreatedDate,
                         Description = a.Description,
                         UpdatedDate = a.UpdatedDate

                     }).FirstOrDefault();
            return model;
        }
        public async Task<ResponseModel> UpdateFAQDetails(FaqViewModel model)
        {
            ResponseModel response = new ResponseModel();

            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var Info = context.FAQ.FirstOrDefault(x => x.Id == model.Id);
                    if (Info != null)
                    {

                        Info.Title = model.Title;
                        Info.Description = model.Description;
                        Info.UpdatedDate = DateTime.Now;
                        context.FAQ.Update(Info);
                    }
                    await context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    response.Status = Status.Success;
                }
                catch (Exception ex)
                {
                    logger.Error(ex);
                    response.Status = Status.Failure;
                    await transaction.RollbackAsync();
                }
            }
            return response;
        }
        public async Task<ResponseModel> DeleteFAQDetails(Guid Id)
        {
            ResponseModel model = new ResponseModel();
            var Info = context.FAQ.FirstOrDefault(a => a.Id == Id);
            if (Info != null)
            {
                var list = context.FAQ.Find(Info.Id);
                if (Info != null)
                {
                    context.Remove(list);
                    await context.SaveChangesAsync();
                    model.Status = Status.Success;
                }
            }

            return model;
        }
        public (int TotalCount, int FilteredCount, dynamic News) GetFAQDetails(int skip, int take, string sortColumn, string sortColumnDir, string searchValue)
        {
            var FAQdata = context.FAQ.Select(x => new
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description
            });
            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            {
                switch (sortColumn)
                {
                    case "Title":
                        FAQdata = sortColumnDir == "asc" ? FAQdata.OrderBy(x => x.Title) : FAQdata.OrderByDescending(x => x.Title);
                        break;

                }

            }
            if (!string.IsNullOrEmpty(searchValue))
            {
                FAQdata = FAQdata.Where(m => m.Title.Contains(searchValue));
            }
            int recordsTotal = FAQdata.Count();
            var data = FAQdata.Select(x => new
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description
            }).Skip(skip).Take(take).ToList();
            return (recordsTotal, recordsTotal, data);
        }


        public async Task<ResponseModel> SaveHelpdata(HelpGuidViewModel helpdata)
        {
            ResponseModel model = new ResponseModel();
            using (var transection = await context.Database.BeginTransactionAsync())
            {

                try
                {
                    HelpGuid customerhelp = new HelpGuid()
                    {
                        Id = Guid.NewGuid(),
                        Title = helpdata.Title,
                        Description = helpdata.Description,
                        CreatedBy = helpdata.CreatedBy,
                        CreatedDate = DateTime.Now,
                        UpdatedDate = DateTime.Now,

                    };
                    context.HelpGuid.Add(customerhelp);
                    context.SaveChanges();

                    await context.SaveChangesAsync();
                    await transection.CommitAsync();
                    model.Status = Status.Success;
                }
                catch (Exception ex)
                {
                    await transection.RollbackAsync();
                    model.Status = Status.Failure;
                    model.Message = ex.Message;
                }
            }
            return model;
        }
        public List<HelpGuidViewModel> GetHelpGuidList()
        {
            HelpGuidViewModel model = new HelpGuidViewModel();
            model.HelpList = (from a in context.HelpGuid
                              select new HelpGuidViewModel
                              {
                                  Id = a.Id,
                                  Description = a.Description,
                                  Title = a.Title

                              }).ToList();
            return model.HelpList;
        }
        public HelpGuidViewModel GetHelpGuidData(Guid Id)
        {
            HelpGuidViewModel model = new HelpGuidViewModel();
            var item = context.HelpGuid.FirstOrDefault(a => a.Id == Id);
            model = (from a in context.HelpGuid
                     where a.Id == Id
                     select new HelpGuidViewModel
                     {
                         Title = a.Title,
                         Id = a.Id,
                         CreatedBy = a.CreatedBy,
                         CreatedDate = a.CreatedDate,
                         Description = a.Description,
                         UpdatedDate = a.UpdatedDate

                     }).FirstOrDefault();
            return model;
        }
        public async Task<ResponseModel> UpdateHelpGuidDetails(HelpGuidViewModel Helpdata)
        {
            ResponseModel response = new ResponseModel();

            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var Info = context.HelpGuid.FirstOrDefault(x => x.Id == Helpdata.Id);
                    if (Info != null)
                    {

                        Info.Title = Helpdata.Title;
                        Info.Description = Helpdata.Description;
                        Info.UpdatedDate = DateTime.Now;
                        context.HelpGuid.Update(Info);
                    }
                    await context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    response.Status = Status.Success;
                }
                catch (Exception ex)
                {
                    logger.Error(ex);
                    response.Status = Status.Failure;
                    await transaction.RollbackAsync();
                }
            }
            return response;
        }
        public async Task<ResponseModel> DeleteHelpGuidDetails(Guid Id)
        {
            ResponseModel model = new ResponseModel();
            var Info = context.HelpGuid.FirstOrDefault(a => a.Id == Id);
            if (Info != null)
            {
                var list = context.HelpGuid.Find(Info.Id);
                if (Info != null)
                {
                    context.Remove(list);
                    await context.SaveChangesAsync();
                    model.Status = Status.Success;
                }
            }

            return model;
        }

        public (int TotalCount, int FilteredCount, dynamic News) GetHelpGuidDetails(int skip, int take, string sortColumn, string sortColumnDir, string searchValue)
        {
            var HelpGuiddata = context.HelpGuid.Select(x => new
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description
            });
            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            {
                switch (sortColumn)
                {
                    case "Title":
                        HelpGuiddata = sortColumnDir == "asc" ? HelpGuiddata.OrderBy(x => x.Title) : HelpGuiddata.OrderByDescending(x => x.Title);
                        break;

                }

            }
            if (!string.IsNullOrEmpty(searchValue))
            {
                HelpGuiddata = HelpGuiddata.Where(m => m.Title.Contains(searchValue));
            }
            int recordsTotal = HelpGuiddata.Count();
            var data = HelpGuiddata.Select(x => new
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description
            }).Skip(skip).Take(take).ToList();
            return (recordsTotal, recordsTotal, data);
        }


        public async Task<ResponseModel> SaveTermPrivacydata(TermPrivacyViewModel TermPrivacydata)
        {
            ResponseModel model = new ResponseModel();
            using (var transection = await context.Database.BeginTransactionAsync())
            {

                try
                {
                    TermPrivacy customerTermPrivacy = new TermPrivacy()
                    {
                        Id = Guid.NewGuid(),
                        Title = TermPrivacydata.Title,
                        Description = TermPrivacydata.Description,
                        CreatedBy = TermPrivacydata.CreatedBy,
                        CreatedDate = DateTime.Now,
                        UpdatedDate = DateTime.Now,

                    };
                    context.TermPrivacy.Add(customerTermPrivacy);
                    context.SaveChanges();

                    await context.SaveChangesAsync();
                    await transection.CommitAsync();
                    model.Status = Status.Success;
                }
                catch (Exception ex)
                {
                    await transection.RollbackAsync();
                    model.Status = Status.Failure;
                    model.Message = ex.Message;
                }
            }
            return model;
        }
        public List<TermPrivacyViewModel> GetTermPrivacyList()
        {
            TermPrivacyViewModel model = new TermPrivacyViewModel();
            model.TermPrivacyList = (from a in context.TermPrivacy
                                     select new TermPrivacyViewModel
                                     {
                                         Id = a.Id,
                                         Description = a.Description,
                                         Title = a.Title

                                     }).ToList();
            return model.TermPrivacyList;
        }
        public TermPrivacyViewModel GetTermPrivacyData(Guid Id)
        {
            TermPrivacyViewModel model = new TermPrivacyViewModel();
            var item = context.HelpGuid.FirstOrDefault(a => a.Id == Id);
            model = (from a in context.TermPrivacy
                     where a.Id == Id
                     select new TermPrivacyViewModel
                     {
                         Title = a.Title,
                         Id = a.Id,
                         CreatedBy = a.CreatedBy,
                         CreatedDate = a.CreatedDate,
                         Description = a.Description,
                         UpdatedDate = a.UpdatedDate

                     }).FirstOrDefault();
            return model;
        }
        public async Task<ResponseModel> UpdaTermPrivacyDetails(TermPrivacyViewModel TermPrivacydata)
        {
            ResponseModel response = new ResponseModel();

            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var Info = context.TermPrivacy.FirstOrDefault(x => x.Id == TermPrivacydata.Id);
                    if (Info != null)
                    {

                        Info.Title = TermPrivacydata.Title;
                        Info.Description = TermPrivacydata.Description;
                        Info.UpdatedDate = DateTime.Now;
                        context.TermPrivacy.Update(Info);
                    }
                    await context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    response.Status = Status.Success;
                }
                catch (Exception ex)
                {
                    logger.Error(ex);
                    response.Status = Status.Failure;
                    await transaction.RollbackAsync();
                }
            }
            return response;
        }
        public async Task<ResponseModel> DeleteTermPrivacytDetails(Guid Id)
        {
            ResponseModel model = new ResponseModel();
            var Info = context.TermPrivacy.FirstOrDefault(a => a.Id == Id);
            if (Info != null)
            {
                var list = context.TermPrivacy.Find(Info.Id);
                if (Info != null)
                {
                    context.Remove(list);
                    await context.SaveChangesAsync();
                    model.Status = Status.Success;
                }
            }

            return model;
        }
        public (int TotalCount, int FilteredCount, dynamic News) GetTermPrivacyDetails(int skip, int take, string sortColumn, string sortColumnDir, string searchValue)
        {
            var TermPrivacydata = context.TermPrivacy.Select(x => new
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description
            });
            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            {
                switch (sortColumn)
                {
                    case "Title":
                        TermPrivacydata = sortColumnDir == "asc" ? TermPrivacydata.OrderBy(x => x.Title) : TermPrivacydata.OrderByDescending(x => x.Title);
                        break;

                }

            }
            if (!string.IsNullOrEmpty(searchValue))
            {
                TermPrivacydata = TermPrivacydata.Where(m => m.Title.Contains(searchValue));
            }
            int recordsTotal = TermPrivacydata.Count();
            var data = TermPrivacydata.Select(x => new
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description
            }).Skip(skip).Take(take).ToList();
            return (recordsTotal, recordsTotal, data);
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
        public async Task<ResponseModel> DeleteGalleryImage(string Id)
        {
            ResponseModel model = new ResponseModel();
            var Info = context.ProductGallery.FirstOrDefault(x => x.Id == new Guid(Id));
            if (Info != null)
            {
                var data = context.ProductGallery.Find(Info.Id);
                if (data != null)
                {
                    context.ProductGallery.Remove(data);
                }
                await context.SaveChangesAsync();
                model.Status = Status.Success;
            }
            return model;
        }
        public async Task<TrackingFormsViewModel> GetCertificateData(int Id)
        {
            TrackingFormsViewModel model = new TrackingFormsViewModel();
            try
            {
                model = (from trackForm in context.TrackingForms
                         join custInfo in context.CustomerInfo on trackForm.CustId equals custInfo.Id
                         join custAddress in context.CustomerAddressMasters on custInfo.CustomerAddressId equals custAddress.Id
                         join state in context.StateMaters on custAddress.StateId equals state.Id
                         join district in context.DistricttMasters on custAddress.DistricttId equals district.Id
                         join product in context.ProductMasters on trackForm.TestkitId equals product.Id
                         select new TrackingFormsViewModel
                         {
                             Id = trackForm.Id,
                             TestkitId = trackForm.TestkitId,
                             Place = trackForm.Place,
                             MobileNo = trackForm.MobileNo,
                             Time = trackForm.Time,
                             Date = trackForm.Date,
                             TestResults = trackForm.TestResults,
                             FileUrl = trackForm.FileUrl,
                             IsVerified = trackForm.IsVerified,
                             Email = custInfo.Email,
                             CustId = custInfo.Id,
                             Address = custAddress.Address,
                             StateName = state.StateName,
                             DistricttName = district.DistricttName,
                             ProductName = product.ProductName,
                             AuthentiCode = product.Authenticode,
                             INVNo = product.INVNo,
                             QrCode = trackForm.QrCode
                         }).FirstOrDefault(x => x.Id == Id);
                var data = context.TrackingForms.Where(x => x.Id == Id).FirstOrDefault();
                if (data.VerifiedBy!=null && data.VerifiedBy!="")
                {
                    Guid id =new Guid(data.VerifiedBy);
                    model.Name = context.CustomerInfo.Where(x => x.Id == id).FirstOrDefault().Name;
                }
            }
            catch (Exception ex)
            {

            }
            return model;
        }

        //public List<OrderHistory> OrderHistoryList()
        //{
        //    List<OrderHistory> details = new List<OrderHistory>();
        //    try
        //    {
        //        details = (from a in context.Order
        //                   join b in context.CustomerInfo on a.UserId equals b.Id
        //                   join c in context.Payment on a.PaymentId equals c.Id
        //                   join d in context.Order_Items on a.Id equals d.OrderId
        //                   join e in context.ShippingTracking on a.Id equals e.OrderId
        //                   select new OrderHistory
        //                   {
        //                       Id = a.Id.ToString(),
        //                       Order_Id = a.Id.ToString(),
        //                       UserName = b.Name,
        //                       Paymentstatus = c.Status,
        //                       OrderDate = a.CreatedDate,
        //                       TotalAmount = a.TotalAmount,
        //                       Shippingstatus = e.Status,
        //                       CreatedDate=a.CreatedDate,
        //                       Qty = context.Order_Items.Where(y => y.OrderId == a.Id).Sum(y => y.Qty).ToString()
        //                   }).OrderByDescending(x=>x.CreatedDate).ToList();
        //        return details;
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}
        public (int TotalCount, int FilteredCount, dynamic Categorys) OrderHistoryList(int skip, int take, string sortColumn, string sortColumnDir, string searchValue)
        {
            List<OrderHistory> Orders = new List<OrderHistory>();
            Orders = (from a in context.Order
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
            int recordsTotal = Orders.Count();
            var data = Orders.Select(x => new
            {
                Id = x.Id.ToString(),
                Order_Id = x.Order_Id.ToString(),
                UserName = x.UserName,
                Paymentstatus = x.Paymentstatus,
                OrderDate = x.OrderDate.ToString("dd-MM-yyyy"),
                TotalAmount = x.TotalAmount,
            }).Skip(skip).Take(take).ToList();
            return (recordsTotal, recordsTotal, data);
        }

        public async Task<ResponseModel> Saveform(string Name, Guid UserId)
        {
            ResponseModel res = new ResponseModel();
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    Form form = new Form();
                    form.ID = Guid.NewGuid();
                    form.Name = Name;
                    form.CreatedBy = UserId;
                    form.CreatedDate = DateTime.Now;
                    context.Form.Add(form);
                    res.OrderId = form.ID;
                    await context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    res.Status = Status.Success;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    res.Status = Status.Failure;
                    res.Message = ex.Message;
                }
            }
            return res;
        }
        public (int TotalCount, int FilteredCount, dynamic Customers) GetStaffRollList(int skip, int take, string sortColumn, string sortColumnDir, string searchValue)
        {
            var customers = context.CustomerInfo.Where(x => x.IsActive && x.UserType != UserType.Admin && x.UserType != UserType.SubAdmin).Select(x => new
            {
                Id = x.Id,
                Name = x.Name,
                Email = x.Email,
                UserType = x.UserType,

            });
            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            {
                switch (sortColumn)
                {
                    case "Name":
                        customers = sortColumnDir == "asc" ? customers.OrderBy(x => x.Email) : customers.OrderByDescending(x => x.Name);
                        break;
                    case "Email":
                        customers = sortColumnDir == "asc" ? customers.OrderBy(x => x.Email) : customers.OrderByDescending(x => x.Email);
                        break;
                }

            }
            if (!string.IsNullOrEmpty(searchValue))
            {
                customers = customers.Where(m => m.Name.Contains(searchValue) || m.Email.Contains(searchValue));
            }
            int recordsTotal = customers.Count();
            var data = customers.Select(x => new
            {
                Id = x.Id,
                Name = x.Name,
                Email = x.Email,
                UserType = x.UserType,
            }).Skip(skip).Take(take).ToList();
            return (recordsTotal, recordsTotal, data);
        }

        public async Task<ResponseModel> UpdateRole(CustomerInfo model)
        {
            ResponseModel response = new ResponseModel();

            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var Info = context.CustomerInfo.FirstOrDefault(x => x.Id == model.Id);
                    if (Info != null)
                    {
                        Info.UserType = model.UserType;
                        context.CustomerInfo.Update(Info);
                    }
                    await context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    response.Status = Status.Success;
                }
                catch (Exception ex)
                {
                    logger.Error(ex);
                    response.Status = Status.Failure;
                    await transaction.RollbackAsync();
                }
            }
            return response;
        }

        public async Task<ResponseModel> SaveSimpleText(FormPropertyViewModel data)
        {
            ResponseModel res = new ResponseModel();
            int row = 1;
            using (var tarnsaction = context.Database.BeginTransaction())
            {
                try
                {
                    if (data.Type == 0)
                    {
                        data.InputType = FieldType.SingleLineText;
                    }
                    if (data.Type == 1)
                    {
                        data.InputType = FieldType.ParagraphText;
                    }
                    if (data.Type == 5)
                    {
                        data.InputType = FieldType.Numbers;
                    }
                    if (data.Type == 6)
                    {
                        data.InputType = FieldType.Name;
                    }
                    if (data.Type == 7)
                    {
                        data.InputType = FieldType.Email;
                    }
                    if (data.Type == 8)
                    {
                        data.InputType = FieldType.NumberSlider;
                    }
                    if (data.Type == 9)
                    {
                        data.InputType = FieldType.Captcha;
                    }
                    if (data.Type == 10)
                    {
                        data.InputType = FieldType.Photo;
                    }
                    var data1 = context.FormProperty.Where(x => x.FormId == new Guid(data.fId)).ToList();
                    if (data1.Count != 0)
                    {
                        int maxnum = data1.Max(x => Convert.ToInt32(x.RowNumber));
                        row = maxnum + 1;
                    }
                    else
                    {
                        row =  1;
                    }
                    FormProperty fp = new FormProperty();
                    fp.Id = Guid.NewGuid();
                    fp.FormId = new Guid(data.fId);
                    fp.RowNumber = row.ToString();
                    fp.CreatedDate = DateTime.Now;
                    fp.FieldType = data.InputType;
                    fp.Name = data.Name;
                    fp.CreatedBy = data.CreatedBy;
                    fp.Captcha = data.Captcha;
                    context.FormProperty.Add(fp);
                    await context.SaveChangesAsync();
                    await tarnsaction.CommitAsync();
                    res.Status = Status.Success;
                }
                catch (Exception)
                {
                    res.Status = Status.Failure;
                    await tarnsaction.RollbackAsync();
                }
            }
            return res;
        }

        public async Task<ResponseModel> SaveTextField(FormPropertyViewModel data)
        {
            ResponseModel res = new ResponseModel();
            int row = 1;
            using (var tarnsaction = context.Database.BeginTransaction())
            {
                try
                {
                    if (data.Type == 0)
                    {
                        data.InputType = FieldType.SingleLineText;
                    }
                    if (data.Type == 1)
                    {
                        data.InputType = FieldType.ParagraphText;
                    }
                    if (data.Type == 5)
                    {
                        data.InputType = FieldType.Numbers;
                    }
                    if (data.Type == 6)
                    {
                        data.InputType = FieldType.Name;
                    }
                    if (data.Type == 7)
                    {
                        data.InputType = FieldType.Email;
                    }
                    if (data.Type == 8)
                    {
                        data.InputType = FieldType.NumberSlider;
                    }
                    if (data.Type == 9)
                    {
                        data.InputType = FieldType.Captcha;
                    }
                    if (data.Type == 10)
                    {
                        data.InputType = FieldType.Photo;
                    }
                    if (data.Type == 11)
                    {
                        data.InputType = FieldType.TextField;
                    }
                    if (data.Type == 12)
                    {
                        data.InputType = FieldType.DatePicker;
                    }
                    if (data.Type == 13)
                    {
                        data.InputType = FieldType.EIN_No;
                    }
                    if (data.Type == 14)
                    {
                        data.InputType = FieldType.HeadingTextField;
                    }
                    if (data.Type == 15)
                    {
                        data.InputType = FieldType.CheckboxBlack;
                    }
                    if (data.Type == 16)
                    {
                        data.InputType = FieldType.ListTextField;
                    }
                    if (data.Type == 17)
                    {
                        data.InputType = FieldType.MultiLineText;
                    }
                    if (data.Type == 18)
                    {
                        data.InputType = FieldType.LeftRightText;
                    }

                    var data1 = context.FormProperty.Where(x => x.FormId == new Guid(data.fId)).ToList();
                    if (data1.Count != 0)
                    {
                        int maxnum = data1.Max(x => Convert.ToInt32(x.RowNumber));
                        row = maxnum + 1;
                    }
                    else
                    {
                        row = 1;
                    }
                    FormProperty fp = new FormProperty();
                    fp.Id = Guid.NewGuid();
                    fp.FormId = new Guid(data.fId);
                    fp.RowNumber = row.ToString();
                    fp.CreatedDate = DateTime.Now;
                    fp.FieldType = data.InputType;
                    fp.Name = data.Name;
                    fp.CreatedBy = data.CreatedBy;
                    fp.Captcha = data.Captcha;
                    context.FormProperty.Add(fp);
                    await context.SaveChangesAsync();
                    await tarnsaction.CommitAsync();
                    res.Status = Status.Success;
                }
                catch (Exception)
                {
                    res.Status = Status.Failure;
                    await tarnsaction.RollbackAsync();
                }
            }
            return res;
        }

        public async Task<ResponseModel> SaveDateField(FormPropertyViewModel data)
        {
            ResponseModel res = new ResponseModel();
            int row = 1;
            using (var tarnsaction = context.Database.BeginTransaction())
            {
                try
                {
                    if (data.Type == 0)
                    {
                        data.InputType = FieldType.SingleLineText;
                    }
                    if (data.Type == 1)
                    {
                        data.InputType = FieldType.ParagraphText;
                    }
                    if (data.Type == 5)
                    {
                        data.InputType = FieldType.Numbers;
                    }
                    if (data.Type == 6)
                    {
                        data.InputType = FieldType.Name;
                    }
                    if (data.Type == 7)
                    {
                        data.InputType = FieldType.Email;
                    }
                    if (data.Type == 8)
                    {
                        data.InputType = FieldType.NumberSlider;
                    }
                    if (data.Type == 9)
                    {
                        data.InputType = FieldType.Captcha;
                    }
                    if (data.Type == 10)
                    {
                        data.InputType = FieldType.Photo;
                    }
                    if (data.Type == 11)
                    {
                        data.InputType = FieldType.TextField;
                    }
                    if (data.Type == 12)
                    {
                        data.InputType = FieldType.DatePicker;
                    }
                    var data1 = context.FormProperty.Where(x => x.FormId == new Guid(data.fId)).ToList();
                    if (data1.Count != 0)
                    {
                        int maxnum = data1.Max(x => Convert.ToInt32(x.RowNumber));
                        row = maxnum + 1;
                    }
                    else
                    {
                        row = 1;
                    }
                    FormProperty fp = new FormProperty();
                    fp.Id = Guid.NewGuid();
                    fp.FormId = new Guid(data.fId);
                    fp.RowNumber = row.ToString();
                    fp.CreatedDate = DateTime.Now;
                    fp.FieldType = data.InputType;
                    fp.Name = data.Name;
                    fp.CreatedBy = data.CreatedBy;
                    fp.Captcha = data.Captcha;
                    context.FormProperty.Add(fp);
                    await context.SaveChangesAsync();
                    await tarnsaction.CommitAsync();
                    res.Status = Status.Success;
                }
                catch (Exception)
                {
                    res.Status = Status.Failure;
                    await tarnsaction.RollbackAsync();
                }
            }
            return res;
        }


        public async Task<ResponseModel> Savecheckbox(FormPropertyViewModel data)
        {
            ResponseModel res = new ResponseModel();
            int row = 1;
            int checkrow = 1;
            using (var tarnsaction = context.Database.BeginTransaction())
            {
                try
                {
                    var data1 = context.FormProperty.Where(x => x.FormId == new Guid(data.fId)).ToList();
                    if (data1.Count != 0)
                    {
                        int maxnum = data1.Max(x => Convert.ToInt32(x.RowNumber));
                        row = maxnum + 1;
                    }
                    if (data.Type == 4 || data.Type == 16)
                    {
                        
                        if(data.Type == 16)
                        {
                            data.InputType = FieldType.ListTextField;

                        }
                        else
                        {
                            data.InputType = FieldType.Checkboxes;
                        }
                        

                        FormProperty fp = new FormProperty();
                        fp.Id = Guid.NewGuid();
                        fp.FormId = new Guid(data.fId);
                        fp.RowNumber = row.ToString();
                        fp.CreatedDate = DateTime.Now;
                        fp.FieldType = data.InputType;
                        fp.Name = data.Name;
                        fp.CreatedBy = data.CreatedBy;
                        context.FormProperty.Add(fp);
                        for (var item = 0; item < data.CheckBoxView.CheckboxName.Length; item++)
                        {
                            CheckBox check = new CheckBox();
                            check.Id = Guid.NewGuid();
                            check.FormPropertyId = fp.Id;
                            check.CreatedDate = DateTime.Now;
                            check.RowNumber = checkrow;
                            check.Name = data.CheckBoxView.CheckboxName[item];
                            context.CheckBox.Add(check);
                            checkrow++;
                        }
                        res.Status = Status.Success;
                        await context.SaveChangesAsync();
                        await tarnsaction.CommitAsync();
                    }
                    if (data.Type == 3)
                    {
                        data.InputType = FieldType.MultipleChoice;
                     
                        FormProperty fp = new FormProperty();
                        fp.Id = Guid.NewGuid();
                        fp.FormId = new Guid(data.fId);
                        fp.RowNumber = row.ToString();
                        fp.CreatedDate = DateTime.Now;
                        fp.FieldType = data.InputType;
                        fp.Name = data.Name;
                        fp.CreatedBy = data.CreatedBy;
                        context.FormProperty.Add(fp);
                        for (var item = 0; item < data.MultipleChoiceView.ChoiceName.Length; item++)
                        {
                            MultipleChoice check = new MultipleChoice();
                            check.Id = Guid.NewGuid();
                            check.FormPropertyId = fp.Id;
                            check.CreatedDate = DateTime.Now;
                            check.RowNumber = checkrow;
                            check.Name = data.MultipleChoiceView.ChoiceName[item];
                            context.MultipleChoice.Add(check);
                            checkrow++;
                        }
                        res.Status = Status.Success;
                        await context.SaveChangesAsync();
                        await tarnsaction.CommitAsync();
                    }
                    if (data.Type == 2)
                    {
                        data.InputType = FieldType.Dropdown;
                      
                        FormProperty fp = new FormProperty();
                        fp.Id = Guid.NewGuid();
                        fp.FormId = new Guid(data.fId);
                        fp.RowNumber = row.ToString();
                        fp.CreatedDate = DateTime.Now;
                        fp.FieldType = data.InputType;
                        fp.Name = data.Name;
                        fp.CreatedBy = data.CreatedBy;
                        context.FormProperty.Add(fp);
                        for (var item = 0; item < data.DropdownView.OptionName.Length; item++)
                        {
                            Dropdown check = new Dropdown();
                            check.Id = Guid.NewGuid();
                            check.FormPropertyId = fp.Id;
                            check.CreatedDate = DateTime.Now;
                            check.RowNumber = checkrow;
                            check.Name = data.DropdownView.OptionName[item];
                            context.Dropdown.Add(check);
                            checkrow++;
                        }
                        res.Status = Status.Success;
                        await context.SaveChangesAsync();
                        await tarnsaction.CommitAsync();
                    }
                }
                catch (Exception)
                {
                    res.Status = Status.Failure;
                    await tarnsaction.RollbackAsync();
                }
            }
            return res;
        }

        public async Task<ResponseModel> SaveBlackcheckbox(FormPropertyViewModel data)
        {
            ResponseModel res = new ResponseModel();
            int row = 1;
            int checkrow = 1;
            using (var tarnsaction = context.Database.BeginTransaction())
            {
                try
                {
                    var data1 = context.FormProperty.Where(x => x.FormId == new Guid(data.fId)).ToList();
                    if (data1.Count != 0)
                    {
                        int maxnum = data1.Max(x => Convert.ToInt32(x.RowNumber));
                        row = maxnum + 1;
                    }
                    if (data.Type == 15)
                    {
                        data.InputType = FieldType.CheckboxBlack;

                        FormProperty fp = new FormProperty();
                        fp.Id = Guid.NewGuid();
                        fp.FormId = new Guid(data.fId);
                        fp.RowNumber = row.ToString();
                        fp.CreatedDate = DateTime.Now;
                        fp.FieldType = data.InputType;
                        fp.Name = data.Name;
                        fp.CreatedBy = data.CreatedBy;
                        context.FormProperty.Add(fp);
                        for (var item = 0; item < data.CheckBoxView.CheckboxName.Length; item++)
                        {
                            CheckBox check = new CheckBox();
                            check.Id = Guid.NewGuid();
                            check.FormPropertyId = fp.Id;
                            check.CreatedDate = DateTime.Now;
                            check.RowNumber = checkrow;
                            check.Name = data.CheckBoxView.CheckboxName[item];
                            context.CheckBox.Add(check);
                            checkrow++;
                        }
                        res.Status = Status.Success;
                        await context.SaveChangesAsync();
                        await tarnsaction.CommitAsync();
                    }
                     }
                catch (Exception)
                {
                    res.Status = Status.Failure;
                    await tarnsaction.RollbackAsync();
                }
            }
            return res;
        }


        public async Task<FormPropertyViewModel> GetForm(Guid? Id)
        {
            FormPropertyViewModel data = new FormPropertyViewModel();
            try
            {
                if (Id != null)
                {
                    data.FormPropertyViewList = (from a in context.Form.Where(x => x.ID == Id)
                                                 join b in context.FormProperty on a.ID equals b.FormId
                                                 select new FormPropertyViewModel
                                                 {
                                                     FormName = a.Name,
                                                     Name = b.Name,
                                                     RowNumber = Convert.ToInt32(b.RowNumber),
                                                     InputType = b.FieldType,
                                                     Captcha = b.Captcha,
                                                     Id = b.Id,
                                                     CheckBoxViewList = (from c in context.CheckBox.Where(x => x.FormPropertyId == b.Id)
                                                                         select new CheckBoxViewModel
                                                                         {
                                                                             CheckboxText = c.Name,
                                                                             RowNumber = c.RowNumber,
                                                                             Id=c.Id
                                                                         }).OrderBy(x => x.RowNumber).ToList(),
                                                     MultipleChoiceViewList = (from d in context.MultipleChoice.Where(x => x.FormPropertyId == b.Id)
                                                                               select new MultipleChoiceViewModel
                                                                               {
                                                                                   ChoiceText = d.Name,
                                                                                   RowNumber = d.RowNumber,
                                                                                   Id=d.Id
                                                                               }).OrderBy(x => x.RowNumber).ToList(),
                                                     DropdownViewList = (from e in context.Dropdown.Where(x => x.FormPropertyId == b.Id)
                                                                         select new DropdownViewModel
                                                                         {
                                                                             OptionText = e.Name,
                                                                             RowNumber = e.RowNumber,
                                                                             Id=e.Id
                                                                         }).OrderBy(x => x.RowNumber).ToList(),
                                                 }).OrderBy(x => x.RowNumber).ToList();
                    data.formview = (from f in context.Form.Where(x => x.ID == Id)
                                     select new FormView
                                     {
                                         FormId = f.ID,
                                         FormName = f.Name
                                     }).FirstOrDefault();
                }
            }
            catch (Exception)
            {

                throw;
            }
            return data;
        }

        public ResponseModel deleteformproperty(Guid? Id)
        {
            ResponseModel res = new ResponseModel();
            try
            {
                var data = context.FormProperty.Find(Id);
                if (data != null)
                {
                    if (data.FieldType == FieldType.Dropdown)
                    {
                        var data1 = context.Dropdown.Where(x => x.FormPropertyId == data.Id).ToList();
                        if (data1 != null)
                        {
                            foreach (var item in data1)
                            {
                                context.Dropdown.Remove(item);
                                context.SaveChanges();
                            }
                        }
                        context.FormProperty.Remove(data);
                        context.SaveChanges();
                    }
                    else if (data.FieldType == FieldType.Checkboxes)
                    {
                        var data1 = context.CheckBox.Where(x => x.FormPropertyId == data.Id).ToList();
                        if (data1 != null)
                        {
                            foreach (var item in data1)
                            {
                                context.CheckBox.Remove(item);
                                context.SaveChanges();
                            }

                        }
                        context.FormProperty.Remove(data);
                        context.SaveChanges();
                    }
                    else if (data.FieldType == FieldType.MultipleChoice)
                    {
                        var data1 = context.MultipleChoice.Where(x => x.FormPropertyId == data.Id).ToList();
                        if (data1 != null)
                        {
                            foreach (var item in data1)
                            {
                                context.MultipleChoice.Remove(item);
                                context.SaveChanges();
                            }
                        }
                        context.FormProperty.Remove(data);
                        context.SaveChanges();
                    }
                    else
                    {
                        context.FormProperty.Remove(data);
                        context.SaveChanges();
                    }
                    res.Status = Status.Success;
                }
            }
            catch (Exception ex)
            {
                res.Status = Status.Failure;
            }
            return res;
        }
        public async Task<int> UpdateNumber(Guid? Id)
        {
            var Res = 0;
            int b = 1;
            try
            {
                var formproperty = context.FormProperty.Where(x => x.FormId == Id).OrderBy(x => Convert.ToInt32(x.RowNumber)).ToList();
                if (formproperty != null)
                {
                    foreach (var item in formproperty)
                    {
                        var fp = context.FormProperty.Find(item.Id);
                        fp.RowNumber = b.ToString();
                        b++;
                    }

                }
                Res = await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
            return Res;
        }
        public (int TotalCount, int FilteredCount, dynamic Customers) getform(int skip, int take, string sortColumn, string sortColumnDir, string searchValue)
        {
            var customers = context.Form.Select(x => new
            {
                Id = x.ID,
                Name = x.Name,
                CreatedDate = x.CreatedDate,
                Product = (context.ProductMasters.Where(p => p.FormIds == x.ID.ToString()).FirstOrDefault().ProductName)
            }) ;
            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            {
                switch (sortColumn)
                {
                    case "FormName":
                        customers = sortColumnDir == "asc" ? customers.OrderBy(x => x.Name) : customers.OrderByDescending(x => x.Name);
                        break;
                    case "CreatedDate":
                        customers = sortColumnDir == "asc" ? customers.OrderBy(x => x.CreatedDate) : customers.OrderByDescending(x => x.CreatedDate);
                        break;
                }

            }
            else
            {
                customers = customers.OrderByDescending(x => x.CreatedDate);
            }
            if (!string.IsNullOrEmpty(searchValue))
            {
                customers = customers.Where(x => x.Name.Contains(searchValue) || x.CreatedDate.ToString("dd-MM-yyyy").Contains(searchValue));
            }
            int recordsTotal = customers.Count();
           
            var data = customers.Select(x => new
            {
                Id = x.Id,
                Name = x.Name,
                CreatedDate = x.CreatedDate.ToString("dd-MM-yyyy"),
                Product=x.Product
            }).Skip(skip).Take(take).ToList();
            return (recordsTotal, recordsTotal, data);
        }
        public async Task<ResponseModel> DeleteFormDetails(Guid? Id)
        {
            ResponseModel model = new ResponseModel();
            try
            {
                var Info = context.Form.FirstOrDefault(x => x.ID == Id);
                if (Info != null)
                {
                    var data = context.FormProperty.Where(x => x.FormId == Info.ID).ToList();
                    if (data != null)
                    {
                        foreach (var item11 in data)
                        {

                            if (item11.FieldType == FieldType.Dropdown)
                            {
                                var data1 = context.Dropdown.Where(x => x.FormPropertyId == item11.Id).ToList();
                                if (data1 != null)
                                {
                                    foreach (var item in data1)
                                    {
                                        context.Dropdown.Remove(item);
                                        await context.SaveChangesAsync();
                                    }
                                }
                                context.FormProperty.Remove(item11);
                                await context.SaveChangesAsync();
                            }
                            else if (item11.FieldType == FieldType.Checkboxes)
                            {
                                var data1 = context.CheckBox.Where(x => x.FormPropertyId == item11.Id).ToList();
                                if (data1 != null)
                                {
                                    foreach (var item in data1)
                                    {
                                        context.CheckBox.Remove(item);
                                        await context.SaveChangesAsync();
                                    }

                                }
                                context.FormProperty.Remove(item11);
                                await context.SaveChangesAsync();
                            }
                            else if (item11.FieldType == FieldType.MultipleChoice)
                            {
                                var data1 = context.MultipleChoice.Where(x => x.FormPropertyId == item11.Id).ToList();
                                if (data1 != null)
                                {
                                    foreach (var item in data1)
                                    {
                                        context.MultipleChoice.Remove(item);
                                        await context.SaveChangesAsync();
                                    }
                                }
                                context.FormProperty.Remove(item11);
                                await context.SaveChangesAsync();
                            }
                            else
                            {
                                context.FormProperty.Remove(item11);
                                await context.SaveChangesAsync();
                            }
                        }
                    }
                    context.Form.Remove(Info);
                    await context.SaveChangesAsync();
                    model.Status = Status.Success;
                }
            }
            catch (Exception)
            {
                model.Status = Status.Failure;
            }
            return model;
        }
        public async Task<ResponseModel> saveformresponce(FormPropertyViewModel data)
        {
            ResponseModel res = new ResponseModel();
           using(var transaction=context.Database.BeginTransaction())
            {
                try
                {
                    if (data.FormPropertyViewList!=null)
                    {
                        FormResponce fore = new FormResponce();
                        fore.ResponseId = Guid.NewGuid();
                        fore.FormId = data.formview.FormId;
                        fore.FormName = data.formview.FormName;
                        fore.CustomerId = data.CustomerId;
                        fore.CreatedDate =DateTime.Now;
                        context.FormResponce.Add(fore);
                        foreach (var item in data.FormPropertyViewList)
                        {
                            FormPropertyResponce fpr = new FormPropertyResponce();
                            fpr.Id = Guid.NewGuid();
                            fpr.FormResponceId = fore.ResponseId;
                            fpr.FormPropertyId = item.Id;
                            fpr.FieldType = item.InputType;
                            fpr.CreatedBy = data.CustomerId;
                            fpr.CreatedDate = DateTime.Now;
                            if (item.InputType==FieldType.Dropdown)
                            {
                               
                                fpr.DropdownValue = item.NameValue;
                            }
                            else if(item.InputType == FieldType.Photo)
                            {
                                fpr.ResponceText = data.File;
                            }
                            else if (item.InputType == FieldType.Checkboxes)
                            {
                               
                                foreach (var item1 in item.CheckBoxViewList)
                                {
                                    CheckboxResponce cr = new CheckboxResponce();
                                    cr.Id = Guid.NewGuid();
                                    cr.FormPropertyResponceId = fpr.Id;
                                    cr.CheckboxId = item1.Id;
                                    cr.Check = item1.CheckboxValue;
                                    cr.CreatedDate = DateTime.Now;
                                    context.CheckboxResponce.Add(cr);
                                }
                              
                              
                            }

                            else if (item.InputType == FieldType.CheckboxBlack)
                            {

                                foreach (var item1 in item.CheckBoxViewList)
                                {
                                    CheckboxResponce cr = new CheckboxResponce();
                                    cr.Id = Guid.NewGuid();
                                    cr.FormPropertyResponceId = fpr.Id;
                                    cr.CheckboxId = item1.Id;
                                    cr.Check = item1.CheckboxValue;
                                    cr.CreatedDate = DateTime.Now;
                                    context.CheckboxResponce.Add(cr);
                                }


                            }

                            else if (item.InputType == FieldType.MultipleChoice)
                            {
                                fpr.MultipleChoiceValue=item.NameValue;                     
                            }
                            else if (item.InputType == FieldType.NumberSlider)
                            {
                                fpr.NumberSliderValue = Convert.ToInt32(item.NameValue);
                            }
                            else
                            {
                                fpr.ResponceText = item.NameValue;
                            }
                            context.FormPropertyResponce.Add(fpr);
                        }
                         await context.SaveChangesAsync();
                        await transaction.CommitAsync();
                        res.Status = Status.Success;
                    }
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    res.Status = Status.Failure;
                }
            }
            return res;
        }

        public async Task<CustomerInfoViewModel> GetProduct()
        {
            CustomerInfoViewModel cus = new CustomerInfoViewModel();
            try
            {
                cus.ListPro = (from a in context.CustomerInfo
                               select new ProList
                                {
                                    Id = a.Id,
                                    Name = a.Name,
                                    DateCreated = a.DateCreated
                               }).OrderByDescending(x => x.DateCreated).ToList();
            }
            catch (Exception ex)
            {

            }
            return cus;
        }

        public async Task<ResponseModel> assignproform(CustomerInfoViewModel data)
        {
            ResponseModel res = new ResponseModel();
            using (var transation = context.Database.BeginTransaction())
            {
                try
                {
                    if (data.FormIds!=null)
                    {
                        foreach (var item in data.ListPro1)
                        {
                            if (item.check==true)
                            {
                                var cus = context.CustomerInfo.FirstOrDefault(x => x.Id == item.Id);
                                if (cus!=null)
                                {
                                    cus.FormIds = data.FormIds;
                                    context.CustomerInfo.Update(cus);
                                }
                            }
                        }
                        await context.SaveChangesAsync();
                        await transation.CommitAsync();
                        res.Status = Status.Success;                
                    }
                }
                catch (Exception ex)
                {
                    await transation.RollbackAsync ();
                    res.Status = Status.Failure;
                }
            }
            return res;
        }

        public ResponseModel menuassign(MenuPermissionViewModel data)
        {
            ResponseModel res = new ResponseModel();
            try
            {                                                                                               
                var exist = context.MenuPermission.Where(x=>x.SubAdminId==data.SubAdminId).FirstOrDefault();
                if (exist==null)
                {
                    MenuPermission mp = new MenuPermission();
                    mp.Id= Guid.NewGuid();
                    foreach (var item in data.MenuList1)
                    {
                        if (item.Menuname== "Mall Manage" && item.check==true)
                        {
                            mp.MallManage = true;
                        }
                        else if(item.Menuname == "Customer Manage" && item.check == true)
                        {
                            mp.CustomerManage = true;
                        }
                        else if (item.Menuname == "Ticket Support Manage" && item.check == true)
                        {
                            mp.TicketSupportManage = true;
                        }
                        else if (item.Menuname == "Setting" && item.check == true)
                        {
                            mp.Setting = true;
                        }
                        else if (item.Menuname == "Authenticator Manage" && item.check == true)
                        {
                            mp.AuthenticatorManage = true;
                        }
                        else if (item.Menuname == "Front End Manage" && item.check == true)
                        {
                            mp.FrontEndManage = true;
                        }
                        else if (item.Menuname == "Certificate Manage" && item.check == true)
                        {
                            mp.CertificateManage = true;
                        }
                        else if (item.Menuname == "News Manage" && item.check == true)
                        {
                            mp.NewsManage = true;
                        }
                        else if (item.Menuname == "Form Manage" && item.check == true)
                        {
                            mp.FormManage = true;
                        }
                    }
                    mp.SubAdminId = data.SubAdminId;
                    mp.AdminId = data.AdminId;
                    mp.CreatedDate = DateTime.Now;
                    context.MenuPermission.Add(mp);
                    context.SaveChanges();
                    res.Status = Status.Success;
                }
                else
                {
                    foreach (var item in data.MenuList1)
                    {
                        if (item.Menuname == "Mall Manage")
                        {
                            exist.MallManage = item.check;
                        }
                        else if (item.Menuname == "Customer Manage" )
                        {
                            exist.CustomerManage = item.check;
                        }
                        else if (item.Menuname == "Ticket Support Manage" )
                        {
                            exist.TicketSupportManage = item.check;
                        }
                        else if (item.Menuname == "Setting" )
                        {
                            exist.Setting = item.check;
                        }
                        else if (item.Menuname == "Authenticator Manage" )
                        {
                            exist.AuthenticatorManage = item.check;
                        }
                        else if (item.Menuname == "Front End Manage")
                        {
                            exist.FrontEndManage = item.check;
                        }
                        else if (item.Menuname == "Certificate Manage")
                        {
                            exist.CertificateManage = item.check;
                        }
                        else if (item.Menuname == "News Manage" )
                        {
                            exist.NewsManage = item.check;
                        }
                        else if (item.Menuname == "Form Manage")
                        {
                            exist.FormManage = item.check;
                        }
                    }
                    exist.AdminId = data.AdminId;
                    exist.UpdatedDate = DateTime.Now;
                    context.MenuPermission.Update(exist);
                    context.SaveChanges();
                    res.Status = Status.Success;
                }
            }
            catch (Exception ex)
            {
                res.Status = Status.Failure;
            }

            return res;
        }
        public FormRes GetFormDetails()
        {
            FormRes model = new FormRes();
            try
            {
                model = (from a in context.FormResponce
                         select new FormRes
                         {
                             FormId = a.FormId,
                             FormName = a.FormName,
                             FormPropertyViewList = (from b in context.FormPropertyResponce.Where(x => x.FormResponceId == a.ResponseId)
                                                     select new FormPropertyViewModel
                                                     {
                                                         InputType = b.FieldType,
                                                         Name = context.FormProperty.Where(f => f.Id == b.FormPropertyId).FirstOrDefault().Name,
                                                         RowNumber = Convert.ToInt32(context.FormProperty.Where(f => f.Id == b.FormPropertyId).FirstOrDefault().RowNumber),
                                                         NameValue = b.ResponceText,
                                                         NumberSliderValue = b.NumberSliderValue,
                                                         DropdownView = (from d in context.Dropdown.Where(x => x.FormPropertyId == b.FormPropertyId)
                                                                         select new DropdownViewModel
                                                                         {
                                                                             OptionText = d.Name
                                                                         }).FirstOrDefault(),
                                                         MultipleChoiceViewList = (from e in context.MultipleChoice.Where(x => x.FormPropertyId == b.Id)
                                                                                   select new MultipleChoiceViewModel
                                                                                   {
                                                                                       Id = e.Id,
                                                                                       ChoiceText = e.Name,
                                                                                   }).ToList(),
                                                         CheckBoxViewList = (from f in context.CheckboxResponce.Where(x => x.FormPropertyResponceId == b.Id)
                                                                             select new CheckBoxViewModel
                                                                             {
                                                                                 Id = f.CheckboxId,
                                                                                 CheckboxValue = f.Check,
                                                                                 CheckboxText = context.CheckBox.Where(x => x.Id == f.CheckboxId).FirstOrDefault().Name,
                                                                             }).ToList(),
                                                     }).OrderBy(x => x.RowNumber).ToList()
                         }).FirstOrDefault();
            }
            catch (Exception ex)
            {

                throw;
            }

            return model;

        }
        public async Task<ResponseModel> TicketMessage(TicketMessageSystemViewModels TicketMessage)
        {
            ResponseModel model = new ResponseModel();
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    TicketMessageSystem data = new TicketMessageSystem();
                    data.Id = Guid.NewGuid();
                    data.Description = TicketMessage.Description;
                    data.UserId = TicketMessage.UserId;
                    data.TicketId = TicketMessage.TicketId;
                    data.CreatedDate = DateTime.Now;
                    data.CustomerId = TicketMessage.CustomerId;
                    data.NotificationAd = false;
                    data.NotificationCust = true;
                    context.TicketMessageSystem.Add(data);
                    await context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    model.Status = Status.Success;
                }
                catch (Exception ex)
                {
                    model.Status = Status.Failure;
                    await transaction.RollbackAsync();
                }

            }
            return model;
        }
        public List<TicketMessageSystemViewModels> ViewUserMessageList(Guid UserId, Guid? TicketId)
        {
            List<TicketMessageSystemViewModels> model = new List<TicketMessageSystemViewModels>();
            try
            {
                if (UserId != null)
                {
                    var Id = Guid.Empty;
                    var CustomerName = "";
                    var Name2 = context.CustomerInfo.Where(x => x.Id == UserId).FirstOrDefault();
                    var Cutomer = context.Ticket.Where(x => x.Id == TicketId).FirstOrDefault();
                    if (Cutomer.CustomerId!=null)
                    {
                         Id = new Guid(Cutomer.CustomerId);
                    }
                    var Name1 = context.CustomerInfo.Where(x => x.Id == Id).FirstOrDefault();
                    var UserName = Name2.Name;
                    if (Name1!=null)
                    {
                         CustomerName = Name1.Name;
                    }
                    model = (from a in context.TicketMessageSystem.Where(x =>x.TicketId==TicketId)
                             select new TicketMessageSystemViewModels
                             {
                                 Id = a.Id,
                                 Description = a.Description,
                                 CreatedDate = a.CreatedDate,
                                 CustomerId = a.CustomerId,
                                 UserId = a.UserId,
                                 UserName= UserName,
                                 CustomerName= CustomerName
                             }).OrderBy(z => z.CreatedDate).ToList();
                }
                return model;
            }
            catch (Exception ex)
            {

            }
            return model;
        }

        public (int TotalCount, int FilteredCount, dynamic Customers) GetCertificateList(int skip, int take, string sortColumn, string sortColumnDir, string searchValue, string data)
        {
            if (data == "1")
            {
                var customers = context.TrackingForms.Where(x => x.IsVerified == 1).Select(x => new
                {
                    Id = x.Id,
                    Name = x.Name,
                    ProductName = context.ProductMasters.Where(a => a.Id == x.TestkitId).FirstOrDefault().ProductName,
                    CertificateImage = x.CertificateImage,
                    IsVerified = x.IsVerified,
                    Date = x.Date

                });


                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    switch (sortColumn)
                    {
                        case "Name":
                            customers = sortColumnDir == "asc" ? customers.OrderBy(x => x.Name) : customers.OrderByDescending(x => x.Name);
                            break;
                        case "Date":
                            customers = sortColumnDir == "asc" ? customers.OrderBy(x => x.Date) : customers.OrderByDescending(x => x.Date);
                            break;

                    }

                }
                if (!string.IsNullOrEmpty(searchValue))
                {
                    customers = customers.Where(m => m.Name.Contains(searchValue));
                }
                int recordsTotal = customers.Count();
                var data1 = customers.Select(x => new
                {
                    Id = x.Id,
                    Name = x.Name,
                    ProductName = x.ProductName,
                    CertificateImage = x.CertificateImage,
                    IsVerified = x.IsVerified,
                    Date = x.Date

                }).Skip(skip).Take(take).ToList();
                return (recordsTotal, recordsTotal, data1);
            }
            else if (data == "0")
            {
                var customers = context.TrackingForms.Where(x => x.IsVerified == 0).Select(x => new
                {
                    Id = x.Id,
                    Name = x.Name,
                    ProductName = context.ProductMasters.Where(a => a.Id == x.TestkitId).FirstOrDefault().ProductName,
                    CertificateImage = x.CertificateImage,
                    IsVerified = x.IsVerified,
                    Date = x.Date
                });

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    switch (sortColumn)
                    {
                        case "Name":
                            customers = sortColumnDir == "asc" ? customers.OrderBy(x => x.Name) : customers.OrderByDescending(x => x.Name);
                            break;
                        case "Date":
                            customers = sortColumnDir == "asc" ? customers.OrderBy(x => x.Date) : customers.OrderByDescending(x => x.Date);
                            break;
                    }

                }
                if (!string.IsNullOrEmpty(searchValue))
                {
                    customers = customers.Where(m => m.Name.Contains(searchValue));
                }
                int recordsTotal = customers.Count();
                var data1 = customers.Select(x => new
                {
                    Id = x.Id,
                    Name = x.Name,
                    ProductName = x.ProductName,
                    CertificateImage = x.CertificateImage,
                    IsVerified = x.IsVerified,
                    Date = x.Date
                }).Skip(skip).Take(take).ToList();
                return (recordsTotal, recordsTotal, data1);
            }
            else
            {
                var customers = context.TrackingForms.Where(x => x.IsVerified == 2).Select(x => new
                {
                    Id = x.Id,
                    Name = x.Name,
                    ProductName = context.ProductMasters.Where(a => a.Id == x.TestkitId).FirstOrDefault().ProductName,
                    CertificateImage = x.CertificateImage,
                    IsVerified = x.IsVerified,
                    Date = x.Date

                });

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    switch (sortColumn)
                    {
                        case "Name":
                            customers = sortColumnDir == "asc" ? customers.OrderBy(x => x.Name) : customers.OrderByDescending(x => x.Name);
                            break;
                        case "Date":
                            customers = sortColumnDir == "asc" ? customers.OrderBy(x => x.Date) : customers.OrderByDescending(x => x.Date);
                            break;

                    }

                }
                if (!string.IsNullOrEmpty(searchValue))
                {
                    customers = customers.Where(m => m.Name.Contains(searchValue));
                }
                int recordsTotal = customers.Count();
                var data1 = customers.Select(x => new
                {
                    Id = x.Id,
                    Name = x.Name,
                    ProductName = x.ProductName,
                    CertificateImage = x.CertificateImage,
                    IsVerified = x.IsVerified,
                    Date = x.Date

                }).Skip(skip).Take(take).ToList();
                return (recordsTotal, recordsTotal, data1);

            }
        }
        public (int TotalCount, int FilteredCount, dynamic Customers) GetCertificateFilterList(int skip, int take, string sortColumn, string sortColumnDir, string searchValue, string data, string date)
        {
            if (data == "1")
            {
                var customers2 = context.TrackingForms.Where(x => x.IsVerified == 1 && x.Date == date).Select(x => new
                {
                    Id = x.Id,
                    Name = x.Name,
                   // ProductName = context.ProductMasters.Where(a => a.Id == x.TestkitId).FirstOrDefault().ProductName,
                    CertificateImage = x.CertificateImage,
                    IsVerified = x.IsVerified,
                    Date = x.Date

                });

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    switch (sortColumn)
                    {
                        case "Name":
                            customers2 = sortColumnDir == "asc" ? customers2.OrderBy(x => x.Name) : customers2.OrderByDescending(x => x.Name);
                            break;
                        case "Date":
                            customers2 = sortColumnDir == "asc" ? customers2.OrderBy(x => x.Date) : customers2.OrderByDescending(x => x.Date);
                            break;

                    }

                }
                if (!string.IsNullOrEmpty(searchValue))
                {
                    customers2 = customers2.Where(m => m.Name.Contains(searchValue));
                }
                int recordsTotal = customers2.Count();
                var data2 = customers2.Select(x => new
                {
                    Id = x.Id,
                    Name = x.Name,
                   // PorductName=x.ProductName,
                    CertificateImage = x.CertificateImage,
                    IsVerified = x.IsVerified,
                    Date = x.Date

                }).Skip(skip).Take(take).ToList();
                return (recordsTotal, recordsTotal, data2);
            }
            else if (data == "0")
            {
                var customers2 = context.TrackingForms.Where(x => x.IsVerified == 0 && x.Date == date).Select(x => new
                {
                    Id = x.Id,
                    Name = x.Name,
                   // ProductName = context.ProductMasters.Where(a => a.Id == x.TestkitId).FirstOrDefault().ProductName,
                    CertificateImage = x.CertificateImage,
                    IsVerified = x.IsVerified,
                    Date = x.Date

                });

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    switch (sortColumn)
                    {
                        case "Name":
                            customers2 = sortColumnDir == "asc" ? customers2.OrderBy(x => x.Name) : customers2.OrderByDescending(x => x.Name);
                            break;
                        case "Date":
                            customers2 = sortColumnDir == "asc" ? customers2.OrderBy(x => x.Date) : customers2.OrderByDescending(x => x.Date);
                            break;

                    }

                }
                if (!string.IsNullOrEmpty(searchValue))
                {
                    customers2 = customers2.Where(m => m.Name.Contains(searchValue));
                }
                int recordsTotal = customers2.Count();
                var data2 = customers2.Select(x => new
                {
                    Id = x.Id,
                    Name = x.Name,
                  //  productName=x.ProductName,
                    CertificateImage = x.CertificateImage,
                    IsVerified = x.IsVerified,
                    Date = x.Date
                }).Skip(skip).Take(take).ToList();
                return (recordsTotal, recordsTotal, data2);
            }
            else
            {
                var customers2 = context.TrackingForms.Where(x => x.IsVerified == 2 && x.Date == date).Select(x => new
                {
                    Id = x.Id,
                    Name = x.Name,
                   // ProductName = context.ProductMasters.Where(a => a.Id == x.TestkitId).FirstOrDefault().ProductName,
                    CertificateImage = x.CertificateImage,
                    IsVerified = x.IsVerified,
                    Date = x.Date

                });

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    switch (sortColumn)
                    {
                        case "Name":
                            customers2 = sortColumnDir == "asc" ? customers2.OrderBy(x => x.Name) : customers2.OrderByDescending(x => x.Name);
                            break;
                        case "Date":
                            customers2 = sortColumnDir == "asc" ? customers2.OrderBy(x => x.Date) : customers2.OrderByDescending(x => x.Date);
                            break;
                    }
                }
                if (!string.IsNullOrEmpty(searchValue))
                {
                    customers2 = customers2.Where(m => m.Name.Contains(searchValue));
                }
                int recordsTotal = customers2.Count();
                var data2 = customers2.Select(x => new
                {
                    Id = x.Id,
                    Name = x.Name,
                  //  ProductName=x.ProductName,
                    CertificateImage = x.CertificateImage,
                    IsVerified = x.IsVerified,
                    Date = x.Date

                }).Skip(skip).Take(take).ToList();
                return (recordsTotal, recordsTotal, data2);
            }
        }

        public async Task<ProductViewModel> FormResponseDetails(Guid? UserID)
        {
            ProductViewModel form = new ProductViewModel();
            try
            {
                form.FormResPro =await (from a in context.FormResponce.Where(x=>x.CustomerId==UserID)
                                        join t in context.Form on a.FormId equals t.ID
                                        select new formDetailsList
                               {
                                   ResponseId = a.ResponseId,
                                   Name = a.FormName,
                                   CustomerName = (context.CustomerInfo.Where(p => p.Id == a.CustomerId).FirstOrDefault().Name),
                                   FormIds = a.FormId,
                                   DateCreated = a.CreatedDate
                               }).OrderByDescending(x => x.DateCreated).ToListAsync();
            }
            catch (Exception ex)
            {

            }
            return form;
        }

        public (int TotalCount, int FilteredCount, dynamic Customers) FormResponseDetails(int skip, int take, string sortColumn, string sortColumnDir, string searchValue)
        {
            var formd =
                (from a in context.FormResponce
                 join t in context.Form on a.FormId equals t.ID
                 select new
            {
                ResponseId = a.ResponseId,
                Name = a.FormName,
                CustomerId = (context.CustomerInfo.Where(p => p.Id == a.CustomerId).FirstOrDefault().Name),
                FormIds = a.FormId,
                DateCreated = a.CreatedDate
            });
            if (!(string.IsNullOrEmpty(sortColumn)))
            {
                switch (sortColumn)
                {
                    case "Name":
                        formd = sortColumnDir == "asc" ? formd.OrderBy(x => x.Name) : formd.OrderByDescending(x => x.Name);
                        break;
                    case "DateCreated":
                        formd = sortColumnDir == "asc" ? formd.OrderBy(x => x.DateCreated) : formd.OrderByDescending(x => x.DateCreated);
                        break;
                }

            }else
            {
                formd = formd.OrderByDescending(x => x.DateCreated);
            }
            if (!string.IsNullOrEmpty(searchValue))
            {
                formd = formd.Where(m => m.Name.Contains(searchValue));
            }
            int recordsTotal = formd.Count();
          
            var data = formd.Select(a => new
            {
                ResponseId = a.ResponseId,
                Name = a.Name,
                CustomerId = a.CustomerId,
                FormIds = a.ResponseId,
                DateCreated = a.DateCreated.ToString("dd-MM-yyyy")
            }).Skip(skip).Take(take).ToList();
            return (recordsTotal, recordsTotal, data);
        }

        public async Task<FormPropertyViewModel> GetFormDetail(Guid? FormIds)
        {
            FormPropertyViewModel data = new FormPropertyViewModel();
           // FormRes model = new FormRes();
            try
            {
                data.formres = (from a in context.FormResponce.Where(u => u.ResponseId == FormIds)                              
                                select new FormRes
                         {
                             FormId = a.FormId,
                             FormName = a.FormName,
                             FormPropertyViewList = (from b in context.FormPropertyResponce.Where(x => x.FormResponceId == a.ResponseId)
                                                     select new FormPropertyViewModel
                                                     {
                                                         InputType = b.FieldType,
                                                         Name = context.FormProperty.Where(f => f.Id == b.FormPropertyId).FirstOrDefault().Name,
                                                         RowNumber = Convert.ToInt32(context.FormProperty.Where(f => f.Id == b.FormPropertyId).FirstOrDefault().RowNumber),
                                                         NameValue = b.ResponceText,
                                                         NumberSliderValue = b.NumberSliderValue,
                                                         MultipleChoiceValue = b.MultipleChoiceValue,
                                                         DropdownView = (from d in context.Dropdown.Where(x => x.FormPropertyId == b.FormPropertyId)
                                                                         select new DropdownViewModel
                                                                         {
                                                                             OptionText = d.Name
                                                                         }).FirstOrDefault(),
                                                         MultipleChoiceViewList = (from e in context.MultipleChoice.Where(x => x.FormPropertyId == b.FormPropertyId)
                                                                                   select new MultipleChoiceViewModel
                                                                                   {
                                                                                       Id = e.Id,
                                                                                       ChoiceText = e.Name,
                                                                                   }).ToList(),
                                                         CheckBoxViewList = (from f in context.CheckboxResponce.Where(x => x.FormPropertyResponceId == b.Id)
                                                                             select new CheckBoxViewModel
                                                                             {
                                                                                 Id = f.CheckboxId,
                                                                                 CheckboxValue = f.Check,
                                                                                 CheckboxText = context.CheckBox.Where(x => x.Id == f.CheckboxId).FirstOrDefault().Name,
                                                                             }).ToList(),
                                                     }).OrderBy(x => x.RowNumber).ToList()
                         }).FirstOrDefault();
            }
            catch (Exception ex)
            {

                throw;
            }

            return data;
        }

    }
}