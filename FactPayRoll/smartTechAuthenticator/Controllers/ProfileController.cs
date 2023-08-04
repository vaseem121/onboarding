using smartTechAuthenticator.Services.Comman;
using smartTechAuthenticator.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using smartTechAuthenticator.Models;
using System.Threading.Tasks;
using smartTechAuthenticator.Services.Comman.CustomFilters;

namespace smartTechAuthenticator.Controllers
{
    [Authorized]
    public class ProfileController : Controller
    {
        private readonly ICommanServices _comman;
        public ProfileController(ICommanServices comman)
        {
            _comman = comman;
        }
        [HttpGet]
        public ActionResult UserProfile()
        {
            CustomerInfo model = new CustomerInfo();
            Guid Id = new Guid(Session["UserId"].ToString());
            model = _comman.EditUserProfile(Id);
            return View(model);
        }
        [HttpPost]
        public async Task<ActionResult> UserProfile(CustomerInfo info)
        {
            CustomerInfo model = new CustomerInfo();

            ResponseModel response = await _comman.UpdateUserDetails(info);
            if (response.Status == Status.Success)
            {
                this.ShowMessage("success", "Details updated successfully", ToastType.success);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                this.ShowMessage("failiure ", "Details not upated successfully ", ToastType.error);
            }
            return View(model);

        }

    }
}