using smartTechAuthenticator.Models;
using smartTechAuthenticator.Services.Comman;
using smartTechAuthenticator.Services.Comman.CustomFilters;
using smartTechAuthenticator.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace smartTechAuthenticator.Controllers
{
    public class AuthController : Controller
    {
        private readonly ICommanServices _comman;
        public AuthController(ICommanServices comman)
        {
            _comman = comman;
        }
        // GET: Auth
        [Authorized]
        public async Task<ActionResult> Index(string authcode)
        {
            var UserId = Session["UserId"].ToString();
            var Veryfy = await _comman.VerifyCustomerByCode(UserId, authcode);
            if (Veryfy.Status == Status.Failure)
            {
                this.ShowMessage("failiure ", Veryfy.Message, ToastType.success);
                TempData["result"] = Veryfy;
                ModelState.Clear();
                TempData["Message"] = "Your Authcode not Matched, Please Try Again.";
                return RedirectToAction("Index", "Home");
            }
            var response = await _comman.VerifyQrByCode(authcode, UserId);
            TempData["result"] = response;
            ModelState.Clear();
            if (response.Status == Status.Success)
            {
                TestkitCheckList checkList = new TestkitCheckList()
                {
                    CustId = Guid.Parse(Session["UserId"].ToString()),
                    Qrcode = authcode,
                    Status = 0,
                    CreatedTs = DateTime.UtcNow
                };
                ResponseModel model1 = await _comman.SaveScanHistory(checkList);
                TempData["productInfo"] = response.Data;
                return RedirectToAction("Step1", "Home");
            }
            return RedirectToAction("Index", "Home");
        }
    }
}