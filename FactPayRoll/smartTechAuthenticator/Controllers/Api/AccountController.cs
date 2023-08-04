using Microsoft.Win32;
using Nest;
using smartTechAuthenticator.Models;
using smartTechAuthenticator.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web.Http;

namespace smartTechAuthenticator.Controllers.Api
{
    public class AccountController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
        [HttpGet]
        [Route("api/Account/TestApi")]
        public IHttpActionResult TestApi()
        {
          
            return Ok("Ok");

        }

        ApplicationDbContext appDB = new ApplicationDbContext();

        [HttpPost]
        [Route("api/Account/Register")]
        public IHttpActionResult Register(CustomerInfoViewModel reg)
{
            var res = new Response { Status = -1, Msg = "", UserId = "" };
            try
            {
                if (ModelState.IsValid)
                {
                    var user = appDB.CustomerInfo.Any(e => e.Email == reg.Email);
                    if (user == true)
                    {
                        res.Msg = "Email Already Exists !";
                        res.Status = -1;
                        return Ok(res);
}
                    CustomerInfo data = new CustomerInfo();
                    data.Id = Guid.NewGuid();
                    data.FName = reg.FName;
                    data.LName = reg.LName;
                    data.MobileNo = reg.MobileNo;
                    data.Email = reg.Email;
                    data.UserPass = reg.UserPass;
                    data.Address1 = "";
                    data.Address2 = "";
                    data.Address3 = "";
                    data.DeviceId = "";
                    data.Name = reg.FName;
                    data.Nric = "";
                    data.UserType = (UserType)reg.UserType;
                    data.DateCreated = DateTime.Now;
                    data.UpdatedDate = DateTime.Now;
                    appDB.CustomerInfo.Add(data);
                    appDB.SaveChanges();
                    res.Status = 1;
                    res.Msg = "You are Registered Successfully.";
                    res.UserId = data.Id.ToString();
                }
                else
                {
                    res.Msg = "failed !";
                }
            }
            catch (Exception ex)
            {
                res.Msg = "failed !";
                return Ok(res);
            }
            return Ok(res);
        }

        [HttpPost]
        [Route("api/Account/Login")]
        public IHttpActionResult Login(Login log)
        {
            var res = new Response { Status = -1, Msg = "", UserId = "" };
            try
            {
                if (log != null)
                {
                    if (!Regex.IsMatch(log.Email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase))
                    {
                        res.Msg = "Invalid Email !";
                    }
                    var checkmail = appDB.CustomerInfo.Where(a => a.Email == log.Email).FirstOrDefault();
                    if (checkmail != null)
                    {
                        var data = appDB.CustomerInfo.Where(a => a.Email == log.Email && a.UserPass == log.Password).FirstOrDefault();
                        if (data != null)
                        {
                            res.UserId = data.Id.ToString();
                            res.Email = data.Email;
                            res.Roles = (int)data.UserType;
                            res.Msg = "success";
                            res.Status = 1;
                        }
                        else
                        {
                            res.Msg = "Password is incorrect";
                            res.Status = -1;
                        }
                    }
                    else
                    {
                        res.Msg = "Email is incorrect";
                        res.Status = -1;
                    }
                }
            }
            catch (Exception Ex)
            {
                res.Msg = Ex.Message;
                res.Status = -1;
            }
            return Ok(res);
        }

        [HttpPost]
        [Route("api/Account/ForgotPassword")]
        public IHttpActionResult ForgotPassword(ForgotPassword obj)
        {
            var res = new Response { Status = -1, Msg = "" };
            try
            {
                if (obj != null)
                {
                    var data = appDB.CustomerInfo.Where(a => a.Email == obj.Email).FirstOrDefault();
                    if (data != null)
                    {
                        Random rnd = new Random();
                        string Code = "";
                        for (int i = 0; i < 4; i++)
                        {
                            Code += rnd.Next(0, 9);
                        }
                        var code = Code;

                        MailMessage mail = new MailMessage();
                        mail.To.Add(obj.Email);
                        mail.From = new MailAddress("info.securranty@gmail.com");
                        mail.Subject = "Uploaded codes";
                        string CurrentYear = DateTime.Now.Year.ToString();
                        string Body =
                            "<br/>Reset your password use this following code -" + code +
                            "<br/><br/>Regards" +
                            "<br/> Fact PayRoll.";
                        string subject = "Reset Password";
                        mail.Body = Body;
                        mail.Subject = subject;
                        mail.IsBodyHtml = true;
                        SmtpClient smtp = new SmtpClient();
                        smtp.EnableSsl = true;
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new System.Net.NetworkCredential("info.securranty@gmail.com", "fdcvsoepctucemfh"); // Enter seders User name and password  
                        smtp.Host = "smtp.gmail.com";
                        smtp.Port = 587;
                        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                        smtp.Send(mail);


                        res.UserId = data.Id.ToString();
                        res.Email = data.Email;
                        res.UserName = data.FName;
                        res.OTP = code;
                        res.Msg = "Please check your email to reset your password.";
                        res.Status = 1;

                        if (res.Status == 1)
                        {
                            var data1 = appDB.CustomerInfo.Where(a => a.Id == new Guid(res.UserId)).FirstOrDefault();
                            if (data1 != null)
                            {
                                var newadata = appDB.CustomerInfo.Find(data1.Id);
                                newadata.ForgotPasswordOTP = code;
                                DateTime now = DateTime.Now;
                                newadata.FPExpTime = now.AddMinutes(5);
                                appDB.SaveChanges();

                                res.Msg = "OTP send! Please check your email to reset your password.";
                                res.Status = 1;
                            }
                            else
                            {
                                res.Msg = "Invalid!";
                                res.Status = -1;
                            }

                        }
                        else
                        {
                            res.Msg = "Invalid Email!";
                            res.Status = -1;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                res.Status = -1;
                res.Msg = ex.Message;
            }
            return Ok(res);
        }


        [HttpPost]
        [Route("api/Account/ForgotPasswordConfirmation")]
        public IHttpActionResult ForgotPasswordConfirmation(ForgotPassword obj)
        {
            var res = new Response { Status = -1, Msg = "" };
            try
            {
                if (obj != null)
                {
                    var data = appDB.CustomerInfo.Where(a => a.Id == new Guid(obj.id)).FirstOrDefault();
                    if (data != null)
                    {
                        if (DateTime.Now < data.FPExpTime)
                        {
                            if (obj.OTP == data.ForgotPasswordOTP)
                            {
                                var newadata = appDB.CustomerInfo.Find(data.Id);
                                newadata.UserPass = obj.Password;
                                appDB.SaveChanges();

                                res.UserId = data.Id.ToString();
                                res.Email = data.Email;
                                res.UserName = data.FName;
                                res.Msg = "Reset password Successfully !";
                                res.Status = 1;


                            }
                            else
                            {
                                res.Msg = "OTP not matched!";
                                res.Status = -1;
                            }
                        }
                        else
                        {
                            res.Msg = "OTP Expired. Please send again!";
                            res.Status = -1;
                        }
                    }
                    else
                    {
                        res.Msg = "Invalid!";
                        res.Status = -1;
                    }
                }
            }
            catch (Exception ex)
            {
                res.Status = -1;
                res.Msg = ex.Message;
            }
            return Ok(res);
        }



    }
}