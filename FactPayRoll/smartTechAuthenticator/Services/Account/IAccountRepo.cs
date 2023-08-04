using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using smartTechAuthenticator.ViewModels;
using smartTechAuthenticator.Models;

namespace smartTechAuthenticator.Services.Account
{
    public interface IAccountRepo
    {
        Task<ResponseModel> Register(CustomerInfoViewModel customer);
        Task<ResponseModel> Update(CustomerInfo customer); 
        Task<ResponseModel> ForgetPassword(SignInViewModel customerInfo); 
        Task<ResponseModel> ConfirmEmail(SignInViewModel customerInfo);
        Task<ResponseModel> SignIn(SignInViewModel customerInfo);
        Task<CustomerInfo> GetProfileDetails(Guid UserId);

        Task<ResponseModel> UpdateProfileDetails(CustomerInfo model);
        Task<ResponseModel> ChangePassword(PasswordChangeModel customerInfo);


    }
}
