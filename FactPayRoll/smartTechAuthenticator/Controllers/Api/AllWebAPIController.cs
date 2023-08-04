using Nest;
using smartTechAuthenticator.Models;
using smartTechAuthenticator.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace smartTechAuthenticator.Controllers.Api
{
    public class AllWebAPIController : ApiController
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

        ApplicationDbContext appDB = new ApplicationDbContext();

        [HttpPost]
        [Route("api/WebAPI/SaveCompanyInfo")]
        public IHttpActionResult SaveCompanyInfo(CompanyDetailsViewModel company)
             {
            var res = new Response { Status = -1, Msg = "Invalid Data" };
            if (ModelState.IsValid)
            {
                using (var transaction = appDB.Database.BeginTransaction())
                {
                    try
                    {
                        CompanyDetails data = new CompanyDetails();
                        data.CompanyId = Guid.NewGuid();
                        data.UserId = company.UserId;
                        data.CompanyName = company.CompanyName;
                        data.Location = company.Location;
                        data.Website = company.Website;
                        data.Email = company.Email;
                        data.PhoneNumber = company.PhoneNumber;
                        data.Password = company.Password;
                        data.FormIds = "";
                        data.CreatedDate = DateTime.Now;
                        data.UpdatedDate = DateTime.Now;
                        appDB.CompanyDetails.Add(data);
                        appDB.SaveChanges();
                        transaction.Commit();
                        res.Status = 1;
                        res.Msg = "Company Details Saved Successcfully !";
                        res.UserId = data.UserId.ToString();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        res.Status = -1;
                        res.Msg = "Company Not Saved !";
                    }
                }
            }
            return Ok(res);
        }


        [HttpPost]
        [Route("api/WebAPI/GetCompanyList")]
        public IHttpActionResult GetCompanyList(CompanyDetailsViewModel res)
        {
            // var resp = new Response { Status = -1, Msg = "Invalid Data !" };
            List<CompanyDetailsViewModel> companylist = new List<CompanyDetailsViewModel>();

            try
            {
                if (res.UserId != null && res != null)
                {
                    companylist = (from cate in appDB.CompanyDetails.Where(x => x.UserId == res.UserId)
                                   select new CompanyDetailsViewModel
                                   {
                                       CompanyId = cate.CompanyId,
                                       CompanyName = cate.CompanyName,
                                       Location = cate.Location,
                                       Website = cate.Website,
                                       Email = cate.Email,
                                       PhoneNumber = cate.PhoneNumber,
                                       Password = cate.Password,
                                       UserId = cate.UserId,
                                       CreatedDate = cate.CreatedDate,
                                       UpdatedDate = cate.UpdatedDate


                                   }).ToList();

                }
            }
            catch (Exception ex)
            {

            }
            return Ok(companylist);
        }

        [HttpPost]
        [Route("api/WebAPI/UpdateCompanyInfo")]
        public IHttpActionResult UpdateCompanyInfo(CompanyDetailsViewModel company)
        {
            var res = new Response { Status = -1, Msg = "Invalid Data" };

            try
            {
                if (company.CompanyId != null)
                {
                    var data = appDB.CompanyDetails.Where(b => b.CompanyId == company.CompanyId).FirstOrDefault();
                    data.CompanyName = company.CompanyName;
                    data.Location = company.Location;
                    data.Website = company.Website;
                    data.Email = company.Email;
                    data.PhoneNumber = company.PhoneNumber;
                   // data.Password = company.Password;
                    data.UpdatedDate = DateTime.Now;
                    appDB.SaveChanges();
                    res.Status = 1;
                    res.Msg = "Company Details Updated Successcfully!";

                }
            }
            catch (Exception ex)
            {
                res.Status = -1;
                res.Msg = "Company Details Not Updated!";
            }
            return Ok(res);
        }

        [HttpPost]
        [Route("api/WebAPI/GetCompanyDetailsById")]
        public IHttpActionResult GetCompanyDetailsById(CompanyDetailsViewModel res)
        {
            CompanyDetailsViewModel companyData = new CompanyDetailsViewModel();

            try
            {
                if (res.CompanyId != null && res != null)
                {
                    companyData = (from comp in appDB.CompanyDetails.Where(x => x.CompanyId == res.CompanyId)
                                   select new CompanyDetailsViewModel
                                   {
                                       CompanyId = comp.CompanyId,
                                       UserId = comp.UserId,
                                       CompanyName = comp.CompanyName,
                                       Location = comp.Location,
                                       Website = comp.Website,
                                       Email = comp.Email,
                                       PhoneNumber = comp.PhoneNumber,
                                       //Password = comp.Password,
                                       CreatedDate = comp.CreatedDate,
                                       UpdatedDate = comp.UpdatedDate


                                   }).FirstOrDefault();

                }
            }
            catch (Exception ex)
            {

            }
            return Ok(companyData);
        }


        [HttpPost]
        [Route("api/WebAPI/SaveEmployeeDetails")]
        public IHttpActionResult SaveEmployeeDetails(CustomerInfoViewModel emp)
        {
            var res = new Response { Status = -1, Msg = "Invalid Data" };
            
                using (var transaction = appDB.Database.BeginTransaction())
                {
                    try
                    {
                        CustomerInfo data = new CustomerInfo();
                        data.Id = Guid.NewGuid();
                        data.Name = emp.Name;
                        data.Location = emp.Location;
                        data.Website = emp.Website;
                        data.MobileNo = emp.MobileNo;
                        data.Email = emp.Email;
                        data.UserPass = emp.UserPass;
                        data.UserType = (UserType)3;
                        data.DateCreated = DateTime.Now;
                        data.UpdatedDate = DateTime.Now;
                        data.Nric = "";
                        data.DeviceId = "";
                        data.Address1 = "";
                        data.Address2 = "";
                        data.Address3 = "";
                        data.CompanyId = 0;
                        data.CustomerAddressId = 0;
                        data.IsActive = true;
                        data.FName = emp.FName;
                        data.LName = emp.LName;
                        data.AddedBy = emp.AddedBy;
                        appDB.CustomerInfo.Add(data);
                        appDB.SaveChanges();
                        transaction.Commit();
                        res.Status = 1;
                        res.Msg = "Employee Details Saved Successcfully !";
                        res.UserId = data.Id.ToString();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        res.Status = -1;
                        res.Msg = "Employee Details Not Saved !";
                    }
                }
           
            return Ok(res);
        }

        [HttpPost]
        [Route("api/WebAPI/GetEmployeeList")]
        public IHttpActionResult GetEmployeeList(CustomerInfoViewModel res)
        {
            List<CustomerInfoViewModel> employeelist = new List<CustomerInfoViewModel>();

            try
            {
                if (res.AddedBy != null && res != null)
                {
                    employeelist = (from cate in appDB.CustomerInfo.Where(x => x.AddedBy == res.AddedBy && x.UserType == (UserType)3)
                                    select new CustomerInfoViewModel
                                    {
                                        Id = cate.Id,
                                        Name = cate.Name,
                                        Location = cate.Location,
                                        Website = cate.Website,
                                        Email = cate.Email,
                                        MobileNo = cate.MobileNo,
                                        UserPass = cate.UserPass,
                                        AddedBy = cate.AddedBy,
                                        DateCreated = cate.DateCreated,
                                        UpdatedDate = cate.UpdatedDate


                                    }).ToList();

                }
            }
            catch (Exception ex)
            {

            }
            return Ok(employeelist);
        }

        [HttpPost]
        [Route("api/WebAPI/UpdateEmployeeDetails")]
        public IHttpActionResult UpdateEmployeeDetails(CustomerInfoViewModel emp)
        {
            var res = new Response { Status = -1, Msg = "Invalid Data" };

            try
            {
                if (emp.Id != null)
                {
                    var data = appDB.CustomerInfo.Where(b => b.Id == emp.Id).FirstOrDefault();
                    data.Name = emp.Name;
                    data.Location = emp.Location;
                    data.Website = emp.Website;
                    data.Email = emp.Email;
                    data.MobileNo = emp.MobileNo;
                    //data.UserPass = emp.UserPass;
                    data.UpdatedDate = DateTime.Now;
                    appDB.SaveChanges();
                    res.Status = 1;
                    res.Msg = "Employee Details Updated Successcfully!";

                }
            }
            catch (Exception ex)
            {
                res.Status = -1;
                res.Msg = "Employee Details Not Updated!";
            }
            return Ok(res);
        }

        [HttpPost]
        [Route("api/WebAPI/GetEmployeeDetailsById")]
        public IHttpActionResult GetEmployeeDetailsById(CustomerInfoViewModel res)
        {
            CustomerInfoViewModel EmpData = new CustomerInfoViewModel();

            try
            {
                if (res.Id != null && res != null)
                {
                    EmpData = (from comp in appDB.CustomerInfo.Where(x => x.Id == res.Id)
                               select new CustomerInfoViewModel
                               {
                                 Id = comp.Id,
                                   AddedBy = comp.AddedBy,
                                  Name = comp.Name,
                                   Location = comp.Location,
                                   Website = comp.Website,
                                   Email = comp.Email,
                                   MobileNo = comp.MobileNo,
                                   //UserPass = comp.UserPass,
                                   DateCreated = comp.DateCreated,
                                   UpdatedDate = comp.UpdatedDate


                               }).FirstOrDefault();

                }
            }
            catch (Exception ex)
            {

            }
            return Ok(EmpData);
        }


        [HttpPost]
        [Route("api/WebAPI/SaveAccountantDetails")]
        public IHttpActionResult SaveAccountantDetails(CustomerInfoViewModel emp)
        {
            var res = new Response { Status = -1, Msg = "Invalid Data" };

            using (var transaction = appDB.Database.BeginTransaction())
            {
                try
                {
                    CustomerInfo data = new CustomerInfo();
                    data.Id = Guid.NewGuid();
                    data.Name = emp.Name;
                    data.Location = emp.Location;
                    data.Website = emp.Website;
                    data.MobileNo = emp.MobileNo;
                    data.Email = emp.Email;
                    data.UserPass = emp.UserPass;
                    data.UserType = (UserType)1;
                    data.DateCreated = DateTime.Now;
                    data.UpdatedDate = DateTime.Now;
                    data.Nric = "";
                    data.DeviceId = "";
                    data.Address1 = "";
                    data.Address2 = "";
                    data.Address3 = "";
                    data.CompanyId = 0;
                    data.CustomerAddressId = 0;
                    data.IsActive = true;
                    data.FName = emp.FName;
                    data.LName = emp.LName;
                    data.AddedBy = emp.AddedBy;
                    appDB.CustomerInfo.Add(data);
                    appDB.SaveChanges();
                    transaction.Commit();
                    res.Status = 1;
                    res.Msg = "Accountant Details Saved Successcfully !";
                    res.UserId = data.Id.ToString();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    res.Status = -1;
                    res.Msg = "Accountant Details Not Saved !";
                }
            }

            return Ok(res);
        }

        [HttpPost]
        [Route("api/WebAPI/GetAccountantList")]
        public IHttpActionResult GetAccountantList(CustomerInfoViewModel res)
        {
            List<CustomerInfoViewModel> employeelist = new List<CustomerInfoViewModel>();

            try
            {
                if (res.AddedBy != null && res != null)
                {
                    employeelist = (from cate in appDB.CustomerInfo.Where(x => x.AddedBy == res.AddedBy
                                    && x.UserType == (UserType)1)
                                    select new CustomerInfoViewModel
                                    {
                                        Id = cate.Id,
                                        Name = cate.Name,
                                        Location = cate.Location,
                                        Website = cate.Website,
                                        Email = cate.Email,
                                        MobileNo = cate.MobileNo,
                                        UserPass = cate.UserPass,
                                        AddedBy = cate.AddedBy,
                                        DateCreated = cate.DateCreated,
                                        UpdatedDate = cate.UpdatedDate


                                    }).ToList();

                }
            }
            catch (Exception ex)
            {

            }
            return Ok(employeelist);
        }

        [HttpPost]
        [Route("api/WebAPI/UpdateAccountantDetails")]
        public IHttpActionResult UpdateAccountantDetails(CustomerInfoViewModel emp)
        {
            var res = new Response { Status = -1, Msg = "Invalid Data" };

            try
            {
                if (emp.Id != null)
                {
                    var data = appDB.CustomerInfo.Where(b => b.Id == emp.Id).FirstOrDefault();
                    data.Name = emp.Name;
                    data.Location = emp.Location;
                    data.Website = emp.Website;
                    data.Email = emp.Email;
                    data.MobileNo = emp.MobileNo;
                    //data.UserPass = emp.UserPass;
                    data.UpdatedDate = DateTime.Now;
                    appDB.SaveChanges();
                    res.Status = 1;
                    res.Msg = "Accountant Details Updated Successfully!";

                }
            }
            catch (Exception ex)
            {
                res.Status = -1;
                res.Msg = "Accountant Details Not Updated!";
            }
            return Ok(res);
        }

        [HttpPost]
        [Route("api/WebAPI/GetAccountantDetailsById")]
        public IHttpActionResult GetAccountantDetailsById(CustomerInfoViewModel res)
        {
            CustomerInfoViewModel EmpData = new CustomerInfoViewModel();

            try
            {
                if (res.Id != null && res != null)
                {
                    EmpData = (from comp in appDB.CustomerInfo.Where(x => x.Id == res.Id)
                               select new CustomerInfoViewModel
                               {
                                   Id = comp.Id,
                                   AddedBy = comp.AddedBy,
                                   Name = comp.Name,
                                   Location = comp.Location,
                                   Website = comp.Website,
                                   Email = comp.Email,
                                   MobileNo = comp.MobileNo,
                                  // UserPass = comp.UserPass,
                                   DateCreated = comp.DateCreated,
                                   UpdatedDate = comp.UpdatedDate


                               }).FirstOrDefault();

                }
            }
            catch (Exception ex)
            {

            }
            return Ok(EmpData);
        }

        [HttpPost]
        [Route("api/AllWebAPI/GetFormDetails")]
        public IHttpActionResult GetFormDetails(FormAPIViewModel res)
        {
            FormAPIViewModel data = new FormAPIViewModel();
            try
            {
                if (res.Id != null)
                {
                    data.FormPropertyViewList = (from a in appDB.Form.Where(x => x.ID == res.Id)
                                                 join b in appDB.FormProperty on a.ID equals b.FormId
                                                 select new FormAPIViewModel
                                                 {
                                                     //FormName = a.Name,
                                                     Name = b.Name,
                                                     RowNumber = Convert.ToInt32(b.RowNumber),
                                                     InputType = b.FieldType,
                                                     //Captcha = b.Captcha,
                                                     //Id = b.Id,
                                                     CheckBoxViewList = (from c in appDB.CheckBox.Where(x => x.FormPropertyId == b.Id)
                                                                         select new CheckBoxViewModel
                                                                         {
                                                                             CheckboxText = c.Name,
                                                                             RowNumber = c.RowNumber,
                                                                            // Id = c.Id
                                                                         }).OrderBy(x => x.RowNumber).ToList(),
                                                     MultipleChoiceViewList = (from d in appDB.MultipleChoice.Where(x => x.FormPropertyId == b.Id)
                                                                               select new MultipleChoiceViewModel
                                                                               {
                                                                                   ChoiceText = d.Name,
                                                                                   RowNumber = d.RowNumber,
                                                                                   //Id = d.Id
                                                                               }).OrderBy(x => x.RowNumber).ToList(),
                                                     DropdownViewList = (from e in appDB.Dropdown.Where(x => x.FormPropertyId == b.Id)
                                                                         select new DropdownViewModel
                                                                         {
                                                                             OptionText = e.Name,
                                                                             RowNumber = e.RowNumber,
                                                                             //Id = e.Id
                                                                         }).OrderBy(x => x.RowNumber).ToList(),
                                                 }).OrderBy(x => x.RowNumber).ToList();
                    data.FormHeader = (from f in appDB.Form.Where(x => x.ID == res.Id)
                                     select new FormView
                                     {
                                         FormId = f.ID,
                                         FormName = f.Name
                                     }).FirstOrDefault();
                    data.ResponseText = "Success";
                    data.Id = res.Id;
                }
               
            }
            catch (Exception)
            {
                data.ResponseText = "Failure";

                throw;
            }
            return Ok(data);
        }

        [HttpPost]
        [Route("api/WebAPI/GetFormList")]
        public IHttpActionResult GetFormList(FormViewModel res)
        {
            List<FormViewModel> Formlist = new List<FormViewModel>();

            try
            {
                if (res != null)
                {
                    Formlist = (from data in appDB.Form
                                    select new FormViewModel
                                    {
                                        ID = data.ID,
                                        Name = data.Name
                                    }).ToList();

                }
            }
            catch (Exception ex)
            {

            }
            return Ok(Formlist);
        }

        [HttpPost]
        [Route("api/WebAPI/GetUserDetailsById")]
        public IHttpActionResult GetUserDetailsById(CustomerInfoViewModel res)
        {
            CustomerInfoViewModel userData = new CustomerInfoViewModel();

            try
            {
                if (res.Id != null && res != null)
                {
                    userData = (from comp in appDB.CustomerInfo.Where(x => x.Id == res.Id)
                                   select new CustomerInfoViewModel
                                   {
                                       Id = comp.Id,
                                       Name = comp.Name,
                                       FName=comp.FName,
                                       LName=comp.LName,
                                       Location = comp.Location,
                                       Website = comp.Website,
                                       Email = comp.Email,
                                       MobileNo = comp.MobileNo,
                                       //UserPass = comp.UserPass,
                                       AddedBy=comp.AddedBy,
                                       Photo=comp.Photo,
                                       DateCreated = comp.DateCreated,
                                       UpdatedDate = comp.UpdatedDate
                                   }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
            }
            return Ok(userData);
        }

        //[HttpPost]
        //[Route("api/WebAPI/UpdateUserDetails")]
        //public IHttpActionResult UpdateUserDetails(CustomerInfoViewModel emp)
        //{
        //    var res = new Response { Status = -1, Msg = "Invalid Data" };

        //    try
        //    {
        //        if (emp.Id != null)
        //        {
        //            var data = appDB.CustomerInfo.Where(b => b.Id == emp.Id).FirstOrDefault();
        //           //data.Name = emp.Name;
        //            data.FName=emp.FName;
        //            data.LName = emp.LName;
        //            data.Location = emp.Location;
        //            data.Website = emp.Website;
        //            data.Email = emp.Email;
        //            data.MobileNo = emp.MobileNo;
        //            data.UserPass = emp.UserPass;
        //            data.Photo = emp.Photo;
        //            data.UpdatedDate = DateTime.Now;
        //            appDB.SaveChanges();
        //            res.Status = 1;
        //            res.Msg = "User Details Updated Successcfully!";

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        res.Status = -1;
        //        res.Msg = "User Details Not Updated!";
        //    }
        //    return Ok(res);
        //}

        [HttpPost]
        [Route("api/WebAPI/GetTotalNo")]
        public IHttpActionResult GetTotalNo(CustomerInfoViewModel res)
        {
            
             CustomerInfoViewModel data = new CustomerInfoViewModel();
            try
            {
                if (res.AddedBy != null && res != null)
                {
                    var Totalemployee =  appDB.CustomerInfo.Where(x => x.AddedBy == res.AddedBy
                                    && x.UserType == (UserType)3).Count();
                    var Totalaccountant = appDB.CustomerInfo.Where(x => x.AddedBy == res.AddedBy
                                  && x.UserType == (UserType)1).Count();

                   
                    data.TotalAccountant = Totalaccountant;
                    data.TotalEmployees = Totalemployee;

                }
            }
            catch (Exception ex)
            {

            }
            return Ok(data);
        }



        [HttpPost]
        [Route("api/WebAPI/UpdateUserDetails")]
        public IHttpActionResult UpdateUserDetails(CustomerInfoViewModel emp)
        {
            var res = new Response { Status = -1, Msg = "Invalid Data" };

            try
            {
                if (emp.Id != null)
                {
                    var data = appDB.CustomerInfo.Where(b => b.Id == emp.Id).FirstOrDefault();
                    data.Name = emp.Name;
                    data.FName = emp.FName;
                    data.LName = emp.LName;
                    data.Location = emp.Location;
                    data.Website = emp.Website;
                    data.Email = emp.Email;
                    data.MobileNo = emp.MobileNo;
                    //data.UserPass = emp.UserPass;
                    data.Photo = emp.Photo;
                    data.UpdatedDate = DateTime.Now;
                    appDB.SaveChanges();
                    res.Status = 1;
                    res.Msg = "User Details Updated Successfully!";

                }
            }
            catch (Exception ex)
            {
                res.Status = -1;
                res.Msg = "User Details Not Updated!";
            }
            return Ok(res);
        }



        [HttpPost]
        [Route("api/WebAPI/GetSubmittedFormList")]
        public IHttpActionResult GetSubmittedFormList(HtmlResponseData res)
        {
            List<HtmlResponseData> formlist = new List<HtmlResponseData>();

            try
            {
                if (res.UserId != null && res != null)
                {
                    formlist = (from cate in appDB.HtmlResponseData.Where(x => x.UserId == res.UserId )
                                    select new HtmlResponseData
                                    {
                                        FormId = cate.FormId,
                                        UserId=cate.UserId
                                       
                                    }).ToList();

                }
            }
            catch (Exception ex)
            {

            }
            return Ok(formlist);
        }

    }
}