using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using Nest;
using Newtonsoft.Json;
using smartTechAuthenticator.Models;
using smartTechAuthenticator.Services.Account;
using smartTechAuthenticator.Services.Comman;
using smartTechAuthenticator.Services.Comman.CustomFilters;
using smartTechAuthenticator.Services.Customers;
using smartTechAuthenticator.Services.Mall;
using smartTechAuthenticator.ViewModels;
using static QRCoder.PayloadGenerator;
using Status = smartTechAuthenticator.ViewModels.Status;

namespace smartTechAuthenticator.Controllers
{
 public class HomeController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly ICommanServices _comman;
        private readonly ICustomersService customers;
        private readonly IMallService mall;
        private readonly IAccountRepo account;
        public HomeController(ICommanServices comman, ICustomersService _customers, ApplicationDbContext _context, IAccountRepo _account)
        {
            account = _account;
            _comman = comman;
            customers = _customers;
            context = _context;
        }
        //public ActionResult Index()
        //{

        //    BannerCarouselViewModel model = new BannerCarouselViewModel();
        //    model.BannerCarouselList = customers.GrtBannerCarouselList();
        //    model.NewsList = customers.GrtNewsAllList();
        //    model.CompanyList = customers.GetProductList();
        //    return View(model);
        //}

        [HttpGet]
        [Obsolete]
        public ActionResult Index()
        {
            //string hostName = Dns.GetHostName();
            //string SignedIP = Dns.GetHostByName(hostName).AddressList[0].ToString();
            //TempData["SignedIP"] = SignedIP;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Obsolete]
        public async Task<ActionResult> Index(SignInViewModel customer, string returnUrl)
        {
            ResponseModel model = new ResponseModel();
            if (ModelState.IsValid)
            {
                model = await account.SignIn(customer);
                if (model.Status == Status.Success)
                {
                    ModelState.Clear();
                    Guid userId = model.Data.Id;
                    string name = model.Data.Name;
                    string email = model.Data.Email;
                    string Role = model.Data.UserType.ToString();
                    this.SetUserId(userId, name, email, Role);
                    this.ShowMessage("success", "Welcome to Fact PayRoll portal,", ToastType.success);
                    if (model.Data.UserType == UserType.Admin)
                    {
                        return RedirectToAction("Index", "Customer");
                    }
                    else if (model.Data.UserType == UserType.SubAdmin)
                    {
                        return RedirectToAction("Index", "Customer");
                    }
                    else if (model.Data.UserType == UserType.Customer)
                    {
                        if (!String.IsNullOrEmpty(returnUrl))
                            return Redirect(returnUrl);
                        else
                            return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    TempData["error"] = "Invalid user name or password !";
                }
            }
            return View();
        }



        public ActionResult Post(Guid Id)
        {
            NewsViewModel model = new NewsViewModel();
            model = customers.PostNewsData(Id);
            return View(model);
        }

        public ActionResult IndexPartial()
        {
            NewsViewModel model = new NewsViewModel();
            model.NewsList = customers.GetNewList();
            return PartialView(model);
        }
        public ActionResult ProductAuthenticity()
        {
            return View();
        }
        [Authorized]
        [HttpGet]
        public ActionResult ScanQr()
        {
            return View();
        }
        [Authorized]
        public ActionResult ManualSubmit()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ManualSubmit(ManualSubmitViewModel model)
        {
            if (ModelState.IsValid)
            {
                var UserId = Session["UserId"].ToString();
                var Veryfy = await _comman.VerifyCustomerByCode(UserId, model.CodeText);
                if (Veryfy.Status == Status.Failure)
                {
                    this.ShowMessage("failiure ", Veryfy.Message, ToastType.success);
                    TempData["result"] = Veryfy;
                    ModelState.Clear();
                    return View();
                }
                var response = await _comman.VerifyQrByCode(model.CodeText, UserId);
                TempData["result"] = response;
                ModelState.Clear();
                if (response.Status == Status.Success)
                {
                    TestkitCheckList checkList = new TestkitCheckList()
                    {
                        CustId = Guid.Parse(Session["UserId"].ToString()),
                        Qrcode = model.CodeText,
                        Status = 0,
                        CreatedTs = DateTime.UtcNow
                    };
                    ResponseModel model1 = await _comman.SaveScanHistory(checkList);
                    TempData["productInfo"] = response.Data;
                    return Redirect("Step1");
                }
            }
            return View();
        }    
        [Authorized]
        [HttpGet]
        public ActionResult Step1()
        {
            if (TempData["productInfo"] != null)
            {

                CheckListModel model = new CheckListModel();
                var data = TempData["productInfo"] as dynamic;
                string qrCode = data.QrCode;
                ProductMaster productMaster = new ProductMaster()
                {
                    Id = data.Id,
                    ProductName = data.ProductName,
                    ProductCategory = data.ProductCategory,
                    //QrCodeMaster = data.QrCodeMaster,
                    Description = data.Description,
                };
                model.Data = productMaster;
                model.TestkitChecks = _comman.TestkitChecks(Guid.Parse(Session["UserId"].ToString()), qrCode).ToList();
                TempData["productInfo"] = data;
                return View(model);
            }
            else
            {
                return Redirect("ManualSubmit");
            }
        }

        #region Step-2 begin
        [Authorized]
        [HttpGet]
        public ActionResult Step2()
        {
            TrackingFormsViewModel model = new TrackingFormsViewModel();
            TrackingFormsViewModel model1 = new TrackingFormsViewModel();
            Guid Id = new Guid(Session["UserId"].ToString());
            model = _comman.EditRecord(Id);
            var productInfo = TempData["productInfo"] as dynamic;
            if (productInfo != null)
            {
                if (model == null)
                {
                    model1 = _comman.EditRecordNew(Id);
                    model1.CustId = Id;
                    model1.TestkitId = productInfo.Id;
                    model1.QrCode = productInfo.QrCode;
                    TempData["productInfo"] = productInfo;
                    return View(model1);
                }
                else
                {
                    model.CustId = Id;
                    model.TestkitId = productInfo.Id;
                    model.QrCode = productInfo.QrCode;
                    TempData["productInfo"] = productInfo;
                    return View(model);
                }
            }
            else
            {
                return Redirect("ManualSubmit");
            }
        }
        [HttpPost]
        public async Task<ActionResult> Step2(TrackingFormsViewModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await _comman.SaveStep2(model);
                if (response.Status == Status.Success)
                {
                    TempData["productInfo"] = response.Data;
                    this.ShowMessage("success", "Informaton submitted successfully", ToastType.success);
                    return RedirectToAction("Step3", "Home");
                }
                else
                {
                    this.ShowMessage("error", "Unable to procceed", ToastType.error);
                }
            }
            return View();
        }
        #endregion Step-2 end

        #region Step-3 begin
        [Authorized]
        [HttpGet]
        public ActionResult Step3()
        {
            TrackingTestTypeViewModel model = new TrackingTestTypeViewModel();
            var productInfo = TempData["productInfo"] as dynamic;
            if (productInfo != null)
            {
                model.TrackingId = productInfo.Id;
                return View(model);
            }
            else
            {
                this.ShowMessage("Error", "Unable to proceed", ToastType.error);
                return RedirectToAction("Step2");
            }
        }
        [HttpPost]
        public async Task<ActionResult> Step3(TrackingTestTypeViewModel model, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                //var UserId = Session["UserId"].ToString();
                var response = await _comman.SaveStep3(model);
                if (response.Status == Status.Success && response.Data != null)
                {
                    TempData["trackingInfo"] = response.Data;
                    this.ShowMessage("success", "Informaton submitted successfully", ToastType.success);
                    return RedirectToAction("SaveFormResponce", "Home", new { ProductId = response.OrderId });
                }
                else
                {
                    this.ShowMessage("error", "Unable to procceed", ToastType.error);
                }
            }
            return View();
        }
        #endregion Step-3 end
        [Authorized]
        [HttpGet]
        public ActionResult GenerateCertificate()
        {
            if (TempData["trackingInfo"] != null)
            {
                var data = TempData["trackingInfo"] as TrackingFormsViewModel;
                TempData["trackingInfo"] = data;
                return View(data);
            }
            else
            {
                return Redirect("ManualSubmit");
            }

        }
        [HttpGet]
        public async Task<ActionResult> ScanResult(string Code)
        {
            var UserId = Session["UserId"].ToString();

            var response = await _comman.VerifyQrByCode(Code, UserId);
            TempData["result"] = response;
            ModelState.Clear();
            if (response.Status == Status.Success)
            {
                TestkitCheckList checkList = new TestkitCheckList()
                {
                    CustId = Guid.Parse(Session["UserId"].ToString()),
                    Qrcode = Code,
                    Status = 0,
                    CreatedTs = DateTime.UtcNow
                };
                ResponseModel model1 = await _comman.SaveScanHistory(checkList);
                TempData["productInfo"] = response.Data;
                return Redirect("Step1");
            }
            return View();
        }

