using NLog;
using smartTechAuthenticator.Services.Comman;
using smartTechAuthenticator.Services.Comman.CustomFilters;
using smartTechAuthenticator.Services.Customers;
using smartTechAuthenticator.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace smartTechAuthenticator.Controllers.Administrator
{
    [Authorized]
    public class CategoryController : Controller
    {
        private readonly ICustomersService customers;
        private readonly ICommanServices commanServices;
        public readonly Logger logger;
        public CategoryController(ICustomersService _customers, ICommanServices _commanServices, Logger _Logger)
        {
            customers = _customers;
            commanServices = _commanServices;
            logger = _Logger;
        }
        // GET: Category
        public ActionResult Index()
        {
            if (Session["Role"].ToString() != "Admin" && Session["Role"].ToString() != "SubAdmin")
            {
                return RedirectToAction("AccessDenide", "Account");
            }
            return View();
        }

        [HttpPost]
        public ActionResult GetCategorys()
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
                (int TotalCount, int FilteredCount, dynamic Customers) data = customers.GetCategorys(skip, take, sortColumn, sortColumnDir, searchValue);
                return Json(new { draw = draw, recordsFiltered = data.TotalCount, recordsTotal = data.TotalCount, data = data.Customers });
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        

        [HttpGet]

        public async Task<ActionResult> ViewEmployee(string Id)
        {
            CustomerInfoViewModel model = new CustomerInfoViewModel();
            model = await customers.GetProduct(Guid.Parse(Id));
            return View(model);

        }

        public ActionResult CreateCategory()
        {
            if (Session["Role"].ToString() != "Admin" && Session["Role"].ToString() != "SubAdmin")
            {
                return RedirectToAction("AccessDenide", "Account");
            }
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> CreateCategory(CustomerInfoViewModel model)
        {
            
                ResponseModel response = await customers.CreateCategory(model);
                if (response.Status == Status.Success)
                {
                    this.ShowMessage("success", "Category create successfully", ToastType.success);
                    return RedirectToAction("Index");
                }
                else
                {
                    this.ShowMessage("failiure ", "Category not create successfully ", ToastType.success);
                }
                  
            return View(model);
        }

        


        [HttpGet]
        public async Task<ActionResult> CategoryDetails(string Id)
        {
            if (Session["Role"].ToString() != "Admin" && Session["Role"].ToString() != "SubAdmin")
            {
                return RedirectToAction("AccessDenide", "Account");
            }
            var CategoryInfofo = await customers.GetCategoryDetail(Id);           
            return View(CategoryInfofo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CategoryDetails(CustomerInfoViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ResponseModel response = await customers.UpdateCategoryData(model);
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
                logger.Error(ex);
                this.ShowMessage("Error", "An error occured, please after some time,", ToastType.success);
            }
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> DeleteCategory(CustomerInfoViewModel model)
        {
            return Json(await customers.DeleteCategoryDetails(model), JsonRequestBehavior.AllowGet);
        }

    }
}