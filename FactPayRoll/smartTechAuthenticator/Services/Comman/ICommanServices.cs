using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using smartTechAuthenticator.Models;
using smartTechAuthenticator.ViewModels;

namespace smartTechAuthenticator.Services.Comman
{
    public interface ICommanServices
    {
        List<SelectListItem> GetStates();
        List<SelectListItem> GetDistrictts(int StateId);

        Task<ResponseModel> VerifyQrByCode(string Code, string CustomerId);
        Task<ResponseModel> VerifyCustomerByCode(string CustomerId, string Code);
        IQueryable<TestkitCheckList> TestkitChecks(Guid CustomerId, string QrCode);
        Task<ResponseModel> SaveScanHistory(TestkitCheckList testkit);
        Task<ResponseModel> SaveStep2(TrackingFormsViewModel tracking);
        TrackingFormsViewModel EditRecord(Guid Id);
        TrackingFormsViewModel EditRecordNew(Guid Id);
        CustomerInfo EditUserProfile(Guid Id);
        Task<ResponseModel> UpdateUserDetails(CustomerInfo info);
        Task<ResponseModel> SaveStep3(TrackingTestTypeViewModel tracking);
        List<TrackingFormsViewModel> ViewUserRecordList(Guid customerid);
        List<TrackingFormsViewModel> ViewUserRecordSearchList(Guid customerid,string ProuctName);
        TrackingForms CertificateSave(TrackingForms forms);
    }
}