        public ActionResult TrackingForm()
        {
            return View();
        }
        public ActionResult SelfTestResult()
        {
            return View();
        }
        public ActionResult Settings()
        {
            return View();
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();

        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public class testtt
        {
            public string data { get; set; }
            public int Id { get; set; }
        }

        [HttpPost]
        public JsonResult PdftoImg(testtt Imgdata)
        {
            var Id = Imgdata.Id;
            TrackingForms model = new TrackingForms();
            var newimage = Imgdata.data.Split(',');
            string tempGuid1 = Guid.NewGuid().ToString().Substring(0, 5);
            string imagename = tempGuid1 + "_CirtificateImage.Jpeg";
            byte[] imageBytes = Convert.FromBase64String(newimage[1]);
            MemoryStream ms2 = new MemoryStream(imageBytes, 0, imageBytes.Length);
            ms2.Write(imageBytes, 0, imageBytes.Length);
            System.Drawing.Image image2 = System.Drawing.Image.FromStream(ms2, true);
            //geting server folder path 
            string spath = System.Web.HttpContext.Current.Server.MapPath("~/Content/Certificate/");
            string path = spath + imagename;
            image2.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);
            var name = Path.GetFileName(path);
            model.CertificateImage = name;
            model.Id = Id;
            var data = _comman.CertificateSave(model);
            return Json(data);
        }
        public ActionResult Record(TrackingFormsViewModel model)
        {
            return View(model);
        }

