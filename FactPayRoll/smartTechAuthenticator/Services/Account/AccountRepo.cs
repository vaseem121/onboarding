using smartTechAuthenticator.Models;
using smartTechAuthenticator.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.EntityFrameworkCore;
using NLog;
using System.Net;

namespace smartTechAuthenticator.Services.Account
{
    public class AccountRepo : IAccountRepo
    {
        private readonly ApplicationDbContext context;
        public readonly Logger Logger;
        public AccountRepo(ApplicationDbContext _applicationDbContext)
        {
            context = _applicationDbContext;
            Logger = LogManager.GetCurrentClassLogger();
        }

        public Task<ResponseModel> ConfirmEmail(SignInViewModel customerInfo)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseModel> ForgetPassword(SignInViewModel customerInfo)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseModel> Register(CustomerInfoViewModel customer)
        {
            ResponseModel model = new ResponseModel();
            using (var transection =await context.Database.BeginTransactionAsync())
            {
                try
                {
                    CustomerAddresses customerAddress = new CustomerAddresses()
                    {
                        StateId = customer.StateId,
                        DistricttId = customer.DistricttId,
                        PostCode = customer.PostCode,
                        Address = customer.Address
                    }; 
                    context.CustomerAddressMasters.Add(customerAddress);
                    context.SaveChanges();
                    CustomerInfo customerInfo = new CustomerInfo()
                    {
                        Name = customer.Name,
                        Email = customer.Email,
                        UserPass = customer.UserPass,
                        MobileNo = customer.MobileNo,
                        DateCreated = DateTime.UtcNow,
                        CustomerAddressId = customerAddress.Id,
                        Nric = "",
                        Address1 = "",
                        Address2 = "",
                        Address3 = "",
                        DeviceId = Guid.NewGuid().ToString(),
                        UserType=UserType.Customer,
                        IsActive=true,
                        Photo = customer.Photo
                    };
                    customerInfo.DateCreated = DateTime.UtcNow; 
                    await context.CustomerInfo.AddAsync(customerInfo);
                    await context.SaveChangesAsync();
                    await transection.CommitAsync();
                    model.Status = Status.Success;
                }
                catch (Exception ex)
                {
                    await transection.RollbackAsync();
                    model.Status = Status.Failure;
                    model.Message = ex.Message;
                    Logger.Error(ex);
                }
            }
            return model;
        }
        [Obsolete]
        public async Task<ResponseModel> SignIn(SignInViewModel customerInfo)
        {
            ResponseModel model = new ResponseModel();
            LoginActivity Ipdata = new LoginActivity();
           // string hostName = Dns.GetHostName();
           // string SignedIP = Dns.GetHostByName(hostName).AddressList[0].ToString();
           try
            {
                var custInfo =await context.CustomerInfo.FirstOrDefaultAsync(x => x.Email == customerInfo.UserName && x.UserPass == customerInfo.Password);
                //LoginActivity customerLoginActivity = new LoginActivity();
                //customerLoginActivity.Id = new Guid();
                //customerLoginActivity.UserName = custInfo.Name;
                //customerLoginActivity.IP_Address = SignedIP;
                //customerLoginActivity.EventDateTime = DateTime.Now;
                //customerLoginActivity.CustomerUserId = custInfo.Id;
                //   context.LoginActivity.Add(customerLoginActivity);
                //   context.SaveChanges();
                
                if (custInfo != null)
                {
                    model.Status = Status.Success;
                    model.Data = custInfo;
                }
                else
                    model.Status = Status.Failure;
            }
            catch (Exception ex)
            {
                model.Status = Status.Failure;
                model.Message = ex.Message;
                Logger.Error(ex);
            }
            return model;
        }

        public async Task<ResponseModel> Update(CustomerInfo customer)
        {
            ResponseModel model = new ResponseModel();
            try
            {
                await context.AddAsync(customer);
                context.Entry(customer).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return new ResponseModel() { Status = Status.Success };
            }
            catch (Exception ex)
            {
                model.Status = Status.Success;
                model.Message = ex.Message;
                Logger.Error(ex);
            }
            return model;
        }

        public async Task<CustomerInfo> GetProfileDetails(Guid UserId)
        {
            try
            {
                 CustomerInfo model = new CustomerInfo();
                // model = context.CustomerInfo.Where(m => m.Id == UserId).FirstOrDefault();
                  model = await context.CustomerInfo.Where(m => m.Id == UserId).FirstOrDefaultAsync();
                return model;
                //  return model;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<ResponseModel> UpdateProfileDetails(CustomerInfo model)
        {
            ResponseModel obj = new ResponseModel();
            try
            {
                var data = context.CustomerInfo.Find(model.Id);
                data.Name = model.Name;
                data.MobileNo = model.MobileNo;
                data.Address1 = model.Address1;
                data.Address2 = model.Address2;
                data.Address3 = model.Address3;
                context.Update(data);
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


        public async Task<ResponseModel> ChangePassword(PasswordChangeModel customer)
        {
            ResponseModel model = new ResponseModel();
            var login = context.CustomerInfo.Where(u => u.Id == new Guid(customer.Id) && u.UserPass == customer.Password).FirstOrDefault();
            if (login != null)
            {
                if (customer.Confirmpwd == customer.newPassword)
                {
                    login.UserPass = customer.Confirmpwd;
                    login.UserPass = customer.newPassword;             
                    await context.SaveChangesAsync();
                    return new ResponseModel() { Status = Status.Success };
                }
            }else
            {
                model.Status = Status.Failure;
            }
            return model;
        }

    }
}