using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using smartTechAuthenticator.ViewModels;
using smartTechAuthenticator.Services.Comman;
using smartTechAuthenticator.Services.Comman.CustomFilters;
using smartTechAuthenticator.Services.Customers;
using System.Threading.Tasks;
using NLog;
using System.IO;
using System.Text;
using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;
using ExcelDataReader;
using System.Web.Hosting;
using smartTechAuthenticator.Models;

namespace smartTechAuthenticator.Controllers.Administrator
{
    [Authorized]
    public class CustomerController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly ICustomersService customers;
        private readonly ICommanServices commanServices;
        public readonly Logger logger;
        public CustomerController(ICustomersService _customers, ICommanServices _commanServices, Logger _Logger, ApplicationDbContext _context)
        {
            customers = _customers;
            commanServices = _commanServices;
            logger = _Logger;
            context = _context;
        }
        public ActionResult Index()
        {
            MenuPermission data = new MenuPermission();
            if (Session["Role"].ToString() != "Admin" && Session["Role"].ToString() != "SubAdmin")
            {
                return RedirectToAction("AccessDenide", "Account");
}
            Guid UserId = new Guid(Session["UserId"].ToString());
            data = context.MenuPermission.Where(x => x.SubAdminId == UserId).FirstOrDefault();
            var count = customers.NoofCustomer();
            TempData["count"] = count;
            var codes = customers.NoofCodes();
            TempData["Code"] = codes;

            var test = customers.NoofTest();
            TempData["test"] = test;

            var penddingCertificate = customers.NoOfPendding();
            TempData["penddingCertificate"] = penddingCertificate;

            var PostingNews = customers.NoOfPostingNews();
            TempData["PostingNews"] = PostingNews;

            var UnSolvedTicket = customers.NoOfUnSolvedTicket();
            TempData["UnSolvedTicket"] = UnSolvedTicket;

            var TotalTicket = customers.NoOfTotalTicket();
            TempData["TotalTicket"] = TotalTicket;

            var OrderList = customers.NoOfOrderList();
            TempData["OrderList"] = OrderList;

            var DailyOrderList = customers.NoOfDailyOrderList();
            TempData["DailyOrderList"] = DailyOrderList;

            return View(data);
        }
        [HttpGet]
        public ActionResult Customers()
        {
            if (Session["Role"].ToString() != "Admin" && Session["Role"].ToString() != "SubAdmin")
            {
                return RedirectToAction("AccessDenide", "Account");
            }
            return View();
        }
        /// <summary>
        /// This method is used to bind all registered customers
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetCustomers()
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
                (int TotalCount, int FilteredCount, dynamic Customers) data = customers.GetCustomers(skip, take, sortColumn, sortColumnDir, searchValue);
                return Json(new { draw = draw, recordsFiltered = data.TotalCount, recordsTotal = data.TotalCount, data = data.Customers });
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        [HttpPost]
        public ActionResult GetCustomersHistory()
        {
            try
            {
                var CustomerId = Session["CustId"].ToString();
                var draw = Request.Form.GetValues("draw").FirstOrDefault();
                var start = Request.Form.GetValues("start").FirstOrDefault();
                var length = Request.Form.GetValues("length").FirstOrDefault();
                var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
                var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();
                int take = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                (int TotalCount, int FilteredCount, dynamic Customers) data = customers.GetCustomersTestHistory(skip, take, sortColumn, sortColumnDir, searchValue, CustomerId);
                return Json(new { draw = draw, recordsFiltered = data.TotalCount, recordsTotal = data.TotalCount, data = data.Customers });
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpPost]
        public ActionResult GetCustomersIP_History()
        {
            try
            {
                var CustomerId = Session["CustId"].ToString();
                var draw = Request.Form.GetValues("draw").FirstOrDefault();
                var start = Request.Form.GetValues("start").FirstOrDefault();
                var length = Request.Form.GetValues("length").FirstOrDefault();
                var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
                var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();
                int take = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                (int TotalCount, int FilteredCount, dynamic Customers) data = customers.GetCustomersIPHistory(skip, take, sortColumn, sortColumnDir, searchValue, CustomerId);
                return Json(new { draw = draw, recordsFiltered = data.TotalCount, recordsTotal = data.TotalCount, data = data.Customers });
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        [HttpPost]
        public ActionResult GetCustomersOrderHistory()
        {
            try
            {
                var CustomerId = Session["CustId"].ToString();
                var draw = Request.Form.GetValues("draw").FirstOrDefault();
                var start = Request.Form.GetValues("start").FirstOrDefault();
                var length = Request.Form.GetValues("length").FirstOrDefault();
                var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
                var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();
                int take = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                (int TotalCount, int FilteredCount, dynamic Customers) data = customers.GetCustomersOrderHistory(skip, take, sortColumn, sortColumnDir, searchValue, CustomerId);
                return Json(new { draw = draw, recordsFiltered = data.TotalCount, recordsTotal = data.TotalCount, data = data.Customers });
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        [HttpGet]
        public async Task<ActionResult> CustomerDetails(string customerId)
        {
            var custInfofo = await customers.GetCustomers(Guid.Parse(customerId));
            var CustId = custInfofo.Id;
            Session["CustId"] = CustId;
            IEnumerable<SelectListItem> States = commanServices.GetStates();
            ViewData["States"] = States;
            IEnumerable<SelectListItem> items = commanServices.GetDistrictts(custInfofo.StateId);
            ViewData["Districstts"] = items;
            return View(custInfofo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CustomerDetails(CustomerInfoViewModel customer)
        {
            CustomerInfoViewModel model = new CustomerInfoViewModel();
            try
            {
                if (ModelState.IsValid)
                {
                    ResponseModel response = await customers.UpdateCustomerDetails(customer);
                    if (response.Status == Status.Success)
                        this.ShowMessage("success", "Details updated successfully", ToastType.success);
                    else
                        this.ShowMessage("failiure ", "Details not upated successfully ", ToastType.error);
                }
                model = await customers.GetCustomers(customer.Id);
                IEnumerable<SelectListItem> States = commanServices.GetStates();
                ViewData["States"] = States;
                IEnumerable<SelectListItem> items = commanServices.GetDistrictts(model.StateId);
                ViewData["Districstts"] = items;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                this.ShowMessage("Error", "An error occured, please after some time,", ToastType.success);
            }
            return View(model);
        }
        [HttpPost]
        public async Task<JsonResult> DeleteCustomerDetais(CustomerInfoViewModel customer)
        {
            return Json(await customers.DeleteCustomerDetails(customer), JsonRequestBehavior.AllowGet);
        }


        public ActionResult ManageQr()
        {
            if (Session["Role"].ToString() != "Admin" && Session["Role"].ToString() != "SubAdmin")
            {
                return RedirectToAction("AccessDenide", "Account");
            }
            // HostingEnvironment.QueueBackgroundWorkItem(ctx => GenrateQrFileMethod());
            ManageQrViewModel model = new ManageQrViewModel();
            model.ManageQrList = customers.GetQrData();
            model.ProductList = customers.GetAllProductData();

            // model.ProductId = model.ProductList[0].Id;
            IEnumerable<SelectListItem> AllProductData = customers.GetAllProductData1();
            ViewData["AllProductData"] = AllProductData;
            return View(model);
        }

        public ActionResult UploadQrCode()
        {
            if (Session["Role"].ToString() != "Admin" && Session["Role"].ToString() != "SubAdmin")
            {
                return RedirectToAction("AccessDenide", "Account");
            }
            IEnumerable<SelectListItem> ProductCategory = customers.GetProductCategory();
            ViewData["ProductCategory"] = ProductCategory;
            return View();
        }

        public void GenrateQrFileMethod()
        {
            var data = context.QrCodeMasters.Where(x => x.QrImageUrl == null).Take(100).ToList();
            if (data != null)
            {
                foreach (var item in data)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        QRCodeGenerator QrGenerator = new QRCodeGenerator();
                        QRCodeData QrCodeInfo = QrGenerator.CreateQrCode(item.QrCode, QRCodeGenerator.ECCLevel.Q);
                        QRCode QrCode = new QRCode(QrCodeInfo);
                        using (Bitmap bitMap = QrCode.GetGraphic(20))
                        {
                            bitMap.Save(ms, ImageFormat.Png);
                            var image = "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());
                            var imagen = Convert.ToBase64String(ms.ToArray()).ToString();
                            string tempGuid1 = Guid.NewGuid().ToString().Substring(0, 20);
                            string imagename = tempGuid1 + "_QrImage.Jpeg";
                            byte[] imageBytes = Convert.FromBase64String(imagen);
                            MemoryStream ms2 = new MemoryStream(imageBytes, 0, imageBytes.Length);
                            ms2.Write(imageBytes, 0, imageBytes.Length);
                            System.Drawing.Image image2 = System.Drawing.Image.FromStream(ms2, true);
                            //geting server folder path 
                            string spath = System.Web.HttpContext.Current.Server.MapPath("~/Content/UploadedQrFiles/");
                            string path = spath + imagename;
                            //Save Image to server folder
                            image2.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);
                            var newdata = context.QrCodeMasters.Find(item.Id);
                            newdata.QrImageUrl = imagename;
                            context.SaveChangesAsync();
                        }
                    }
                }
            }
        }


        [HttpPost]
        public ActionResult UploadQrCode(HttpPostedFileBase file, NewManageQrViewModel qrmodel)
        {
            StringBuilder strbuild = new StringBuilder();
            ManageQrViewModel model = new ManageQrViewModel();
            try
            {
                var allowedExtensions = new[] { ".txt", ".xlsx", ".xls", ".csv" };
                if (file == null)
                {
                    this.ShowMessage("Error", "File is Requird!,", ToastType.error);
                    return RedirectToAction("ManageQr");
                }
                var checkextension = Path.GetExtension(file.FileName).ToLower();

                if (!allowedExtensions.Contains(checkextension))
                {
                    this.ShowMessage("Error", "Only text,xlsx,csv file allow !,", ToastType.error);
                    return RedirectToAction("ManageQr");
                }
                if (checkextension.Contains(".xlsx"))
                {
                    if (file.ContentLength > 0)
                    {
                        string _filename = file.FileName;
                        string _FileName = Path.GetFileName(file.FileName);
                        string tempGuid = Guid.NewGuid().ToString().Substring(0, 5);
                        string FileName = tempGuid + "_" + _FileName;
                        string _path = Path.Combine(Server.MapPath("~/Content/UploadedFiles"), FileName);
                        file.SaveAs(_path);

                        //  HostingEnvironment.QueueBackgroundWorkItem(ctx => ExcelFileMethod(_path, qrmodel));

                        using (var stream = System.IO.File.Open(_path, FileMode.Open, FileAccess.Read))
                        {
                            using (var reader = ExcelReaderFactory.CreateReader(stream))
                            {

                                while (reader.Read()) //Each row of the file
                                {
                                    var a = reader.GetValue(0).ToString();
                                    if (a.Length == 16 || a.Length == 17)
                                    {
                                        var check = customers.CheckGenrateQrCode(a);
                                        if (check.Status == Status.Success)
                                        {
                                            model.QrCode = a;
                                            //  model.QrImageUrl = imagename;
                                            model.ProductId = qrmodel.ProductId;
                                            model.CategoryId = qrmodel.CategoryId;
                                            customers.GenrateQrCode(model);
                                            //using (MemoryStream ms = new MemoryStream())
                                            //{
                                            //    QRCodeGenerator QrGenerator = new QRCodeGenerator();
                                            //    QRCodeData QrCodeInfo = QrGenerator.CreateQrCode(a, QRCodeGenerator.ECCLevel.Q);
                                            //    QRCode QrCode = new QRCode(QrCodeInfo);
                                            //    using (Bitmap bitMap = QrCode.GetGraphic(20))
                                            //    {
                                            //        bitMap.Save(ms, ImageFormat.Png);
                                            //        var image = "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());
                                            //        var imagen = Convert.ToBase64String(ms.ToArray()).ToString();
                                            //        string tempGuid1 = Guid.NewGuid().ToString().Substring(0, 9);
                                            //        string imagename = tempGuid1 + "_QrImage.Jpeg";
                                            //        byte[] imageBytes = Convert.FromBase64String(imagen);
                                            //        MemoryStream ms2 = new MemoryStream(imageBytes, 0, imageBytes.Length);
                                            //        ms2.Write(imageBytes, 0, imageBytes.Length);
                                            //        System.Drawing.Image image2 = System.Drawing.Image.FromStream(ms2, true);
                                            //        //geting server folder path 
                                            //        string spath = System.Web.HttpContext.Current.Server.MapPath("~/Content/UploadedQrFiles/");
                                            //        string path = spath + imagename;
                                            //        //Save Image to server folder
                                            //        image2.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);
                                            //        model.QrCode = a;
                                            //        model.QrImageUrl = imagename;
                                            //        model.ProductId = qrmodel.ProductId;
                                            //        model.CategoryId = qrmodel.CategoryId;
                                            //        customers.GenrateQrCode(model);
                                            //    }
                                            //}
                                        }
                                    }

                                }
                            }
                        }
                    }

                }
                if (checkextension.Contains(".csv"))
                {
                    if (file.ContentLength > 0)
                    {

                        string _filename = file.FileName;
                        string _FileName = Path.GetFileName(file.FileName);
                        string tempGuid = Guid.NewGuid().ToString().Substring(0, 5);
                        string FileName = tempGuid + "_" + _FileName;
                        string _path = Path.Combine(Server.MapPath("~/Content/UploadedFiles"), FileName);
                        file.SaveAs(_path);



                        string csvData = System.IO.File.ReadAllText(_path);
                        foreach (string row in csvData.Split('\n'))
                        {
                            if (!string.IsNullOrEmpty(row))
                            {
                                var a1 = row.Split(',')[0];
                                string a = a1.Remove(a1.Length - 2);
                                if (a.Length == 16 || a.Length == 17)
                                {
                                    var check = customers.CheckGenrateQrCode(a);
                                    if (check.Status == Status.Success)
                                    {
                                        using (MemoryStream ms = new MemoryStream())
                                        {
                                            QRCodeGenerator QrGenerator = new QRCodeGenerator();
                                            QRCodeData QrCodeInfo = QrGenerator.CreateQrCode(a, QRCodeGenerator.ECCLevel.Q);
                                            QRCode QrCode = new QRCode(QrCodeInfo);
                                            using (Bitmap bitMap = QrCode.GetGraphic(20))
                                            {
                                                bitMap.Save(ms, ImageFormat.Png);
                                                var image = "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());
                                                var imagen = Convert.ToBase64String(ms.ToArray()).ToString();
                                                string tempGuid1 = Guid.NewGuid().ToString().Substring(0, 9);
                                                string imagename = tempGuid1 + "_QrImage.Jpeg";
                                                byte[] imageBytes = Convert.FromBase64String(imagen);
                                                MemoryStream ms2 = new MemoryStream(imageBytes, 0, imageBytes.Length);
                                                ms2.Write(imageBytes, 0, imageBytes.Length);
                                                System.Drawing.Image image2 = System.Drawing.Image.FromStream(ms2, true);
                                                //geting server folder path 
                                                string spath = System.Web.HttpContext.Current.Server.MapPath("~/Content/UploadedQrFiles/");
                                                string path = spath + imagename;
                                                //Save Image to server folder
                                                image2.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);
                                                model.QrCode = a;
                                                model.QrImageUrl = imagename;
                                                model.ProductId = qrmodel.ProductId;
                                                model.CategoryId = qrmodel.CategoryId;
                                                customers.GenrateQrCode(model);
                                            }
                                        }

                                    }
                                }
                            }
                        }



                    }
                }

                if (checkextension.Contains(".txt"))
                {

                    if (file.ContentLength > 0)
                    {
                        string _FileName = Path.GetFileName(file.FileName);
                        string tempGuid = Guid.NewGuid().ToString().Substring(0, 5);
                        string FileName = tempGuid + "_" + _FileName;
                        string _path = Path.Combine(Server.MapPath("~/Content/UploadedFiles"), FileName);
                        file.SaveAs(_path);
                        if (!string.IsNullOrEmpty(_path))
                        {
                            HostingEnvironment.QueueBackgroundWorkItem(ctx => TextFileMethod(FileName, qrmodel));
                        }
                    }
                }
                this.ShowMessage("success", "Data Save successfully,Barcode generation in progress please check in few minutes", ToastType.success);
                return RedirectToAction("ManageQr");
            }
            catch (Exception ex)
            {
                this.ShowMessage("Error", ex.Message, ToastType.error);
                logger.Error(ex);
                return RedirectToAction("ManageQr");
            }

            IEnumerable<SelectListItem> ProductCategory = customers.GetProductCategory();
            ViewData["ProductCategory"] = ProductCategory;
            return View();
        }

        public void TextFileMethod(string FileName, NewManageQrViewModel qrmodel)
        {
            ManageQrViewModel model = new ManageQrViewModel();
            using (StreamReader sr = new StreamReader(Path.Combine(Server.MapPath("~/Content/UploadedFiles"), FileName)))
            {
                var ab = 0;
                while (sr.Peek() >= 0)
                {
                    var a = sr.ReadLine();

                    if (a.Length == 16 || a.Length == 17)
                    {
                        var check = customers.CheckGenrateQrCode(a);
                        if (check.Status == Status.Success)
                        {
                            model.QrCode = a;
                            //  model.QrImageUrl = imagename;
                            model.ProductId = qrmodel.ProductId;
                            model.CategoryId = qrmodel.CategoryId;
                            customers.GenrateQrCode(model);

                            //using (MemoryStream ms = new MemoryStream())
                            //{
                            //    QRCodeGenerator QrGenerator = new QRCodeGenerator();
                            //    QRCodeData QrCodeInfo = QrGenerator.CreateQrCode(a, QRCodeGenerator.ECCLevel.Q);
                            //    QRCode QrCode = new QRCode(QrCodeInfo);
                            //    using (Bitmap bitMap = QrCode.GetGraphic(20))
                            //    {
                            //        bitMap.Save(ms, ImageFormat.Png);
                            //        var image = "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());
                            //        var imagen = Convert.ToBase64String(ms.ToArray()).ToString();
                            //        string tempGuid1 = Guid.NewGuid().ToString().Substring(0, 9);
                            //        string imagename = tempGuid1 + "_QrImage.Jpeg";
                            //        byte[] imageBytes = Convert.FromBase64String(imagen);
                            //        MemoryStream ms2 = new MemoryStream(imageBytes, 0, imageBytes.Length);
                            //        ms2.Write(imageBytes, 0, imageBytes.Length);
                            //        System.Drawing.Image image2 = System.Drawing.Image.FromStream(ms2, true);
                            //        //geting server folder path 
                            //        string spath = System.Web.HttpContext.Current.Server.MapPath("~/Content/UploadedQrFiles/");
                            //        string path = spath + imagename;
                            //        //Save Image to server folder
                            //        image2.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);
                            //        model.QrCode = a;
                            //        model.QrImageUrl = imagename;
                            //        model.ProductId = qrmodel.ProductId;
                            //        model.CategoryId = qrmodel.CategoryId;
                            //        customers.GenrateQrCode(model);
                            //    }
                            //}
                        }
                    }

                }
            }
        }



        public void ExcelFileMethod(string FileName, NewManageQrViewModel qrmodel)
        {
            ManageQrViewModel model = new ManageQrViewModel();

        }


        [HttpPost]
        public async Task<JsonResult> GetProductByCategory(ProductCategoryViewModel model)
        {
            var a = customers.GetProductByCategory(model);

            return Json(a, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetQrAllData()
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
                (int TotalCount, int FilteredCount, dynamic Customers) data = customers.GetQrData(skip, take, sortColumn, sortColumnDir, searchValue);
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
        public ActionResult UploadFile(HttpPostedFileBase file)
        {
            StringBuilder strbuild = new StringBuilder();
            ManageQrViewModel model = new ManageQrViewModel();
            try
            {
                var allowedExtensions = new[] { ".txt" };
                if (file == null)
                {
                    this.ShowMessage("Error", "File is Requird!,", ToastType.error);
                    return RedirectToAction("ManageQr");
                }
                var checkextension = Path.GetExtension(file.FileName).ToLower();

                if (!allowedExtensions.Contains(checkextension))
                {
                    this.ShowMessage("Error", "Only text file allow !,", ToastType.error);
                    return RedirectToAction("ManageQr");
                }
                if (file.ContentLength > 0)
                {
                    string _FileName = Path.GetFileName(file.FileName);
                    string tempGuid = Guid.NewGuid().ToString().Substring(0, 5);
                    string FileName = tempGuid + "_" + _FileName;
                    string _path = Path.Combine(Server.MapPath("~/Content/UploadedFiles"), FileName);
                    file.SaveAs(_path);
                    if (!string.IsNullOrEmpty(_path))
                    {
                        using (StreamReader sr = new StreamReader(Path.Combine(Server.MapPath("~/Content/UploadedFiles"), FileName)))
                        {
                            while (sr.Peek() >= 0)
                            {
                                var a = sr.ReadLine();
                                if (a.Length == 16)
                                {
                                    using (MemoryStream ms = new MemoryStream())
                                    {
                                        QRCodeGenerator QrGenerator = new QRCodeGenerator();
                                        QRCodeData QrCodeInfo = QrGenerator.CreateQrCode(a, QRCodeGenerator.ECCLevel.Q);
                                        QRCode QrCode = new QRCode(QrCodeInfo);
                                        using (Bitmap bitMap = QrCode.GetGraphic(20))
                                        {
                                            bitMap.Save(ms, ImageFormat.Png);
                                            var image = "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());
                                            var imagen = Convert.ToBase64String(ms.ToArray()).ToString();
                                            string tempGuid1 = Guid.NewGuid().ToString().Substring(0, 5);
                                            string imagename = tempGuid1 + "_QrImage.Jpeg";
                                            byte[] imageBytes = Convert.FromBase64String(imagen);
                                            MemoryStream ms2 = new MemoryStream(imageBytes, 0, imageBytes.Length);
                                            ms2.Write(imageBytes, 0, imageBytes.Length);
                                            System.Drawing.Image image2 = System.Drawing.Image.FromStream(ms2, true);
                                            //geting server folder path 
                                            string spath = System.Web.HttpContext.Current.Server.MapPath("~/Content/UploadedQrFiles/");
                                            string path = spath + imagename;
                                            //Save Image to server folder
                                            image2.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);
                                            model.QrCode = a;
                                            model.QrImageUrl = path;
                                            customers.GenrateQrCode(model);
                                        }
                                    }
                                }

                            }
                        }
                    }
                }
                this.ShowMessage("success", "Data Save successfully", ToastType.success);
                return RedirectToAction("ManageQr");
            }
            catch (Exception ex)
            {
                this.ShowMessage("Error", "File upload failed !,", ToastType.error);
                logger.Error(ex);
                return RedirectToAction("ManageQr");
            }
        }

        public static Image LoadBase64(string base64)
        {
            byte[] bytes = Convert.FromBase64String(base64);
            Image image;
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                image = Image.FromStream(ms);
            }
            return image;
        }

        //public async Task<ActionResult> DeleteQrCode(int id = 0)
        //{
        //    var a = await customers.DeleteQrData(id);
        //    if (a.Status == Status.Success)
        //    {
        //        this.ShowMessage("success", "Data Delete successfully", ToastType.Success);
        //        return RedirectToAction("ManageQr");
        //    }
        //    return RedirectToAction("ManageQr");
        //}

        [HttpPost]
        public async Task<JsonResult> DeleteQrCode(ManageQrViewModel model)
        {
            return Json(await customers.DeleteQrData(model), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> UpdateProduct(ProductViewModel model)
        {
            try
            {
                //  ResponseModel response = await customers.UpdateCustomerDetails(customer);
                ResponseModel response = await customers.UpdateProductDetails(model);
                if (response.Status == Status.Success)
                    this.ShowMessage("success", "Details updated successfully", ToastType.success);
                else
                    this.ShowMessage("failiure ", "Details not upated successfully ", ToastType.success);
                return Json("1");
            }
            catch (Exception ex)
            {
                return Json("Error");
            }

        }

        public ActionResult Products()
        {
            if (Session["Role"].ToString() != "Admin" && Session["Role"].ToString() != "SubAdmin")
            {
                return RedirectToAction("AccessDenide", "Account");
            }
            CustomerInfoViewModel model = new CustomerInfoViewModel();
            model.CompanyList = customers.GetProductList();
            return View(model);
        }


        [HttpPost]
        public ActionResult GetAllProducts()
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
                (int TotalCount, int FilteredCount, dynamic Customers) data = customers.GetProductAllList(skip, take, sortColumn, sortColumnDir, searchValue);
                return Json(new { draw = draw, recordsFiltered = data.TotalCount, recordsTotal = data.TotalCount, data = data.Customers });
            }
            catch (Exception ex)
            {
                return null;
            }
        }



        public ActionResult CreateProduct()
        {
            if (Session["Role"].ToString() != "Admin" && Session["Role"].ToString() != "SubAdmin")
            {
                return RedirectToAction("AccessDenide", "Account");
            }
            CustomerInfoViewModel data = new CustomerInfoViewModel();
            CustomerInfoViewModel model = new CustomerInfoViewModel();
            //IEnumerable<SelectListItem> ProductCategory = customers.GetProductCategory();
            //ViewData["ProductCategory"] = ProductCategory;
            // IEnumerable<SelectListItem> items = commanServices.GetDistrictts(model.StateId);
            // ViewData["Districstts"] = items;
            return View(data);
        }
        [HttpPost]
        public async Task<ActionResult> CreateProduct(CustomerInfoViewModel model)
        {
            if (ModelState.IsValid)
            {
                //if (file != null)
                //{
                //    if (file.ContentLength > 0)
                //    {
                //        //string _FileName = Path.GetFileName(file.FileName);
                //        //string tempGuid = Guid.NewGuid().ToString().Substring(0, 5);
                //        //string FileName = tempGuid + "_" + _FileName;
                //       // string _path = Path.Combine(Server.MapPath("~/Content/Product"), FileName);
                //        //file.SaveAs(_path);
                //       // model.Photo = FileName;
                //    }
                //}
                ResponseModel response = await customers.CreateProduct(model);
                if (response.Status == Status.Success)
                {
                    this.ShowMessage("success", "Company create successfully", ToastType.success);
                    return RedirectToAction("Products");
                }
                else
                {
                    this.ShowMessage("failiure ", "Company not create successfully ", ToastType.success);
                }
            }
            //IEnumerable<SelectListItem> ProductCategory = customers.GetProductCategory();
            //ViewData["ProductCategory"] = ProductCategory;
            return View(model);
        }


        [HttpPost]
        public async Task<JsonResult> DeleteProduct(CustomerInfoViewModel model)
        {
            return Json(await customers.DeleteProductDetails(model), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> ProductDetails(string Id)
        {
            if (Session["Role"].ToString() != "Admin" && Session["Role"].ToString() != "SubAdmin")
            {
                return RedirectToAction("AccessDenide", "Account");
            }
            var custInfofo = await customers.GetProduct(Guid.Parse(Id));
            //IEnumerable<SelectListItem> ProductCategory = customers.GetProductCategory();
            //ViewData["ProductCategory"] = ProductCategory;
            return View(custInfofo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ProductDetails(CustomerInfoViewModel product)
        {
            CustomerInfoViewModel model = new CustomerInfoViewModel();

            try
            {
                if (ModelState.IsValid)
                {
                    //if (file != null)
                    //{
                    //    //string _FileName = Path.GetFileName(file.FileName);
                    //    //string tempGuid = Guid.NewGuid().ToString().Substring(0, 5);
                    //    //string FileName = tempGuid + "_" + _FileName;
                    //    //string _path = Path.Combine(Server.MapPath("~/Content/Product"), FileName);
                    //    //file.SaveAs(_path);
                    //    //product.Photo = FileName;
                    //}
                    ResponseModel response = await customers.UpdateProductData(product);
                    if (response.Status == Status.Success)
                    {
                        this.ShowMessage("success", "Details updated successfully", ToastType.success);
                        return RedirectToAction("Products");
                    }
                    else
                        this.ShowMessage("failiure ", "Details not upated successfully ", ToastType.error);
                }
                model = await customers.GetProduct(product.Id);
                
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                this.ShowMessage("Error", "An error occured, please after some time,", ToastType.success);
            }
            return View(model);
        }

        public ActionResult ViewCustomer(Guid CustomerId)
        {
            TempData["id"] = CustomerId;

            ViewTestHistoryModel model = new ViewTestHistoryModel();
            model.ViewDataList = customers.ViewCustomerTestList(CustomerId);


            return View(model);
        }


        [HttpPost]
        public JsonResult UpdateQrCodeImage()
        {
            try
            {
                var data = context.QrCodeMasters.Where(x => x.QrImageUrl == null).ToList();
                if (data != null)
                {
                    foreach (var item in data)
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            QRCodeGenerator QrGenerator = new QRCodeGenerator();
                            QRCodeData QrCodeInfo = QrGenerator.CreateQrCode(item.QrCode, QRCodeGenerator.ECCLevel.Q);
                            QRCode QrCode = new QRCode(QrCodeInfo);
                            using (Bitmap bitMap = QrCode.GetGraphic(20))
                            {
                                bitMap.Save(ms, ImageFormat.Png);
                                var image = "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());
                                var imagen = Convert.ToBase64String(ms.ToArray()).ToString();
                                string tempGuid1 = Guid.NewGuid().ToString().Substring(0, 20);
                                string imagename = tempGuid1 + "_QrImage.Jpeg";
                                byte[] imageBytes = Convert.FromBase64String(imagen);
                                MemoryStream ms2 = new MemoryStream(imageBytes, 0, imageBytes.Length);
                                ms2.Write(imageBytes, 0, imageBytes.Length);
                                System.Drawing.Image image2 = System.Drawing.Image.FromStream(ms2, true);
                                //geting server folder path 
                                string spath = System.Web.HttpContext.Current.Server.MapPath("~/Content/UploadedQrFiles/");
                                string path = spath + imagename;
                                //Save Image to server folder
                                image2.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);
                                customers.UpdateQrCode(item.Id, imagename);
                            }
                        }
                    }
                }
                return Json("1");
            }
            catch (Exception ex)
            {
                return Json("Error");
            }

        }
        [HttpPost]
        public async Task<JsonResult> ResetQRData(ManageQrViewModel model)
        {
            return Json(await customers.ResetQRData(model), JsonRequestBehavior.AllowGet);
        }
        public ActionResult CheckQr()
        {
            if (Session["Role"].ToString() != "Admin" && Session["Role"].ToString() != "SubAdmin")
            {
                return RedirectToAction("AccessDenide", "Account");
            }
            ManageQrViewModel model = new ManageQrViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult GetCheckQrAllData()
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
                (int TotalCount, int FilteredCount, dynamic Customers) data = customers.GetCheckQrData(skip, take, sortColumn, sortColumnDir, searchValue);
                return Json(new { draw = draw, recordsFiltered = data.TotalCount, recordsTotal = data.TotalCount, data = data.Customers });
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ActionResult ManageBannerCarousal()
        {
            if (Session["Role"].ToString() != "Admin" && Session["Role"].ToString() != "SubAdmin")
            {
                return RedirectToAction("AccessDenide", "Account");
            }
            ManageQrViewModel model = new ManageQrViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult GetBannerCarousals()
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
                (int TotalCount, int FilteredCount, dynamic Customers) data = customers.GetBannerCarousal(skip, take, sortColumn, sortColumnDir, searchValue);
                return Json(new { draw = draw, recordsFiltered = data.TotalCount, recordsTotal = data.TotalCount, data = data.Customers });
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ActionResult BannerCarousal()
        {
            if (Session["Role"].ToString() != "Admin" && Session["Role"].ToString() != "SubAdmin")
            {
                return RedirectToAction("AccessDenide", "Account");
            }
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> BannerCarousal(HttpPostedFileBase file, BannerCarouselViewModel model)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            if (file == null)
            {
                ViewBag.Message = "Photo is Required !";
                return View();
            }
            var checkextension = Path.GetExtension(file.FileName).ToLower();

            if (!allowedExtensions.Contains(checkextension))
            {
                ViewBag.Message = "Only JPG,JPEG,PNG file allowed !";
                return View();
            }
            if (file != null)
            {
                if (file.ContentLength > 0)
                {
                    string _FileName = Path.GetFileName(file.FileName);
                    string tempGuid = Guid.NewGuid().ToString().Substring(0, 5);
                    string FileName = tempGuid + "_" + _FileName;
                    string _path = Path.Combine(Server.MapPath("~/Content/BannerUploadedFiles"), FileName);
                    file.SaveAs(_path);
                    model.Photo = FileName;
                }
            }

            if (ModelState.IsValid)
            {
                ResponseModel response = await customers.CreateBannerCarousal(model);
                if (response.Status == Status.Success)
                {
                    this.ShowMessage("success", "Banner create successfully", ToastType.success);
                    return RedirectToAction("ManageBannerCarousal", "Customer");
                }
                else
                {
                    this.ShowMessage("failiure ", "Banner not create successfully ", ToastType.success);
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<JsonResult> DeleteBannerCarousal(BannerCarouselViewModel model)
        {
            return Json(await customers.DeleteBannerCarousalDetails(model), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> BannerCarousalDetails(string Id)
        {
            if (Session["Role"].ToString() != "Admin" && Session["Role"].ToString() != "SubAdmin")
            {
                return RedirectToAction("AccessDenide", "Account");
            }
            var BannerInfofo = await customers.GetBannerCarousalDetail(Id);
            return View(BannerInfofo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> BannerCarousalDetails(HttpPostedFileBase file, BannerCarouselViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (file != null)
                    {
                        if (file.ContentLength > 0)
                        {
                            string _FileName = Path.GetFileName(file.FileName);
                            string tempGuid = Guid.NewGuid().ToString().Substring(0, 5);
                            string FileName = tempGuid + "_" + _FileName;
                            string _path = Path.Combine(Server.MapPath("~/Content/BannerUploadedFiles"), FileName);
                            file.SaveAs(_path);
                            model.Photo = FileName;
                        }
                    }

                    ResponseModel response = await customers.UpdateBannerCarousalData(model);
                    if (response.Status == Status.Success)
                    {
                        this.ShowMessage("success", "Details updated successfully", ToastType.success);
                        return RedirectToAction("ManageBannerCarousal", "Customer");
                    }
                    else
                        this.ShowMessage("failiure ", "Details not upated successfully ", ToastType.error);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                this.ShowMessage("Error", "An error occured, please after some time,", ToastType.success);
            }
            return View();
        }
        [HttpGet]

        public async Task<ActionResult> ViewProduct(string Id)
        {
           CustomerInfoViewModel model = new CustomerInfoViewModel();
            model = await customers.GetProduct(Guid.Parse(Id));
            return View(model);

        }
        public ActionResult Ticket()
        {
            if (Session["Role"].ToString() != "Admin" && Session["Role"].ToString() != "SubAdmin")
            {
                return RedirectToAction("AccessDenide", "Account");
            }
            return View();
        }
        public ActionResult Tickets()
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
                (int TotalCount, int FilteredCount, dynamic Customers) data = customers.Tickets(skip, take, sortColumn, sortColumnDir, searchValue);
                return Json(new { draw = draw, recordsFiltered = data.TotalCount, recordsTotal = data.TotalCount, data = data.Customers });
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        [HttpPost]
        public ActionResult filterTickets(string startDate = null,string endDate=null, int status = 0)
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
                (int TotalCount, int FilteredCount, dynamic Customers) data = customers.Tickets(skip, take, sortColumn, sortColumnDir, searchValue, startDate, endDate, status);
                return Json(new { draw = draw, recordsFiltered = data.TotalCount, recordsTotal = data.TotalCount, data = data.Customers });
            }
            catch (Exception ex)
            {
                return null;
            }

        }


        public async Task<ActionResult> ViewTicket(Guid? Id)
        {

            List<TicketMessageSystemViewModels> model = new List<TicketMessageSystemViewModels>();
            var UserId = new Guid(Session["UserId"].ToString());
            TicketViewModel data = new TicketViewModel();
            if (Id != null)
            {
                data = await customers.TicketDetails(Id);
            }
            if (UserId != null)
            {
                model = customers.ViewUserMessageList(UserId, Id);
            }
            data.ViewTicketMessageList = model;
            data.TicketId1 = Id;
            return View(data);
        }
        [HttpPost]
        public async Task<ActionResult> ViewTicket(TicketViewModel mod)
        {
            var UserId = Session["UserId"].ToString();

            ResponseModel data = new ResponseModel();
            if (UserId != null)
            {
                mod.UpdatedBy = UserId;
                ResponseModel response = await customers.UpdateTicket(mod);
                if (response.Status == Status.Success)
                {
                    this.ShowMessage("success", "Ticket updated successfully", ToastType.success);
                    return RedirectToAction("Ticket", "Customer");
                }
                else
                {
                    this.ShowMessage("failiure ", "Ticket not upated successfully ", ToastType.error);
                }
            }
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> TicketMessageSystem(TicketViewModel TicketMessage)
        {
            TicketMessageSystemViewModels model = new TicketMessageSystemViewModels();
            var UserId = Session["UserId"].ToString();
            if (TicketMessage.Description != null)
            {
                model.Description = TicketMessage.Description;
                model.UserId = new Guid(UserId);
                model.TicketId = new Guid(TicketMessage.TicketId1.ToString());
                model.CustomerId = new Guid(TicketMessage.CustomerId);
                ResponseModel response = await customers.TicketMessage(model);
                if (response.Status == Status.Success)
                {
                    this.ShowMessage("success", "Message Send successfully", ToastType.success);
                    return Json(response);
                    //return RedirectToAction("ViewTicket", "Customer", new { id = TicketMessage.TicketId1 });
                }
                else
                {
                    this.ShowMessage("failiure ", "Message not Send successfully ", ToastType.success);
                }
            }
            return RedirectToAction("ViewTicket", "Customer", new { id = TicketMessage.TicketId1 });
            //return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> EditCustProfile()
        {
            Guid UserId = new Guid(Session["UserId"].ToString());
            CustomerInfoViewModel model = new CustomerInfoViewModel();
            model = await customers.GetProfileDetails(UserId);
            IEnumerable<SelectListItem> States = commanServices.GetStates();
            ViewData["States"] = States;
            IEnumerable<SelectListItem> items = commanServices.GetDistrictts(model.StateId);
            ViewData["Districstts"] = items;
            return View(model);

        }

        [HttpPost]
        public async Task<ActionResult> EditCustProfile(CustomerInfoViewModel model, HttpPostedFileBase file)
        {
            ResponseModel obj = new ResponseModel();
            try
            {
                if (file != null)
                {
                    if (file.ContentLength > 0)
                    {
                        string Extension = Path.GetExtension(file.FileName).ToLower();
                        if (!(Extension == ".jpg" || Extension == ".png" || Extension == ".jpeg"))
                        {
                            ViewBag.error = "File should be .jpg .png or .jpeg only";
                            return View(model);
                        }
                        string _FileName = Path.GetFileName(file.FileName);
                        string tempGuid = Guid.NewGuid().ToString().Substring(0, 5);
                        string FileName = tempGuid + "_" + _FileName;
                        string _path = Path.Combine(Server.MapPath("~/Content/Profile"), FileName);
                        file.SaveAs(_path);
                        model.Photo = FileName;
                    }
                }

                obj = await customers.UpdateProfileDetails(model);
                if (obj.Status == Status.Success)
                {
                    this.ShowMessage("success", "Details updated successfully", ToastType.success);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    this.ShowMessage("failiure ", "Details not upated successfully ", ToastType.error);
                }
            }
            catch (Exception)
            {
                this.ShowMessage("Error", "An error occured, please after some time,", ToastType.success);
            }

            return View();
        }

        public async Task<ActionResult> ViewCustProfile()
        {
            Guid UserId = new Guid(Session["UserId"].ToString());
            CustomerInfoViewModel model = new CustomerInfoViewModel();
            model = await customers.GetProfileDetails(UserId);
            IEnumerable<SelectListItem> States = commanServices.GetStates();
            ViewData["States"] = States;
            IEnumerable<SelectListItem> items = commanServices.GetDistrictts(model.StateId);
            ViewData["Districstts"] = items;
            return View(model);
        }

        public async Task<ActionResult> ViewAdminProfile()
        {
            Guid UserId = new Guid(Session["UserId"].ToString());
            CustomerInfoViewModel model = new CustomerInfoViewModel();
            model = await customers.GetProfileDetails(UserId);
            IEnumerable<SelectListItem> States = commanServices.GetStates();
            ViewData["States"] = States;
            IEnumerable<SelectListItem> items = commanServices.GetDistrictts(model.StateId);
            ViewData["Districstts"] = items;
            return View(model);
        }

        public ActionResult Shipping()
        {
            return View();
        }
        public ActionResult GetShipping()
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
                (int TotalCount, int FilteredCount, dynamic Customers) data = customers.Shipping(skip, take, sortColumn, sortColumnDir, searchValue);
                return Json(new { draw = draw, recordsFiltered = data.TotalCount, recordsTotal = data.TotalCount, data = data.Customers });
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        public ActionResult CreateShipping()
        {
            ShippingViewModel model = new ShippingViewModel();
            Guid UserId = new Guid(Session["UserId"].ToString());
            IEnumerable<SelectListItem> States = customers.GetStates();
            ViewData["States"] = States;
            IEnumerable<SelectListItem> d = commanServices.GetDistrictts(3);
            TempData["Test"] = d;
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreateShipping(ShippingViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid UserId = new Guid(Session["UserId"].ToString());
                model.CreatedBy = UserId.ToString();
                ResponseModel response = await customers.CreateShiping(model);
                if (response.Status == Status.Success)
                {
                    this.ShowMessage("success", "Shiping create successfully", ToastType.success);
                    return RedirectToAction("Shipping", "Customer");
                }
                else
                {
                    this.ShowMessage("failiure ", "Shiping not create successfully ", ToastType.success);
                }
            }
            IEnumerable<SelectListItem> States = customers.GetStates();
            ViewData["States"] = States;
            IEnumerable<SelectListItem> d = commanServices.GetDistrictts(3);
            TempData["Test"] = d;
            return View(model);
        }

        public async Task<ActionResult> EditShiping(string id)
        {
            ShippingViewModel model = new ShippingViewModel();
            if (id != null)
            {
                model = await customers.EditShip(id);
                IEnumerable<SelectListItem> States = customers.GetStates();
                //IEnumerable<SelectListItem> District = customers.GetDistrictts(Convert.ToInt32(model.StateId), Convert.ToInt32(model.DistricttId));
                ViewData["States"] = States;
                // ViewData["District"]=District;  
            }
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> EditShiping(ShippingViewModel model)
        {
            ResponseModel res = new ResponseModel();
            if (ModelState.IsValid)
            {
                try
                {
                    res = await customers.UpdateShip(model);
                    if (res.Status == Status.Success)
                    {
                        this.ShowMessage("success", "Shiping Updated successfully", ToastType.success);
                        return RedirectToAction("Shipping", "Customer");
                    }
                    else
                    {
                        this.ShowMessage("failiure ", "Shiping not Updated successfully ", ToastType.success);
                    }
                }
                catch (Exception)
                {
                    this.ShowMessage("failiure ", "Shiping not Updated successfully ", ToastType.success);
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<JsonResult> DeleteShiping(string Id)
        {
            return Json(await customers.DeleteShipingDetails(Id), JsonRequestBehavior.AllowGet);
        }
        public ActionResult ShippingTracking()
        {

            return View();
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
                (int TotalCount, int FilteredCount, dynamic Customers) data = customers.GetOrders(skip, take, sortColumn, sortColumnDir, searchValue);
                return Json(new { draw = draw, recordsFiltered = data.TotalCount, recordsTotal = data.TotalCount, data = data.Customers });
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        [HttpPost]
        public ActionResult GetFilterOrder(string date, string status)
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
                (int TotalCount, int FilteredCount, dynamic Customers) data = customers.GetOrders(skip, take, sortColumn, sortColumnDir, searchValue, date, status);
                return Json(new { draw = draw, recordsFiltered = data.TotalCount, recordsTotal = data.TotalCount, data = data.Customers });
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<ActionResult> TarckingDetails(string Id)
        {
            OrderHistory model = new OrderHistory();
            Guid UserId = new Guid(Session["UserId"].ToString());
            if (Id != null)
            {
                model = await customers.OrderHistoryDetails(Id);
                model.CustomerDetail = customers.GetCustomerDetail(new Guid(model.CustId));

            }
            return View(model);
        }
        [HttpPost]
        public async Task<JsonResult> UpdateTracking(string Shipstatus, string OrderId, string TrackingNu, string TrackingCN)
        {
            Guid UserId = new Guid(Session["UserId"].ToString());
            ResponseModel model = new ResponseModel();
            if (Shipstatus != null)
            {
                model = await customers.Updatetarcking(Shipstatus, OrderId, TrackingNu, TrackingCN);
                if (model.Status == Status.Success)
                {
                    return Json(Status.Success);
                }
                else
                {
                    return Json(Status.Failure);
                }
            }
            return Json(Status.Failure);
        }

        public JsonResult MenuData(string SubAdminId = null)
        {
            MenuPermissionViewModel per = new MenuPermissionViewModel();
            if (SubAdminId != null)
            {
                Guid id = new Guid(SubAdminId);
                var mp = context.MenuPermission.Where(x => x.SubAdminId == id).FirstOrDefault();
                if (mp != null)
                {
                    if (mp.MallManage==true)
                    {
                        per.MenuList[0].check = true;
                    }
                    if (mp.CustomerManage == true)
                    {
                        per.MenuList[1].check = true;
                    }
                    if (mp.TicketSupportManage == true)
                    {
                        per.MenuList[2].check = true;
                    }
                    if (mp.Setting == true)
                    {
                        per.MenuList[3].check = true;
                    }
                    if (mp.AuthenticatorManage == true)
                    {
                        per.MenuList[4].check = true;
                    }
                    if (mp.FrontEndManage == true)
                    {
                        per.MenuList[5].check = true;
                    }
                    if (mp.CertificateManage == true)
                    {
                        per.MenuList[6].check = true;
                    }
                    if (mp.NewsManage == true)
                    {
                        per.MenuList[7].check = true;
                    } 
                    if (mp.FormManage == true)
                    {
                        per.MenuList[8].check = true;
                    }
                }
            }
            return Json(per);
        }
        public ActionResult ViewAdmindata()
        {
            MenuPermissionViewModel per = new MenuPermissionViewModel();
            if (Session["Role"].ToString() != "Admin" && Session["Role"].ToString() != "SubAdmin")
            {
                return RedirectToAction("AccessDenide", "Account");
            }

            return View(per);

        }
        [HttpPost]
        public ActionResult ViewAdmindata(MenuPermissionViewModel data)
        {
            var UserId = new Guid(Session["UserId"].ToString());
            ResponseModel res = new ResponseModel();
            try
            {
                data.AdminId = UserId;
                res = customers.menuassign(data);
                if (res.Status == Status.Success)
                {
                    ModelState.Clear();
                    this.ShowMessage("success", "Menus Assigned successfully !,", ToastType.success);
                }
                else
                {
                    this.ShowMessage("error", "Menus not Assigned successfully !", ToastType.error);
                }
            }
            catch (Exception ex)
            {
                this.ShowMessage("error", "Menus not Assigned successfully !", ToastType.error);
            }
            return RedirectToAction("ViewAdmindata", "Customer");
        }
        [ChildActionOnly]
        public ActionResult SubAdminmenu()
        {
            MenuPermissionViewModel per = new MenuPermissionViewModel();
            Guid UserId = new Guid(Session["UserId"].ToString());
            per = (from a in context.MenuPermission.Where(x => x.SubAdminId == UserId)
                   select new MenuPermissionViewModel
                   {
                       AdminId = a.AdminId,
                       SubAdminId = a.SubAdminId,
                       MallManage = a.MallManage,
                       CustomerManage = a.CustomerManage,
                       TicketSupportManage = a.TicketSupportManage,
                       Setting = a.Setting,
                       AuthenticatorManage = a.AuthenticatorManage,
                       FrontEndManage = a.FrontEndManage,
                       CertificateManage = a.CertificateManage,
                       NewsManage = a.NewsManage,
                       FormManage = a.FormManage
                   }).FirstOrDefault();
            return PartialView("_SubAdminMenu", per);
        }

        [HttpPost]
        public ActionResult Admindata()
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
                (int TotalCount, int FilteredCount, dynamic Customers) data = customers.GetAdmindata(skip, take, sortColumn, sortColumnDir, searchValue);
                return Json(new { draw = draw, recordsFiltered = data.TotalCount, recordsTotal = data.TotalCount, data = data.Customers });
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        public ActionResult AdminCreate()
        {
            IEnumerable<SelectListItem> States = commanServices.GetStates();
            ViewData["States"] = States;
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> AdminCreate(CustomerInfoViewModel customer)
        {
            ResponseModel model = new ResponseModel();
            if (ModelState.IsValid)
            {
                model = await customers.Create(customer);
            }
            IEnumerable<SelectListItem> States = commanServices.GetStates();
            ViewData["States"] = States;
            if (model.Status == Status.Success)
            {
                ModelState.Clear();
                this.ShowMessage("success", "You are successfully registered,", ToastType.success);
                return RedirectToAction("Index");
            }
            else
            {
                this.ShowMessage("error", "Registration not successfully", ToastType.error);
            }
            return View();
        }
        public async Task<ActionResult> SubAdminDetail(string customerId)
        {
            var custInfofo = await customers.Subadmindetails(Guid.Parse(customerId));
            IEnumerable<SelectListItem> States = commanServices.GetStates();
            ViewData["States"] = States;
            IEnumerable<SelectListItem> items = commanServices.GetDistrictts(custInfofo.StateId);
            ViewData["Districstts"] = items;
            return View(custInfofo);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SubAdminDetail(CustomerInfoViewModel customer)
        {
            CustomerInfoViewModel model = new CustomerInfoViewModel();
            try
            {

                ResponseModel response = await customers.UpdateSubadmindetails(customer);
                model = await customers.GetCustomers(customer.Id);
                IEnumerable<SelectListItem> States = commanServices.GetStates();
                ViewData["States"] = States;
                IEnumerable<SelectListItem> items = commanServices.GetDistrictts(model.StateId);
                ViewData["Districstts"] = items;
                if (response.Status == Status.Success)
                {
                    this.ShowMessage("success", "Details updated successfully", ToastType.success);
                    return RedirectToAction("ViewAdmindata");
                }
                else
                {
                    this.ShowMessage("error", "Registration not successfully", ToastType.error);
                }


            }
            catch (Exception ex)
            {
                logger.Error(ex);
                this.ShowMessage("Error", "An error occured, please after some time,", ToastType.success);
            }
            return View(model);
        }
        [HttpPost]
        public async Task<JsonResult> DeleteSubadminDetais(CustomerInfoViewModel customer)
        {
            return Json(await customers.DeleteSubadminDetails(customer), JsonRequestBehavior.AllowGet);
        }
        public ActionResult ViewCertificate()
        {

            return View();

        }
        public ActionResult UnVerifiedCertificate()
        {

            return View();

        }
        public ActionResult RejectCertificate()
        {

            return View();

        }
        [HttpPost]
        public ActionResult ViewCertificateVerifiedList(string data, string date)
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
                (int TotalCount, int FilteredCount, dynamic Customers) data1 = customers.GetCertificateList(skip, take, sortColumn, sortColumnDir, searchValue, data);
                (int TotalCount, int FilteredCount, dynamic Customers) data2 = customers.GetCertificateFilterList(skip, take, sortColumn, sortColumnDir, searchValue, data, date);
                if (date != null)
                {
                    return Json(new { draw = draw, recordsFiltered = data2.TotalCount, recordsTotal = data2.TotalCount, data = data2.Customers });
                }
                return Json(new { draw = draw, recordsFiltered = data1.TotalCount, recordsTotal = data1.TotalCount, data = data1.Customers });
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpGet]
        public async Task<ActionResult> EditVeryfied(int Id)
        {
            TrackingForms model = new TrackingForms();
            if (Id > 0)
            {
                model = await customers.EditVerify(Id);

            }
            return View(model);

        }
        [HttpPost]
        public async Task<ActionResult> UpdateEditVeryfied(TrackingForms Track)
        {
            TrackingForms model = new TrackingForms();
            var UserId = Session["UserId"].ToString();
            Track.VerifiedBy = UserId;
            ResponseModel response = await customers.UpdateVerify(Track);
            if (response.Status == Status.Success)
            {
                this.ShowMessage("success", "Details updated successfully", ToastType.success);
                return RedirectToAction("ViewCertificate");
            }
            else
            {
                this.ShowMessage("error", "Registration not successfully", ToastType.error);
            }

            return View(model);

        }
        public ActionResult FAQDetails()
        {
            FaqViewModel model = new FaqViewModel();
            model.FAQList = customers.GetFAQList();
            return View(model);
        }
        public ActionResult GetFAQDetails()
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
                (int TotalCount, int FilteredCount, dynamic News) data = customers.GetFAQDetails(skip, take, sortColumn, sortColumnDir, searchValue);
                return Json(new { draw = draw, recordsFiltered = data.TotalCount, recordsTotal = data.TotalCount, data = data.News });
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public ActionResult EditFAQNews(Guid Id)
        {
            FaqViewModel model = new FaqViewModel();
            if (Session["Role"].ToString() != "Admin" && Session["Role"].ToString() != "SubAdmin") 
            {
                return RedirectToAction("AccessDenide", "Account");
            }
            model = customers.GetFAQData(Id);
            return View(model);
        }
        [ValidateInput(false)]
        [HttpPost]
        public async Task<ActionResult> EditFAQNews(FaqViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    ResponseModel response = await customers.UpdateFAQDetails(model);
                    if (response.Status == Status.Success)
                    {
                        this.ShowMessage("Success", "Updated successfully", ToastType.success);
                        return RedirectToAction("FAQDetails");
                    }
                    else
                    {
                        this.ShowMessage("failiure", "News not Updated Successfully", ToastType.success);
                    }

                }
            }
            catch (Exception)
            {

                throw;
            }

            return View(model);
        }
        public async Task<ActionResult> DeleteFAQNews(Guid Id)
        {
            await customers.DeleteFAQDetails(Id);
            return RedirectToAction("FAQDetails");

        }

        public ActionResult FAQ()
        {
            return View();
        }
        [ValidateInput(false)]
        [HttpPost]
        public async Task<ActionResult> FAQ(FaqViewModel data)
        {
            ResponseModel model = new ResponseModel();
            var UserId = Session["UserId"].ToString();
            if (ModelState.IsValid)
            {
                if (data != null)
                {
                    data.CreatedBy = UserId;
                    model = await customers.SaveFAQdata(data);
                }

            }

            if (model.Status == Status.Success)
            {
                ModelState.Clear();
                this.ShowMessage("success", "You are successfully registered,", ToastType.success);
                return RedirectToAction("FAQDetails");
            }
            else
            {
                this.ShowMessage("error", "Registration not successfully", ToastType.error);
            }
            return View();
        }


        public ActionResult AboutDetails()
        {
            AboutViewModel model = new AboutViewModel();
            model.aboutList = customers.GetAboutList();
            return View(model);
        }
        public ActionResult GetAboutDetails()
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
                (int TotalCount, int FilteredCount, dynamic News) data = customers.GetAboutDetails(skip, take, sortColumn, sortColumnDir, searchValue);
                return Json(new { draw = draw, recordsFiltered = data.TotalCount, recordsTotal = data.TotalCount, data = data.News });
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public ActionResult EditAboutNews(Guid Id)
        {
            AboutViewModel model = new AboutViewModel();
            if (Session["Role"].ToString() != "Admin" && Session["Role"].ToString() != "SubAdmin")
            {
                return RedirectToAction("AccessDenide", "Account");
            }
            model = customers.GetAboutData(Id);
            return View(model);
        }
        [ValidateInput(false)]
        [HttpPost]
        public async Task<ActionResult> EditAboutNews(AboutViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    ResponseModel response = await customers.UpdateAboutDetails(model);
                    if (response.Status == Status.Success)
                    {
                        this.ShowMessage("Success", "Updated successfully", ToastType.success);
                        return RedirectToAction("AboutDetails");
                    }
                    else
                    {
                        this.ShowMessage("failiure", "News not Updated Successfully", ToastType.success);
                    }

                }
            }
            catch (Exception)
            {

                throw;
            }

            return View(model);
        }
        [HttpPost]
        public async Task<ActionResult> DeleteAboutNews(Guid Id)
        {
            await customers.DeleteAboutDetails(Id);
            return RedirectToAction("AboutDetails");

        }

        [HttpGet]
        public ActionResult About()
        {
            return View();
        }
        [ValidateInput(false)]
        [HttpPost]
        public async Task<ActionResult> About(AboutViewModel data)
        {
            ResponseModel model = new ResponseModel();
            var UserId = Session["UserId"].ToString();
            if (ModelState.IsValid)
            {
                if (data != null)
                {
                    data.CreatedBy = UserId;
                    model = await customers.Savedata(data);
                }

            }

            if (model.Status == Status.Success)
            {
                ModelState.Clear();
                this.ShowMessage("success", "You are successfully registered,", ToastType.success);
                return RedirectToAction("AboutDetails");
            }
            else
            {
                this.ShowMessage("error", "Registration not successfully", ToastType.error);
            }
            return View();
        }



        public ActionResult HelpGuidDetails()
        {
            HelpGuidViewModel model = new HelpGuidViewModel();
            model.HelpList = customers.GetHelpGuidList();
            return View(model);
        }
        public ActionResult GetHelpGuidDetails()
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
                (int TotalCount, int FilteredCount, dynamic News) data = customers.GetHelpGuidDetails(skip, take, sortColumn, sortColumnDir, searchValue);
                return Json(new { draw = draw, recordsFiltered = data.TotalCount, recordsTotal = data.TotalCount, data = data.News });
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public ActionResult EditHelpGuidNews(Guid Id)
        {
            HelpGuidViewModel model = new HelpGuidViewModel();
            if (Session["Role"].ToString() != "Admin" && Session["Role"].ToString() != "SubAdmin")
            {
                return RedirectToAction("AccessDenide", "Account");
            }
            model = customers.GetHelpGuidData(Id);
            return View(model);
        }
        [ValidateInput(false)]
        [HttpPost]
        public async Task<ActionResult> EditHelpGuidNews(HelpGuidViewModel Helpdata)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    ResponseModel response = await customers.UpdateHelpGuidDetails(Helpdata);
                    if (response.Status == Status.Success)
                    {
                        this.ShowMessage("Success", "Updated successfully", ToastType.success);
                        return RedirectToAction("HelpGuidDetails");
                    }
                    else
                    {
                        this.ShowMessage("failiure", "News not Updated Successfully", ToastType.success);
                    }

                }
            }
            catch (Exception)
            {

                throw;
            }

            return View(Helpdata);
        }
        public async Task<ActionResult> DeleteHelpGuidNews(Guid Id)
        {
            await customers.DeleteHelpGuidDetails(Id);
            return RedirectToAction("HelpGuidDetails");

        }
        public ActionResult HelpGuild()
        {
            return View();
        }
        [ValidateInput(false)]
        [HttpPost]
        public async Task<ActionResult> HelpGuild(HelpGuidViewModel helpdata)
        {
            ResponseModel model = new ResponseModel();
            var UserId = Session["UserId"].ToString();
            if (ModelState.IsValid)
            {
                if (helpdata != null)
                {
                    helpdata.CreatedBy = UserId;
                    model = await customers.SaveHelpdata(helpdata);
                }

            }

            if (model.Status == Status.Success)
            {
                ModelState.Clear();
                this.ShowMessage("success", "You are successfully registered,", ToastType.success);
                return RedirectToAction("HelpGuidDetails");
            }
            else
            {
                this.ShowMessage("error", "Registration not successfully", ToastType.error);
            }
            return View();
        }



        public ActionResult TermPrivacyDetails()
        {
            TermPrivacyViewModel model = new TermPrivacyViewModel();
            model.TermPrivacyList = customers.GetTermPrivacyList();
            return View(model);
        }
        public ActionResult GetTermPrivacyDetails()
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
                (int TotalCount, int FilteredCount, dynamic News) data = customers.GetTermPrivacyDetails(skip, take, sortColumn, sortColumnDir, searchValue);
                return Json(new { draw = draw, recordsFiltered = data.TotalCount, recordsTotal = data.TotalCount, data = data.News });
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public ActionResult EditTermPrivacyNews(Guid Id)
        {
            TermPrivacyViewModel model = new TermPrivacyViewModel();
            if (Session["Role"].ToString() != "Admin" && Session["Role"].ToString() != "SubAdmin")
            {
                return RedirectToAction("AccessDenide", "Account");
            }
            model = customers.GetTermPrivacyData(Id);
            return View(model);
        }
        [ValidateInput(false)]
        [HttpPost]
        public async Task<ActionResult> EditTermPrivacyNews(TermPrivacyViewModel TermPrivacydata)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    ResponseModel response = await customers.UpdaTermPrivacyDetails(TermPrivacydata);
                    if (response.Status == Status.Success)
                    {
                        this.ShowMessage("Success", "Updated successfully", ToastType.success);
                        return RedirectToAction("TermPrivacyDetails");
                    }
                    else
                    {
                        this.ShowMessage("failiure", "News not Updated Successfully", ToastType.success);
                    }

                }
            }
            catch (Exception)
            {

                throw;
            }

            return View(TermPrivacydata);
        }
        [HttpPost]
        public async Task<ActionResult> DeleteTermPrivacyNews(Guid Id)
        {
            await customers.DeleteTermPrivacytDetails(Id);
            return RedirectToAction("TermPrivacyDetails");

        }
        public ActionResult TermPrivacy()
        {
            return View();
        }
        //public ActionResult Gallery(string ProductId)
        //{
        //    ProductGalleryViewModel model = new ProductGalleryViewModel();
        //    try
        //    {
        //        List<ProductGalleryViewModel> model1 = new List<ProductGalleryViewModel>();
        //        var prodect = context.ProductMasters.Where(a => a.IsActive != false && a.Id == new Guid(ProductId)).FirstOrDefault();
        //        model.ProductGalleryList = customers.GetGallery(ProductId);
        //        model.ProductId = ProductId;
        //        model.ProductName = prodect.ProductName;
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return View(model);
        //}
        //[HttpPost]
        //public async Task<ActionResult> Gallery(HttpPostedFileBase[] file, ProductGalleryViewModel model1)
        //{
        //    List<ProductGalleryViewModel> gallery = new List<ProductGalleryViewModel>();

        //    Guid UserId = new Guid(Session["UserId"].ToString());
        //    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
        //    if (file == null)
        //    {
        //        ViewBag.Message = "Photo is Required !";
        //        return View();
        //    }
        //    try
        //    {
        //        foreach (HttpPostedFileBase files in file)
        //        {
        //            //Checking file is available to save.  
        //            if (file != null)
        //            {
        //                ProductGalleryViewModel model = new ProductGalleryViewModel();
        //                var checkextension = Path.GetExtension(files.FileName).ToLower();
        //                if (!allowedExtensions.Contains(checkextension))
        //                {
        //                    ViewBag.Message = "Only JPG,JPEG,PNG file allowed !";
        //                    return View();
        //                }
        //                if (files.ContentLength > 0)
        //                {
        //                    string _FileName = Path.GetFileName(files.FileName);
        //                    string tempGuid = Guid.NewGuid().ToString().Substring(0, 5);
        //                    string FileName = tempGuid + "_" + _FileName;
        //                    string _path = Path.Combine(Server.MapPath("~/Content/Gallery"), FileName);
        //                    files.SaveAs(_path);
        //                    model.Photo = FileName;
        //                    model.CreatedBy = UserId.ToString();
        //                    model.ProductId = model1.ProductId;
        //                }
        //                gallery.Add(model);
        //                model = null;
        //            }

        //        }
        //        if (gallery.Count > 0)
        //        {
        //            ResponseModel response = await customers.AddGallery(gallery);
        //            if (response.Status == Status.Success)
        //            {
        //                this.ShowMessage("success", "Gallery added successfully !!", ToastType.success);
        //                // return RedirectToAction("Gallery", "Customer");
        //            }
        //            else
        //            {
        //                this.ShowMessage("failiure ", "Gallery not Added !! ", ToastType.success);
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //    return RedirectToAction("Gallery", new { ProductId = model1.ProductId });
        //    // return View(model1.ProductId);
        //}
        [ValidateInput(false)]
        [HttpPost]
        public async Task<ActionResult> TermPrivacy(TermPrivacyViewModel TermPrivacydata)
        {
            ResponseModel model = new ResponseModel();
            var UserId = Session["UserId"].ToString();
            if (ModelState.IsValid)
            {
                if (TermPrivacydata != null)
                {
                    TermPrivacydata.CreatedBy = UserId;
                    model = await customers.SaveTermPrivacydata(TermPrivacydata);
                }
            }
            if (model.Status == Status.Success)
            {
                ModelState.Clear();
                this.ShowMessage("success", "You are successfully registered,", ToastType.success);
                return RedirectToAction("TermPrivacyDetails");
            }
            else
            {
                this.ShowMessage("error", "Registration not successfully", ToastType.error);
            }
            return View();
        }
        [HttpPost]
        public async Task<JsonResult> DeleteGallery(string Id)
        {
            return Json(await customers.DeleteGalleryImage(Id), JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> Certificate(int Id)
        {
            if (Id == 0)
            {
                return RedirectToAction("Index", "Customer");
            }
            TrackingFormsViewModel model = new TrackingFormsViewModel();
            model = await customers.GetCertificateData(Id);
            return View(model);
        }
        public async Task<ActionResult> ViewTestCertificate(int Id)
        {
            if (Id > 0)
            {
                RedirectToAction("Index", "Customer");
            }
            TrackingFormsViewModel model = new TrackingFormsViewModel();
            model = await customers.GetCertificateData(Id);
            return View(model);
        }


        public ActionResult StaffRoll()
        {

            return View();
        }
        [HttpPost]
        public ActionResult GetStaffRoll()
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
                (int TotalCount, int FilteredCount, dynamic Customers) data = customers.GetStaffRollList(skip, take, sortColumn, sortColumnDir, searchValue);
                return Json(new { draw = draw, recordsFiltered = data.TotalCount, recordsTotal = data.TotalCount, data = data.Customers });
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpPost]
        public async Task<JsonResult> ChangeRole(CustomerInfo model)
        {
            return Json(await customers.UpdateRole(model), JsonRequestBehavior.AllowGet);
        }

    }
}