        [Authorized]

        public ActionResult UserRecord(string ProuctName = null, string date = null)
        {
            TrackingFormsViewModel model = new TrackingFormsViewModel();
            var UserId = new Guid(Session["UserId"].ToString());
            if (UserId != null)
            {
                model.ViewTrackingFormsList = _comman.ViewUserRecordList(UserId);
            }
            if (ProuctName != null)
            {
                model.ViewTrackingFormsList = _comman.ViewUserRecordSearchList(UserId, ProuctName.ToString());
            }
            if (date != null && date != "")
            {
                model.ViewTrackingFormsList = model.ViewTrackingFormsList.Where(x => x.Date == date).ToList();
                model.Date = date;
            }
            return View(model);
        }

        public ActionResult Faq()
        {
            FaqViewModel faq = new FaqViewModel();
            faq.FAQList = customers.GetFAQList();
            return View(faq);
        }
        public ActionResult HelpGuild()
        {
            HelpGuidViewModel HelpGuid = new HelpGuidViewModel();
            HelpGuid.HelpList = customers.GetHelpGuidList();
            return View(HelpGuid);
        }
        public ActionResult TermPrivacy()
        {
            TermPrivacyViewModel Term = new TermPrivacyViewModel();
            Term.TermPrivacyList = customers.GetTermPrivacyList();
            return View(Term);
        }

