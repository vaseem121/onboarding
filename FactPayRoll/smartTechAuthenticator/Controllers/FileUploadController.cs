using ExcelDataReader;
using smartTechAuthenticator.Services.Comman;
using smartTechAuthenticator.Services.Comman.CustomFilters;
using smartTechAuthenticator.Services.Customers;
using smartTechAuthenticator.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace smartTechAuthenticator.Controllers
{
    [Authorized]
    public class FileUploadController : Controller
    {
        private readonly ICustomersService customers;
        // GET: FileUpload
        static string connectionString = System.Configuration.ConfigurationManager.AppSettings["ConnectionString"].ToString();
        static string userName = System.Configuration.ConfigurationManager.AppSettings["userName"].ToString();
        static string password = System.Configuration.ConfigurationManager.AppSettings["password"].ToString();
        static string mailFrom = System.Configuration.ConfigurationManager.AppSettings["mailFrom"].ToString();
        static string smtpServer = System.Configuration.ConfigurationManager.AppSettings["smtpServer"].ToString();
        static string port = System.Configuration.ConfigurationManager.AppSettings["port"].ToString();
        public FileUploadController(ICustomersService _customers)
        {
            customers = _customers;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadQrCode(HttpPostedFileBase file, NewManageQrViewModel qrmodel)
        {
            string data = null;
            try
            {
                var Email = Session["userName"].ToString();
                var allowedExtensions = new[] { ".txt", ".xlsx", ".xls", ".csv" };
                if (file == null)
                {
                    this.ShowMessage("Error", "File is Requird!,", ToastType.error);
                    return RedirectToAction("ManageQr", "Customer");
                }
                var checkextension = Path.GetExtension(file.FileName).ToLower();

                if (!allowedExtensions.Contains(checkextension))
                {
                    this.ShowMessage("Error", "Only text,xlsx,csv file allow !,", ToastType.error);
                    return RedirectToAction("ManageQr", "Customer");
                }
                if (checkextension.Contains(".xlsx") || checkextension.Contains(".xls"))
                {
                     data = XlsxFileReadData(file, qrmodel);
                }
                if (checkextension.Contains(".csv"))
                {
                    data = CsvFileReadData(file, qrmodel);
                }

                if (checkextension.Contains(".txt"))
                {
                     data = TextFileReadData(file, qrmodel);

                }


                MailMessage mail = new MailMessage();
                mail.To.Add(Email);
               // mail.To.Add("bansh@nextolive.com");
                mail.From = new MailAddress(mailFrom);
                mail.Subject = "Uploaded codes";
                string Body = "<br/><br/>File Name:"+ file.FileName + "" +
                "<br/>Total Records:"+ data + "" +
                "<br/>Code Uploaded:"+data+"" +
                "<br/>Existing Records:0" +
                "<br/>File Uploaded At:"+DateTime.Now+"" +
                "<br/>File Processed At:"+DateTime.Now+"";
                //mail.Subject = "Uploaded codes";
                //string Body = "<br/><br/>Hi "+ Session["Name"].ToString() + "," +
                //"<br/>Codes Uploaded successfully ! " +
                //"<br/><br/>Regards" +
                //"<br/> SmartTech" +
                //"<br/><p> All Rights Reserved by Smart Tech-admin. Designed and Developed by Smart Tech.</p>";
                mail.Body = Body;
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = new System.Net.NetworkCredential(userName, password); // Enter seders User name and password  
                smtp.Host = smtpServer;
                smtp.Port = Convert.ToInt32(port);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(mail);



                this.ShowMessage("success", "Data Save successfully,Barcode generation in progress, you will be notified via email once code uploaded.", ToastType.success);
                return RedirectToAction("ManageQr", "Customer");
            }
            catch (Exception ex)
            {
                this.ShowMessage("Error", ex.Message, ToastType.error);
                return RedirectToAction("ManageQr", "Customer");
            }

            this.ShowMessage("success", "Data Save successfully,Barcode generation in progress please check in few minutes", ToastType.success);
            return RedirectToAction("ManageQr", "Customer");
        }


        public string TextFileReadData(HttpPostedFileBase file, NewManageQrViewModel qrmodel)
        {
            var ab = 0;
            if (file.ContentLength > 0)
            {
                string _FileName = Path.GetFileName(file.FileName);
                string tempGuid = Guid.NewGuid().ToString().Substring(0, 5);
                string FileName = tempGuid + "_" + _FileName;
                string _path = Path.Combine(Server.MapPath("~/Content/UploadedFiles"), FileName);
                file.SaveAs(_path);
                DataTable dt = new DataTable();
                dt.Columns.Add("QrCode");
                dt.Columns.Add("ProductId", typeof(Guid));
                dt.Columns.Add("CategoryId", typeof(Int32));
                dt.Columns.Add("CreatedDate"); 
                dt.Columns.Add("IsActive", typeof(bool));
                dt.Columns.Add("IsExpire", typeof(bool));

                if (!string.IsNullOrEmpty(_path))
                {
                    using (StreamReader sr = new StreamReader(Path.Combine(Server.MapPath("~/Content/UploadedFiles"), FileName)))
                    {
                       
                        while (sr.Peek() >= 0)
                        {
                            var a = sr.ReadLine();
                            if (a.Length == 16)
                            {
                                var row = dt.NewRow();
                                row["QrCode"] = a;
                                row["ProductId"] = qrmodel.ProductId;
                                row["CategoryId"] = qrmodel.CategoryId;
                                row["CreatedDate"] = DateTime.Now;
                                row["IsActive"] = true;
                                row["IsExpire"] = false;
                                dt.Rows.Add(row);
                                ab++;
                            }
                            
                        }
                    }
                }
                using (var _dconn = new SqlConnection(connectionString))
                {
                    _dconn.Open();

                    //Do a bulk copy for gaining better performance

                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(_dconn))
                    {
                        bulkCopy.BulkCopyTimeout = 3600;

                        bulkCopy.ColumnMappings.Add("QrCode", "QrCode");

                        bulkCopy.ColumnMappings.Add("ProductId", "ProductId");

                        bulkCopy.ColumnMappings.Add("CategoryId", "CategoryId");
                        bulkCopy.ColumnMappings.Add("CreatedDate", "CreatedDate");
                        bulkCopy.ColumnMappings.Add("IsActive", "IsActive");
                        bulkCopy.ColumnMappings.Add("IsExpire", "IsExpire");

                        bulkCopy.DestinationTableName = "dbo.QrCodeMaster";

                        bulkCopy.WriteToServer(dt);
                        _dconn.Close();
                    }
                }

            }

            return ab.ToString() ;
        }

        public string XlsxFileReadData(HttpPostedFileBase file, NewManageQrViewModel qrmodel)
        {
            var ab = 0;
            if (file.ContentLength > 0)
            {
                string _FileName = Path.GetFileName(file.FileName);
                string tempGuid = Guid.NewGuid().ToString().Substring(0, 5);
                string FileName = tempGuid + "_" + _FileName;
                string _path = Path.Combine(Server.MapPath("~/Content/UploadedFiles"), FileName);
                file.SaveAs(_path);
                DataTable dt = new DataTable();
                dt.Columns.Add("QrCode");
                dt.Columns.Add("ProductId", typeof(Guid));
                dt.Columns.Add("CategoryId", typeof(Int32));
                dt.Columns.Add("CreatedDate");
                dt.Columns.Add("IsActive", typeof(bool));
                using (var stream = System.IO.File.Open(_path, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {

                        while (reader.Read()) //Each row of the file
                        {
                            var a = reader.GetValue(0).ToString();
                            if (a.Length == 16)
                            {
                                var row = dt.NewRow();
                                row["QrCode"] = a;
                                row["ProductId"] = qrmodel.ProductId;
                                row["CategoryId"] = qrmodel.CategoryId;
                                row["CreatedDate"] = DateTime.Now;
                                row["IsActive"] = true;
                                dt.Rows.Add(row);
                                ab++;
                            }

                        }
                    }
                }
                using (var _dconn = new SqlConnection(connectionString))
                {
                    _dconn.Open();

                    //Do a bulk copy for gaining better performance

                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(_dconn))
                    {
                        bulkCopy.BulkCopyTimeout = 3600;

                        bulkCopy.ColumnMappings.Add("QrCode", "QrCode");

                        bulkCopy.ColumnMappings.Add("ProductId", "ProductId");

                        bulkCopy.ColumnMappings.Add("CategoryId", "CategoryId");
                        bulkCopy.ColumnMappings.Add("CreatedDate", "CreatedDate");
                        bulkCopy.ColumnMappings.Add("IsActive", "IsActive");

                        bulkCopy.DestinationTableName = "dbo.QrCodeMaster";

                        bulkCopy.WriteToServer(dt);
                        _dconn.Close();
                    }
                }

            }

            return ab.ToString();
        }

        public string CsvFileReadData(HttpPostedFileBase file, NewManageQrViewModel qrmodel)
        {
            var ab = 0;
            if (file.ContentLength > 0)
            {
                string _FileName = Path.GetFileName(file.FileName);
                string tempGuid = Guid.NewGuid().ToString().Substring(0, 5);
                string FileName = tempGuid + "_" + _FileName;
                string _path = Path.Combine(Server.MapPath("~/Content/UploadedFiles"), FileName);
                file.SaveAs(_path);
                DataTable dt = new DataTable();
                dt.Columns.Add("QrCode");
                dt.Columns.Add("ProductId", typeof(Guid));
                dt.Columns.Add("CategoryId", typeof(Int32));
                dt.Columns.Add("CreatedDate");
                dt.Columns.Add("IsActive", typeof(bool));

                string csvData = System.IO.File.ReadAllText(_path);
                foreach (string row in csvData.Split('\n'))
                {
                    if (!string.IsNullOrEmpty(row))
                    {
                        var a1 = row.Split(',')[0];
                        string a = a1.Remove(a1.Length - 2);
                        if (a.Length == 16)
                        {
                            var newrow = dt.NewRow();
                            newrow["QrCode"] = a;
                            newrow["ProductId"] = qrmodel.ProductId;
                            newrow["CategoryId"] = qrmodel.CategoryId;
                            newrow["CreatedDate"] = DateTime.Now;
                            newrow["IsActive"] = true;
                            dt.Rows.Add(newrow);
                            ab++;
                        }
                    }
                }

                using (var _dconn = new SqlConnection(connectionString))
                {
                    _dconn.Open();

                    //Do a bulk copy for gaining better performance

                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(_dconn))
                    {
                        bulkCopy.BulkCopyTimeout = 3600;

                        bulkCopy.ColumnMappings.Add("QrCode", "QrCode");

                        bulkCopy.ColumnMappings.Add("ProductId", "ProductId");

                        bulkCopy.ColumnMappings.Add("CategoryId", "CategoryId");
                        bulkCopy.ColumnMappings.Add("CreatedDate", "CreatedDate");
                        bulkCopy.ColumnMappings.Add("IsActive", "IsActive");

                        bulkCopy.DestinationTableName = "dbo.QrCodeMaster";

                        bulkCopy.WriteToServer(dt);
                        _dconn.Close();
                    }
                }

            }

            return ab.ToString();
        }



    }
}