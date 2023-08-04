using System.Collections.Generic;
using System.Web.Mvc;
using smartTechAuthenticator.Services.Account;
using smartTechAuthenticator.ViewModels;
using smartTechAuthenticator.Services.Comman;
using System.Threading.Tasks;
using System;
using smartTechAuthenticator.Models;
using System.Net.Mail;
using System.Linq;
using System.IO;
using System.Web;
using System.Net;
namespace smartTechAuthenticator.Controllers
{
    public class AccountController : Controller
    {
        static string userName = System.Configuration.ConfigurationManager.AppSettings["userName"].ToString();
        static string password = System.Configuration.ConfigurationManager.AppSettings["password"].ToString();
        static string mailFrom = System.Configuration.ConfigurationManager.AppSettings["mailFrom"].ToString();
        static string smtpServer = System.Configuration.ConfigurationManager.AppSettings["smtpServer"].ToString();
        static string port = System.Configuration.ConfigurationManager.AppSettings["port"].ToString();
        private readonly IAccountRepo account;
        private readonly ICommanServices commanServices;
        private readonly ApplicationDbContext context;

        public AccountController(IAccountRepo _account, ICommanServices _commanServices, ApplicationDbContext _context)
        {
            account = _account;
            commanServices = _commanServices;
            context = _context;
        }
        public ActionResult SignUp()
        {
            IEnumerable<SelectListItem> States = commanServices.GetStates();
            CustomerInfoViewModel model = new CustomerInfoViewModel();
            ViewData["States"] = States;
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SignUp(HttpPostedFileBase file, CustomerInfoViewModel customer)
        {

            ResponseModel model = new ResponseModel();
            IEnumerable<SelectListItem> States = commanServices.GetStates();
            ViewData["States"] = States;
            if (ModelState.IsValid)
            {
                var user = context.CustomerInfo.FirstOrDefault(u => u.Email.Equals(customer.Email));
                if (user != null)
                {
                    this.ShowMessage("error", "User email already exists. Please enter a     different email.", ToastType.error);
                    return View(); 
                }
                else
                {
                    if (file != null)
                    {
                        if (file.ContentLength > 0)
                        {
                            string Extension = Path.GetExtension(file.FileName).ToLower();
                            if (!(Extension == ".jpg" || Extension == ".png" || Extension == ".jpeg"))
                            {
                                ViewBag.error = "File should be .jpg .png or .jpeg only";
                                return View(customer);
                            }
                            string _FileName = Path.GetFileName(file.FileName);
                            string tempGuid = Guid.NewGuid().ToString().Substring(0, 5);
                            string FileName = tempGuid + "_" + _FileName;
                            string _path = Path.Combine(Server.MapPath("~/Content/Profile"), FileName);
                            file.SaveAs(_path);
                            customer.Photo = FileName;
                        }
                    }

                    model = await account.Register(customer);
                }
            }

            if (model.Status == Status.Success)
            {
                ModelState.Clear();
                this.ShowMessage("success", "You are successfully registered,", ToastType.success);
                return RedirectToAction("SignIn");
            }
            else
            {
                this.ShowMessage("error", "Registration not successfully", ToastType.error);
            }
            return View();
        }

        [HttpGet]
        public JsonResult GetDistrictts(string StateId)
        {
            return Json(commanServices.GetDistrictts(Convert.ToInt32(StateId)), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Obsolete]
        public ActionResult SignIn()
        {
            //string hostName = Dns.GetHostName();
            //string SignedIP = Dns.GetHostByName(hostName).AddressList[0].ToString();
            //TempData["SignedIP"] = SignedIP;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Obsolete]
        public async Task<ActionResult> SignIn(SignInViewModel customer, string returnUrl)
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

        [HttpGet]
        public ActionResult Logout()
        {
            Session.Abandon();
            Session.Clear();
            return RedirectToAction("SignIn", "Account");
        }

        public ActionResult AccessDenide()
        {
            return View();
        }

        public async Task<ActionResult> EditProfile()
        {
            Guid UserId = new Guid(Session["UserId"].ToString());
            CustomerInfo model = new CustomerInfo();
            model = await account.GetProfileDetails(UserId);
            return View(model);
        }


        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> ChangePassword(PasswordChangeModel customer)
        {
            if (ModelState.IsValid)
            {
                ResponseModel model = new ResponseModel();
                var UserId = Session["UserId"].ToString();
                customer.Id = UserId;
                var Login = await account.ChangePassword(customer);
                if (Login.Status == Status.Success)
                {
                    this.ShowMessage("success", "Password has been changed successfully !!!", ToastType.success);
                    return RedirectToAction("Index", "Customer");
                }
                else
                {
                    this.ShowMessage("Error ", "Please check your current password !!", ToastType.error);
                }
            }

            return View();
        }
        [HttpPost]
        public async Task<ActionResult> EditProfile(CustomerInfo model)
        {
            ResponseModel obj = new ResponseModel();
            try
            {
                obj = await account.UpdateProfileDetails(model);
                if (obj.Status == Status.Success)
                {
                    this.ShowMessage("success", "Details updated successfully", ToastType.success);
                    return RedirectToAction("Index", "Customer");
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


        public ActionResult ForgotPassword(string email = null)
        {
            ForgotPasswordViewModel model = new ForgotPasswordViewModel();
            model.Email = email;
            //TempData["Otp"] = "fd";
            return View(model);
        }

        [HttpPost]
        public ActionResult ForgotPassword(ForgotPasswordViewModel model)
        {
            int a = 0;
            if (ModelState.IsValid)
            {
                if (model.First != null && model.Second != null && model.Third != null && model.Fourth != null && model.Fifth != null && model.Sixth != null)
                {
                    var OTP = model.First + model.Second + model.Third + model.Fourth + model.Fifth + model.Sixth;

                    var user = context.CustomerInfo.Where(m => m.Email == model.Email).FirstOrDefault();
                    if (DateTime.Now < user.FPExpTime)
                    {
                        if (OTP == user.ForgotPasswordOTP)
                        {
                            TempData["CheckOtp"] = "success";
                            return RedirectToAction("CreatePassword", new { email = model.Email });
                        }
                        else
                        {
                            a = Convert.ToInt32(TempData["OTPerr"]);
                            a++;
                            TempData["OTPerr"] = a.ToString();
                            TempData["Otp"] = "success";
                            if (a >= 5)
                            {
                                TempData["OTPerr"] = null;
                                TempData["Otp"] = null;
                                TempData["ChangePasswordErrerMessage"] = null;
                            }
                            else
                            {
                                this.ShowMessage("Error", "An error occured, Invalid OTP !", ToastType.success);
                            }
                            return RedirectToAction("ForgotPassword", new { email = model.Email });
                        }


                    }
                    else
                    {
                        this.ShowMessage("Error", "An error occured, OTP has expired !", ToastType.success);
                        return RedirectToAction("ForgotPassword");
                    }

                }
                else
                {
                    var user = context.CustomerInfo.Where(m => m.Email == model.Email).FirstOrDefault();
                    if (user != null)
                    {
                        Random rnd = new Random();
                        string Code = "";
                        for (int i = 0; i < 6; i++)
                        {
                            Code += rnd.Next(0, 9);
                        }
                        var msg = SendVerificationEmail(user.Email, user.Name, Code);
                        //var msg = "success";
                        if (msg == "success")
                        {
                            var res = context.CustomerInfo.Where(x => x.Id == user.Id).SingleOrDefault();
                            if (res != null)
                            {
                                DateTime now = DateTime.Now;
                                res.ForgotPasswordOTP = Code;
                                res.FPCreateTime = now;
                                res.FPExpTime = now.AddMinutes(5);//Min
                                context.SaveChanges();
                            }

                            TempData["Otp"] = "success";
                            return RedirectToAction("ForgotPassword", new { email = model.Email });
                        }
                        else
                        {
                            this.ShowMessage("Error", msg, ToastType.success);
                            return View();
                        }
                    }
                    else
                    {
                        this.ShowMessage("Error", "An error occured, Invalid Email !", ToastType.success);
                        return View();
                    }
                }
            }
            return View();
        }

        public ActionResult CreatePassword(string email = null)
        {
            PasswordCreateModel model = new PasswordCreateModel();
            string tempdata = null;
            if (TempData["CheckOtp"] != null)
            {
                tempdata = TempData["CheckOtp"].ToString();
            }

            if (tempdata != "success")
            {
                TempData["ChangePasswordErrerMessage"] = " Please verify !";
                return RedirectToAction("ForgotPassword");
            }
            model.Email = email;
            return View(model);
        }
        [HttpPost]
        public ActionResult CreatePassword(PasswordCreateModel model)
        {
            if (ModelState.IsValid)
            {
                var res = context.CustomerInfo.Where(x => x.Email == model.Email).SingleOrDefault();
                if (res != null)
                {
                    DateTime now = DateTime.Now;
                    res.UserPass = model.Password;
                    context.SaveChanges();
                    return RedirectToAction("SignIn", "Account");
                }
            }
            return View(model);
        }


        private string SendVerificationEmail(string emailId, string FirstName, string otp)
        {
            string CurrentYear = DateTime.Now.Year.ToString();
            string body = "<br/><br/>Hi " + FirstName + "" +
           "<br/><br/>Use the following OTP to complete your forgot password procedures. OTP is valid for 5 minutes.<br/> <b>" + otp + "</b>" +
           " <br/>" +
           "<br/>Thanks," +
           "<br/> SmartTech Ltd." +
           "<br/><p>All Rights Reserved by Smart Tech-admin. Designed and Developed by Smart Tech.</p>";
            string subject = "Confirmation OTP";
            MailMessage mail = new MailMessage();
            mail.To.Add(emailId);
            // mail.To.Add("bansh@nextolive.com");
            mail.From = new MailAddress(mailFrom);
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential(userName, password);
            // Enter seders User name and password  
            smtp.EnableSsl = true;
            smtp.Host = smtpServer;
            smtp.Port = Convert.ToInt32(port);
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.Timeout = 20000;

            try
            {
                smtp.Send(mail);
                return "success";
            }
            catch (Exception se)
            {
                string message = se.Message;
                Exception seie = se.InnerException;
                if (seie != null)
                {
                    message += ", " + seie.Message;
                    Exception seieie = seie.InnerException;
                    if (seieie != null)
                    {
                        message += ", " + seieie.Message;
                    }
                }
                return message;
            }

        }


    }
}