        public ActionResult Abouts()
        {
            AboutViewModel model = new AboutViewModel();
            model.aboutList = customers.GetAboutList();
            return View(model);
        }
        public ActionResult OrderList()
        {

            return View();
        }
        [HttpPost]
        public ActionResult GetOrderList()
        {
            try
            {
                var draw = Request.Form.GetValues("draw").FirstOrDefault();
                var start = Request.Form.GetValues("start").FirstOrDefault();
                var length = Request.Form.GetValues("length").FirstOrDefault();
                var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
                var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();
                int take = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                (int TotalCount, int FilteredCount, dynamic Customers) data = customers.OrderHistoryList(skip, take, sortColumn, sortColumnDir, searchValue);
                IEnumerable<SelectListItem> AllProductData = customers.GetAllProductData1();
                ViewData["AllProductData"] = AllProductData;


                return Json(new { draw = draw, recordsFiltered = data.TotalCount, recordsTotal = data.TotalCount, data = data.Customers });
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        [Authorized]
        public async Task<ActionResult> CreateForm(string Id = null)
        {
            CaptchaResponse response = ValidateCaptcha(Request["g-recaptcha-response"]);
            FormPropertyViewModel f = new FormPropertyViewModel();
            if (Id != null)
            {
                f = await customers.GetForm(new Guid(Id));
                f.fId = Id;
            }
            return View(f);
        }
        [HttpPost]
        public async Task<ActionResult> SaveForm(FormPropertyViewModel data)
        {
            var UserId = new Guid(Session["UserId"].ToString());
            ResponseModel response = new ResponseModel();
            try
            {
                if (data.FormName != null)
                {
                    response = await customers.Saveform(data.FormName, UserId);
                    if (response.Status == Status.Success)
                    {
                        return RedirectToAction("CreateForm", new { Id = response.OrderId });
                    }
                    else
                    {
                        return Json(Status.Failure);
                    }
                }
                else
                {
                    TempData["error"] = "* Form Name is required !";
                }
            }
            catch (Exception ex)
            {

            }
            return RedirectToAction("CreateForm", new { Id = response.OrderId });
        }

        public async Task<ActionResult> SimpleText(FormPropertyViewModel data)
        {
            ResponseModel res = new ResponseModel();
            try
            {
                if (data.fId != null)
                {
                    var UserId = new Guid(Session["UserId"].ToString());
                    data.CreatedBy = UserId;
                    res = await customers.SaveSimpleText(data);
                    if (res.Status == Status.Success)
                    {
                        this.ShowMessage("success", "Informaton submitted successfully", ToastType.success);
                    }
                    else
                    {
                        this.ShowMessage("error", "Unable to procceed", ToastType.error);
                    }
                }
            }
            catch (Exception ex)
            {
                this.ShowMessage("error", "Unable to procceed", ToastType.error);
            }
            return RedirectToAction("CreateForm", new { Id = data.fId });
        }

        public async Task<ActionResult> TextField(FormPropertyViewModel data)
        {
            ResponseModel res = new ResponseModel();
            try
            {
                if (data.fId != null)
                {
                    var UserId = new Guid(Session["UserId"].ToString());
                    data.CreatedBy = UserId;
                    res = await customers.SaveTextField(data);
                    if (res.Status == Status.Success)
                    {
                        this.ShowMessage("success", "Informaton submitted successfully", ToastType.success);
                    }
                    else
                    {
                        this.ShowMessage("error", "Unable to procceed", ToastType.error);
                    }
                }
            }
            catch (Exception ex)
            {
                this.ShowMessage("error", "Unable to procceed", ToastType.error);
            }
            return RedirectToAction("CreateForm", new { Id = data.fId });
        }

        public async Task<ActionResult> DateField(FormPropertyViewModel data)
        {
            ResponseModel res = new ResponseModel();
            try
            {
                if (data.fId != null)
                {
                    var UserId = new Guid(Session["UserId"].ToString());
                    data.CreatedBy = UserId;
                    res = await customers.SaveDateField(data);
                    if (res.Status == Status.Success)
                    {
                        this.ShowMessage("success", "Informaton submitted successfully", ToastType.success);
                    }
                    else
                    {
                        this.ShowMessage("error", "Unable to procceed", ToastType.error);
                    }
                }
            }
            catch (Exception ex)
            {
                this.ShowMessage("error", "Unable to procceed", ToastType.error);
            }
            return RedirectToAction("CreateForm", new { Id = data.fId });
        }

        public async Task<ActionResult> SaveCheckBox(FormPropertyViewModel data)
        {
            ResponseModel res = new ResponseModel();
            try
            {
                if (data.Name != null && data.fId != null)
                {
                    var UserId = new Guid(Session["UserId"].ToString());
                    data.CreatedBy = UserId;
                    res = await customers.Savecheckbox(data);
                    if (res.Status == Status.Success)
                    {
                        this.ShowMessage("success", "Informaton submitted successfully", ToastType.success);
                    }
                    else
                    {
                        this.ShowMessage("error", "Unable to procceed", ToastType.error);
                    }
                }
            }
            catch (Exception ex)
            {
                this.ShowMessage("error", "Unable to procceed", ToastType.error);
            }
            return RedirectToAction("CreateForm", new { Id = data.fId });
        }

        public async Task<ActionResult> SaveBlackCheckBox(FormPropertyViewModel data)
        {
            ResponseModel res = new ResponseModel();
            try
            {
                if (data.Name != null && data.fId != null)
                {
                    var UserId = new Guid(Session["UserId"].ToString());
                    data.CreatedBy = UserId;
                    res = await customers.SaveBlackcheckbox(data);
                    if (res.Status == Status.Success)
                    {
                        this.ShowMessage("success", "Informaton submitted successfully", ToastType.success);
                    }
                    else
                    {
                        this.ShowMessage("error", "Unable to procceed", ToastType.error);
                    }
                }
            }
            catch (Exception ex)
            {
                this.ShowMessage("error", "Unable to procceed", ToastType.error);
            }
            return RedirectToAction("CreateForm", new { Id = data.fId });
        }

        public static CaptchaResponse ValidateCaptcha(string response)
        {
            string secret = "6Lejj2MUAAAAAPL1rK2f4hdRTexI0dyp5SO1Eha3";
            var client = new WebClient();
            var jsonResult = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secret, response));
            return JsonConvert.DeserializeObject<CaptchaResponse>(jsonResult.ToString());
        }
        public ActionResult DeleteFormProprty(Guid? Id, Guid? fId)
        {
            ResponseModel re = new ResponseModel();
            try
            {
                if (Id != null && fId != null)
                {
                    re = customers.deleteformproperty(Id);
                    var res = customers.UpdateNumber(fId);
                    if (re.Status == Status.Success)
                    {
                        this.ShowMessage("success", "Informaton submitted successfully", ToastType.success);
                    }
                    else
                    {
                        this.ShowMessage("error", "Unable to procceed", ToastType.error);
                    }
                }
                else
                {
                    this.ShowMessage("error", "Unable to procceed", ToastType.error);
                }
            }
            catch (Exception ex)
            {
                this.ShowMessage("error", "Unable to procceed", ToastType.error);
            }
            return RedirectToAction("CreateForm", new { Id = fId });
        }
        public  ActionResult FormAssign(string formyhn = null)
        {
            CompanyDetailsViewModel cus = new CompanyDetailsViewModel();
           // cus = await customers.GetProduct();
            //if (formyhn != null)
            //{
            //   // Guid fid = new Guid(formyhn);
            //    cus.ListPro =(from a in context.ProductMasters.Where(x=>x.IsActive==true && x.FormIds== formyhn)
            //                  select new ProList
            //                  {
            //                      Id=a.Id,
            //                      Name=a.ProductName,
            //                      FormIds=a.FormIds,
            //                      IsActive=a.IsActive
            //                  }).ToList();
               
            //        for (var item=0; item<cus.ListPro.Count;item++)
            //        {
            //        if (cus.ListPro[item].FormIds != "" && cus.ListPro[item].FormIds != null)
            //        {
            //            cus.ListPro[0].check = true;
            //        }
            //    }
            //}
            //else
            //{
            //     cus = await customers.GetProduct();
            //}
            return View();
        }

