using smartTechAuthenticator.Services.Comman.CustomFilters;
using smartTechAuthenticator.Services.Customers;
using smartTechAuthenticator.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace smartTechAuthenticator.Controllers
{
    [Authorized]
    public class UserController : Controller
    {
        private readonly ICustomersService customers;
        public UserController(ICustomersService _customers)
        {
            customers = _customers;          
        }
        public ActionResult Dashboard()
        {
            BannerCarouselViewModel model = new BannerCarouselViewModel();
            model.BannerCarouselList = customers.GrtBannerCarouselList();
            return View(model);
        }
    }
}