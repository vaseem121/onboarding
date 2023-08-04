using smartTechAuthenticator.Services.Comman;
using smartTechAuthenticator.Services.Comman.CustomFilters;
using smartTechAuthenticator.Services.Tickets;
using smartTechAuthenticator.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace smartTechAuthenticator.Controllers
{
    [Authorized]
    public class TicketSystemController : Controller
    {
        private readonly ITicketService ticket;
        public TicketSystemController(ITicketService _ticket)
        {
            ticket = _ticket;
        }
        // GET: TicketSystem
        public async Task<ActionResult> Index(string startdate = null, string endtdate = null, int StatusId = 0)
        {
            TicketViewModel model = new TicketViewModel();
            var UserId = Session["userId"].ToString();
            model = await ticket.GetAllTickets(UserId, StatusId);
            if (startdate != null && startdate != "")
            {
                DateTime startDate1 = Convert.ToDateTime(startdate).Date;
                var aa = endtdate.Trim();
                DateTime endDate1 = Convert.ToDateTime(aa).Date;
                model.TicketViewmodelList = model.TicketViewmodelList.Where(x => x.CreatedDate.Date >= startDate1 && x.CreatedDate.Date <= endDate1).ToList();
               // model.date = date;
            }
            return View(model);
        }

        public ActionResult CreateTicket()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> CreateTicket(HttpPostedFileBase file, TicketViewModel tick)
        {
            var UserId = Session["UserId"].ToString();
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            if (file != null)
            {
                var checkextension = Path.GetExtension(file.FileName).ToLower();

                if (!allowedExtensions.Contains(checkextension))
                {
                    this.ShowMessage("Error", "Only JPG,JPEG,PNG file allowed !,", ToastType.error);
                }
                if (file.ContentLength > 0)
                {
                    string _FileName = Path.GetFileName(file.FileName);
                    string tempGuid = Guid.NewGuid().ToString().Substring(0, 5);
                    string FileName = tempGuid + "_" + _FileName;
                    tick.Photo = FileName;
                    string _path = Path.Combine(Server.MapPath("~/Content/Tickets"), FileName);
                    file.SaveAs(_path);
                }
            }
            if (ModelState.IsValid)
            {
                tick.CustomerId = UserId;
                ResponseModel response = await ticket.CreateTicket(tick);
                if (response.Status == Status.Success)
                {
                    this.ShowMessage("success", "Ticket create successfully", ToastType.success);
                    return RedirectToAction("Index", "TicketSystem");
                }
                else
                {
                    return View();
                }
            }
            return View();
        }

        [HttpPost]
        public ActionResult GetTickets()
        {
            try
            {
                var UserId = Session["UserId"].ToString();
                var draw = Request.Form.GetValues("draw").FirstOrDefault();
                var start = Request.Form.GetValues("start").FirstOrDefault();
                var length = Request.Form.GetValues("length").FirstOrDefault();
                var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
                var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();
                int take = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                (int TotalCount, int FilteredCount, dynamic Customers) data = ticket.GetTickets(skip, take, sortColumn, sortColumnDir, searchValue, UserId);
                return Json(new { draw = draw, recordsFiltered = data.TotalCount, recordsTotal = data.TotalCount, data = data.Customers });
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        [HttpGet]
        public async Task<ActionResult> TicketEdit(string Id)
        {
            if (Session["Role"].ToString() != "Customer")
            {
                return RedirectToAction("AccessDenide", "Account");
            }
            List<TicketMessageSystemViewModels> model = new List<TicketMessageSystemViewModels>();
            TicketViewModel data = new TicketViewModel();
            var UserId = new Guid(Session["UserId"].ToString());
            if (UserId != null)
            {
                model = ticket.ViewUserMessageList(UserId, new Guid(Id));
            }

            data = await ticket.GetTicketlDetail(Id);
            data.ViewTicketMessageList = model;
            data.TicketId = new Guid(Id);
            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> TicketEdit(HttpPostedFileBase file, TicketViewModel model)
        {
            try
            {
                var UserId = Session["UserId"].ToString();
                if (ModelState.IsValid)
                {
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                    if (file != null)
                    {
                        var checkextension = Path.GetExtension(file.FileName).ToLower();

                        if (!allowedExtensions.Contains(checkextension))
                        {
                            ViewBag.Message = "Only JPG,JPEG,PNG file allowed !";
                            return View();
                        }
                        if (file.ContentLength > 0)
                        {
                            string _FileName = Path.GetFileName(file.FileName);
                            string tempGuid = Guid.NewGuid().ToString().Substring(0, 5);
                            string FileName = tempGuid + "_" + _FileName;
                            string _path = Path.Combine(Server.MapPath("~/Content/Tickets"), FileName);
                            file.SaveAs(_path);
                            model.Photo = FileName;
                        }
                    }
                    model.UpdatedBy = UserId;
                    ResponseModel response = await ticket.UpdateTicketsData(model);
                    if (response.Status == Status.Success)
                    {
                        this.ShowMessage("success", "Details updated successfully", ToastType.success);
                        return RedirectToAction("Index", "TicketSystem");
                    }
                    else
                    {
                         this.ShowMessage("failiure ", "Details not upated successfully ", ToastType.error);
                        return View();
                    }
                }
            }
            catch (Exception ex)
            {
                this.ShowMessage("Error", "An error occured, please after some time,", ToastType.success);
            }
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> DeleteTicket(TicketViewModel model)
        {
            return Json(await ticket.DeleteTicketDetails(model), JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> ViewTicketDetails(Guid id)
        {
            List<TicketMessageSystemViewModels> model = new List<TicketMessageSystemViewModels>();
            TicketViewModel data = new TicketViewModel();
            var UserId = new Guid(Session["UserId"].ToString());
            if (id != null)
            {
                data = await ticket.TicketDetails(id);
            }

            if (UserId != null)
            {
                model = ticket.ViewUserMessageList(UserId, id);
            }
            data.ViewTicketMessageList = model;
            data.TicketId = id;

            return View(data);
        }

        [HttpPost]
        public async Task<ActionResult> TicketMessageSystem(TicketViewModel TicketMessage)
        {
            TicketMessageSystemViewModels model = new TicketMessageSystemViewModels();
            var UserId = Session["UserId"].ToString();
            if (TicketMessage.Description != null)
            {
                model.Description = TicketMessage.Description;
                model.CustomerId = new Guid(UserId);
                model.TicketId = TicketMessage.TicketId;
                ResponseModel response = await ticket.TicketMessage(model);
                if (response.Status == Status.Success)
                {
                    this.ShowMessage("success", "Message Send successfully", ToastType.success);
                    return Json(response);
                    //return RedirectToAction("TicketEdit", "TicketSystem" ,new { id= TicketMessage.TicketId});
                }
                else
                {
                    this.ShowMessage("failiure ", "Message not Send successfully ", ToastType.success);
                }

            }
            return RedirectToAction("TicketEdit", "TicketSystem", new { id = TicketMessage.TicketId });
        }

        public ActionResult SolvedTicket()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SolvedTickets()
        {
            try
            {
                var UserId = Session["UserId"].ToString();
                var draw = Request.Form.GetValues("draw").FirstOrDefault();
                var start = Request.Form.GetValues("start").FirstOrDefault();
                var length = Request.Form.GetValues("length").FirstOrDefault();
                var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
                var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();
                int take = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                (int TotalCount, int FilteredCount, dynamic Customers) data = ticket.GetSolvedTicket(skip, take, sortColumn, sortColumnDir, searchValue);
                return Json(new { draw = draw, recordsFiltered = data.TotalCount, recordsTotal = data.TotalCount, data = data.Customers });
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        [HttpPost]
        public ActionResult FilterSolvedTickets(string startDate = null, string endDate = null, int status = 0)
        {
            try
            {
                var UserId = Session["UserId"].ToString();
                var draw = Request.Form.GetValues("draw").FirstOrDefault();
                var start = Request.Form.GetValues("start").FirstOrDefault();
                var length = Request.Form.GetValues("length").FirstOrDefault();
                var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
                var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();
                int take = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                (int TotalCount, int FilteredCount, dynamic Customers) data = ticket.GetSolvedTicket(skip, take, sortColumn, sortColumnDir, searchValue, startDate, endDate, status);
                return Json(new { draw = draw, recordsFiltered = data.TotalCount, recordsTotal = data.TotalCount, data = data.Customers });
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        public ActionResult UnSolvedTicket()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UnSolvedTickets()
        {
            try
            {
                var UserId = Session["UserId"].ToString();
                var draw = Request.Form.GetValues("draw").FirstOrDefault();
                var start = Request.Form.GetValues("start").FirstOrDefault();
                var length = Request.Form.GetValues("length").FirstOrDefault();
                var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
                var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();
                int take = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                (int TotalCount, int FilteredCount, dynamic Customers) data = ticket.GetUnSolvedTicket(skip, take, sortColumn, sortColumnDir, searchValue);
                return Json(new { draw = draw, recordsFiltered = data.TotalCount, recordsTotal = data.TotalCount, data = data.Customers });
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        [HttpPost]
        public ActionResult filterUnSolvedTickets(string startDate = null, string endDate = null, int status=0)
        {
            try
            {
                var UserId = Session["UserId"].ToString();
                var draw = Request.Form.GetValues("draw").FirstOrDefault();
                var start = Request.Form.GetValues("start").FirstOrDefault();
                var length = Request.Form.GetValues("length").FirstOrDefault();
                var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
                var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();
                int take = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                (int TotalCount, int FilteredCount, dynamic Customers) data = ticket.GetUnSolvedTicket(skip, take, sortColumn, sortColumnDir, searchValue, startDate , endDate, status);
                return Json(new { draw = draw, recordsFiltered = data.TotalCount, recordsTotal = data.TotalCount, data = data.Customers });
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        [HttpPost]
        public JsonResult GetNotification()
        {
            TicketViewModel model = new TicketViewModel();
            var UserId = Session["UserId"].ToString();
            model = ticket.GetTickNotification(UserId);
            return Json(model);
        }

        [HttpGet]
        public async Task<ActionResult> NotificationView(string Id)
        {
            ResponseModel res = new ResponseModel();
            var UserId = Session["UserId"].ToString();
            if (Id != null)
            {
                res = await ticket.notificaationview(Id);

                if (res.Status == Status.Success)
                {
                    return RedirectToAction("TicketEdit", new { Id = res.OrderId });
                }
                else
                {
                    // RedirectToAction("Index", "Customer");
                    this.ShowMessage("Error", "An error occured, please connect Administartor !,", ToastType.success);
                }
            }
            return RedirectToAction("Index", "Customer");
        }

        [HttpPost]
        public async Task<JsonResult> AddRating(string TicketId, string Rating)
        {
            return Json(await ticket.UpdateRating(TicketId, Rating), JsonRequestBehavior.AllowGet);
        }

    }
}