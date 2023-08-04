using NLog;
using smartTechAuthenticator.Models;
using smartTechAuthenticator.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Net;
using Microsoft.EntityFrameworkCore;
using smartTechAuthenticator.Migrations;

namespace smartTechAuthenticator.Services.Comman
{
    public class CommanServices : ICommanServices
    {
        private readonly ApplicationDbContext context;
        private readonly Logger logger;
        public CommanServices(ApplicationDbContext _context, Logger _logger)
        {
            context = _context;
            logger = _logger;
        }
        public List<SelectListItem> GetDistrictts(int StateId)
        {
            return context.DistricttMasters.Where(x => x.StateId == StateId).Select(x => new SelectListItem() { Text = x.DistricttName, Value = x.Id.ToString() }).ToList();
        }
       

        public List<SelectListItem> GetStates()
        {
            return context.StateMaters.Select(x => new SelectListItem() { Text = x.StateName, Value = x.Id.ToString() }).ToList();
        }

    

        public async Task<ResponseModel> SaveScanHistory(TestkitCheckList testkit)
        {
            ResponseModel model = new ResponseModel();
            await context.TestkitCheckList.AddAsync(testkit);
            await context.SaveChangesAsync();
            model.Status = Status.Success;
            model.Data = testkit;
            return model;
        }
        public  TrackingFormsViewModel EditRecord(Guid Id)
        {
            TrackingFormsViewModel model = new TrackingFormsViewModel();
            model = (from cust in context.CustomerInfo.Where(x=>x.Id==Id)
                     join trackForm in context.TrackingForms.Where(a =>a.CustId==Id) on cust.Id equals trackForm.CustId
                     select new TrackingFormsViewModel
                     {
                         Name = cust.Name,
                         MobileNo = cust.MobileNo,
                         LotNumber = trackForm.LotNumber,
                         Date = DateTime.Now.Date.ToString(),
                         Time = DateTime.Now.ToString("hh:mm"),
                         Place = trackForm.Place
                     }).FirstOrDefault();
            return model;
        }
        public TrackingFormsViewModel EditRecordNew(Guid Id)
        {
            TrackingFormsViewModel model = new TrackingFormsViewModel();
            model = (from cust in context.CustomerInfo.Where(x => x.Id == Id)
                     select new TrackingFormsViewModel
                     {
                         Name = cust.Name,
                         MobileNo = cust.MobileNo,                       
                         Date = DateTime.Now.Date.ToString(),
                         Time = DateTime.Now.ToString("hh:mm"),                    
                     }).FirstOrDefault();
            return model;
        }

        


