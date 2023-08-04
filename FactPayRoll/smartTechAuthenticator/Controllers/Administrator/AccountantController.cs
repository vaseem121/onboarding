using smartTechAuthenticator.Services.Comman;
using smartTechAuthenticator.Services.Customers;
using smartTechAuthenticator.ViewModels;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace smartTechAuthenticator.Controllers.Administrator
{
    public class AccountantController : Controller
    {
        private readonly ICustomersService customers;
        private readonly ICommanServices commanServices;
       
        public AccountantController(ICustomersService _customers, ICommanServices _commanServices)
        {
            customers = _customers;
            commanServices = _commanServices;
        }

        // GET: Accountant
        public ActionResult Index()
        {
            if (Session["Role"].ToString() != "Admin" && Session["Role"].ToString() != "SubAdmin")
            {
                return RedirectToAction("AccessDenide", "Account");
            }
            return View();
        }

        [HttpPost]
        public ActionResult GetAccountants()
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
                (int TotalCount, int FilteredCount, dynamic Customers) data = customers.GetAccountants(skip, take, sortColumn, sortColumnDir, searchValue);
                return Json(new { draw = draw, recordsFiltered = data.TotalCount, recordsTotal = data.TotalCount, data = data.Customers });
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ActionResult CreateAccountant()
        {
            if (Session["Role"].ToString() != "Admin" && Session["Role"].ToString() != "SubAdmin")
            {
                return RedirectToAction("AccessDenide", "Account");
            }
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> CreateAccountant(CustomerInfoViewModel model)
        {

            ResponseModel response = await customers.CreateAccountant(model);
            if (response.Status == Status.Success)
            {
                this.ShowMessage("success", "Accountant create successfully", ToastType.success);
                return RedirectToAction("Index");
            }
            else
            {
                this.ShowMessage("failure ", "Accountantnot create successfully ", ToastType.success);
            }

            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> AccountantDetails(string Id)
        {
            if (Session["Role"].ToString() != "Admin" && Session["Role"].ToString() != "SubAdmin")
            {
                return RedirectToAction("AccessDenide", "Account");
            }
            var CategoryInfofo = await customers.GetAccountantDetail(Id);
            return View(CategoryInfofo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AccountantDetails(CustomerInfoViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ResponseModel response = await customers.UpdateAccountantData(model);
                    if (response.Status == Status.Success)
                    {
                        this.ShowMessage("success", "Details updated successfully", ToastType.success);
                        return RedirectToAction("Index");
                    }
                    else
                        this.ShowMessage("failiure ", "Details not upated successfully ", ToastType.error);
                }
            }
            catch (Exception ex)
            {
                //logger.Error(ex);
                this.ShowMessage("Error", "An error occured, please after some time,", ToastType.success);
            }
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> DeleteAccountant(CustomerInfoViewModel model)
        {
            return Json(await customers.DeleteAccountantDetails(model), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]

        public async Task<ActionResult> ViewAccountant(string Id)
        {
            CustomerInfoViewModel model = new CustomerInfoViewModel();
            model = await customers.GetAccountant(Guid.Parse(Id));
            return View(model);

        }

    }
}