        public JsonResult FormAssign1(string formid = null)
        {
            CustomerInfoViewModel cus = new CustomerInfoViewModel();

            if (formid != null)
            {
                // Guid fid = new Guid(formyhn);
                cus.ListPro = (from a in context.CustomerInfo
                               select new ProList
                               {
                                   Id = a.Id,
                                   Name = a.Name,
                                   FormIds = a.FormIds 
                               }).ToList();
                for (var item = 0; item < cus.ListPro.Count; item++)
                {
                    if (cus.ListPro[item].FormIds != "" && cus.ListPro[item].FormIds != null && cus.ListPro[item].FormIds==formid)
                    {
                        cus.ListPro[item].check = true;
                    }
                }
            }
            return Json(cus);
        }
        [HttpPost]
        public async Task<ActionResult> FormAssign(CustomerInfoViewModel data)
        {
            ResponseModel res = new ResponseModel();
            CustomerInfoViewModel cus = new CustomerInfoViewModel();
            try
            {
                res = await customers.assignproform(data);
                if (res.Status == Status.Success)
                {
                    this.ShowMessage("success", "Form assigned  successfully", ToastType.success);
                }
                else
                {
                    this.ShowMessage("error", "Unable to procceed", ToastType.error);
                }
            }
            catch (Exception)
            {
                this.ShowMessage("error", "Unable to procceed", ToastType.error);
            }

            cus = await customers.GetProduct();
            return View(cus);
        }
        [HttpPost]
        public ActionResult GetForm()
        {
            try
            {
                var draw = Request.Form.GetValues("draw").FirstOrDefault();
                var start = Request.Form.GetValues("start").FirstOrDefault();
                var length = Request.Form.GetValues("length").FirstOrDefault();
                var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
                var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();
                int take = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                (int TotalCount, int FilteredCount, dynamic Customers) data = customers.getform(skip, take, sortColumn, sortColumnDir, searchValue);
                IEnumerable<SelectListItem> AllProductData = customers.GetAllProductData1();
                ViewData["AllProductData"] = AllProductData;
                return Json(new { draw = draw, recordsFiltered = data.TotalCount, recordsTotal = data.TotalCount, data = data.Customers });
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        [HttpPost]
        public async Task<JsonResult> DeleteForm(Guid? Id)
        {
            return Json(await customers.DeleteFormDetails(Id), JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> ViewForm(string Id = null)
        {
            CaptchaResponse response = ValidateCaptcha(Request["g-recaptcha-response"]);
            FormPropertyViewModel f = new FormPropertyViewModel();
            if (Id != null)
            {
                f = await customers.GetForm(new Guid(Id));
                f.fId = Id.ToString();
            }
            return View(f);
        }
        [AllowAnonymous]
        public async Task<ActionResult> TaxFormView(string Id = null, string userid = null)
        {
            CaptchaResponse response = ValidateCaptcha(Request["g-recaptcha-response"]);
            FormPropertyViewModel f = new FormPropertyViewModel();
            if (Id != null)
            {
                f = await customers.GetForm(new Guid(Id));
                f.fId = Id.ToString();
                f.userid=userid;
            }
            return View(f);
        }
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult SaveTaxformView(string formdata = "", string formid = "", string userid = "")
        {

            // Initialization.  
            JsonResult result = new JsonResult();
            if (formdata != "" && formid != "")
            {
                HtmlResponseData cr = new HtmlResponseData();
                cr.Id = Guid.NewGuid();
                cr.FormId = formid;
                cr.UserId = userid;
                cr.HtmlResponse = formdata;
                context.HtmlResponseData.Add(cr);
                context.SaveChanges();
                result.Data = 1;
               
            }
            // Return info.  
            return Json(result);
        }

        public async Task<ActionResult> EditForm(string Id = null)
        {
            CaptchaResponse response = ValidateCaptcha(Request["g-recaptcha-response"]);
            FormPropertyViewModel f = new FormPropertyViewModel();
            if (Id != null)
            {
                f = await customers.GetForm(new Guid(Id));
                f.fId = Id.ToString();
            }
            return View(f);
        }
        [Authorized]
        public async Task<ActionResult> SaveFormResponce(Guid? FormIds)
        {
            //if (ProductId == null)
            //{
            //    return RedirectToAction("Index", "Home");
            //}
            //CaptchaResponse response = ValidateCaptcha(Request["g-recaptcha-response"]);
            FormPropertyViewModel f = new FormPropertyViewModel();
            //var data = context.ProductMasters.Where(x => x.Id == ProductId).FirstOrDefault();
            //TempData["ProId"] = ProductId;
            if (FormIds != null)
            {
                var Formdata = context.Form.Where(x => x.ID == FormIds).FirstOrDefault();
                if(Formdata!=null)
                {
                    f = await customers.GetForm(FormIds);
                    //f.fId = ProductId.ToString();
                }else
                {
                    return RedirectToAction("TaxFormView", "Home");
                }

            }
            else
            {
                return RedirectToAction("TaxFormView", "Home");
            }
            return View(f);
        }
        [Authorized]
        [HttpPost]
        public async Task<ActionResult> SaveFormResponce(FormPropertyViewModel data, HttpPostedFileBase file)
        {
            ResponseModel res = new ResponseModel();
            var UserId = new Guid(Session["UserId"].ToString());
            try
            {
                if (data.FormPropertyViewList != null)
                {
                    data.CustomerId = UserId;
                    if (file != null)
                    {
                        if (file.ContentLength > 0)
                        {
                            string _FileName = Path.GetFileName(file.FileName);
                            string tempGuid = Guid.NewGuid().ToString().Substring(0, 5);
                            string FileName = tempGuid + "_" + _FileName;
                            string _path = Path.Combine(Server.MapPath("~/Content/FormPhoto"), FileName);
                            file.SaveAs(_path);
                            data.File = FileName;
                        }
                    }
                    res = await customers.saveformresponce(data);
                    if (res.Status == Status.Success)
                    {
                        this.ShowMessage("success", "Form submitted successfully", ToastType.success);
                        return RedirectToAction("TaxFormView", "Home");
                    }
                    else
                    {
                        this.ShowMessage("error", "Unable to procceed", ToastType.error);
                        return RedirectToAction("TaxFormView", "Home", new { ProductId = TempData["ProId"] });
                    }
                }
            }
            catch (Exception ex)
            {
                this.ShowMessage("error", "Unable to procceed", ToastType.error);
                return RedirectToAction("TaxFormView", "Home", new { ProductId = TempData["ProId"] });
            }
            return RedirectToAction("TaxFormView", "Home");
        }

        public ActionResult FormResponceDetails()
        {
            return View();
        }

        public async Task<ActionResult> FormDetails()
        {
            //ProductViewModel cus = new ProductViewModel();
            //cus = await customers.FormResponseDetails();
            return View();
        }

        [HttpPost]
        public ActionResult FormDetails1()
        {
            try
            {
                var draw = Request.Form.GetValues("draw").FirstOrDefault();
                var start = Request.Form.GetValues("start").FirstOrDefault();
                var length = Request.Form.GetValues("length").FirstOrDefault();
                var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
                var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();
                int take = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                (int TotalCount, int FilteredCount, dynamic Customers) data = customers.FormResponseDetails(skip, take, sortColumn, sortColumnDir, searchValue);
                return Json(new { draw = draw, recordsFiltered = data.TotalCount, recordsTotal = data.TotalCount, data = data.Customers });
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<ActionResult> ViewFormDetail(Guid? FormIds)
        {
            CaptchaResponse response = ValidateCaptcha(Request["g-recaptcha-response"]);
            FormPropertyViewModel f = new FormPropertyViewModel();
            if (FormIds != Guid.Empty)
            {
                f = await customers.GetFormDetail(FormIds);
                f.fId = FormIds.ToString();
            }
            return View(f);
        }

        [Authorized]
        public async Task<ActionResult> UserForms()
        {
            var UserId = new Guid(Session["UserId"].ToString());
            ProductViewModel cus = new ProductViewModel();
            cus = await customers.FormResponseDetails(UserId);
            return View(cus);
        }

        public async Task<ActionResult> ViewUserForm(Guid? ID)
        {
            CaptchaResponse response = ValidateCaptcha(Request["g-recaptcha-response"]);
            FormPropertyViewModel f = new FormPropertyViewModel();
            if (ID != Guid.Empty)
            {
                f = await customers.GetFormDetail(ID);
                f.fId = ID.ToString();
            }
            return View(f);
        }

        [AllowAnonymous]
        public async Task<ActionResult> SubmitFormView(string FormId = null, string UserId = null)
        {

            HtmlResponseDataViewModel f = new HtmlResponseDataViewModel();
            if (FormId != null)
            {
                f = (from data in context.HtmlResponseData.Where(x => x.FormId == FormId && x.UserId == UserId)
                     select new HtmlResponseDataViewModel

                     {
                         FormId = data.FormId,
                         UserId=data.UserId,
                         HtmlResponse=data.HtmlResponse

                     }).FirstOrDefault();
                
                f.FormId = FormId;
                f.UserId=UserId;
             
            }
            return View(f);
        }


        [HttpPost]
        [ValidateInput(false)]
        public JsonResult UpdateSubmitformView(string formdata = "", string formid = "", string userid = "")
        {

            ResponseModel res = new ResponseModel();
            try
            {
                var data = context.HtmlResponseData.Where(x=>x.FormId == formid && x.UserId == userid).ToList();
                if (data != null)
                {
                  
                        foreach (var item in data)
                        {
                            context.HtmlResponseData.Remove(item);
                            context.SaveChanges();
                        }
                }
                HtmlResponseData cr = new HtmlResponseData();
                cr.Id = Guid.NewGuid();
                cr.FormId = formid;
                cr.UserId = userid;
                cr.HtmlResponse = formdata;
                context.HtmlResponseData.Add(cr);
                context.SaveChanges();
                res.Data = 1;
            }

            catch (Exception ex)
            {
                res.Data = -1;
            }
            return Json(res);
        }
       
    }
}