        public async Task<ResponseModel> SaveStep2(TrackingFormsViewModel tracking)
        {
            ResponseModel model = new ResponseModel();
            try
            {
                var IsExists = context.TrackingForms.FirstOrDefault(x => x.Id == tracking.Id);
                if (IsExists != null)
                {
                    IsExists.Name = tracking.Name;
                    IsExists.MobileNo = tracking.MobileNo;
                    IsExists.LotNumber = tracking.LotNumber;
                    IsExists.Time = tracking.Time;
                    IsExists.Date = tracking.Date;
                    IsExists.Place = tracking.Place;
                    IsExists.TestkitId = tracking.TestkitId;
                    IsExists.CustId = tracking.CustId;
                    IsExists.AntigenType = tracking.AntigenType;
                    IsExists.TestResults = tracking.TestResults;
                    IsExists.QrCode = tracking.QrCode;
                    context.TrackingForms.Update(IsExists);
                    await context.SaveChangesAsync();
                    model.Data = IsExists;
                    model.Status = Status.Success;
                }
                else
                {
                    TrackingForms trackingForms = new TrackingForms()
                    {
                        Id = tracking.Id,
                        Name = tracking.Name,
                        MobileNo = tracking.MobileNo,
                        LotNumber = tracking.LotNumber,
                        Time = tracking.Time,
                        Date = tracking.Date,
                        Place = tracking.Place,
                        TestkitId = tracking.TestkitId,
                        CustId = tracking.CustId,
                        QrCode=tracking.QrCode,
                        AntigenType = "",
                        TestResults = ""
                    };
                    await context.TrackingForms.AddAsync(trackingForms);
                    await context.SaveChangesAsync();
                    model.Data = trackingForms;
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

        public async Task<ResponseModel> SaveStep3(TrackingTestTypeViewModel tracking)
        {
            ResponseModel model = new ResponseModel();
            var name="";
            var IsExists = context.TrackingForms.FirstOrDefault(x => x.Id == tracking.TrackingId);
            if (IsExists != null)
            {
                (string filePath, bool Status) result = UploadDocuments(tracking.FileUrl);
                IsExists.AntigenType = tracking.AntigenType;
                IsExists.TestResults = tracking.TestResults;
                if (result.Status == true)
                {
                    IsExists.FileUrl = result.filePath;
                }
                context.TrackingForms.Update(IsExists);
                await context.SaveChangesAsync();
                model.Status = Status.Success;
                if (IsExists.VerifiedBy!=null)
                {
                    var data1 = context.CustomerInfo.FirstOrDefault(x => x.Id == new Guid(IsExists.VerifiedBy));
                     name = data1.Name;
                }
                var data = (from trackForm in context.TrackingForms
                            join custInfo in context.CustomerInfo on trackForm.CustId equals custInfo.Id
                            join custAddress in context.CustomerAddressMasters on custInfo.CustomerAddressId equals custAddress.Id
                            join state in context.StateMaters on custAddress.StateId equals state.Id
                            join district in context.DistricttMasters on custAddress.DistricttId equals district.Id
                            join product in context.ProductMasters on trackForm.TestkitId equals product.Id
                            select new TrackingFormsViewModel
                            {
                                Id = trackForm.Id,
                                TestkitId = trackForm.TestkitId,
                                Name = trackForm.Name,
                                Place = trackForm.Place,
                                MobileNo = trackForm.MobileNo,
                                Time = trackForm.Time,
                                Date = trackForm.Date,
                                TestResults = trackForm.TestResults,
                                FileUrl = trackForm.FileUrl,
                                Email = custInfo.Email,
                                CustId = custInfo.Id,
                                Address = custAddress.Address,
                                StateName = state.StateName,
                                DistricttName = district.DistricttName,
                                ProductName = product.ProductName,
                                AuthentiCode = product.Authenticode,
                                ProductId=product.Id.ToString(),
                                INVNo = product.INVNo,
                                QrCode = trackForm.QrCode,
                                VerifiedBy = name
                            }).FirstOrDefault(x => x.Id == tracking.TrackingId); 
                 
                model.Data = data;
                model.OrderId = new Guid(data.ProductId);
            }
            return model;
        }

        public IQueryable<TestkitCheckList> TestkitChecks(Guid CustomerId, string QrCode)
        {
            return context.TestkitCheckList.Where(x => x.CustId == CustomerId && x.Qrcode == QrCode);
        }

        public async Task<ResponseModel> VerifyQrByCode(string Codel, string CustomerId)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                var Codedata = context.QrCodeMasters.FirstOrDefault(x => x.QrCode == Codel);
                var codeInfo = (from a in context.ProductMasters
                                    where a.Id == Codedata.ProductId
                                    select new
                                    {
                                        a.Id,
                                        a.ProductName,
                                        a.ProductCategory,
                                        a.QrId,
                                        a.Description,
                                        QrCode= Codel
                                    }).FirstOrDefault();

                //var codeInfo = context.ProductMasters.Join(context.QrCodeMasters,
                //                         prod => prod.QrId,
                //                         code => code.Id,
                //                         (prod, code) => new
                //                         {
                //                             prod.Id,
                //                             prod.ProductName,
                //                             prod.ProductCategory,
                //                             prod.QrId,
                //                             prod.Description,
                //                             code.QrCode
                //                         }).FirstOrDefault(x => x.QrId == Codedata.Id);
                if (codeInfo != null)
                {
                    responseModel.Data = codeInfo;
                    responseModel.Status = Status.Success;
                    responseModel.Message = "Product authenticated successfully";

                    var Info = context.QrCodeMasters.FirstOrDefault(x => x.Id == Codedata.Id);
                    Info.CustomerId = CustomerId;
                    Info.IsExpire = true;
                    await context.SaveChangesAsync();
                }
                else
                {
                    responseModel.Status = Status.Failure;
                    responseModel.Message = "<p class='text-danger'>Warning! You may risk in buying the counterfeit product.Please contact your supplier/ our customer service <a href='#'>info@smarttechengineering.com</a> for further actions.</p>";
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return responseModel;
        }

        public async Task<ResponseModel> VerifyCustomerByCode(string CustomerId, string Code)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                var Codedata = context.QrCodeMasters.FirstOrDefault(x => x.QrCode == Code);
                if(Codedata!=null)
                {      
              if(Codedata.CustomerId==null)
                {
                    responseModel.Status = Status.Success;
                }
                else if(Codedata.CustomerId == CustomerId)
                {
                    responseModel.Status = Status.Success;
                }
                else
                {
                    responseModel.Status = Status.Failure;
                    responseModel.Message = "Warning! It’s used code please contact to this email :- careline@smarttechengineering.com !.";

                }
                }else
                {
                    responseModel.Status = Status.Failure;
                    responseModel.Message = "Warning! code not found please contact to this email :- careline@smarttechengineering.com !.";
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return responseModel;
        }

        private (string filePath, bool Status) UploadDocuments(HttpPostedFileBase file)
        {
            try
            {
                if (file == null)
                {
                    return (string.Empty, false);
                }
                else
                {
                    string _newFileName = Guid.NewGuid().ToString();
                    string _fileExtention = Path.GetExtension(file.FileName);
                    string _filePath = Path.Combine(HttpContext.Current.Server.MapPath("~/Content/UploadedFiles/"), string.Format("{0}{1}", _newFileName, _fileExtention));
                    string name = _newFileName+_fileExtention;
                    file.SaveAs(_filePath);
                    return (name, true);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return (string.Empty, false);
            }
        }

        public List<TrackingFormsViewModel> ViewUserRecordList(Guid customerid)
        {
            
            List<TrackingFormsViewModel> model = new List<TrackingFormsViewModel>();
            
            try
            {
                if (customerid!=null)
                {
                    //var data = context.TrackingForms.Where(x => x.CustId == customerid).ToList();
                    model = (from trackForm in context.TrackingForms.Where(x=>x.CustId== customerid)
                             join product in context.ProductMasters on trackForm.TestkitId equals product.Id
                             select new TrackingFormsViewModel
                             {
                                 ProductName= product.ProductName,
                                 Id=trackForm.Id,
                                // AuthentiCode= product.Authenticode,
                                 AuthentiCode= trackForm.QrCode,
                                 CustId = trackForm.CustId,
                                 Date = trackForm.Date,
                                 Time = trackForm.Time,
                                 Place = trackForm.Place,
                                 TestResults = trackForm.TestResults,
                                 AntigenType = trackForm.AntigenType,
                                 FileUrl = trackForm.FileUrl,
                                 CertificateImage=trackForm.CertificateImage,
                                 IsVerified=trackForm.IsVerified
                             }).OrderByDescending(x=>x.Date).ToList();
                }
                return model;
            }
            catch (Exception ex)
            {

            }
            return model;
        }
        public List<TrackingFormsViewModel> ViewUserRecordSearchList(Guid customerid,string ProuctName)
        {
            List<TrackingFormsViewModel> model = new List<TrackingFormsViewModel>();
            try
            {
                if (ProuctName != null)
                {
                 var  model1 = (from trackForm in context.TrackingForms.Where(x => x.CustId == customerid)
                             join product in context.ProductMasters on trackForm.TestkitId equals product.Id
                             select new TrackingFormsViewModel
                             {
                                 ProductName = product.ProductName,
                                 Id = trackForm.Id,
                                 // AuthentiCode= product.Authenticode,
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
                             }).ToList();

                    model = model1.Where(m => m.ProductName.Contains(ProuctName)).ToList();
                    
                }
                return model;
            }
            catch (Exception ex)
            {

            }
            return model;
        }
        public CustomerInfo EditUserProfile(Guid Id)
        {
            CustomerInfo model = new CustomerInfo();
            model = context.CustomerInfo.Where(x => x.Id == Id).FirstOrDefault();
            return model;
        }
        public async Task<ResponseModel> UpdateUserDetails(CustomerInfo info)
        {
            ResponseModel model = new ResponseModel();

            var Info = context.CustomerInfo.FirstOrDefault(x => x.Id == (info.Id));
            if (Info != null)
            {
                Info.Name = info.Name;
                Info.MobileNo = info.MobileNo;
                Info.Nric = info.Nric;
                Info.Address1 = info.Address1;
                Info.Email = info.Email;
                context.CustomerInfo.Update(Info);
            }
            await context.SaveChangesAsync();
            model.Status = Status.Success;
            return model;
        }

        public TrackingForms CertificateSave(TrackingForms form)
        {
            TrackingForms model = new TrackingForms();
           var   info = context.TrackingForms.FirstOrDefault(x => x.Id == form.Id);
            if (info != null)
            {
                info.CertificateImage = form.CertificateImage;
                
            }
            context.SaveChanges();
            return info;
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
       
        //public Task<FormRes>

    }

}