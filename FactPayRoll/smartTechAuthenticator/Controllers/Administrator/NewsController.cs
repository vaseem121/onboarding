using Microsoft.EntityFrameworkCore;
using QRCoder;
using smartTechAuthenticator.Models;
using smartTechAuthenticator.Services.Comman;
using smartTechAuthenticator.Services.Customers;
using smartTechAuthenticator.ViewModels;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static System.Net.WebRequestMethods;

namespace smartTechAuthenticator.Controllers.Administrator
{
    public class NewsController : Controller
    {
        private readonly ICustomersService customers;
        ApplicationDbContext db = new ApplicationDbContext();
        public NewsController(ICustomersService customersService)
        {
            customers = customersService;
        }
        // GET: News
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult NewsDetails()
        {
            NewsViewModel model = new NewsViewModel();
            model.NewsList = customers.GetNewList();
            return View(model);
        }

        public ActionResult InsertNews()
        {
            return View();
        }

        [ValidateInput(false)]
        [HttpPost]
        public async Task<ActionResult> InsertNews(HttpPostedFileBase file, NewsViewModel news)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var UserId = Session["UserId"].ToString();
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                    if (file == null)
                    {
                        // this.ShowMessage("Error", "File is Requird!,", ToastType.error);
                        ViewBag.Message = "Photo is Required !";
                        return View();
                    }
                    var checkextension = Path.GetExtension(file.FileName).ToLower();

                    if (!allowedExtensions.Contains(checkextension))
                    {
                        this.ShowMessage("Error", "Only JPG,JPEG,PNG file allowed !,", ToastType.error);
                        // return RedirectToAction("ManageQr", "Customer");
                    }
                    if (file != null && file.ContentLength > 0)
                    {
                        string _FileName = Path.GetFileName(file.FileName);
                        string tempGuid = Guid.NewGuid().ToString().Substring(0, 5);
                        string FileName = tempGuid + "_" + _FileName;
                        string _path = Path.Combine(Server.MapPath("~/Content/NewsFile"), FileName);
                        file.SaveAs(_path);
                        news.Photo = FileName;
                    }
                    news.CreatedBy = UserId;
                    ResponseModel response = await customers.InsertNewsDetails(news);

                    if (response.Status == Status.Success)
                    {
                        this.ShowMessage("Success", "Insert successfully", ToastType.success);
                        return RedirectToAction("NewsDetails");
                    }
                    else
                    {
                        this.ShowMessage("failiure", "News not inserted Successfully", ToastType.success);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return View(news);
        }
        public ActionResult EditNews(Guid Id)
        {
            NewsViewModel model = new NewsViewModel();
            if (Session["Role"].ToString() != "Admin")
            {
                return RedirectToAction("AccessDenide", "Account");
            }
            model = customers.GetNewsData(Id);
            return View(model);
        }
        [ValidateInput(false)]
        [HttpPost]
        public async Task<ActionResult> EditNews(HttpPostedFileBase file, NewsViewModel news)
        {
            try
            {
                if (ModelState.IsValid)
                {
                  //  var Description = formValues["Description"];
                    if (file != null && file.ContentLength > 0)
                    {
                        string _FileName = Path.GetFileName(file.FileName);
                        string tempGuid = Guid.NewGuid().ToString().Substring(0, 5);
                        string FileName = tempGuid + "_" + _FileName;
                        string _path = Path.Combine(Server.MapPath("~/Content/NewsFile"), FileName);
                        file.SaveAs(_path);
                        news.Photo = FileName;
                    }


               //     news.Description = Description;
                    ResponseModel response = await customers.UpdateNewsDetails(news);
                    if (response.Status == Status.Success)
                    {
                        this.ShowMessage("Success", "Updated successfully", ToastType.success);
                        return RedirectToAction("NewsDetails");
                    }
                    else
                    {
                        this.ShowMessage("failiure", "News not Updated Successfully", ToastType.success);
                    }

                }
            }
            catch (Exception)
            {

                throw;
            }

            return View(news);
        }
       [HttpPost]
        public async Task<ActionResult> DeleteNews(Guid Id)
        {
            await customers.DeleteNewsDetails(Id);
            return RedirectToAction("NewsDetails");

        }


        public ActionResult GetNewsDetails()
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
                (int TotalCount, int FilteredCount, dynamic about) data = customers.GetNewsDetails(skip, take, sortColumn, sortColumnDir, searchValue);
                return Json(new { draw = draw, recordsFiltered = data.TotalCount, recordsTotal = data.TotalCount, data = data.about });
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        
    }
}